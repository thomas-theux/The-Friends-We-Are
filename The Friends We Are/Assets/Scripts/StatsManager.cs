using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsManager : MonoBehaviour {

	public static float[] transferValues = {0, 0, 0, 0};
	public static string[] transferTexts = {"", "", "", ""};

	public static string transferredText01;
	public static string transferredText02;
	public static string transferredText03;
	public static string transferredText04;


	private void Awake() {
		DontDestroyOnLoad(this.gameObject);
	}

}
