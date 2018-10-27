using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionManager : MonoBehaviour {

	public GameObject eventManager;
	public GameObject questionInterface;
	public GameObject showQuestion;
	public GameObject[] showAnswers;
	public GameObject showTimer;

	public GameObject answerEnteredOne;
	public GameObject answerEnteredTwo;
	public GameObject sameAnswer;

	public GameObject reviewInterface;
	
	public Image questionTime;

	public Text questionsText;
	public Text[] statValues;

	public Slider oldScoreSlider;
	public Slider newScoreSlider;

	public AudioSource hissSound;
	public AudioSource radioIntroVoice;
	public AudioSource radioVoice;
	public AudioSource radioBGMusic;
	public AudioSource popupSound;
	public AudioSource startTimerSound;
	public AudioSource clockTickingSound;
	public AudioSource timeOverSound;
	public AudioSource answerOneSound;
	public AudioSource answerTwoSound;
	public AudioSource showIndicatorsSound;
	public AudioSource sameAnswerSound;
	public AudioSource notSameAnswerSound;
	public AudioSource increaseSound;
	public AudioSource finishedIncreasingSound;

	private bool firstVoice = false;
	private float waitDelay = 0;
	private float answerTime = 10.0f;
	private bool answeringOpen = false;
	private bool stillIncreasing = false;

	private int xDistance = 25;
	private int yDistance = 8;

	private int answerOne = 0;
	private int answerTwo = 0;
	private bool pOneAnswered = false;
	private bool pTwoAnswered = false;
	private bool answersMatch;

	private float getPoints = 5.0f;
	private float remainingTime;

	private float[] valuesArr = {0, 0, 0};
	private float currentValue;
	private float calculatedValue;

	private IEnumerator clockTicker;
	private IEnumerator increaseValuesCo;

	public static List<RadioQuestions.RadioQuestion> questionsArr;

	private int randomIndex;

	private Color defaultColorIndicator = new Color(1, 1, 1, 0.5f);
	private Vector2 indicatorOnePos;
	private Vector2 indicatorTwoPos;


	// private void Awake() {
	// 	eventManager.SetActive(false);

	// 	StartRadio();

	// 	increaseValuesCo = IncreaseValues();
	// }


	private void OnEnable() {
		// Save initial positions of indicators
		indicatorOnePos = new Vector2(
			answerEnteredOne.GetComponent<RectTransform>().anchoredPosition.x,
			answerEnteredOne.GetComponent<RectTransform>().anchoredPosition.y
		);
		indicatorTwoPos = new Vector2(
			answerEnteredTwo.GetComponent<RectTransform>().anchoredPosition.x,
			answerEnteredTwo.GetComponent<RectTransform>().anchoredPosition.y
		);

		eventManager.SetActive(false);

		StartRadio();

		increaseValuesCo = IncreaseValues();
	}


	private void Update() {
		if (answeringOpen) {
			RegisterInput();
		}

		if (GameManager.enableNavigation) {
			GetInput();
		}
	}


	private void GetInput() {
		if(GameManager.playerDark.GetButtonDown("X")) {
			if (stillIncreasing) {
				// Skip stats increasing animation
				SkipStatsAnim();
			} else {
				// Disable navigation
				GameManager.enableNavigation = false;

				// Reset all gameobjects, values and texts
				HardReset();

				// Get next random event
				eventManager.SetActive(true);
			}
		}
	}


	private void HardReset() {
		///////////
		// Reset all gameobjects, values and texts
		///////////

		// Reset all stat values
		for (int k = 0; k < statValues.Length; k++) {
			if (k < statValues.Length -1) {
				statValues[k].text = "0.00";
			} else {
				statValues[k].text = "+0.0%";
			}
			// Reset stats colors
			statValues[k].GetComponent<GradientText>().enabled = false;
		}

		// Reset all other bools
		stillIncreasing = false;
		GameManager.enableNavigation = false;
		answersMatch = false;
		answeringOpen = false;

		// Hide review interface popup
		reviewInterface.SetActive(false);

		// Hide "same answer" indicator
		sameAnswer.SetActive(false);

		// Reset answers match bool
		answersMatch = false;

		// Reset indicator positions
		answerEnteredOne.GetComponent<RectTransform>().anchoredPosition = indicatorOnePos;
		answerEnteredTwo.GetComponent<RectTransform>().anchoredPosition = indicatorTwoPos;

		// Hide player indicators
		answerEnteredOne.SetActive(false);
		answerEnteredTwo.SetActive(false);

		// Reset opacity of indicators
		answerEnteredOne.GetComponent<Image>().color = defaultColorIndicator;
		answerEnteredTwo.GetComponent<Image>().color = defaultColorIndicator;

		// Reset answers
		answerOne = 0;
		answerTwo = 0;
		pOneAnswered = false;
		pTwoAnswered = false;

		// Reset timer
		answerTime = 10.0f;
		questionTime.fillAmount = 1;

		// Hide timer
		showTimer.SetActive(false);

		// Hide answer popups
		for (int j = 0; j < showAnswers.Length; j++) {
			showAnswers[j].SetActive(false);
		}

		// Hide indicators for players
		answerEnteredOne.SetActive(false);
		answerEnteredTwo.SetActive(false);

		// Hide question popup
		showQuestion.SetActive(false);

		// Reset answer texts
		for (int i = 0; i < 4; i++) {
			showAnswers[i].transform.GetChild(0).GetComponent<Text>().text = "";
		}

		// Reset question text
		questionsText.text = "";

		// Hide whole Interface
		questionInterface.SetActive(false);

		///////////
		// Reset END
		///////////
	}


	private void SkipStatsAnim() {
		stillIncreasing = false;

		StopCoroutine(increaseValuesCo);

		for (int i = 0; i < statValues.Length; i++) {
			if (i < statValues.Length -1) {
				statValues[i].text = valuesArr[i].ToString("F2");
			} else {
				statValues[i].text = "+" + valuesArr[i].ToString("F1") + "%";
				newScoreSlider.value = oldScoreSlider.value + valuesArr[i];
			}

			// Color the increased value with a gradient
			statValues[i].GetComponent<GradientText>().enabled = true;
			finishedIncreasingSound.Play();
		}
	}
	

	public void StartRadio() {
		GameManager.enableNavigation = false;
		StartCoroutine(BootRadio());
	}


	IEnumerator BootRadio() {
		yield return new WaitForSeconds(0.5f);
		questionInterface.SetActive(true);
		hissSound.Play();
		yield return new WaitForSeconds(2.4f);
		ShowRadio();
	}


	private void ShowRadio() {
		radioBGMusic.Play();

		// Check if questions have been played before or not and play the according voice file
		if (!firstVoice) {
			// radioIntroVoice.Play();
			// waitDelay = 10.5f;
			waitDelay = 0.5f;
			firstVoice = true;
		} else {
			// radioVoice.Play();
			// waitDelay = 5.5f;
			waitDelay = 0.5f;
		}
		StartCoroutine(WaitForVoice(waitDelay));
	}


	IEnumerator WaitForVoice(float waitDelay) {
		yield return new WaitForSeconds(waitDelay);
		ShowQuestions();
	}


	private void ShowQuestions() {
		// Pick a random question from the array
		randomIndex = Random.Range(0, questionsArr.Count);

		// Write text for question
		questionsText.text = questionsArr[randomIndex].question;
		
		// Write texts for answers
		for (int i = 0; i < 4; i++) {
			showAnswers[i].transform.GetChild(0).GetComponent<Text>().text = questionsArr[randomIndex].answers[i];
			// answersText[i].text = questionsArr[randomIndex].answers[i];
		}
		// Remove the posed question from the array
		questionsArr.RemoveAt(randomIndex);
		
		// Show question
		popupSound.Play();
		showQuestion.SetActive(true);

		// Show inidcators for players
		answerEnteredOne.SetActive(true);
		answerEnteredTwo.SetActive(true);

		StartCoroutine(WaitForQuestion());
	}


	IEnumerator WaitForQuestion() {
		yield return new WaitForSeconds(3.0f);
		StartCoroutine(ShowAnswers());
	}


	IEnumerator ShowAnswers() {
		int popAnswers = 0;

		// Pop up all answers one after another
		while (popAnswers < 4) {
			showAnswers[popAnswers].SetActive(true);
			popupSound.Play();
			yield return new WaitForSeconds(0.25f);
			popAnswers++;
			yield return null;
		}

		yield return new WaitForSeconds(0.5f);

		// Display timer
		showTimer.SetActive(true);

		WaitForAnswer();
	}


	private void WaitForAnswer() {
		answeringOpen = true;
		StartCoroutine(StartTimer());

		startTimerSound.Play();

		clockTicker = ClockTick();
		StartCoroutine(clockTicker);
	}


	IEnumerator StartTimer() {
		while (answerTime > 0) {
			if (answeringOpen) {
				answerTime -= Time.deltaTime;
				questionTime.fillAmount = answerTime / 10;
			} else {
				answerTime = 0;
			}
			yield return null;
		}
		if (answeringOpen) {
			timeOverSound.Play();
			StopAnswering();
		}
	}


	IEnumerator ClockTick() {
		clockTickingSound.Play();
		yield return new WaitForSeconds(1.4f);
	}


	private void StopAnswering() {
		remainingTime = answerTime;
		answeringOpen = false;
		clockTickingSound.Stop();

		StartCoroutine(CompareAnswers());
	}


	private void RegisterInput() {
		// Check if player one answered
		if (!pOneAnswered) {
			if (GameManager.playerDark.GetButton("Square")) {
				answerOne = 1;
				OneAnswered();
			}
			if (GameManager.playerDark.GetButton("Triangle")) {
				answerOne = 2;
				OneAnswered();
			}
			if (GameManager.playerDark.GetButton("Circle")) {
				answerOne = 3;
				OneAnswered();
			}
			if (GameManager.playerDark.GetButton("X")) {
				answerOne = 4;
				OneAnswered();
			}
		}
		
		// Check if player two answered
		if (!pTwoAnswered) {
			if (GameManager.playerLight.GetButton("Square")) {
				answerTwo = 1;
				TwoAnswered();
			}
			if (GameManager.playerLight.GetButton("Triangle")) {
				answerTwo = 2;
				TwoAnswered();
			}
			if (GameManager.playerLight.GetButton("Circle")) {
				answerTwo = 3;
				TwoAnswered();
			}
			if (GameManager.playerLight.GetButton("X")) {
				answerTwo = 4;
				TwoAnswered();
			}
		}

		if (pOneAnswered && pTwoAnswered) {
			StopAnswering();
		}
	}


	private void OneAnswered() {
		valuesArr[0] = 10 - answerTime;
		pOneAnswered = true;
		answerOneSound.Play();
		answerEnteredOne.GetComponent<Image>().color = new Color(1, 1, 1, 1);
	}


	private void TwoAnswered() {
		valuesArr[1] = 10 - answerTime;
		pTwoAnswered = true;
		answerTwoSound.Play();
		answerEnteredTwo.GetComponent<Image>().color = new Color(1, 1, 1, 1);
	}


	IEnumerator CompareAnswers() {
		yield return new WaitForSeconds(1.0f);

		// Show indicator of player one
		if (answerOne > 0) {
			answerEnteredOne.SetActive(true);
			answerEnteredOne.GetComponent<RectTransform>().anchoredPosition = new Vector2(
				showAnswers[answerOne-1].GetComponent<RectTransform>().anchoredPosition.x - xDistance,
				showAnswers[answerOne-1].GetComponent<RectTransform>().anchoredPosition.y + yDistance
			);
			showIndicatorsSound.Play();
		}

		yield return new WaitForSeconds(0.2f);

		// Show indicator of player one
		if (answerTwo > 0) {
			answerEnteredTwo.SetActive(true);
			answerEnteredTwo.GetComponent<RectTransform>().anchoredPosition = new Vector2(
				showAnswers[answerTwo-1].GetComponent<RectTransform>().anchoredPosition.x + xDistance,
				showAnswers[answerTwo-1].GetComponent<RectTransform>().anchoredPosition.y + yDistance
			);
			showIndicatorsSound.Play();
		}

		yield return new WaitForSeconds(0.4f);

		// If both players have chosen the same answer they will get points
		if (answerOne != 0 && answerOne == answerTwo) {
			answersMatch = true;
			sameAnswer.SetActive(true);
			sameAnswer.GetComponent<RectTransform>().anchoredPosition = new Vector2(
				showAnswers[answerOne-1].GetComponent<RectTransform>().anchoredPosition.x,
				showAnswers[answerOne-1].GetComponent<RectTransform>().anchoredPosition.y + 26
			);
			sameAnswerSound.Play();
			yield return new WaitForSeconds(1.4f);
		} else {
			answersMatch = false;
			notSameAnswerSound.Play();
			yield return new WaitForSeconds(1.4f);
		}

		GetPoints();
	}


	private void GetPoints() {
		// Deactivate all question related gameobjects
		questionInterface.SetActive(false);

		// Setting the value for the friends score stat
		if (answersMatch) {
			float addPoints = getPoints + (remainingTime / 2);
			valuesArr[2] = addPoints;

			oldScoreSlider.value = GameManager.overallScore;
			GameManager.overallScore += addPoints;
		}

		// Activate the review popup
		reviewInterface.SetActive(true);
		
		// Call increase function
		StartCoroutine(increaseValuesCo);
	}


	IEnumerator IncreaseValues() {
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
				MapFunction(calculatedValue, 0, 1, 0, valuesArr[tempIndex]);

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
				statValues[tempIndex].text = valuesArr[tempIndex].ToString("F2");
			} else {
				statValues[tempIndex].text = "+" + valuesArr[tempIndex].ToString("F1") + "%";
				newScoreSlider.value = oldScoreSlider.value + valuesArr[tempIndex];
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


}
