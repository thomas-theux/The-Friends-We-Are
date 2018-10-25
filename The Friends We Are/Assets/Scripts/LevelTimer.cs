using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelTimer : MonoBehaviour {

	public GameObject statsManager;
	public GameObject levelInterface;

	public Text levelTimeText;
	private float levelTime = 4;
	public static bool levelEnd;

	public KillAllPoints killAllPointsScript;
	public LevelFade levelFadeScript;


	private void Start() {
		levelTimeText = GameObject.Find("LevelTime").GetComponent<Text>();
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
		// End the level
		levelEnd = true;

		// Destroy all remaining points
		killAllPointsScript.DestroyRemainingPoints();

		// Wait another 2 seconds before going to the Story Overview screen
		yield return new WaitForSeconds(2);

		LevelEnd();
	}


	private void LevelEnd() {
		// Load Story Overview screen
		levelInterface.SetActive(false);
		statsManager.SetActive(true);
		// levelFadeScript.FadeToLevel("6 Summary");

	}
	
}
