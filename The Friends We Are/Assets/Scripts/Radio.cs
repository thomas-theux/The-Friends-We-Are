using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Radio : MonoBehaviour {

	public GameObject hideStats;
	public GameObject radioInterface;
	public GameObject showQuestion;
	public GameObject showAnswers;
	public GameObject answerEnteredOne;
	public GameObject answerEnteredTwo;
	public GameObject answerIndicatorOne;
	public GameObject answerIndicatorTwo;
	public GameObject sameAnswer;
<<<<<<< HEAD
	public GameObject questionsFriendsScore;
	// public Slider questionTimer;
	public Image questionTime;
	public Slider oldScore;
	public Slider newScore;

	public Text questionsText;
	public Text[] answersText;
	public Text friendsScoreValue;
=======
	public Slider questionTimer;

	public Text questionsText;
	public Text answersText;
>>>>>>> parent of e564554... Improved radio interface and reworked displaying answers

	public AudioSource hissSound;
	public AudioSource radioVoiceIntro;
	public AudioSource radioVoice;
	public AudioSource radioBG;
	public AudioSource popupSound;
	public AudioSource clockTicking;
	public AudioSource timeOver;
	public AudioSource answerSoundOne;
	public AudioSource answerSoundTwo;
	public AudioSource showIndicators;
	public AudioSource sameAnswerSound;
	public AudioSource notSameAnswerSound;
	public AudioSource increaseValue;

	private bool firstVoice = false;
	private float waitDelay = 0;
	private float answerTime = 10.0f;
	private bool answeringOpen = false;
	private bool questionOver = false;

	private int answerOne = 0;
	private int answerTwo = 0;
	private bool pOneAnswered = false;
	private bool pTwoAnswered = false;

	private float getPoints = 5.0f;
	private float remainingTime;
	private float addPoints;
	private float oldScoreValue;
	private float newScoreValue;

	private float tempTime = 0;
	private float increaseTime = 2.0f;
	private float currentValue = 0;
	private float calculatedValue = 0;

	private IEnumerator clockTickerCo;
	private IEnumerator increaseValueCo;

	public static List<string> questionsArr = new List<string>();
	public static List<string> answersArr = new List<string>();

	private int randomIndex;


	private void Update() {
		if (answeringOpen) {
			RegisterInput();
		}

		if (GameManager.enableNavigation && questionOver) {
			if(GameManager.playerDark.GetButtonDown("X")) {
				StopCoroutine(increaseValueCo);

				newScore.value = newScoreValue;
				friendsScoreValue.text = "+" + newScoreValue.ToString("F1") + "%";

				sameAnswerSound.Play();
			}
		}
	}


	public void StartRadio() {
		GameManager.enableNavigation = false;
		hideStats.SetActive(false);
		StartCoroutine(BootRadio());
	}


	IEnumerator BootRadio() {
		yield return new WaitForSeconds(0.5f);
		radioInterface.SetActive(true);
		hissSound.Play();
		yield return new WaitForSeconds(2.4f);
		ShowRadio();
	}


	private void ShowRadio() {
		radioBG.Play();
		if (!firstVoice) {
			// radioVoiceIntro.Play();
			waitDelay = 0.5f;
			firstVoice = true;
		} else {
			radioVoice.Play();
			waitDelay = 5.5f;
		}
		StartCoroutine(WaitForVoice(waitDelay));
	}


	IEnumerator WaitForVoice(float waitDelay) {
		yield return new WaitForSeconds(waitDelay);
		ShowQuestions();
	}


	private void ShowQuestions() {
		// Pick random question from array
		randomIndex = Random.Range(0, questionsArr.Count);
		questionsText.text = questionsArr[randomIndex];
		questionsArr.RemoveAt(randomIndex);
		
		popupSound.Play();
		showQuestion.SetActive(true);

		StartCoroutine(WaitForQuestion());
	}


	IEnumerator WaitForQuestion() {
		yield return new WaitForSeconds(3.0f);
		ShowAnswers();
	}


	private void ShowAnswers() {
		popupSound.Play();
		showAnswers.SetActive(true);
		answersText.text = answersArr[randomIndex];
		StartCoroutine(WaitForAnswer());
	}


	IEnumerator WaitForAnswer() {
		yield return new WaitForSeconds(1.0f);

		answeringOpen = true;
		StartCoroutine(StartTimer());

		clockTickerCo = ClockTick();
		StartCoroutine(clockTickerCo);
	}


	IEnumerator StartTimer() {
		while (answerTime > 0) {
			if (answeringOpen) {
				answerTime -= Time.deltaTime;
				questionTimer.value = answerTime * 10;
			} else {
				answerTime = 0;
			}
			yield return null;
		}
		if (answeringOpen) {
			timeOver.Play();
			StopAnswering();
		}
	}


	IEnumerator ClockTick() {
		clockTicking.Play();
		yield return new WaitForSeconds(1.4f);
	}


	private void StopAnswering() {
		remainingTime = answerTime;
		answeringOpen = false;
		clockTicking.Stop();

		StartCoroutine(CompareAnswers());
	}


	private void RegisterInput() {
		// Check if player one answered
		if (!pOneAnswered) {
			if (GameManager.playerDark.GetButton("X")) {
				answerOne = 1;
				OneAnswered();
			}
			if (GameManager.playerDark.GetButton("Circle")) {
				answerOne = 2;
				OneAnswered();
			}
			if (GameManager.playerDark.GetButton("Square")) {
				answerOne = 3;
				OneAnswered();
			}
			if (GameManager.playerDark.GetButton("Triangle")) {
				answerOne = 4;
				OneAnswered();
			}
		}
		
		// Check if player two answered
		if (!pTwoAnswered) {
			if (GameManager.playerLight.GetButton("X")) {
				answerTwo = 1;
				TwoAnswered();
			}
			if (GameManager.playerLight.GetButton("Circle")) {
				answerTwo = 2;
				TwoAnswered();
			}
			if (GameManager.playerLight.GetButton("Square")) {
				answerTwo = 3;
				TwoAnswered();
			}
			if (GameManager.playerLight.GetButton("Triangle")) {
				answerTwo = 4;
				TwoAnswered();
			}
		}

		if (pOneAnswered && pTwoAnswered) {
			StopAnswering();
		}
	}


	private void OneAnswered() {
		pOneAnswered = true;
		answerSoundOne.Play();
		answerEnteredOne.GetComponent<Image>().color = new Color(1, 1, 1, 1);
	}


	private void TwoAnswered() {
		pTwoAnswered = true;
		answerSoundTwo.Play();
		answerEnteredTwo.GetComponent<Image>().color = new Color(1, 1, 1, 1);
	}


	IEnumerator CompareAnswers() {
		yield return new WaitForSeconds(0.5f);

		// Show indicator of player one
		if (answerOne > 0) {
			answerIndicatorOne.SetActive(true);
			answerIndicatorOne.GetComponent<RectTransform>().anchoredPosition = new Vector2(
				answerIndicatorOne.GetComponent<RectTransform>().anchoredPosition.x,
				answerIndicatorOne.GetComponent<RectTransform>().anchoredPosition.y - ((answerOne-1) * 70)
			);
			showIndicators.Play();
		}

		yield return new WaitForSeconds(0.2f);

		// Show indicator of player one
		if (answerTwo > 0) {
			answerIndicatorTwo.SetActive(true);
			answerIndicatorTwo.GetComponent<RectTransform>().anchoredPosition = new Vector2(
				answerIndicatorTwo.GetComponent<RectTransform>().anchoredPosition.x,
				answerIndicatorTwo.GetComponent<RectTransform>().anchoredPosition.y - ((answerTwo-1) * 70)
			);
			showIndicators.Play();
		}

		yield return new WaitForSeconds(0.4f);

		// If both players have chosen the same answer they will get points
		if (answerOne != 0 && answerOne == answerTwo) {
			sameAnswer.SetActive(true);
			sameAnswer.GetComponent<RectTransform>().anchoredPosition = new Vector2(
				sameAnswer.GetComponent<RectTransform>().anchoredPosition.x,
				sameAnswer.GetComponent<RectTransform>().anchoredPosition.y - ((answerOne-1) * 70)
			);
			sameAnswerSound.Play();
			yield return new WaitForSeconds(0.5f);
			GetPoints();
		} else {
			notSameAnswerSound.Play();
		}
	}


	private void GetPoints() {
		addPoints = getPoints + (remainingTime / 2);

		oldScoreValue = GameManager.overallScore;
		newScoreValue = GameManager.overallScore + addPoints;
		questionsFriendsScore.SetActive(true);
		oldScore.value = GameManager.overallScore;

		increaseValueCo = IncreaseScore(oldScoreValue, newScoreValue);
		StartCoroutine(increaseValueCo);

		GameManager.enableNavigation = true;
		questionOver = true;

		// Save new friends score to overallScore
		GameManager.overallScore = newScore.value;
	}


	IEnumerator IncreaseScore(float oldScoreValue, float newScoreValue) {
		yield return new WaitForSeconds(1.0f);

		while (tempTime < increaseTime) {
			tempTime += Time.deltaTime;

			// Calculate value for slider
			MapFunction(tempTime, 0, increaseTime, 0, 1);
			ApplyFormula(currentValue);
			MapFunction(calculatedValue, 0, 1, oldScoreValue, newScoreValue);

			// Display calculated value on slider
			newScore.value = currentValue;

			// Calculate value for text field
			MapFunction(tempTime, 0, increaseTime, 0, 1);
			ApplyFormula(currentValue);
			MapFunction(calculatedValue, 0, 1, 0, addPoints);

			// Display calculated value in text field
			friendsScoreValue.text = "+" + currentValue.ToString("F1") + "%";

			increaseValue.Play();
			yield return null;
		}

		sameAnswerSound.Play();
		print("FERTICH");
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
