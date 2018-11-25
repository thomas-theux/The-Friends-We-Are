using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blow : MonoBehaviour {

	public GameObject blowGO;

	private Vector2[] gaugePos = {
		new Vector2(-600, 0),
		new Vector2(600, 0)
	};


	private void OnEnable() {
		blowGO.SetActive(true);

		blowGO.transform.GetChild(0).transform.localPosition = gaugePos[CampfirexStory.activePlayer];
	}


	private void OnDisable() {
		blowGO.SetActive(false);
	}

}
