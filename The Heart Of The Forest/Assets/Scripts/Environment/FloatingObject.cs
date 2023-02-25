using UnityEngine;

public class FloatingObject : MonoBehaviour
{
	[SerializeField, Tooltip("How high the fragment can float from it's initial position")]
	private float floatHeight = 0.2f;

	[SerializeField, Tooltip("How fast the fragment moves between floating positions")]
	private float floatSpeed = 1.0f;

	/// <summary>
	/// Point to float around
	/// </summary>
	private Vector3 floatCenter = Vector3.zero;

	private void Start() => floatCenter = transform.position;

 	private void Update() => transform.position = floatCenter + Vector3.up * Mathf.Sin(Time.time * floatSpeed) * floatHeight;
}
