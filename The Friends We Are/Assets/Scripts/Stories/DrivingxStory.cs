﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Rewired;

public class DrivingxStory : MonoBehaviour {

	public GameObject bus;
	public Camera mainCamera;

	private Rigidbody rb;
	private float currentSpeed;
	private float tempSpeed;
	private float speed = 10;
	private float speedMax = 40;
	private float speedIncrease = 1.0f;
	private float speedDecrease = 0.8f;

	private bool isAccelerating = false;

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

		string printSpeed = Mathf.Round(currentSpeed * 10f) / 10f  + " km/h";
		print(printSpeed);
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
		if (accelerating > 0) {
			isAccelerating = true;
			Vector3 movement = new Vector3(0, 0, accelerating);
			rb.AddForce(movement * speed);
		} else {
			isAccelerating = false;
		}

		// Decrease speed by braking
		if (braking > 0.5f) {
			rb.drag += 0.01f;
		} else {
			rb.drag = 0.5f;
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
		if (currentSpeed > 0) {
			tempSpeed = Mathf.Pow(currentSpeed, -0.7f);
		}
		if (steering != 0) {
			Vector3 steer = new Vector3(steering, 0, 0);
			rb.AddForce(steer * tempSpeed * 300);
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