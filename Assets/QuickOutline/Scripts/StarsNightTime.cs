using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class FadeSphereOnLightRotation : MonoBehaviour
{
    public Light directionalLight; // Assign the directional light
    public Material sphereMaterial; // Assign the Shader Graph material
    public float fadeDuration = 3f; // Duration of fade-in/out in seconds

    public Volume postProcessingVolume; // Reference to the post-processing volume
    private ColorAdjustments colorAdjustments; // Color Adjustments for post-processing

    private float targetAlpha = 0f; // Target alpha value
    private static readonly int AlphaPropertyID = Shader.PropertyToID("_Alpha"); // Shader property name
    private float currentAlpha = 0f; // Current alpha value for the fade
    private float targetPostExposure = 0f; // Target post-exposure value
    private float currentPostExposure = 0f; // Current post-exposure value

    void Start()
    {
        // Ensure the material starts fully transparent
        if (sphereMaterial == null)
        {
            Debug.LogError("Sphere material is not assigned. Please assign it in the Inspector.");
            return;
        }

        sphereMaterial.SetFloat(AlphaPropertyID, 0f); // Start fully transparent
        currentAlpha = 0f; // Match initial transparency

        // Ensure the post-processing volume is assigned
        if (postProcessingVolume == null)
        {
            Debug.LogError("Post-processing volume is not assigned. Please assign it in the Inspector.");
            return;
        }

        // Retrieve the Color Adjustments component from the volume
        if (postProcessingVolume.profile.TryGet<ColorAdjustments>(out colorAdjustments))
        {
            colorAdjustments.postExposure.value = 0f; // Start with post-exposure at 0
            currentPostExposure = 0f;
        }
        else
        {
            Debug.LogError("Color Adjustments override is missing from the post-processing profile. Please add it.");
        }
    }

    void Update()
    {
        // Ensure the sphere material and post-processing volume are assigned
        if (sphereMaterial == null || colorAdjustments == null)
        {
            return;
        }

        // Get the X rotation of the directional light
        float lightRotationX = directionalLight.transform.eulerAngles.x;

        // Determine the target alpha and post-exposure based on light rotation
        if (lightRotationX >= 352f || lightRotationX < 0.01f) // When rotation is near 352
        {
            targetAlpha = 1f; // Sphere should fade in to fully visible
            targetPostExposure = 2f; // Post-exposure should increase to 1
        }
        else if (lightRotationX >= 160f && lightRotationX < 352f) // When rotation is between 160 and 352
        {
            targetAlpha = 1f; // Keep it fully visible
            targetPostExposure = 2f; // Keep post-exposure at 1
        }
        else if (lightRotationX >= 0f && lightRotationX < 160f) // When rotation is between 0 and 160
        {
            targetAlpha = 0f; // Sphere should fade out to fully transparent
            targetPostExposure = 0f; // Post-exposure should decrease to 0
        }

        // Smoothly transition the current alpha to the target alpha
        if (Mathf.Abs(currentAlpha - targetAlpha) > 0.01f) // Only update if there’s a noticeable difference
        {
            currentAlpha = Mathf.MoveTowards(currentAlpha, targetAlpha, Time.deltaTime / fadeDuration);
            sphereMaterial.SetFloat(AlphaPropertyID, currentAlpha);
        }
        else
        {
            // Clamp to ensure exact values when close
            currentAlpha = targetAlpha;
            sphereMaterial.SetFloat(AlphaPropertyID, currentAlpha);
        }

        // Smoothly transition the post-exposure to the target value
        if (Mathf.Abs(currentPostExposure - targetPostExposure) > 0.01f) // Only update if there's a noticeable difference
        {
            currentPostExposure = Mathf.MoveTowards(currentPostExposure, targetPostExposure, Time.deltaTime / fadeDuration);
            colorAdjustments.postExposure.value = currentPostExposure;
        }
        else
        {
            // Clamp to ensure exact values when close
            currentPostExposure = targetPostExposure;
            colorAdjustments.postExposure.value = currentPostExposure;
        }
    }
}
