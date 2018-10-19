using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radio : MonoBehaviour {

	public GameObject hideStats;

	public AudioSource hissSound;
	public AudioSource radioVoiceIntro;
	public AudioSource radioVoice;
	public AudioSource radioBG;

	private bool firstVoice = false;

	public GameObject radioInterface;


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
			firstVoice = true;
		} else {
			radioVoice.Play();
		}
	}


	private void ShowQuestions() {

	}


	private void ShowAnswers() {

	}


	private void StartTimer() {

	}


	private void RegisterInput() {

	}


	private void CompareAnswers() {

	}


	private void GetPoints() {

	}


}
