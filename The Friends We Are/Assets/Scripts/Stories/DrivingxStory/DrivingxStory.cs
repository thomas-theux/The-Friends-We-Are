using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Rewired;

public class DrivingxStory : MonoBehaviour {

	public GameObject bus;
	public Camera mainCamera;

	private Rigidbody rb;
	private float currentSpeed;
	private float tempSpeed;
	private float steerThreshold = 20.0f;
	private float speed = 10;
	private float speedMax = 30;
	private float speedIncrease = 1.0f;
	private float speedDecrease = 0.8f;

	public Text velocity;
	public Text score;

	private bool isAccelerating = false;
	private bool isBraking = false;

	public static float drivingScore = 0;

	// Actions for this story/minigame
	private float accelerating;
	private float braking;
	private float steering;


	private void Start() {
		rb = bus.GetComponent<Rigidbody>();

		StartCoroutine(DisableGravity());
	}


	private void Update() {
		GetInput();

		ActionsDark();
		ActionsLight();

		currentSpeed = rb.velocity.magnitude;
		velocity.text = Mathf.Round(currentSpeed * 10f) / 10f  + " km/h";
		score.text = drivingScore + "";
	}


	private void GetInput() {
		// Get input from player one (dark)
		accelerating = GameManager.playerDark.GetAxis("R2");
		braking = GameManager.playerDark.GetAxis("L2");

		// Get input from player two (light)
		steering = GameManager.playerDark.GetAxis("LS Horizontal");
	}


	private void ActionsDark() {
		// Increase speed by accelerating
		if (accelerating > 0 && !isBraking) {
			isAccelerating = true;
			Vector3 movement = new Vector3(0, 0, accelerating);
			rb.AddForce(movement * speed);
		} else if (accelerating <= 0) {
			isAccelerating = false;
		}

		// Decrease speed by braking
		if (braking > 0.5f && !isAccelerating) {
			isBraking = true;
			rb.drag += 0.02f;
		} else if (braking <= 0.5f) {
			isBraking = false;
			rb.drag = 0.3f;
		}

		// Limit speed when bus reaches maximum
		if (accelerating > 0) {
			if (speed < speedMax) {
				speed = currentSpeed + speedIncrease;
			} else {
				speed = speedMax;
			}
		} else {
			if (speed > 0) {
				speed -= speedDecrease;
			} else {
				speed = 0;
			}
		}
	}


	private void ActionsLight() {
		// Steering the bus
		if (steering != 0) {
			if (currentSpeed < steerThreshold) {
				tempSpeed = currentSpeed;
			} else {
				tempSpeed = Mathf.Pow(currentSpeed, -0.7f) * 300;
			}
		}
		if (rb.velocity.z > 1) {
			Vector3 steer = new Vector3(steering, 0, 0);
			rb.AddForce(steer * tempSpeed);
		}
	}


	// Disable gravity after the bus has fallen down on the street
	IEnumerator DisableGravity() {
		yield return new WaitForSeconds(2);
		rb.useGravity = false;
		rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
		mainCamera.transform.SetParent(bus.transform);
	}

}