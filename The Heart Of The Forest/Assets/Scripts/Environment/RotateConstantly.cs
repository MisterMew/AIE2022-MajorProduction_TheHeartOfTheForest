/*
 * 
 * Date Created: 23.08.2022
 * Author: Lewis Comstive
 *
 */

 /*
  * 
  * Changelog:
  *	Lewis - Initial creation
  *
  */

using UnityEngine;

public class RotateConstantly : MonoBehaviour
{
	[SerializeField, Tooltip("Degrees per second")]
	private float rotationSpeed = 2.0f;

	private void Update() => transform.localEulerAngles -= Vector3.forward * rotationSpeed * Time.deltaTime;
}
