using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusAudio : MonoBehaviour {

	private AudioSource motorSound;


	private void Awake() {
		motorSound = GetComponent<AudioSource>();
	}

	private void Update() {
		motorSound.pitch = (DrivingxStory.currentSpeed / 400) + 0.5f;
	}


	// public IEnumerator FadeSound() {
	// 	while (motorSound.volume > 0) {
	// 		motorSound.volume -= 0.1f;
	// 		yield return new WaitForSeconds(0.1f);
	// 	}
	// }
}
