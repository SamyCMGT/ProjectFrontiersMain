using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class FadeSphereOnLightRotation : MonoBehaviour
{
    public Light directionalLight; 
    public Material sphereMaterial; 
    public float fadeDuration = 3f; 

    public Volume postProcessingVolume; 
    private ColorAdjustments colorAdjustments; 

    private float targetAlpha = 0f; 
    private static readonly int AlphaPropertyID = Shader.PropertyToID("_Alpha"); 
    private float currentAlpha = 0f; 
    private float targetPostExposure = 0f; 
    private float currentPostExposure = 0f; 

    void Start()
    {

        sphereMaterial.SetFloat(AlphaPropertyID, 0f); 
        currentAlpha = 0f; 

       
        if (postProcessingVolume.profile.TryGet<ColorAdjustments>(out colorAdjustments))
        {
            colorAdjustments.postExposure.value = 0f; 
            currentPostExposure = 0f;
        }
    }

    void Update()
    {
        if (sphereMaterial == null || colorAdjustments == null)
        {
            return;
        }

        float lightRotationX = directionalLight.transform.eulerAngles.x;

        if (lightRotationX >= 352f || lightRotationX < 0.01f) // When rotation is near 352
        {
            targetAlpha = 1f; // Sphere should fade in to fully visible
            targetPostExposure = 2f; 
        }
        else if (lightRotationX >= 160f && lightRotationX < 352f) // When rotation is between 160 and 352
        {
            targetAlpha = 1f; // Keep it fully visible
            targetPostExposure = 2f; 
        }
        else if (lightRotationX >= 0f && lightRotationX < 160f) // When rotation is between 0 and 160
        {
            targetAlpha = 0f; // Sphere should fade out to fully transparent
            targetPostExposure = 0f; 
        }

        if (Mathf.Abs(currentAlpha - targetAlpha) > 0.01f) 
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

        if (Mathf.Abs(currentPostExposure - targetPostExposure) > 0.01f)
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
