/*
 * Date Created: 29/10/2022
 * Author: Nicholas Connell
 */

using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightScaleCopy : MonoBehaviour
{
    private Light2D m_light;
    [SerializeField] private Transform m_parent;

    [Tooltip("The scale of the light by the parent transform")]
    [SerializeField] private float m_lightScale = 1.0f;

    private void Awake()
    {
        m_light = GetComponent<Light2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_parent.localScale.x == 0)
        {
            m_light.pointLightOuterRadius = 0;
        }
        else
        {
            m_light.pointLightOuterRadius = m_parent.localScale.x * m_lightScale;
        }
    }
}
