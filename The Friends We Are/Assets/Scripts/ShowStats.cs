using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowStats : MonoBehaviour {

	public Text text01;
	public Text text02;
	public Text text03;
	public Text text04;

	public AudioSource increaseValue;
	public AudioSource finishedIncreasing;

	public Text[] showValues;
	public Text[] showTexts;
	private int index = 0;

	private float increaseTime = 1.0f;
	private float tempTime = 0;
	private float currentValue = 0;
	private float calculatedValue = 0;
	private float[] increasingValues = {0, 0, 0, 0};


	private void Start() {
		// Show text in UI that got transferred from the Story
		for (int i = 0; i < 4; i++) {
			showTexts[i].text = StatsManager.transferTexts[i];
		}

		StartCoroutine(IncreaseValue());
	}


	// Show the single stats by animating them starting from 0
	IEnumerator IncreaseValue() {
		yield return new WaitForSeconds(1);

		while (index < 4) {
			while (tempTime < increaseTime) {
				tempTime += Time.deltaTime;
				MapFunction(tempTime, 0, increaseTime, 0, 1);
				ApplyFormula(currentValue);
				MapFunction(calculatedValue, 0, 1, 0, StatsManager.transferValues[index]);
				increasingValues[index] = currentValue;

				if (increasingValues[index] < 10) {
					showValues[index].text = "0" + Mathf.Floor(increasingValues[index]);
				} else {
					showValues[index].text = Mathf.Floor(increasingValues[index]) + "";
				}
				increaseValue.Play();

				yield return null;
			}
			finishedIncreasing.Play();

			tempTime = 0;
			index++;

			// yield return new WaitForSeconds(0.05f);
		}

		index = 3;
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

}
