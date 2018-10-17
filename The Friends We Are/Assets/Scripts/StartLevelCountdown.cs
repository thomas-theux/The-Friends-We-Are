using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartLevelCountdown : MonoBehaviour {

	public Text levelStartCountdownText;
	public AudioSource countdownSound;
	public AudioSource goSound;

	public GameObject burnout;

	private float levelStartCountdownTime = 3;
	private bool startCountdown = false;
	private int beepAmount = 3;

	public static bool startLevel = false;


	private void Start() {
		StartCoroutine(StartCountdownDelay());
	}


	private void Update() {
		if (startCountdown) {
			CountDown();
		}
	}


	// Initially wait 1 second before countdown starts
	IEnumerator StartCountdownDelay() {
		yield return new WaitForSeconds(1);
		startCountdown = true;
		StartCoroutine(CounterBeep());
	}


	// Countdown until the level begins
	private void CountDown() {
		if (levelStartCountdownTime > 0.01f) {
			levelStartCountdownTime -= Time.deltaTime;
			levelStartCountdownText.text = Mathf.Ceil(levelStartCountdownTime) + "";
		} else {
			levelStartCountdownTime = 1;
			levelStartCountdownText.text = "GO!";
			StartCoroutine(DeleteTextDelay());
			startLevel = true;
			startCountdown = false;
		}
	}


	// Countdown sound
	IEnumerator CounterBeep() {
		while (beepAmount > 0) {
			countdownSound.Play();
			beepAmount--;
			yield return new WaitForSeconds(1);
		}
	}


	// Show GO! message for another 1 seconds before disabling it
	IEnumerator DeleteTextDelay() {
		// yield return new WaitForSeconds(0.1f);
		Instantiate(burnout, new Vector3(6, 0, -2), burnout.transform.rotation);
		Instantiate(burnout, new Vector3(4, 0, -2), burnout.transform.rotation);
		goSound.Play();
		yield return new WaitForSeconds(1);
		levelStartCountdownText.enabled = false;
	}

}
