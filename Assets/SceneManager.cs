using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [Header("Scene Settings")]
    public int SceneIndexG = 0;
    public PlayerMovementAdvanced pm;

    [Header("Fade Settings")]
    [SerializeField] private CanvasGroup fadeCanvasGroup; 
    [SerializeField] private float fadeDuration = 3f; 

    private bool isFading = false;

    private void Start()
    {
        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.alpha = 0f; 
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isFading)
        {
            StartCoroutine(FadeAndLoadScene());
        }
    }

    private IEnumerator FadeAndLoadScene()
    {
        isFading = true;

        yield return StartCoroutine(Fade(1f));

        if (pm != null)
        {
            pm.destroyPlayer();
        }

        SceneManager.LoadScene(SceneIndexG);
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

        fadeCanvasGroup.alpha = targetAlpha;
    }
}
