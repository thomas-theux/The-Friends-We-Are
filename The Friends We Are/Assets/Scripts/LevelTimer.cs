using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelTimer : MonoBehaviour {

	private GameObject reviewInterfaceGO;
	public GameObject levelInterfaceGO;

	public Text levelTimeText;
	private float levelTimeDefault = 24;
	private float levelTime;
	public static bool levelEnd;

	// private LevelFade levelFadeScript;


	private void OnEnable() {
		levelTime = levelTimeDefault;
		reviewInterfaceGO = GameObject.Find("StatsManager").transform.GetChild(0).gameObject;
		levelTimeText = GameObject.Find("LevelTime").GetComponent<Text>();
		// levelFadeScript = GameObject.Find("LevelFader").GetComponent<LevelFade>();
	}


	private void Update() {
		if (StartLevelCountdown.startLevel) {
			CountdownLevelTime();
		}
	}


	private void CountdownLevelTime() {
		if (levelTime > 0) {
			levelTime -= Time.deltaTime;
			levelTimeText.text = Mathf.Ceil(levelTime) + "";
		} else {
			StartCoroutine(LastSeconds());
			StartLevelCountdown.startLevel = false;
		}
	}


	IEnumerator LastSeconds() {
		// Wait another second before going to the Story Overview screen
		yield return new WaitForSeconds(1);

		// End the level
		LevelEnd();
	}


	private void LevelEnd() {
		// Load Story Overview screen
		// levelInterfaceGO.SetActive(false);
		reviewInterfaceGO.SetActive(true);
		levelEnd = true;

		levelTimeText.text = "";

		this.enabled = false;
	}
	
}
