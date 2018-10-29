using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsHolder : MonoBehaviour {

	public static bool transferBool = false;
	public static float[] transferValues = {0, 0, 0, 0, 0};
	public static string[] transferTexts = {"", "", "", "", ""};


	private void Awake() {
		// DontDestroyOnLoad(this.gameObject);
	}

}
