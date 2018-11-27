using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampfirexStory : MonoBehaviour {
	
	public GameObject[] microGameScripts;

	public static int activePlayer;
	private int newMicroGame;
	private int oldMicroGame;
	private float rndTimeframe;
	private bool microGameActive;

	private float minTime = 2.0f;
	private float maxTime = 5.0f;
	
	public static Vector2[] gaugePos = {
		new Vector2(-400, 0),
		new Vector2(400, 0)
	};


	private void Start() {
		// Randomize start player
		activePlayer = Random.Range(0, GameManager.playerCount);

		// Start first round
		NextSession();
	}


	private void Update() {
		if (microGameActive) {
			rndTimeframe -= Time.deltaTime;

			if (rndTimeframe <= 0) {
				NextSession();
			}
		}
	}


	private void NextSession() {
		// Switch to the other player
		SwitchPlayers();

		// Pick a random micro game that will be displayed
		PickRndMicroGame();

		// Define a random timeframe for this micro game
		PickRndTimeframe();

		// Activate the script for the specific micro game
		ActivateMicroGameScript();

		// Set old micro game to be disabled later
		SetOldMicroGame();
	}


	private void SwitchPlayers() {
		activePlayer = activePlayer == 0 ? 1 : 0;
	}


	private void PickRndMicroGame() {
		newMicroGame = Random.Range(0, microGameScripts.Length);
	}


	private void PickRndTimeframe() {
		rndTimeframe = Random.Range(minTime, maxTime);
	}


	private void ActivateMicroGameScript() {
		microGameScripts[oldMicroGame].SetActive(false);
		microGameScripts[newMicroGame].SetActive(true);
	}


	private void SetOldMicroGame() {
		oldMicroGame = newMicroGame;

		microGameActive = true;
	}

}
