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
}
