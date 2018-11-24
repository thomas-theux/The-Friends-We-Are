using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Rewired;

public class GameManager : MonoBehaviour {

	private StartLevelCountdown startLevelCountdownScript;
	private LevelTimer levelTimerScript;

	public static int playerCount = 2;
	
	public static int tripDays = 5;

	public static int storyAP = 3;
	public static int radioAP = 2;
	public static int maxAP = 6;

	public static int storyChance = 100;
	public static int questionChance = 0;

	public static bool enableNavigation;
	public static bool skipStats = false;

	public static float overallScore;

	// REWIRED PLUGIN
	public static Player playerDark;
	public static Player playerLight;
	

	private void Awake() {
		DontDestroyOnLoad(this.gameObject);
		Cursor.visible = false;
		
		playerDark = ReInput.players.GetPlayer(0);
		playerLight = ReInput.players.GetPlayer(1);

		startLevelCountdownScript = GetComponent<StartLevelCountdown>();
		levelTimerScript = GetComponent<LevelTimer>();
	}


	// private void Update() {
	// 	if (StoryGameHandler.storyGameIsActive) {
	// 		StoryGameHandler.storyGameIsActive = false;
	// 		StartCoroutine(FadeDelay());
	// 	}
	// }


	IEnumerator FadeDelay() {
		yield return new WaitForSeconds(1);

		// Activate time scripts
		ActivateTimerScripts();
	}


	private void ActivateTimerScripts() {
		startLevelCountdownScript.enabled = true;
		levelTimerScript.enabled = true;
	}
	
}
