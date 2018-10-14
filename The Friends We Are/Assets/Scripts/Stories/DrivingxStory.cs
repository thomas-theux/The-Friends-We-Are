using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Rewired;

public class DrivingxStory : MonoBehaviour {

	public GameObject bus;
	public Camera mainCamera;

	private Rigidbody rb;
	private float speed = 10;

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

		string printSpeed = Mathf.Round(rb.velocity.magnitude * 10f) / 10f  + " km/h";
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
			Vector3 movement = new Vector3(0, 0, accelerating);
			rb.AddForce(movement * speed);
		}

		// Decrease speed by braking
		if (braking > 0.5f) {
			print("Bremsen");
		}

		if (speed < 50) {
			speed = rb.velocity.magnitude + 1;
		} else {
			speed = 50;
		}
	}


	private void ActionsLight() {
		if (steering > 0.5f) {
			print("Lenken");
		}
	}


	// Disable gravity after the bus has fallen down on the street
	IEnumerator DisableGravity() {
		yield return new WaitForSeconds(2);
		rb.useGravity = false;
		rb.constraints = RigidbodyConstraints.FreezeRotation;
		rb.constraints = RigidbodyConstraints.FreezePositionY;
		mainCamera.transform.SetParent(bus.transform);
	}

}