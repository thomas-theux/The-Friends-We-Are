using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsManager : MonoBehaviour {

	public static float transferredValue01;
	public static string transferredText01;

	public static float transferredValue02;
	public static string transferredText02;

	public static float transferredValue03;
	public static string transferredText03;

	public static float transferredValue04;
	public static string transferredText04;


	private void Awake() {
		DontDestroyOnLoad(this.gameObject);
	}

}
