using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsDisplayer : MonoBehaviour {

	private LevelFade levelFadeScript;

	private IEnumerator increaseValues;

	public AudioSource increaseValue;
	public AudioSource finishedIncreasing;
	public AudioSource btnContinue;

	public Text[] showValues;
	public Text[] showTexts;
	public Slider oldScore;
	public Slider newScore;
	private int index = 0;

	private float increaseTime = 0.8f;
	private float tempTime = 0;
	private float currentValue = 0;
	private float calculatedValue = 0;
	private float[] increasingValues = {0, 0, 0, 0, 0};

	private int overallValues = 5;

	public static bool didSkip;


	private void Start() {
		// Get the level fader script
		levelFadeScript = GameObject.Find("LevelFader").GetComponent<LevelFade>();

		// Set old score to current overall score that is saved in Game Manager
		oldScore.value = GameManager.overallScore;

		// Show text in UI that got transferred from the Story
		for (int i = 0; i < 4; i++) {
			showTexts[i].text = StatsHolder.transferTexts[i];
		}

		// Enable basic navigation
		GameManager.enableNavigation = true;

		increaseValues = IncreaseValue();
		StartCoroutine(increaseValues);
	}


	private void Update() {
		SkipStatsAnim();

		GetInput();
	}


	private void GetInput() {
		// Get input from player one (dark)
		if (GameManager.enableNavigation) {
			if(GameManager.playerDark.GetButtonDown("X")) {
				if (!GameManager.skipStats) {
					didSkip = true;
					GameManager.skipStats = true;
				} else {
					StartCoroutine(Continue());
				}
				btnContinue.Play();
			}
		}
	}


	private void SkipStatsAnim() {
		if (GameManager.skipStats) {
			StopCoroutine(increaseValues);

			if (didSkip) {
				for (int j = 0; j < 4; j++) {
					if (StatsHolder.transferValues[j] >= 10) {
						showValues[j].text = StatsHolder.transferValues[j].ToString("F0") + "";
					} else {
						showValues[j].text = "0" + StatsHolder.transferValues[j].ToString("F0") + "";
					}
					showValues[j].color = new Color(1.0f, 0.427451f, 0.003921569f);
				}
				showValues[overallValues-1].text = "+" + StatsHolder.transferValues[overallValues-1].ToString("F1") + "%";
				newScore.value = StatsHolder.transferValues[overallValues-1];
				// GameManager.skipStats = false;
			}
			// Save new friends score to Game Manager
			GameManager.overallScore = newScore.value;

			// // Save current score for UI slider interface
			// oldScore.value = newScore.value;
		}
	}


	// Show the single stats by animating them starting from 0
	IEnumerator IncreaseValue() {
		yield return new WaitForSeconds(1);

		while (index < overallValues) {
			while (tempTime < increaseTime) {
				tempTime += Time.deltaTime;
				MapFunction(tempTime, 0, increaseTime, 0, 1);
				ApplyFormula(currentValue);
				MapFunction(calculatedValue, 0, 1, 0, StatsHolder.transferValues[index]);
				increasingValues[index] = currentValue;

				if (increasingValues[index] < 9) {
					if (index < overallValues - 1) {
						// Show other stats
						showValues[index].text = "0" + increasingValues[index].ToString("F0");
					} else {
						// Show friends score
						showValues[index].text = "+" + increasingValues[index].ToString("F1") + "%";
						newScore.value = increasingValues[index];
					}
					
				} else {
					if (index < overallValues - 1) {
						// Show other stats
						showValues[index].text = increasingValues[index].ToString("F0");
					} else {
						// Show friends score
						showValues[index].text = "+" + increasingValues[index].ToString("F1") + "%";
						newScore.value = increasingValues[index];
					}
				}
				increaseValue.Play();

				yield return null;
			}
			// After increasing the values we show the final value that got transferred from the story script
			if (index < overallValues - 1) {
				if (StatsHolder.transferValues[index] >= 10) {
					showValues[index].text = StatsHolder.transferValues[index].ToString("F0") + "";
				} else {
					showValues[index].text = "0" + StatsHolder.transferValues[index].ToString("F0") + "";
				}
			} else {
				showValues[index].text = "+" + StatsHolder.transferValues[index].ToString("F1") + "%";
			}
			// Color the increased value orange
			showValues[index].color = new Color(1.0f, 0.427451f, 0.003921569f);
			finishedIncreasing.Play();

			tempTime = 0;
			index++;

			// yield return new WaitForSeconds(0.05f);
		}

		index = overallValues - 1;

		// Stats can not be skipped anymore after they are fully displayed
		GameManager.skipStats = true;
	}


	// The formula with which the stats gets increased
	private void ApplyFormula(float currentValue) {
		// Formula: y = (x-1)^3 + 1
		calculatedValue = Mathf.Pow(currentValue, 1);
	}


	// Map function to map the numbers / Dreisatz
	private void MapFunction(float number, float oldMin, float oldMax, float newMin, float newMax) {
		currentValue = newMin + (newMax - newMin) * (number - oldMin) / (oldMax - oldMin);
	}


	private IEnumerator Continue() {
		yield return new WaitForSeconds(0.5f);
		// Fade to next level
		levelFadeScript.FadeToLevel("5 Announcement");
	}

}
