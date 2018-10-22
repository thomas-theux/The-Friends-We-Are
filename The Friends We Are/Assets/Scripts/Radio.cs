using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Radio : MonoBehaviour {

	public GameObject hideStats;
	public GameObject radioInterface;
	public GameObject showQuestion;
	public GameObject showAnswers;
	public GameObject answerIndicatorOne;
	public GameObject answerIndicatorTwo;
	public GameObject sameAnswer;
	public Slider questionTimer;

	public Text questionsText;
	public Text answersText;

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

	private bool firstVoice = false;
	private float waitDelay = 0;
	private float answerTime = 10.0f;
	private bool answeringOpen = false;

	private int answerOne = 0;
	private int answerTwo = 0;
	private bool pOneAnswered = false;
	private bool pTwoAnswered = false;

	private IEnumerator clockTicker;

	public static List<string> questionsArr = new List<string>();
	public static List<string> answersArr = new List<string>();

	private int randomIndex;


	private void Update() {
		if (answeringOpen) {
			RegisterInput();
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
		yield return new WaitForSeconds(1.0f);
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

		clockTicker = ClockTick();
		StartCoroutine(clockTicker);
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
		answeringOpen = false;
		clockTicking.Stop();

		CompareAnswers();
	}


	private void RegisterInput() {
		// Check if player one answered
		if (!pOneAnswered) {
			if (GameManager.playerDark.GetButton("X")) {
				answerOne = 1;
				pOneAnswered = true;
				answerSoundOne.Play();
			}
			if (GameManager.playerDark.GetButton("Circle")) {
				answerOne = 2;
				pOneAnswered = true;
				answerSoundOne.Play();
			}
			if (GameManager.playerDark.GetButton("Square")) {
				answerOne = 3;
				pOneAnswered = true;
				answerSoundOne.Play();
			}
			if (GameManager.playerDark.GetButton("Triangle")) {
				answerOne = 4;
				pOneAnswered = true;
				answerSoundOne.Play();
			}
		}
		
		// Check if player two answered
		if (!pTwoAnswered) {
			if (GameManager.playerLight.GetButton("X")) {
				answerTwo = 1;
				pTwoAnswered = true;
				answerSoundTwo.Play();
			}
			if (GameManager.playerLight.GetButton("Circle")) {
				answerTwo = 2;
				pTwoAnswered = true;
				answerSoundTwo.Play();
			}
			if (GameManager.playerLight.GetButton("Square")) {
				answerTwo = 3;
				pTwoAnswered = true;
				answerSoundTwo.Play();
			}
			if (GameManager.playerLight.GetButton("Triangle")) {
				answerTwo = 4;
				pTwoAnswered = true;
				answerSoundTwo.Play();
			}
		}

		if (pOneAnswered && pTwoAnswered) {
			StopAnswering();
		}
	}


	private void CompareAnswers() {
		if (answerOne > 0) {
			answerIndicatorOne.SetActive(true);
			answerIndicatorOne.GetComponent<RectTransform>().anchoredPosition = new Vector2(
				answerIndicatorOne.GetComponent<RectTransform>().anchoredPosition.x,
				answerIndicatorOne.GetComponent<RectTransform>().anchoredPosition.y - ((answerOne-1) * 70)
			);
		}
		if (answerTwo > 0) {
			answerIndicatorTwo.SetActive(true);
			answerIndicatorTwo.GetComponent<RectTransform>().anchoredPosition = new Vector2(
				answerIndicatorTwo.GetComponent<RectTransform>().anchoredPosition.x,
				answerIndicatorTwo.GetComponent<RectTransform>().anchoredPosition.y - ((answerTwo-1) * 70)
			);
		}
		showIndicators.Play();

		if (answerOne == answerTwo) {
			sameAnswer.SetActive(true);
			sameAnswer.GetComponent<RectTransform>().anchoredPosition = new Vector2(
				sameAnswer.GetComponent<RectTransform>().anchoredPosition.x,
				sameAnswer.GetComponent<RectTransform>().anchoredPosition.y - ((answerOne-1) * 70)
			);
			sameAnswerSound.Play();
			GetPoints();
		} else {
			notSameAnswerSound.Play();
		}
	}


	private void GetPoints() {
		print("Friendscore increased!");
	}


}
