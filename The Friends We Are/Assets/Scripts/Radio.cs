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
			radioVoiceIntro.Play();
			waitDelay = 10.5f;
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
		yield return new WaitForSeconds(3.0f);

		StartCoroutine(StartTimer());
		answeringOpen = true;

		clockTicker = ClockTick();
		StartCoroutine(clockTicker);
	}


	IEnumerator StartTimer() {
		while(answerTime > 0) {
			answerTime -= Time.deltaTime;
			questionTimer.value = answerTime * 10;
			yield return null;
		}
		timeOver.Play();
		StopAnswering();
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
				answerOne = 1;
				pTwoAnswered = true;
				answerSoundTwo.Play();
			}
			if (GameManager.playerLight.GetButton("Circle")) {
				answerOne = 2;
				pTwoAnswered = true;
				answerSoundTwo.Play();
			}
			if (GameManager.playerLight.GetButton("Square")) {
				answerOne = 3;
				pTwoAnswered = true;
				answerSoundTwo.Play();
			}
			if (GameManager.playerLight.GetButton("Triangle")) {
				answerOne = 4;
				pTwoAnswered = true;
				answerSoundTwo.Play();
			}
		}

		if (pOneAnswered && pTwoAnswered) {
			StopAnswering();
		}
	}


	private void CompareAnswers() {
		answerIndicatorOne.SetActive(true);
		answerIndicatorOne.transform.position = new Vector3(answerIndicatorOne.transform.position.x, -39 - ((answerOne-1) * 70), 0);
		answerIndicatorTwo.SetActive(true);
		answerIndicatorOne.transform.position = new Vector3(answerIndicatorTwo.transform.position.x, -39 - ((answerTwo-1) * 70), 0);
		showIndicators.Play();

		if (answerOne == answerTwo) {
			sameAnswer.SetActive(true);
			sameAnswer.transform.position = new Vector3(sameAnswer.transform.position.x, -33 - ((answerOne-1) * 70), 0);
			sameAnswerSound.Play();
			GetPoints();
		} else {
			notSameAnswerSound.Play();
		}
	}


	private void GetPoints() {

	}


}
