using System.Collections; // Needed for coroutines
using UnityEngine;
using TMPro;  // TextMeshPro UI for displaying dialogue

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue Settings")]
    [SerializeField] private TMP_Text dialogueText;  // TextMeshPro UI Text for displaying dialogue
    [SerializeField] private string[] dialogueLines; // Array of dialogue lines
    [SerializeField] private float typingSpeed = 0.05f; // Speed of text appearing
    [SerializeField] private float delayBetweenLines = 5f; // Delay in seconds between each dialogue line

    [Header("Game Settings")]
    //[SerializeField] private ShardsCollected shardCount; // Reference to your shard collection scriptable object
   // [SerializeField] private int totalShards = 5; // Total number of shards to collect before stopping the dialogue
    private bool isWaitingForShardCollection = false; // To control when shard collection should trigger dialogue

    [Header("UI Panel Settings")]
    [SerializeField] private CanvasGroup uiPanelCanvasGroup; // Reference to the CanvasGroup on your UI Panel
    [SerializeField] private float fadeSpeed = 1f; // Speed at which the panel fades in and out

    // Timeout settings
    [SerializeField] private float timeoutDuration = 8f; // Time in seconds before the panel fades out
    private float lastLineTime; // The time when the last line was shown

    private int currentLineIndex = 0; // Keeps track of which dialogue line to show
    private bool isDialogueActive = false;

    private void OnEnable()
    {
        // Start the dialogue when the player spawns into the scene
        StartDialogue();
    }

    private void StartDialogue()
    {
        // Reset the dialogue system
        currentLineIndex = 0;
        isDialogueActive = true;

        // Ensure the UI Panel is visible when dialogue starts
        FadePanel(1f);

        // Show the first 4 lines one after another with delays in between
        ShowNextLine();
    }

    public void ShowNextLine()
    {
        if (currentLineIndex < dialogueLines.Length)
        {
            // Stop the current typing coroutine if it's running
            StopAllCoroutines();

            // Start typing the next line
            StartCoroutine(ShowLineWithDelay(dialogueLines[currentLineIndex]));

            // Increment to the next line
            currentLineIndex++;

            // Reset the timeout timer after showing a new line
            lastLineTime = Time.time;

            // Ensure the panel becomes visible again when new dialogue starts
            FadePanel(1f); // Make sure the background becomes visible when a new line is triggered

            // If we have shown the first 4 lines, wait for shard collection before showing the next lines
            if (currentLineIndex > 4)
            {
                isWaitingForShardCollection = true; // Wait for shard collection to proceed with further lines
            }
            else
            {
                // Automatically show the next line after the delay
                StartCoroutine(WaitAndShowNextLine());
            }
        }
        else
        {
            // Dialogue complete, hide the panel
            isDialogueActive = false;
            dialogueText.text = ""; // Clear the text
            FadePanel(0f); // Make the panel transparent
        }
    }

    private IEnumerator ShowLineWithDelay(string sentence)
    {
        // Type out the sentence
        yield return StartCoroutine(TypeSentence(sentence));
    }

    private IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    private IEnumerator WaitAndShowNextLine()
    {
        // Wait for the specified time before showing the next line
        yield return new WaitForSeconds(delayBetweenLines);

        // Show the next line in the sequence
        ShowNextLine();
    }

   /* public void CheckForShardCollection()
    {
        // If the dialogue is still active and waiting for shard collection, show the next line when a shard is collected
        if (isWaitingForShardCollection)
        {
            if (shardCount.Value < totalShards)
            {
                // A shard was collected, show the next line
                ShowNextLine();
            }
            else if (shardCount.Value >= totalShards && currentLineIndex < dialogueLines.Length)
            {
                // If the player collected all shards, show the final dialogue line
                isWaitingForShardCollection = false; // Reset the flag
                ShowNextLine(); // Show the last line
            }
        }
    }*/

    private void FadePanel(float targetAlpha)
    {
        // Fade the panel to the target alpha value (0 for transparent, 1 for visible)
        StartCoroutine(FadeCanvasGroup(uiPanelCanvasGroup, targetAlpha));
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float targetAlpha)
    {
        float startAlpha = canvasGroup.alpha;
        float timeElapsed = 0f;

        while (timeElapsed < fadeSpeed)
        {
            // Gradually change the alpha value
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, timeElapsed / fadeSpeed);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure the final alpha value is set
        canvasGroup.alpha = targetAlpha;
    }

    private void Update()
    {
        // Check if the time since the last line exceeds the timeout duration
        if (isDialogueActive && Time.time - lastLineTime > timeoutDuration)
        {
            // If no new line has been triggered within 8 seconds, fade out the dialogue and the background
            FadePanel(0f); // Make the panel transparent
            dialogueText.text = ""; // Clear the dialogue text
        }
    }
}
