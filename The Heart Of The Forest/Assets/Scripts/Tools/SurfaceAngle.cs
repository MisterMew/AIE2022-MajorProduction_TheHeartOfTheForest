/*
 * Date Created: 05.09.2022
 * Author: Jazmin Fazzolari
 * Contributors: -
 */

using UnityEngine;

namespace HotF.Tools
{
    /// <summary>
    /// Handles the calcualtions of a surface angle to determane the direction an entity is facing, and the slope of the the surface
    /// </summary>
    public class SurfaceAngle : MonoBehaviour
    {
        /* Variables */
        [SerializeField] private Transform frontRay;
        [SerializeField] private Transform rearRay;
        [SerializeField] private bool enableDebugGizmo = true;
        [SerializeField] private LayerMask layerMask;

        private float surfaceAngle = 0F;
        private bool upHill = false;
        private bool flatSurface = false;

        private RaycastHit2D rearHit;
        private RaycastHit2D frontHit;

        /* Start is called before the first frame update */
        void Start()
        {
            rearHit.distance = 0F;
        }

        // Update is called once per frame
        void Update()
        {
            HandleRearRaycast();
            HandleFrontRaycast();
            SurfaceValidation();
        }

        private void HandleRearRaycast()
        {
            rearRay.rotation = Quaternion.Euler(-gameObject.transform.rotation.x, 0, 0); //Gameobject rotation does not affect raycast direction

            if (Physics2D.Raycast(rearRay.position, rearRay.TransformDirection(-Vector2.up), Mathf.Infinity, layerMask))
            {
                Debug.DrawRay(rearRay.position, rearRay.TransformDirection(-Vector2.up) * rearHit.distance, Color.yellow); //Draws the gizmo ray
                surfaceAngle = Vector2.Angle(rearHit.normal, Vector2.up); //Compare hit normal with vector to get ground angle
                Debug.Log(surfaceAngle);
            }
            else
            {
                Debug.DrawRay(rearRay.position, rearRay.TransformDirection(-Vector2.up) * 1000, Color.red);
                upHill = false;
                flatSurface = false;
                Debug.Log("Downhill");
            }
        }

        private void HandleFrontRaycast()
        {
            frontRay.rotation = Quaternion.Euler(-gameObject.transform.rotation.x, 0, 0);

            Vector3 frontRayStartPos = new Vector3(frontRay.position.x, rearRay.position.y, frontRay.position.z);

            if (Physics2D.Raycast(frontRayStartPos, rearRay.TransformDirection(-Vector2.up), Mathf.Infinity, layerMask))
            {
                Debug.DrawRay(frontRayStartPos, frontRay.TransformDirection(-Vector2.up) * frontHit.distance, Color.yellow);
                surfaceAngle = Vector2.Angle(frontHit.normal, Vector2.up); //Compare hit normal with vector to get ground angle
                Debug.Log(surfaceAngle);
            }
            else
            {
                Debug.DrawRay(frontRayStartPos, frontRay.TransformDirection(-Vector2.up) * 1000, Color.red);
                upHill = true;
                flatSurface = false;
                Debug.Log("Uphill");
            }
        }

        /// <summary>
        /// Checks to determine when the object is going uphill, downhill, or on a flat surface
        /// </summary>
        private void SurfaceValidation()
        {
            if (frontHit.distance < rearHit.distance)
            {
                upHill = true;
                flatSurface = false;
                Debug.Log("Uphill");
            }
            else if (frontHit.distance > rearHit.distance)
            {
                upHill = false;
                flatSurface = false;
                Debug.Log("Downhill");
            }
            else if (frontHit.distance == rearHit.distance)
            {
                flatSurface = true;
                Debug.Log("Flat Surface");
            }
        }
    }
}