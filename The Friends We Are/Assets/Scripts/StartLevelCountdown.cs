using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartLevelCountdown : MonoBehaviour {

	public Text levelStartCountdownText;
	public AudioSource countdownSound;

	private float levelStartCountdownTime = 3;
	private bool stopCountdown = false;

	public static bool startLevel = false;


	private void Start() {
		StartCoroutine(CounterBeep());
	}


	private void Update() {
		CountDown();
	}


	private void CountDown() {
		if (!stopCountdown) {
			if (levelStartCountdownTime > 0.1f) {
				levelStartCountdownTime -= Time.deltaTime;
				levelStartCountdownText.text = Mathf.Ceil(levelStartCountdownTime) + "";
			} else {
				levelStartCountdownTime = 0;
				levelStartCountdownText.text = "GO!";
				StartCoroutine(DeleteTextDelay());
				startLevel = true;
				stopCountdown = true;
			}
		}
	}


	IEnumerator CounterBeep() {
		while (levelStartCountdownTime > 0.1f) {
			yield return new WaitForSeconds(1);
			countdownSound.Play();
		}
	}


	IEnumerator DeleteTextDelay() {
		yield return new WaitForSeconds(2);
		levelStartCountdownText.enabled = false;
	}

}
