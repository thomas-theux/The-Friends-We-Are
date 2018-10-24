using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HonkHorns : MonoBehaviour {

	public AudioSource hornSound;
	private bool isHonking = false;
	private bool honkManually = false;


	private void Update() {
		if (LevelTimer.levelEnd) {
			if (!isHonking) {
				StartCoroutine(Honking());
			}
		}

		if (GameManager.playerDark.GetButtonDown("R1") && !honkManually) {
			StartCoroutine(HonkManually());
		}
	}


	IEnumerator HonkManually() {
		honkManually = true;
		hornSound.Play();
		yield return new WaitForSeconds(0.5f);
		honkManually = false;
	}


	IEnumerator Honking() {
		isHonking = true;
		float hornDelay = Random.Range(10, 30);
		yield return new WaitForSeconds(hornDelay);
		hornSound.Play();
		isHonking = false;
	}

}
