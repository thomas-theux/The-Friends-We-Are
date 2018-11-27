using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BreakSticks : MonoBehaviour {

	public GameObject twigsGO;
	public Slider twigsSlider;

	private bool isIncreasing = false;

	private float currentValue = 0;
	private float moveSpeed = 80.0f;


	private void OnEnable() {
		twigsGO.SetActive(true);

		twigsGO.transform.GetChild(0).transform.localPosition = CampfirexStory.gaugePos[CampfirexStory.activePlayer];
	}


	private void Update() {
		if (isIncreasing) {
			IncreaseValue();
		} else {
			DecreaseValue();
		}

		twigsSlider.value = currentValue;
	}

	
	private void IncreaseValue() {
		currentValue += moveSpeed * Time.deltaTime;

		if (currentValue >= 100) {
			isIncreasing = false;
		}
	}

	
	private void DecreaseValue() {
		currentValue -= moveSpeed * Time.deltaTime;

		if (currentValue <= 0) {
			isIncreasing = true;
		}
	}


	private void OnDisable() {
		twigsGO.SetActive(false);
	}

}
