using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;

public class BreakSticks : MonoBehaviour {

	public GameObject twigsGO;
	public Slider twigsSlider;
	public GameObject rangeGO;

	private bool isIncreasing = false;

	private float currentValue = 0;
	private float moveSpeed = 80.0f;

	private bool buttonR1;
	private bool buttonL1;

	private bool isPressedR1;
	private bool isPressedL1;

	private float timerR1 = 1.0f;
	private float timerL1 = 1.0f;
	private float timerDef = 1.0f;

	private float rangeMin;
	private float rangeMax;
	private float sliderMax = 100;

	private float addPoints = 1.0f;


	private void OnEnable() {
		twigsGO.SetActive(true);

		twigsGO.transform.GetChild(0).transform.localPosition = CampfirexStory.gaugePos[CampfirexStory.activePlayer];

		CalculateRange();
	}


	private void CalculateRange() {
		float rangeWidth = rangeGO.transform.localScale.x * sliderMax;

		rangeMin = (sliderMax/2) - (rangeWidth / 2);
		rangeMax = rangeMin + rangeWidth;
		print(rangeWidth);
	}


	private void Update() {
		if (isIncreasing) {
			IncreaseValue();
		} else {
			DecreaseValue();
		}

		GetInput();

		ActionsInput();

		ButtonTimer();

		if (isPressedR1 && isPressedL1) {
			GetPoints();
		}

		twigsSlider.value = currentValue;
	}

	
	private void IncreaseValue() {
		currentValue += moveSpeed * Time.deltaTime;

		if (currentValue >= 100) {
			isIncreasing = false;
		}
	}

	
	private void DecreaseValue() {
		currentValue -= moveSpeed * Time.deltaTime;

		if (currentValue <= 0) {
			isIncreasing = true;
		}
	}


	private void GetInput() {
		// CHANGE THIS FROM ZERO TO CAMPFIREXSTORY.ACTIVEPLAYER !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
		buttonR1 = ReInput.players.GetPlayer(0).GetButtonDown("R1");
		buttonL1 = ReInput.players.GetPlayer(0).GetButtonDown("L1");
	}


	private void ActionsInput() {
		if (buttonR1) {
			if (twigsSlider.value > rangeMin && twigsSlider.value < rangeMax) {
				isPressedR1 = true;
			}
		}

		if (buttonL1) {
			if (twigsSlider.value > rangeMin && twigsSlider.value < rangeMax) {
				isPressedL1 = true;
			}
		}
	}

	private void ButtonTimer() {
		if (isPressedR1) {
			timerR1 -= Time.deltaTime;
			if (timerR1 <= 0) {
				isPressedR1 = false;
				timerR1 = timerDef;
			}
		}

		if (isPressedL1) {
			timerL1 -= Time.deltaTime;
			if (timerL1 <= 0) {
				isPressedL1 = false;
				timerL1 = timerDef;
			}
		}
	}

	private void GetPoints() {
		isPressedR1 = false;
		isPressedL1 = false;

		timerR1 = timerDef;
		timerL1 = timerDef;

		CampfirexStory.sticksPercentage += addPoints;
	}


	private void OnDisable() {
		twigsGO.SetActive(false);
	}

}
