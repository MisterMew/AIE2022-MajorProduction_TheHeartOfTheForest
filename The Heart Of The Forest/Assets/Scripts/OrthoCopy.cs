/*
 * Date Created: 05/10/2022
 * Author: Nicholas Connell
 */

using UnityEngine;

public class OrthoCopy : MonoBehaviour
{
    private Camera m_mainCam;
    private Camera m_currentCam;

    private void Awake()
    {
        //Cache components
        m_mainCam = Camera.main;
        m_currentCam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        //Update ortho size of overlay camera
        m_currentCam.orthographicSize = m_mainCam.orthographicSize;
    }
}
