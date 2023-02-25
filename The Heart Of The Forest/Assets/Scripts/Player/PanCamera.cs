/*
 * Dated Created: 29/10/2022
 * Author: Nicholas Connell
 */

using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PanCamera : MonoBehaviour
{
    [SerializeField] private bool m_canPan;
    private CinemachineFramingTransposer m_framingTransposer;
    private Vector2 m_originalValues;

    [SerializeField] private InputActionReference panControls;

    [Tooltip("The main cinemachine camera that follows the player")]
    [SerializeField] private CinemachineVirtualCamera m_mainVcam;

    [Tooltip("How much the camera should pan")]
    [SerializeField, Range(0, 1)] private float m_panAmount = .3f;

    [Tooltip("How fast the camera will pan")]
    [SerializeField] private float m_panSpeed = 1;

    private void Awake()
    {
        m_framingTransposer = m_mainVcam.GetCinemachineComponent<CinemachineFramingTransposer>();

        m_originalValues.x = m_framingTransposer.m_ScreenX;
        m_originalValues.y = m_framingTransposer.m_ScreenY;
    }

    private void Update()
    {
        if (m_canPan)
        {
            m_framingTransposer.m_ScreenX = Mathf.Lerp(m_framingTransposer.m_ScreenX, m_originalValues.x - panControls.action.ReadValue<Vector2>().x * m_panAmount, m_panSpeed * 2 * Time.deltaTime);
            m_framingTransposer.m_ScreenY = Mathf.Lerp(m_framingTransposer.m_ScreenY, m_originalValues.y + panControls.action.ReadValue<Vector2>().y * m_panAmount, m_panSpeed * 2 * Time.deltaTime);
        }
    }
}
