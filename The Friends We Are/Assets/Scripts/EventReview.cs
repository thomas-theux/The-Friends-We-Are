using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventReview : MonoBehaviour {

	public Text[] statValues;

	public static bool stillIncreasing = false;
	public static bool answersMatch = false;

	public Slider oldScoreSlider;
	public Slider newScoreSlider;

	public AudioSource increaseSound;
	public AudioSource finishedIncreasingSound;
	public AudioSource notSameAnswerSound;

	private float currentValue;
	private float calculatedValue;

	private IEnumerator increaseValuesCo;


	private void OnEnable() {
		increaseValuesCo = IncreaseValues();
	}


	public void StartIncreaser() {
		StartCoroutine(increaseValuesCo);
	}


	public IEnumerator IncreaseValues() {
		yield return new WaitForSeconds(0.5f);

		// Enable navigation so players can skip if they like
		GameManager.enableNavigation = true;
		stillIncreasing = true;

		float tempTime = 0;
		float increaseTime = 1.0f;
		int tempIndex = 0;

		// Increase values as long as there's time left
		while (tempIndex < statValues.Length) {
			while (tempTime < increaseTime) {
				tempTime += Time.deltaTime;

				MapFunction(tempTime, 0, increaseTime, 0, 1);
				ApplyFormula(currentValue);
				MapFunction(calculatedValue, 0, 1, 0, StatsHolder.transferValues[tempIndex]);
				// MapFunction(calculatedValue, 0, 1, 0, valuesArr[tempIndex]);

				if (tempIndex < statValues.Length -1) {
					statValues[tempIndex].text = currentValue.ToString("F2");
				} else {
					if (answersMatch) {
						statValues[tempIndex].text = "+" + currentValue.ToString("F1") + "%";
						newScoreSlider.value = oldScoreSlider.value + currentValue;
					} else {
						tempTime = increaseTime;
						notSameAnswerSound.Play();
					}
				}

				increaseSound.Play();
				yield return null;
			}

			// Set the values to the real values
			if (tempIndex < statValues.Length -1) {
				statValues[tempIndex].text = StatsHolder.transferValues[tempIndex].ToString("F2");
				// statValues[tempIndex].text = valuesArr[tempIndex].ToString("F2");
			} else {
				statValues[tempIndex].text = "+" + StatsHolder.transferValues[tempIndex].ToString("F1") + "%";
				// statValues[tempIndex].text = "+" + valuesArr[tempIndex].ToString("F1") + "%";
				newScoreSlider.value = oldScoreSlider.value + StatsHolder.transferValues[tempIndex];
				// newScoreSlider.value = oldScoreSlider.value + valuesArr[tempIndex];
			}

			// Color the increased value with a gradient
			statValues[tempIndex].GetComponent<GradientText>().enabled = true;
			finishedIncreasingSound.Play();

			tempTime = 0;
			tempIndex++;
		}

		// Stop the possibility to skip the increasing animation
		stillIncreasing = false;
	}


	// Map function to map the numbers / Dreisatz
	private void MapFunction(float number, float oldMin, float oldMax, float newMin, float newMax) {
		currentValue = newMin + (newMax - newMin) * (number - oldMin) / (oldMax - oldMin);
	}


	// The formula with which the stats gets increased
	private void ApplyFormula(float currentValue) {
		// Formula: y = (x-1)^3 + 1
		calculatedValue = Mathf.Pow(currentValue, 1);
	}


	public void SkipStatsAnim() {
		stillIncreasing = false;

		StopCoroutine(increaseValuesCo);

		for (int i = 0; i < statValues.Length; i++) {
			if (i < statValues.Length -1) {
				statValues[i].text = StatsHolder.transferValues[i].ToString("F2");
				// statValues[i].text = valuesArr[i].ToString("F2");
			} else {
				statValues[i].text = "+" + StatsHolder.transferValues[i].ToString("F1") + "%";
				// statValues[i].text = "+" + valuesArr[i].ToString("F1") + "%";
				newScoreSlider.value = oldScoreSlider.value + StatsHolder.transferValues[i];
				// newScoreSlider.value = oldScoreSlider.value + valuesArr[i];
			}

			// Color the increased value with a gradient
			statValues[i].GetComponent<GradientText>().enabled = true;
			finishedIncreasingSound.Play();
		}
	}

}
