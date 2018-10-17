using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HonkHorns : MonoBehaviour {

	public AudioSource hornSound;
	private bool isHonking = false;


	private void Update() {
		if (!isHonking) {
			StartCoroutine(Honking());
		}
	}


	IEnumerator Honking() {
		isHonking = true;
		float hornDelay = Random.Range(10, 20);
		yield return new WaitForSeconds(hornDelay);
		hornSound.Play();
		isHonking = false;
	}

}
