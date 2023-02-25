/*
 * Date Created: 05/10/2022
 * Author: Nicholas Connell
 */

using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class FadeEnvironmentLights : MonoBehaviour
{
    private float m_startIntensity;

    [SerializeField] private float m_fadeTime = 1.0f;

    [SerializeField] private Light m_light;
    [SerializeField] private Light2D m_light2D;

    private void Awake()
    {
        //Get the starting intensity
        if (m_light)
            m_startIntensity = m_light.intensity;
    }

    /// <summary>
    /// Fades a light according to a timer and intensity.
    /// </summary>
    /// <param name="intensity"></param>
    IEnumerator LightFade(float intensity)
    {
        float waitTime = m_fadeTime;
        float elapsedTime = 0;

        float curIntensity = m_light ? m_light.intensity : m_light2D.intensity;

        while (elapsedTime < waitTime)
        {
            //3D lights
            if (m_light)
            {
                m_light.intensity = Mathf.Lerp(curIntensity, intensity, elapsedTime / waitTime);
                //RenderSettings.ambientIntensity = Mathf.Lerp(curIntensity, intensity, elapsedTime / waitTime);
            }

            //2D lights
            if (m_light2D)
                m_light2D.intensity = Mathf.Lerp(curIntensity, intensity, elapsedTime / waitTime);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    public void FadeIntensity(float intensity)
    {
        StartCoroutine(LightFade(intensity));
    }

    public void SetLightInstant(float intensity)
    {
        //3D lights
        if (m_light)
        {
            m_light.intensity = intensity;
            //RenderSettings.ambientIntensity = Mathf.Lerp(curIntensity, intensity, elapsedTime / waitTime);
        }

        //2D lights
        if (m_light2D)
        {
            m_light2D.intensity = intensity;
        }
    }
}
