using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;

public class Blow : MonoBehaviour {

	public GameObject blowGO;
	public Slider blowSlider;

	private float blowStrength;
	private float decreaseValue = 1.0f;
	private float tempDecrease;


	private void OnEnable() {
		blowGO.SetActive(true);

		blowGO.transform.GetChild(0).transform.localPosition = CampfirexStory.gaugePos[CampfirexStory.activePlayer];
	}


	private void Update() {
		GetInput();

		if (blowStrength > 0) {
			ActionsInput();
		}

		if (decreaseValue > 0) {
			DecreaseWind();
		}
	}


	private void GetInput() {
		// blowStrength = ReInput.players.GetPlayer(CampfirexStory.activePlayer).GetAxis("R2");
		blowStrength = ReInput.players.GetPlayer(0).GetAxis("R2");
	}


	private void ActionsInput() {
		blowSlider.value += (tempDecrease + blowStrength) - (blowSlider.value / 100);
	}


	private void DecreaseWind() {
		tempDecrease = decreaseValue + (blowSlider.value / 100 * 2);
		blowSlider.value -= tempDecrease;
	}


	private void OnDisable() {
		blowGO.SetActive(false);
	}

}
