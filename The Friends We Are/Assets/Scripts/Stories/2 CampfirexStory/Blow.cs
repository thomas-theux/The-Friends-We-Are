using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;

public class Blow : MonoBehaviour {

	public GameObject blowGO;
	public Image rangeBG;
	
	public Slider blowSlider;
	public Slider minSlider;
	public Slider maxSlider;

	private float blowStrength;
	private float tempDecrease;

	private float speedMultiplier = 1.2f;

	private float rangeMin;
	private float rangeMax;

	private float addPoints = 0.1f;


	private void OnEnable() {
		blowGO.SetActive(true);

		blowGO.transform.GetChild(0).transform.localPosition = CampfirexStory.gaugePos[CampfirexStory.activePlayer];

		RandomizeRange();
	}


	private void Update() {
		GetInput();

		if (blowStrength > 0) {
			ActionsInput();
		} else {
			DecreaseWind();
		}

		if (blowSlider.value > rangeMin && blowSlider.value < rangeMax) {
			GetPoints();
		}

		// print(CampfirexStory.blowPercentage);
	}


	private void GetInput() {
		// blowStrength = ReInput.players.GetPlayer(CampfirexStory.activePlayer).GetAxis("R2");
		// CHANGE THIS FROM ZERO TO CAMPFIREXSTORY.ACTIVEPLAYER !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
		blowStrength = ReInput.players.GetPlayer(0).GetAxis("R2");
	}


	private void ActionsInput() {
		blowSlider.value += tempDecrease + blowStrength;
	}


	private void DecreaseWind() {
		// tempDecrease = (blowSlider.value / 100) * 2;
		tempDecrease = (Mathf.Pow((blowSlider.value / 100), speedMultiplier)) * 2;
		blowSlider.value -= tempDecrease;
	}


	private void RandomizeRange() {
		// Set new range
		int rndRange = Random.Range(10, 30);
		int rndMin = Random.Range(0, 100 - rndRange);
		rangeMin = rndMin;
		rangeMax = rndMin + rndRange;

		// Set new time after which the range changes
		float renewTime = Random.Range(1, 4);

		// Show min and max
		minSlider.value = rangeMin;
		maxSlider.value = rangeMax;

		StartCoroutine(RangeTimer(renewTime));
	}


	private void GetPoints() {
		CampfirexStory.blowPercentage += addPoints;
	}


	IEnumerator RangeTimer(float waitTime) {
		yield return new WaitForSeconds(waitTime);

		RandomizeRange();
	}


	private void OnDisable() {
		blowGO.SetActive(false);
	}

}
