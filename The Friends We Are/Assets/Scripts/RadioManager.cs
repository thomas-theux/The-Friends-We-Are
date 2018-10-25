using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadioManager : MonoBehaviour {

	// public GameObject statsInterface;
	public GameObject questionInterface;
	public GameObject showQuestion;

	public GameObject[] showAnswers;

	public GameObject showTimer;

	public GameObject answerEnteredOne;
	public GameObject answerEnteredTwo;
	public GameObject sameAnswer;
	
	public Image questionTime;

	public Text questionsText;

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

	private bool firstVoice = false;
	private float waitDelay = 0;
	private float answerTime = 10.0f;
	private bool answeringOpen = false;

	private int xDistance = 25;
	private int yDistance = 8;

	private int answerOne = 0;
	private int answerTwo = 0;
	private bool pOneAnswered = false;
	private bool pTwoAnswered = false;

	private float getPoints = 5.0f;
	private float remainingTime;

	private IEnumerator clockTicker;

	public static List<RadioQuestions.RadioQuestion> questionsArr;

	private int randomIndex;


	private void Start() {
		StartRadio();
	}


	private void Update() {
		if (answeringOpen) {
			RegisterInput();
		}
		// print(questionsText.cachedTextGenerator.lineCount);
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
			yield return new WaitForSeconds(0.2f);
			popAnswers++;
			yield return null;
		}

		// Display timer
		showTimer.SetActive(true);

		StartCoroutine(WaitForAnswer());
	}


	IEnumerator WaitForAnswer() {
		yield return new WaitForSeconds(0.3f);

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
				// questionTimer.value = answerTime * 10;
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
		pOneAnswered = true;
		answerOneSound.Play();
		answerEnteredOne.GetComponent<Image>().color = new Color(1, 1, 1, 1);
	}


	private void TwoAnswered() {
		pTwoAnswered = true;
		answerTwoSound.Play();
		answerEnteredTwo.GetComponent<Image>().color = new Color(1, 1, 1, 1);
	}


	IEnumerator CompareAnswers() {
		yield return new WaitForSeconds(0.5f);

		// Show indicator of player one
		if (answerOne > 0) {
			answerEnteredOne.SetActive(true);
			answerEnteredOne.GetComponent<RectTransform>().anchoredPosition = new Vector2(
				showAnswers[answerOne-1].GetComponent<RectTransform>().anchoredPosition.x - xDistance,
				showAnswers[answerOne-1].GetComponent<RectTransform>().anchoredPosition.y + yDistance
				// answerEnteredOne.GetComponent<RectTransform>().anchoredPosition.x,
				// answerEnteredOne.GetComponent<RectTransform>().anchoredPosition.y - ((answerOne-1) * 70)
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
			sameAnswer.SetActive(true);
			sameAnswer.GetComponent<RectTransform>().anchoredPosition = new Vector2(
				showAnswers[answerOne-1].GetComponent<RectTransform>().anchoredPosition.x,
				showAnswers[answerOne-1].GetComponent<RectTransform>().anchoredPosition.y + 26
			);
			sameAnswerSound.Play();
			yield return new WaitForSeconds(0.5f);
			GetPoints();
		} else {
			notSameAnswerSound.Play();
		}
	}


	private void GetPoints() {
		float addPoints = getPoints + (remainingTime / 2);
		print("Score increased by " + addPoints + " points!");

		GameManager.overallScore += addPoints;
	}


}
