using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Radio : MonoBehaviour {

	public GameObject hideStats;
	public GameObject radioInterface;
	public GameObject showQuestion;
	public GameObject showAnswers;
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

	private bool firstVoice = false;
	private float waitDelay = 0;
	private float answerTime = 10.0f;

	private IEnumerator clockTicker;

	private List<string> questionsArr = new List<string>();
	private List<string> answersArr = new List<string>();

	private int randomIndex;



	private void Start() {

		questionsArr.AddRange(new string[] {
			"If you find out that a very close friend has incurable cancer – what would you do?",
			"twentyfour"
		});

		answersArr.AddRange(new string[] {
			"Spend more time with her/him." + "\n" +
			"Don’t spend any time anymore." + "\n" +
			"Slowly distance myself from her/him." + "\n" +
			"I would not change anything.",

			"twentyfour" + "\n" +
			"twentyfour" + "\n" +
			"twentyfour" + "\n" +
			"twentyfour"
		});
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

		clockTicker = ClockTick();
		StartCoroutine(clockTicker);
	}


	IEnumerator StartTimer() {
		while(answerTime > 0) {
			answerTime -= Time.deltaTime;
			questionTimer.value = answerTime * 10;
			yield return null;
		}
		// StopCoroutine(clockTicker);
		clockTicking.Stop();
		timeOver.Play();

		CompareAnswers();
	}


	IEnumerator ClockTick() {
		clockTicking.Play();
		yield return new WaitForSeconds(1.4f);
	}


	private void RegisterInput() {

	}


	private void CompareAnswers() {
		
	}


	private void GetPoints() {

	}


}
