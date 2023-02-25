/*
 * Date Created: 17/10/2022
 * Author: Nicholas Connell
 */

using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class RingExpand : MonoBehaviour
{
    private Transform m_transform;
    private Light2D m_light;

    private float m_startIntensity;
    private int m_currentPulse;

    [Tooltip("How fast the ring expands")]
    [SerializeField] private float m_expandSpeed = .05f;
    [Tooltip("The lifespan of the ring")]
    [SerializeField] private float m_destroyTimer = 2.0f;

    [Tooltip("The amount of pulses before the ring expands")]
    [SerializeField] private int m_pulseNum = 2;
    [Tooltip("How long the small pulse expand will last")]
    [SerializeField] private float m_smallExpandTimer = 0.3f;
    [Tooltip("How long the small pulse shrink will last")]
    [SerializeField] private float m_smallShrinkTimer = 1.0f;

    [Tooltip("he size the circle will expand to when pulsing")]
    [SerializeField] private float m_smallSize = 0.05f;
    [Tooltip("How mush the pulse will shrink")]
    [SerializeField] private float m_shrinkAmount = 0.01f;

    [Tooltip("The rotating speed of the ring")]
    [SerializeField] private float m_rotationSpeed = 0.3f;

    [SerializeField] private GameObject m_lightPrefab;

    private void Awake()
    {
        //Cache components
        m_transform = GetComponent<Transform>();
        m_light = GetComponent<Light2D>();

        m_startIntensity = m_light.intensity;
    }

    private void Start()
    {
        if (m_pulseNum < 1)
            StartCoroutine(MainExpand());
        else
            StartCoroutine(SmallExpand());
    }

    private void Update()
    {
        //Update the rotation
        Vector3 rot = m_transform.eulerAngles;
        rot.z += m_rotationSpeed;
        m_transform.eulerAngles = rot;
    }

    /// <summary>
    /// The expand enumerator for the small pulses
    /// </summary>
    IEnumerator SmallExpand()
    {
        //Set the wait time and elapsed time
        float elapsedTime = 0;
        float waitTime = m_smallExpandTimer;
        Vector3 currentScale = m_transform.localScale;

        //While the elapsed is less than the wait time...
        while (elapsedTime < waitTime)
        {
            //Lerp the local scale 
            m_transform.localScale = Vector3.Lerp(currentScale, new Vector3(m_smallSize, m_smallSize, m_smallSize), Mathf.SmoothStep(0, 1, elapsedTime / waitTime));

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        //Start the small shrink co-routine
        StartCoroutine(SmallShrink());
    }

    /// <summary>
    /// The shrink enumerator for the small pulses
    /// </summary>
    IEnumerator SmallShrink()
    {
        //Set the elapsedTime and wait time
        float elapsedTime = 0;
        float waitTime = m_smallShrinkTimer;

        //Set the scales
        Vector3 currentScale = m_transform.localScale;
        Vector3 smallerScale = new Vector3(currentScale.x - m_shrinkAmount, currentScale.y - m_shrinkAmount, currentScale.z - m_shrinkAmount);

        //While the elapsed time is less than the wait time...
        while (elapsedTime < waitTime)
        {
            //Lerp the scale of the transform
            m_transform.localScale = Vector3.Lerp(currentScale, smallerScale, Mathf.SmoothStep(0, 1, elapsedTime / waitTime));

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        m_currentPulse++;

        if (m_currentPulse < m_pulseNum)
            StartCoroutine(SmallExpand());
        else
            StartCoroutine(MainExpand());
    }

    /// <summary>
    /// The large expand which happens after the pulses
    /// </summary>
    IEnumerator MainExpand()
    {
        float elapsedTime = 0;
        float waitTime = m_destroyTimer;

        while (elapsedTime < waitTime)
        {
            //Expand the local scale
            m_transform.localScale += new Vector3(m_expandSpeed, m_expandSpeed, m_expandSpeed);

            m_light.intensity = Mathf.Lerp(m_startIntensity, 0, elapsedTime / waitTime);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        //Destroy this gameobject
        Destroy(gameObject);
    }

    public void InstantiateLight()
    {
        Instantiate(m_lightPrefab);
    }
}
