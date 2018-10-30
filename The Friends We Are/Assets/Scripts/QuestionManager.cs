using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class QuestionManager : MonoBehaviour {

	private QuestionReview questionReviewScript;

	public GameObject eventManager;
	public GameObject questionInterface;
	public GameObject showQuestion;
	public GameObject[] showAnswers;
	public GameObject showTimer;

	public GameObject answerEnteredOne;
	public GameObject answerEnteredTwo;
	public GameObject sameAnswer;

	// Include in RESETS
	public GameObject matchingIcon;
	public GameObject notMatchingIcon;
	public GameObject[] titles;
	public GameObject[] percentages;
	public GameObject summaryLine;
	public GameObject fsValue;
	// ////////////////

	public GameObject reviewInterface;
	
	public Image questionTime;

	public Text questionsText;
	public Text[] statValues;
	public Text totalPercentage;

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
	public AudioSource showStatSound;
	public AudioSource totalPercentageSound;

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
	private float[] remainingTimes = {0, 0};
	private int answersMatchInt = 0;

	private float[] valuesArr = {0, 0, 0};
	private float currentValue;
	private float calculatedValue;
	private float statsWait = 0.4f;

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
		// Get the QuestionReview script
		questionReviewScript = GetComponent<QuestionReview>();

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
				// Reload scene
				SceneManager.LoadScene(SceneManager.GetActiveScene().name);

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

		oldScoreSlider.value = newScoreSlider.value;

		// Reset all stat values
		for (int k = 0; k < statValues.Length; k++) {
			if (k < statValues.Length -1) {
				statValues[k].text = "";
			} else {
				statValues[k].text = "";
			}
			// Reset values
			valuesArr[k] = 0;
			// Reset stats colors
			// statValues[k].GetComponent<GradientText>().enabled = false;
		}

		// Hide titles
		for (int l = 0; l < titles.Length; l++) {
			titles[l].SetActive(false);
		}

		// Hide percentages
		for (int m = 0; m < percentages.Length; m++) {
			percentages[m].SetActive(false);
		}

		// Hide seconds suffix
		for (int n = 0; n < statValues.Length -1; n++) {
			statValues[n].transform.GetChild(0).gameObject.SetActive(false);
		}

		// Hide summary line
		summaryLine.SetActive(false);

		// Hide overall percentage
		fsValue.SetActive(false);

		// Hide matchings answers icon
		matchingIcon.SetActive(false);
		notMatchingIcon.SetActive(false);

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
				statValues[i].text = valuesArr[i].ToString("F1");
				statValues[i].transform.GetChild(0).gameObject.SetActive(true);
				titles[i+1].SetActive(true);
				percentages[i+1].SetActive(true);
			} else {
				statValues[i].text = "+" + valuesArr[i].ToString("F1");
				newScoreSlider.value = oldScoreSlider.value + valuesArr[i];
			}

			// Show icon according to if the answers match or not
			if (answersMatch) {
				matchingIcon.SetActive(true);
				sameAnswerSound.Play();
			} else {
				notMatchingIcon.SetActive(true);
				notSameAnswerSound.Play();
			}

			// Show title for answer match
			titles[0].SetActive(true);

			// Show percentage for answer match
			percentages[0].SetActive(true);

			// Show summary line
			summaryLine.SetActive(true);

			// Show overal percentage
			fsValue.SetActive(true);

			// Color the increased value with a gradient
			// statValues[i].GetComponent<GradientText>().enabled = true;
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
		} else {
			answersMatch = false;
			notSameAnswerSound.Play();
		}

		yield return new WaitForSeconds(1.4f);

		GetPoints();
	}


	private void GetPoints() {
		// Deactivate all question related gameobjects
		questionInterface.SetActive(false);

		// Setting the value for the friends score stat
		if (answersMatch) {
			remainingTimes[0] = (10 - valuesArr[0]) / 5;
			remainingTimes[1] = (10 - valuesArr[1]) / 5;

			float addPoints = getPoints + remainingTimes[0] + remainingTimes[1];
			valuesArr[2] = addPoints;

			oldScoreSlider.value = GameManager.overallScore;
			GameManager.overallScore += addPoints;
		}

		// Activate the review popup
		reviewInterface.SetActive(true);
		
		// Call increase function
		// StartCoroutine(increaseValuesCo);

		// Call script to calculate and review questions
		if (answersMatch) { answersMatchInt = 1; }
		else { answersMatchInt = 0; }

		questionReviewScript.OrganizeVariables(answersMatchInt, valuesArr[0], valuesArr[1]);

	}


	IEnumerator IncreaseValues() {
		yield return new WaitForSeconds(statsWait);

		// Enable navigation so players can skip if they like
		GameManager.enableNavigation = true;
		stillIncreasing = true;

		// Show if answers match or not first
		if (answersMatch) {
			matchingIcon.SetActive(true);
			sameAnswerSound.Play();
		} else {
			notMatchingIcon.SetActive(true);
			notSameAnswerSound.Play();
		}

		// Show title for matching answers
		yield return new WaitForSeconds(statsWait);
		titles[0].SetActive(true);
		showStatSound.Play();

		// Show percentage players get for having the same answer
		yield return new WaitForSeconds(statsWait);
		percentages[0].SetActive(true);
		showStatSound.Play();

		yield return new WaitForSeconds(statsWait);

		float tempTime = 0;
		float increaseTime = 1.0f;
		int tempIndex = 0;

		// Increase values as long as there's time left
		while (tempIndex < statValues.Length) {

			// Show the value that will be increased
			if (tempIndex < statValues.Length -1) {
				statValues[tempIndex].enabled = true;
				statValues[tempIndex].transform.GetChild(0).gameObject.SetActive(true);
			}

			while (tempTime < increaseTime) {
				tempTime += Time.deltaTime;

				MapFunction(tempTime, 0, increaseTime, 0, 1);
				ApplyFormula(currentValue);
				MapFunction(calculatedValue, 0, 1, 0, valuesArr[tempIndex]);

				if (tempIndex < statValues.Length -1) {
					statValues[tempIndex].text = currentValue.ToString("F1");
				} else {
					if (answersMatch) {
						statValues[tempIndex].text = "+" + currentValue.ToString("F1");
						newScoreSlider.value = oldScoreSlider.value + currentValue;
					} else {
						tempTime = increaseTime;
					}
				}

				increaseSound.Play();
				yield return null;
			}

			// Set the values to the real values
			if (tempIndex < statValues.Length -1) {
				statValues[tempIndex].text = valuesArr[tempIndex].ToString("F1");

				// Color the increased value with a gradient
				// statValues[tempIndex].GetComponent<GradientText>().enabled = true;
				finishedIncreasingSound.Play();
			} else {
				statValues[tempIndex].text = "+" + valuesArr[tempIndex].ToString("F1");
				newScoreSlider.value = oldScoreSlider.value + valuesArr[tempIndex];

				// If the answers are the same it will color it and play a positive sound
				if (answersMatch) {
					// Color the increased value with a gradient
					// statValues[tempIndex].GetComponent<GradientText>().enabled = true;
					finishedIncreasingSound.Play();
				} else {
					yield return new WaitForSeconds(0.3f);
					statValues[tempIndex].color = new Color(1, 0, 0, 1.0f);
					notSameAnswerSound.Play();
				}

				// After all stats increasing show the final overall percentage
				if (answersMatch) {
					yield return new WaitForSeconds(statsWait);
					totalPercentage.text = "" + Mathf.Round((oldScoreSlider.value + valuesArr[tempIndex]) * 10) / 10;
					totalPercentageSound.Play();
				}
			}

			if (tempIndex < statValues.Length -1) {
				// Show title for value
				yield return new WaitForSeconds(statsWait);
				titles[tempIndex+1].SetActive(true);
				showStatSound.Play();

				// Show percentage for values
				yield return new WaitForSeconds(statsWait);
				percentages[tempIndex+1].SetActive(true);
				percentages[tempIndex+1].GetComponent<Text>().text = "+" + Mathf.Round(remainingTimes[tempIndex] * 10) / 10;
				showStatSound.Play();

				yield return new WaitForSeconds(statsWait);
			}

			if (tempIndex == statValues.Length -2) {
				summaryLine.SetActive(true);
				fsValue.SetActive(true);
			}

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
