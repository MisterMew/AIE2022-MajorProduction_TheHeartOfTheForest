/*
 * 
 * Date Created: 22.08.2022
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

namespace HotF.Environment
{
	[RequireComponent(typeof(Collider2D))]
	public class GlideArea : MonoBehaviour
	{
		[SerializeField, Tooltip("Velocity to set to gliding rigidbody in this area")]
		public float maxVelocity = 10.0f;

		private void Start() => GetComponent<Collider2D>().isTrigger = true;
	}
}
