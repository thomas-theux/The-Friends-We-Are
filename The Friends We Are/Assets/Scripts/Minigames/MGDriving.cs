using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;

public class MGDriving : MonoBehaviour {

	public Slider rpmGauge;

	private float buttonThreshold = 0.5f;

	private float rpm = 0;
	private float[] rpmChangeSpeed = {200, 160, 120, 80, 40, 10};
	private int speedIndex = 0;
	private float rpmCap = 8000;

	private float[] areaArr;

	public float drivingScore = 0;

	private bool clutchHit = false;

	// REWIRED PLUGIN
	private Player playerDark;
	private Player playerLight;

	private float accelerate;
	private bool shift;
	private float clutch;
	private float steer;


	void Awake() {

		playerDark = ReInput.players.GetPlayer(0);
		playerLight = ReInput.players.GetPlayer(1);

	}

	void Start() {
		areaArr = new[] {0, (rpmCap * 0.4f), (rpmCap * 0.5f), (rpmCap * 0.6f), (rpmCap * 0.7f), (rpmCap * 0.8f), rpmCap};
	}

	void Update() {

		print(rpmChangeSpeed[speedIndex]);

		GetInput();

		PlayerDark();
		PlayerLight();

	}

	// Get input from the players controllers
	private void GetInput() {

		accelerate = playerDark.GetAxis("R2");
		shift = playerDark.GetButtonDown("X");

		clutch = playerDark.GetAxis("L2");
		steer = playerDark.GetAxis("LS Horizontal");

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
			print((speedIndex + 1) + ". Gang");
			rpm = 0;
		}

	}

	// Steer and shift
	private void PlayerLight() {

		// Hitting the clutch
		if (speedIndex <= rpmChangeSpeed.Length -2) {
			if (accelerate < 0.7f && clutch > buttonThreshold) {
				// Success: Player Two hits the clutch while Player One is not accelerating
			} else if (accelerate >= 0.7f && clutch > buttonThreshold) {
				// Fail: Player Two hits the clutch while Player One is accelerating
			}
		}
		

		// GetPoints();
	}

	// Calculating points for shifting
	private void GetPoints() {

		// The 6 areas for shifting
		if (rpm >= areaArr[0] && rpm < areaArr[1]) {
			// print("-20 Punkte");
		}
		if (rpm >= areaArr[1] && rpm < areaArr[2]) {
			// print("10 Punkte");
		}
		if (rpm >= areaArr[2] && rpm < areaArr[3]) {
			// print("50 Punkte");
		}
		if (rpm >= areaArr[3] && rpm < areaArr[4]) {
			// print("100 Punkte");
		}
		if (rpm >= areaArr[4] && rpm < areaArr[5]) {
			// print("10 Punkte");
		}
		if (rpm >= areaArr[5] && rpm < areaArr[6]) {
			// print("-20 Punkte");
		}

	}

}
