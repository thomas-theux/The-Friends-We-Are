using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Rewired;

public class DrivingxStory : MonoBehaviour {

	public GameObject bus;
	public Camera mainCamera;

	public GameObject burnout;
	public AudioSource burnoutSound;

	private Rigidbody rb;

	private bool initialStart;
	private bool spawnBurnouts;

	private Vector3 movement;
	public static float currentSpeed;
	private float tempSpeed;
	private float steerThreshold = 20.0f;
	private float speed = 10;
	public float speedMax = 24;
	private float speedIncrease = 1.0f;
	private float speedDecrease = 0.8f;

	private float maxSpeed;
	private float allSpeedValues;
	private int divideSpeed;
	private float averageSpeed;

	private float transfer;

	public Text velocity;
	public Text score;

	private bool isAccelerating = false;
	private bool isBraking = false;

	public static float drivingScore = 0;

	private bool statsSaved = false;


	// Actions for this story/minigame
	private float accelerating;
	private float braking;
	private float steering;


	private void Start() {
		rb = bus.GetComponent<Rigidbody>();

		StartCoroutine(DisableGravity());
	}


	private void Update() {
		// Spawn burnouts when level starts
		if (StartLevelCountdown.startLevel && !spawnBurnouts) {
			Instantiate(burnout, new Vector3(6, 0, -2), burnout.transform.rotation);
			Instantiate(burnout, new Vector3(4, 0, -2), burnout.transform.rotation);
			burnoutSound.Play();

			spawnBurnouts = true;
		}

		if (StartLevelCountdown.startLevel) {
			GetInput();

			currentSpeed = rb.velocity.magnitude;
			// velocity.text = Mathf.Round(currentSpeed * 1f) / 1f  + " km/h";
			velocity.text = currentSpeed.ToString("F0") + " km/h";
			score.text = drivingScore + "";

			MaximumSpeed();

			AverageSpeed();
		}

		if (LevelTimer.levelEnd && !statsSaved) {

			// Save the single values for the stats overview
			StatsManager.transferValues = new float[] {
				Mathf.Ceil(bus.transform.position.z),
				Mathf.Ceil(maxSpeed),
				Mathf.Ceil(averageSpeed),
				drivingScore
			};

			// Save the titles for the stats
			StatsManager.transferTexts = new string[] {
				"Meters Driven",
				"Maximum Speed",
				"Average Speed",
				"Experience Gained"
			};

			statsSaved = true;
		}
	}


	private void FixedUpdate() {
		if (StartLevelCountdown.startLevel) {
			if (initialStart) {
				ActionsDark();
				ActionsLight();
			} else {
				InitialStart();
			}
		}
	}


	private void LateUpdate() {
		CameraMovement();
	}


	private void GetInput() {
		// Get input from player one (dark)
		accelerating = GameManager.playerDark.GetAxis("R2");
		braking = GameManager.playerDark.GetAxis("L2");

		// Get input from player two (light)
		steering = GameManager.playerDark.GetAxis("LS Horizontal");
	}


	// Giving the players a quick start for a better player experience
	private void InitialStart() {
		if (currentSpeed < 10) {
			movement = new Vector3(0, 0, 2f);
			rb.AddForce(movement * speed);
		} else {
			initialStart = true;
		}
	}


	private void ActionsDark() {
		// Increase speed by accelerating
		if (accelerating > 0 && !isBraking) {
			isAccelerating = true;
			movement = new Vector3(0, 0, accelerating);
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
			// Rotate bus when steering
			// if (bus.transform.rotation.y < 0.05f && steering > 0) {
			// 	bus.transform.Rotate(0, steering, 0);
			// }
			// if (bus.transform.rotation.y > -0.05f && steering < 0) {
			// 	bus.transform.Rotate(0, steering, 0);
			// }

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


	// Move camera out if the bus drives faster
	private void CameraMovement() {
		mainCamera.orthographicSize = 16 + (currentSpeed / 20);
	}


	// Get stats for highest speed
	private void MaximumSpeed() {
		if (currentSpeed > maxSpeed) {
			maxSpeed = currentSpeed;
		}
	}


	// Get stats for average speed
	private void AverageSpeed() {
		allSpeedValues += currentSpeed;
		divideSpeed++;
		averageSpeed = allSpeedValues / divideSpeed;
	}


	// Disable gravity after the bus has fallen down on the street
	IEnumerator DisableGravity() {
		yield return new WaitForSeconds(2);
		rb.useGravity = false;
		rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
	}

}