using UnityEngine;

public class EnvironmentGroundColorChanger : MonoBehaviour
{
    // Speed of color change
    [SerializeField] private float colorChangeSpeed = 1f;

    // Time tracker for hue
    private float colorTime;

    void Update()
    {
        // Increment time based on the speed
        colorTime += Time.deltaTime * colorChangeSpeed;

        // Calculate the color based on HSV to RGB conversion
        Color newColor = Color.HSVToRGB((colorTime % 1f), 1f, 1f);

        // Set the ground color in the environment lighting
        RenderSettings.ambientGroundColor = newColor;
    }
}
