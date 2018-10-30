﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionReview : MonoBehaviour {

	public GameObject eventManagerGO;
	public GameObject reviewInterfaceGO;
	public GameObject reviewPopup;

	public GameObject[] statTitleGOArr;
	public GameObject[] actualStatGOArr;
	public GameObject[] getPercentageGOArr;

	public GameObject[] statSuffixGOArr;
	public GameObject[] percentageSuffixGOArr;
	public GameObject totalFriendsScoreGO;

	public Sprite matchSprite;
	public Sprite noMatchSprite;

	public float[] actualStatValuesArr = {0, 0, 0};
	public float[] getPercentageValuesArr = {0, 0, 0};
	private float pointsForMatch = 5.0f;

	private int tempIndex = 0;
	private float tempTime = 0.0f;
	private float currentValue = 0;
	private float calculatedValue = 0;
	private float tempScore = 0;
	private float totalScore = 0;
	private float addPoints = 0;

	public Slider oldScoreSlider;
	public Slider newScoreSlider;

	private bool isMatching = false;
	private bool isIncreasing = false;

	private IEnumerator showStatsCo;

	public AudioSource increaseSound;
	public AudioSource finishedIncreasingSound;
	public AudioSource showStatSound;
	public AudioSource notMatchingSound;

	private float shortDelay = 0.3f;
	private float medDelay = 0.5f;
	// private float longDelay = 1.0f;


	private void OnEnable() {
		showStatsCo = ShowStats();
	}


	private void Update() {
		// Check if the navigation is enabled yet
		if (GameManager.enableNavigation) {
			SkipAnimations();
		}
	}


	public void OrganizeVariables(float matchingAnswers, float answerTimeOne, float answerTimeTwo) {
		actualStatValuesArr[0] = matchingAnswers;
		actualStatValuesArr[1] = answerTimeOne;
		actualStatValuesArr[2] = answerTimeTwo;

		if (actualStatValuesArr[0] == 1) {
			isMatching = true;
		} else {
			isMatching = false;
		}

		CalculatePercentage();
	}


	private void CalculatePercentage() {

		// Show review popup
		reviewPopup.SetActive(true);
		
		// Calculate and save the percentages for all 3 values
		for (int i = 0; i < statTitleGOArr.Length; i++) {
			// If the answers match, show the check icon and give 5 points
			if (isMatching) {
				actualStatGOArr[0].GetComponent<Image>().sprite = matchSprite;
				float tempPercentage = Mathf.Floor(pointsForMatch * 10) / 10;
				getPercentageValuesArr[0] = tempPercentage;

				// Calculate and save the percentage each player gets for answering
				tempPercentage = Mathf.Floor(((10 - actualStatValuesArr[i]) / 5) * 10) / 10;
				getPercentageValuesArr[i] = tempPercentage;
			} else {
				// Give 0 points because answers do not match
				actualStatGOArr[0].GetComponent<Image>().sprite = noMatchSprite;
				getPercentageValuesArr[i] = 0;
			}
		}

		// After calculating the percentage we start showing the stats
		StartCoroutine(showStatsCo);

	}


	private void SkipAnimations() {
		// Check if the player hits the X button
		if (GameManager.playerDark.GetButtonDown("X")) {
			// Check if the animation is ongoing or not
			if (isIncreasing) {
				StopCoroutine(showStatsCo);
				ShowFinalValues();
			} else {
				// Save score to overallScore
				for (int m = 0; m < getPercentageValuesArr.Length; m++) {
					addPoints += getPercentageValuesArr[m];
				}
				GameManager.overallScore += addPoints;

				// Disable navigation
				GameManager.enableNavigation = false;

				// Reset all values
				ResetValues();

				// Call next random event
				eventManagerGO.SetActive(true);
			}
		}
	}


	IEnumerator ShowStats() {
		yield return new WaitForSeconds(medDelay);

		// Enable navigation so players can skip if they like
		GameManager.enableNavigation = true;
		isIncreasing = true;

		float increaseTime = 1.0f;

		while (tempIndex < statTitleGOArr.Length) {

			// Show the title of the stat
			statTitleGOArr[tempIndex].SetActive(true);
			showStatSound.Play();

			yield return new WaitForSeconds(shortDelay);

			// Show the actual stat and their suffix (like 's' for seconds)
			actualStatGOArr[tempIndex].SetActive(true);
			statSuffixGOArr[tempIndex].SetActive(true);
			if (tempIndex != 0) {
				actualStatGOArr[tempIndex].GetComponent<Text>().text = actualStatValuesArr[tempIndex].ToString("F1");
			}
			showStatSound.Play();

			yield return new WaitForSeconds(shortDelay);

			// Show the percentage value and their suffix (like % for percentage)
			getPercentageGOArr[tempIndex].SetActive(true);
			percentageSuffixGOArr[tempIndex].SetActive(true);

			// Increase the percentage value the players get for succeeding
			if (isMatching) {
				while (tempTime < increaseTime) {
					tempTime += Time.deltaTime;

					MapFunction(tempTime, 0, increaseTime, 0, 1);
					ApplyFormula(currentValue);
					MapFunction(calculatedValue, 0, 1, 0, getPercentageValuesArr[tempIndex]);

					getPercentageGOArr[tempIndex].GetComponent<Text>().text = currentValue.ToString("F1");

					totalScore = oldScoreSlider.value + currentValue + tempScore;
					newScoreSlider.value = totalScore;
					totalFriendsScoreGO.GetComponent<Text>().text = "" + totalScore.ToString("F1");

					increaseSound.Play();
					yield return null;
				}

				finishedIncreasingSound.Play();
			} else {
				notMatchingSound.Play();
			}

			float getPercentage = getPercentageValuesArr[tempIndex];
			getPercentageGOArr[tempIndex].GetComponent<Text>().text = "" + getPercentage.ToString("F1");
			newScoreSlider.value = oldScoreSlider.value + getPercentage + tempScore;
			totalFriendsScoreGO.GetComponent<Text>().text = "" + newScoreSlider.value.ToString("F1");
			tempScore += getPercentage;

			yield return new WaitForSeconds(medDelay);

			tempIndex++;
			tempTime = 0;
		}

		// Stop the possibility to skip the animation
		isIncreasing = false;
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

	
	private void ShowFinalValues() {
		// The animation is stopped
		isIncreasing = false;

		// Calculate the remaining stats that have to be shown
		int repeatShow = statTitleGOArr.Length - tempIndex;

		for (int j = tempIndex; j < repeatShow; j++) {
			// Show the title of the stat
			statTitleGOArr[j].SetActive(true);

			// Show the actual stat and their suffix (like 's' for seconds)
			actualStatGOArr[j].SetActive(true);
			statSuffixGOArr[j].SetActive(true);
			if (j != 0) {
				actualStatGOArr[j].GetComponent<Text>().text = actualStatValuesArr[j].ToString("F1");
			}

			// Show all percentages
			getPercentageGOArr[j].SetActive(true);
			percentageSuffixGOArr[j].SetActive(true);

			// Show final values and save temporary score after each increasing
			float finalValue = getPercentageValuesArr[j];
			tempScore += finalValue;
			getPercentageGOArr[j].GetComponent<Text>().text = finalValue.ToString("F1");
			totalFriendsScoreGO.GetComponent<Text>().text = (GameManager.overallScore + tempScore).ToString("F1");
			newScoreSlider.value = tempScore + oldScoreSlider.value;
		}

		// Play corresponding sounds
		if (isMatching) {
			finishedIncreasingSound.Play();
		} else {
			notMatchingSound.Play();
		}
	}

	
	private void ResetValues() {
		oldScoreSlider.value = GameManager.overallScore;
		newScoreSlider.value = 0;

		for (int k = 0; k < statTitleGOArr.Length; k++) {
			if (k != 0) { actualStatGOArr[k].GetComponent<Text>().text = ""; }
			getPercentageGOArr[k].GetComponent<Text>().text = "";
			actualStatValuesArr[k] = 0;
			getPercentageValuesArr[k] = 0;
		}

		tempIndex = 0;
		tempTime = 0.0f;
		currentValue = 0;
		calculatedValue = 0;
		tempScore = 0;
		totalScore = 0;
		addPoints = 0;

		isMatching = false;
		isIncreasing = false;

		reviewPopup.SetActive(false);
		
		for (int l = 0; l < statTitleGOArr.Length; l++) {
			statTitleGOArr[l].SetActive(false);

			actualStatGOArr[l].SetActive(false);
			statSuffixGOArr[l].SetActive(false);

			getPercentageGOArr[l].SetActive(false);
			percentageSuffixGOArr[l].SetActive(false);
		}
		
		reviewInterfaceGO.SetActive(false);
	}

}
