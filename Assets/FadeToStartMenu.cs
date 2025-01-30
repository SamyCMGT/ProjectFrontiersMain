using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeToStartMenu : MonoBehaviour
{
    [Header("Fade Settings")]
    [SerializeField] private CanvasGroup fadeCanvasGroup; // Reference to a CanvasGroup
    [SerializeField] private float fadeDuration = 1f; // Time for the fade effect
    [SerializeField] private string startMenuSceneName = "StartMenuScene"; // Name of the scene to load
    [SerializeField] private float delayBeforeSceneLoad = 5f; // Delay before loading scene

    [Header("Timer Settings")]
    public float timeBeforeFade = 15f; // Time before fading starts (public for easy adjustment)

    [Header("UI Elements")]
    [SerializeField] private Button skipButton; // Reference to the UI button

    private bool isFading = false;

    private void Start()
    {
        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.alpha = 0f;
        }

        // Start the countdown before fading
        StartCoroutine(WaitThenFade());

        // Assign button click event if a button is set
        if (skipButton != null)
        {
            skipButton.onClick.AddListener(SkipFade);
        }
        else
        {
            Debug.LogWarning("Skip Button not assigned in the Inspector!");
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        StartFadeIfNotStarted();
    }

    private IEnumerator WaitThenFade()
    {
        yield return new WaitForSeconds(timeBeforeFade);
        StartFadeIfNotStarted();
    }

    public void SkipFade()
    {
        // Called when the UI button is pressed
        StartFadeIfNotStarted();
    }

    private void StartFadeIfNotStarted()
    {
        if (!isFading)
        {
            StartCoroutine(FadeAndLoadScene());
        }
    }

    private IEnumerator FadeAndLoadScene()
    {
        isFading = true;

        // Fade the screen to black
        yield return StartCoroutine(Fade(1f));

        // Wait for the delay
        yield return new WaitForSeconds(delayBeforeSceneLoad);

        // Load the StartMenuScene
        if (!string.IsNullOrEmpty(startMenuSceneName))
        {
            SceneManager.LoadScene(startMenuSceneName);
        }

        // Fade back to transparency
        StartCoroutine(Fade(0f));
    }

    private IEnumerator Fade(float targetAlpha)
    {
        float startAlpha = fadeCanvasGroup.alpha;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            fadeCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        // Ensure the final alpha value is set
        fadeCanvasGroup.alpha = targetAlpha;
    }
}