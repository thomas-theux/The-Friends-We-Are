using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrepareWurst : MonoBehaviour {

	public GameObject wurstGO;

	
	private void OnEnable() {
		wurstGO.SetActive(true);

		wurstGO.transform.GetChild(0).transform.localPosition = CampfirexStory.gaugePos[CampfirexStory.activePlayer];
	}


	private void OnDisable() {
		wurstGO.SetActive(false);
	}

}
