using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestStats : MonoBehaviour {
	
	private void Awake() {
		StatsManager.transferredValue01 = 24;
		StatsManager.transferredText01 = "Meters";
		
		StatsManager.transferredValue02 = 89;
		StatsManager.transferredText02 = "Maximum";
		
		StatsManager.transferredValue03 = 48;
		StatsManager.transferredText03 = "Average";
		
		StatsManager.transferredValue04 = 204;
		StatsManager.transferredText04 = "Experience";
	}
}
