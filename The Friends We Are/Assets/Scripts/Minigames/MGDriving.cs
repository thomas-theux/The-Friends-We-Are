using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;

public class MGDriving : MonoBehaviour {

	public Slider rpmGauge;
	public Text gearText;
	public Text minigameScore;

	private float buttonThreshold = 0.5f;

	private float rpm = 0;
	private float[] rpmChangeSpeed = {200, 160, 120, 80, 40, 20};
	private int speedIndex = 0;
	private float rpmCap = 8000;

	private float[] areaArr;

	public float drivingScore = 0;
	float pointsToAdd = 200;
	float pointsToSubtract;

	private float clutchSeconds;

	// REWIRED PLUGIN
	private Player playerDark;
	// private Player playerLight;

	private float accelerate;
	private bool shift;
	private float clutch;
	// private float steer;


	void Awake() {

		playerDark = ReInput.players.GetPlayer(0);
		// playerLight = ReInput.players.GetPlayer(1);

	}

	void Start() {
		areaArr = new[] {0, (rpmCap * 0.4f), (rpmCap * 0.5f), (rpmCap * 0.6f), (rpmCap * 0.7f), (rpmCap * 0.8f), rpmCap};
	}

	void Update() {

		GetInput();

		PlayerDark();
		PlayerLight();

	}

	// Get input from the players controllers
	private void GetInput() {

		accelerate = playerDark.GetAxis("R2");
		shift = playerDark.GetButtonDown("X");

		clutch = playerDark.GetAxis("L2");
		// steer = playerDark.GetAxis("LS Horizontal");

		// clutch = playerLight.GetAxis("L2");
		// steer = playerLight.GetAxis("LS Horizontal");

	}

	// Accelerate and hit clutch
	private void PlayerDark() {

		// Increase the rpm when the player is accelerating
		if (accelerate > buttonThreshold && rpm < rpmCap) {
			rpm += rpmChangeSpeed[speedIndex];
		}

		// Limiting the rpm
		if (rpm > rpmCap) { rpm = rpmCap; }

		// Lowering the rpm over time
		if (rpm > 0 && accelerate < buttonThreshold) {
			rpm -= rpm * 0.02f;
		} else if (rpm <= 0) {
			rpm = 0;
		}
		
		// Visualize the rpm gauge
		rpmGauge.value = rpm;

		// Shifting gears
		if (shift && clutch > buttonThreshold && speedIndex <= rpmChangeSpeed.Length -2) {
			GetPoints();
			speedIndex++;
			gearText.text = (speedIndex + 1) + "";
			StartCoroutine(DecreaseRPM());
		}

		// Getting points for keeping pace
		if (speedIndex == rpmChangeSpeed.Length -1) {
			CheckAreas();
		}

	}

	// Steer and shift
	private void PlayerLight() {

		// Hitting the clutch but only if the car isn't already in 6th gear
		if (speedIndex <= rpmChangeSpeed.Length -2) {
			if (clutch > buttonThreshold) {
				if (accelerate < 0.7f) {
					// Success: Player Two hits the clutch while Player One is not accelerating
					StartShifting();
				} else if (accelerate >= 0.7f) {
					// Fail: Player Two hits the clutch while Player One is accelerating
				}
			}
			
		}
		
	}

	// Calculating points for shifting
	private void GetPoints() {

		// Reset values
		pointsToAdd = 200;
		pointsToSubtract = 0;

		// Check if playes shifted outside the perfect are and calculate accordingly
		if (rpm < areaArr[3]) {
			pointsToSubtract = (areaArr[3] - rpm) / 10;
		} else if (rpm > areaArr[4]) {
			pointsToSubtract = (rpm - areaArr[4]) / 10;
		}

		// The 6 areas for shifting
		if (rpm >= areaArr[0] && rpm < areaArr[1]) {
			// print("-20 Punkte");
			pointsToSubtract *= 3;
		}
		if (rpm >= areaArr[1] && rpm < areaArr[2]) {
			// print("10 Punkte");
			pointsToSubtract *= 2;
		}
		if (rpm >= areaArr[2] && rpm < areaArr[3]) {
			// print("50 Punkte");
			pointsToSubtract *= 1;
		}
		if (rpm >= areaArr[3] && rpm < areaArr[4]) {
			// print("100 Punkte");
		}
		if (rpm >= areaArr[4] && rpm < areaArr[5]) {
			// print("10 Punkte");
			pointsToSubtract *= 1;
		}
		if (rpm >= areaArr[5] && rpm < areaArr[6]) {
			// print("-20 Punkte");
			pointsToSubtract *= 3;
		}

		// Calculate points and push to overall minigame score
		pointsToAdd -= pointsToSubtract;
		drivingScore += pointsToAdd;

		// Display score
		minigameScore.text = Mathf.Ceil(drivingScore) + "";

	}

	private void StartShifting() {
		// Starting to count seconds when clutch has been hit
		// clutchSeconds += Time.deltaTime;
		// print(clutchSeconds);
	}

	private void CheckAreas() {
		// The 6 areas for getting points
		if (rpm >= areaArr[0] && rpm < areaArr[1]) {
			pointsToAdd = 0;
		}
		if (rpm >= areaArr[1] && rpm < areaArr[2]) {
			pointsToAdd = -1;
		}
		if (rpm >= areaArr[2] && rpm < areaArr[3]) {
			pointsToAdd = 1;
		}
		if (rpm >= areaArr[3] && rpm < areaArr[4]) {
			pointsToAdd = 3;
		}
		if (rpm >= areaArr[4] && rpm < areaArr[5]) {
			pointsToAdd = 1;
		}
		if (rpm >= areaArr[5] && rpm < areaArr[6]) {
			pointsToAdd = -1;
		}

		// Display score
		drivingScore += pointsToAdd;
		minigameScore.text = Mathf.Ceil(drivingScore) + "";
	}

	IEnumerator DecreaseRPM() {
		while (rpm > 0) {
			rpm -= 400;
			yield return null;
		}
	}

}
