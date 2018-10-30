using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionReview : MonoBehaviour {

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

	private float currentValue = 0;
	private float calculatedValue = 0;
	private float tempScore = 0;
	private float totalScore = 0;

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


	private void Start() {
		showStatsCo = ShowStats();
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
				getPercentageValuesArr[0] = pointsForMatch;

				// Calculate and save the percentage each player gets for answering
				getPercentageValuesArr[i] = (10 - actualStatValuesArr[i]) / 5;
			} else {
				// Give 0 points because answers do not match
				actualStatGOArr[0].GetComponent<Image>().sprite = noMatchSprite;
				getPercentageValuesArr[i] = 0;
				print(getPercentageValuesArr[i]);
			}
		}

		// for (int i = 0; i < 3; i++) {
		// 	if (i == 0) {
		// 		// If the answers match, show the check icon and give 5 points
		// 		if (isMatching) {
		// 			actualStatGOArr[0].GetComponent<Image>().sprite = matchSprite;
		// 			getPercentageValuesArr[0] = pointsForMatch;
		// 		} else {
		// 			actualStatGOArr[0].GetComponent<Image>().sprite = noMatchSprite;
		// 			getPercentageValuesArr[0] = 0.0f;
		// 		}
		// 	} else {
		// 		// Calculate and save the percentage each player gets for answering
		// 		getPercentageValuesArr[i] = (10 - actualStatValuesArr[i]) / 5;
		// 	}
		// }

		// After calculating the percentage we start showing the stats
		StartCoroutine(showStatsCo);

	}


	IEnumerator ShowStats() {
		yield return new WaitForSeconds(medDelay);

		// Enable navigation so players can skip if they like
		GameManager.enableNavigation = true;
		isIncreasing = true;

		int tempIndex = 0;
		float tempTime = 0.0f;
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

					// Increase the slider value according to the percentage value
					totalScore = oldScoreSlider.value + tempScore + currentValue;
					newScoreSlider.value = totalScore;
					totalFriendsScoreGO.GetComponent<Text>().text = totalScore.ToString("F1");

					increaseSound.Play();
					yield return null;
				}

				finishedIncreasingSound.Play();
			} else {
				notMatchingSound.Play();
			}

			// Show final values and save temporary score after each increasing
			float finalValue = (Mathf.Round(getPercentageValuesArr[tempIndex] * 10) / 10);
			tempScore += finalValue;
			getPercentageGOArr[tempIndex].GetComponent<Text>().text = finalValue.ToString("F1");
			totalFriendsScoreGO.GetComponent<Text>().text = (Mathf.Round(totalScore * 10) / 10).ToString("F1");

			yield return new WaitForSeconds(medDelay);

			tempTime = 0;
			tempIndex++;
		}
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

}
