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
	public static int collectedPoints;
	private float steeringMultiplier = 1.4f;
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
	private float addFriendsScore;
	private float addToScore = 100;

	private float transfer;

	public Text velocity;
	public Text score;

	private bool isAccelerating = false;
	private bool isBraking = false;

	private float experienceScore = 0;
	public static float experiencePerPoint;

	private bool statsSaved = false;

	private int tier1 = 130;
	private int tier2 = 420;
	private int tier3 = 710;
	private int tier4 = 1000;


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
			velocity.text = currentSpeed.ToString("F0") + " km/h";
			score.text = experienceScore + "";

			MaximumSpeed();

			AverageSpeed();

			DrivingScore();
		}

		if (LevelTimer.levelEnd && !statsSaved) {

			// Calculate the percentage the friends score is increasing
			CalculatePercentage();

			// Save the single values for the stats overview
			StatsManager.transferValues = new float[] {
				bus.transform.position.z,
				maxSpeed,
				averageSpeed,
				experienceScore,
				addFriendsScore
			};

			// Transfer values WITH ceiling
			// StatsManager.transferValues = new float[] {
			// 	Mathf.Ceil(bus.transform.position.z),
			// 	Mathf.Ceil(maxSpeed),
			// 	Mathf.Ceil(averageSpeed),
			// 	drivingScore,
			// 	addFriendsScore
			// };

			// Save the titles for the stats
			StatsManager.transferTexts = new string[] {
				"Meters Driven",
				"Maximum Speed",
				"Average Speed",
				"Experience Gained",
				"Friends Score"
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
			Vector3 steer = new Vector3(steering * steeringMultiplier, 0, 0);
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


	private void DrivingScore() {
		experienceScore = experiencePerPoint * (collectedPoints / 10);
	}


	private void CalculatePercentage() {
		float overallScore = (bus.transform.position.z / 1000) * (
			maxSpeed +
			averageSpeed +
			experienceScore
		);

		float minEvents = GameManager.tripDays * (GameManager.maxAP / GameManager.storyAP);
		float maxEvents = GameManager.tripDays * (GameManager.maxAP / GameManager.radioAP);

		float calculateAllEvents = (minEvents * GameManager.storyChance + maxEvents * GameManager.radioChance) / 100;

		float rankMultiplier = 0;

		// CALCULATING THE SCORE WITHOUT RANKS
		rankMultiplier = (overallScore + addToScore) / 1000;

		// CALCULATING THE SCORE WITH RANKS
		// Get rank D
		// if (overallScore < tier1) {
		// 	rankMultiplier = 0.6f;
		// 	print("Rank D");
		// }
		// // Get rank C
		// if (overallScore >= tier1 && overallScore < tier2) {
		// 	rankMultiplier = 0.7f;
		// 	print("Rank C");
		// }
		// // Get rank B
		// if (overallScore >= tier2 && overallScore < tier3) {
		// 	rankMultiplier = 0.8f;
		// 	print("Rank B");
		// }
		// // Get rank A
		// if (overallScore >= tier3 && overallScore < tier4) {
		// 	rankMultiplier = 0.9f;
		// 	print("Rank A");
		// }
		// // Get rank S
		// if (overallScore >= tier4) {
		// 	rankMultiplier = 1.0f;
		// 	print("Rank S");
		// }

		addFriendsScore = (100 / calculateAllEvents) * rankMultiplier;

	}


	// Disable gravity after the bus has fallen down on the street
	IEnumerator DisableGravity() {
		yield return new WaitForSeconds(2);
		rb.useGravity = false;
		rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
	}

}