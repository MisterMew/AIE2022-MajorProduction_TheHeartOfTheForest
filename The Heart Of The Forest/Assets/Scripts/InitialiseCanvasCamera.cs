/*
 * Date Created: 24.09.2022
 * Author: Jazmin Fazzolari
 * Contributors: -
 */

using UnityEngine;

namespace HotF.Graphics
{
    public class InitialiseCanvasCamera : MonoBehaviour
    {
        /* Variables */
        [SerializeField] private Camera camera;
        [SerializeField] private Canvas canvas;
        [SerializeField] private float offsetPlaneDistance = 0.2F;


        private void Start()
        {
            canvas.worldCamera = camera; //Assign world camera with camera go
            canvas.planeDistance = camera.nearClipPlane + offsetPlaneDistance; //Set plane distance with offset
        }
    }
}