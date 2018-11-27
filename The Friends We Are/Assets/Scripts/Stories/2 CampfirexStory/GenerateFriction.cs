using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateFriction : MonoBehaviour {

	public GameObject frictionGO;


	private void OnEnable() {
		frictionGO.SetActive(true);

		frictionGO.transform.GetChild(0).transform.localPosition = CampfirexStory.gaugePos[CampfirexStory.activePlayer];
	}


	private void OnDisable() {
		frictionGO.SetActive(false);
	}
}
