using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Rewired;

public class GameManager : MonoBehaviour {

	public static int playerCount = 2;

	// REWIRED PLUGIN
	public static Player playerDark;
	public static Player playerLight;

	private void Awake() {
		DontDestroyOnLoad(this.gameObject);
		
		playerDark = ReInput.players.GetPlayer(0);
		playerLight = ReInput.players.GetPlayer(1);
	}
	
}
