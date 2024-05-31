using UnityEngine;

public class LightingManager : MonoBehaviour
{
    public Material skyboxMaterial;
    public Light sunLight;

    private void Start()
    {
        // Configurar la Skybox
        RenderSettings.skybox = skyboxMaterial;

        // Configurar la luz del sol
        if (sunLight != null)
        {
            RenderSettings.sun = sunLight;
        }

        // Actualizar la iluminación global
        DynamicGI.UpdateEnvironment();
    }
}
