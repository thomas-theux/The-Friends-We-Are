using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class GenerateFriction : MonoBehaviour {

	public GameObject frictionGO;

	public GameObject directionGO;
	public GameObject stickGO;

	private float stickInputX;
	private float stickInputY;
	private float posMultiplier = 50;
	private float stickZoneOuter = 0.5f;
	private float stickZoneInner = 0.4f;

	private bool firstActivation;

	private int directionCurrent;
	private int directionGoal;


	private float addPoints = 0.1f;


	private void OnEnable() {
		frictionGO.SetActive(true);

		frictionGO.transform.GetChild(0).transform.localPosition = CampfirexStory.gaugePos[CampfirexStory.activePlayer];
	}


	private void Update() {
		GetInput();

		TransformStick();
		
		InputActions();
	}


	private void GetInput() {
		// CHANGE THIS FROM ZERO TO CAMPFIREXSTORY.ACTIVEPLAYER !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
		stickInputX = ReInput.players.GetPlayer(0).GetAxis("RS Horizontal");
		stickInputY = ReInput.players.GetPlayer(0).GetAxis("RS Vertical");
	}


	private void TransformStick() {
		stickGO.transform.localPosition = new Vector2(stickInputX * posMultiplier, stickInputY * posMultiplier);
	}


	private void InputActions() {
		if (firstActivation) {

			// Check if the current direction is the goal direction
			if (stickInputY > stickZoneOuter && stickInputX > -stickZoneInner && stickInputX < stickZoneInner) {
				directionCurrent = 0;
				if (directionCurrent == directionGoal) {
					GetPoints();
					directionGoal++;
				}
			}
			if (stickInputX > stickZoneOuter && stickInputY > -stickZoneInner && stickInputY < stickZoneInner) {
				directionCurrent = 1;
				if (directionCurrent == directionGoal) {
					GetPoints();
					directionGoal++;
				}
			}
			if (stickInputY < -stickZoneOuter && stickInputX > -stickZoneInner && stickInputX < stickZoneInner) {
				directionCurrent = 2;
				if (directionCurrent == directionGoal) {
					GetPoints();
					directionGoal++;
				}
			}
			if (stickInputX < -stickZoneOuter && stickInputY > -stickZoneInner && stickInputY < stickZoneInner) {
				directionCurrent = 3;
				if (directionCurrent == directionGoal) {
					GetPoints();
					directionGoal = 0;
				}
			}

		} else {
			// Set initial goal direction to where to go next
			if (stickInputY > stickZoneOuter) {
				directionGoal = 1;
				firstActivation = true;
			} else if (stickInputX > stickZoneOuter) {
				directionGoal = 2;
				firstActivation = true;
			} else if (stickInputY < -stickZoneOuter) {
				directionGoal = 3;
				firstActivation = true;
			} else if (stickInputX < -stickZoneOuter) {
				directionGoal = 0;
				firstActivation = true;
			}

		}
	}


	private void GetPoints() {
		CampfirexStory.frictionPercentage += addPoints;
	}


	private void OnDisable() {
		frictionGO.SetActive(false);
	}
}
