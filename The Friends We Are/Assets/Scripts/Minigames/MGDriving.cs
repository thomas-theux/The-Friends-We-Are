﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;

public class MGDriving : MonoBehaviour {

	public Slider rpmGauge;

	private float rpm = 0;
	private float rpmIncrease = 160;
	private float rpmDecrease = 60;
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

		GetInput();

		PlayerDark();
		PlayerLight();

	}

	// Get input from the players controllers
	private void GetInput() {

		accelerate = playerDark.GetAxis("R2");
		shift = playerDark.GetButtonDown("X");

		clutch = playerLight.GetAxis("L2");
		steer = playerLight.GetAxis("LS Horizontal");

	}

	// Accelerate and hit clutch
	private void PlayerDark() {

		// Increase the rpm when the player is accelerating
		if (accelerate > 0.5f && rpm < rpmCap) {
			rpm += rpmIncrease;
		}

		// Limiting the rpm
		if (rpm > rpmCap) { rpm = rpmCap; }

		// Lowering the rpm over time
		if (rpm > 0) { rpm -= rpmDecrease; }
		else { rpm = 0; }
		
		// Visualize the rpm gauge
		rpmGauge.value = rpm;

		// Shifting gears
		if (shift && clutch > 0.5f) {
			print("Gschaltet!");
		}

	}

	// Steer and shift
	private void PlayerLight() {

		// Player Two hits the clutch for Player One to shift
		if (accelerate < 0.7f && clutch > 0.5f) {
			// Success: Player Two hits the clutch while Player One is not accelerating
			// print("Clutch");
		} else if (accelerate >= 0.7f && clutch > 0.5f) {
			// Fail: Player Two hits the clutch while Player One is accelerating
			print("FAIL");
		}

		// GetPoints();
	}

	// Calculating points for shifting
	private void GetPoints() {

		// The 6 areas for shifting
		if (rpm >= areaArr[0] && rpm < areaArr[1]) {
			print("Area 01");
		}
		if (rpm >= areaArr[1] && rpm < areaArr[2]) {
			print("Area 02");
		}
		if (rpm >= areaArr[2] && rpm < areaArr[3]) {
			print("Area 03");
		}
		if (rpm >= areaArr[3] && rpm < areaArr[4]) {
			print("Area 04");
		}
		if (rpm >= areaArr[4] && rpm < areaArr[5]) {
			print("Area 05");
		}
		if (rpm >= areaArr[5] && rpm < areaArr[6]) {
			print("Area 06");
		}

	}

}
