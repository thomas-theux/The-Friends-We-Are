using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrivingTest : MonoBehaviour {

	private Rigidbody rb;
	public float speed;

	private void Start() {
		rb = GetComponent<Rigidbody>();
	}

	private void FixedUpdate() {
		float accelerating = GameManager.playerDark.GetAxis("R2");

		Vector3 movement = new Vector3(0, 0, accelerating);

		rb.AddForce(movement * speed);
	}
}
