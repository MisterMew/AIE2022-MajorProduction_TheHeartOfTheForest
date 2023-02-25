using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinMeRightRound : MonoBehaviour
{
	public float speed = 25;

	void Update() => transform.eulerAngles += Vector3.up * speed * Time.deltaTime;
}
