using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowStats : MonoBehaviour {

	public Text text01;
	public Text text02;
	public Text text03;
	public Text text04;

	public Text[] showValues;
	private int index = 0;

	private float increaseTime = 1.4f;
	private float tempTime = 0;
	private float currentValue = 0;
	private float calculatedValue = 0;
	private float[] increasingValues = {0, 0, 0, 0};


	private void Start() {
		text01.text = StatsManager.transferredText01;
		text02.text = StatsManager.transferredText02;
		text03.text = StatsManager.transferredText03;
		text04.text = StatsManager.transferredText04;

		StartCoroutine(IncreaseValue());
	}


	IEnumerator IncreaseValue() {
		yield return new WaitForSeconds(1);

		while (index < 4) {
			while (tempTime < increaseTime) {
				tempTime += Time.deltaTime;
				MapFunction(tempTime, 0, increaseTime, 0, 1);
				ApplyFormula(currentValue);
				MapFunction(calculatedValue, 0, 1, 0, StatsManager.transferValues[index]);
				increasingValues[index] = currentValue;

				showValues[index].text = Mathf.Floor(increasingValues[index]) + "";

				yield return null;
			}
			tempTime = 0;
			index++;

			yield return new WaitForSeconds(0.2f);
		}

		index = 3;
	}


	private void ApplyFormula(float currentValue) {
		// Formula: y = (x-1)^3 + 1
		calculatedValue = Mathf.Pow(currentValue, 1);
	}


	private void MapFunction(float number, float oldMin, float oldMax, float newMin, float newMax) {
		currentValue = newMin + (newMax - newMin) * (number - oldMin) / (oldMax - oldMin);
	}

}
