using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowStats : MonoBehaviour {

	public Text value01;
	public Text text01;
	private float increase01;

	public Text value02;
	public Text text02;
	private float increase02;

	public Text value03;
	public Text text03;
	private float increase03;

	public Text value04;
	public Text text04;
	private float increase04;

	private int increaseNext = 0;


	private void Start() {
		text01.text = StatsManager.transferredText01;
		text02.text = StatsManager.transferredText02;
		text03.text = StatsManager.transferredText03;
		text04.text = StatsManager.transferredText04;
	}


	private void Update() {

		for (int i = 0; i < 4; i++) {
			while (increase01 < StatsManager.transferredValue01) {
				increase01 += 4;
			}
			increase01 = StatsManager.transferredValue01;
		}


	
		
		if (increase02 < StatsManager.transferredValue02 && increaseNext == 1) { increase02 += 1; }
		else if (increaseNext == 1) {
			increase02 = StatsManager.transferredValue02;
			increaseNext++;
		}
		
		if (increase03 < StatsManager.transferredValue03 && increaseNext == 2) { increase03 += 1; }
		else if (increaseNext == 2) {
			increase03 = StatsManager.transferredValue03;
			increaseNext++;
		}
		
		if (increase04 < StatsManager.transferredValue04 && increaseNext == 3) { increase04 += 3; }
		else if (increaseNext == 3) {
			increase04 = StatsManager.transferredValue04;
			increaseNext++;
		}

		value01.text = increase01 + "";
		value02.text = increase02 + "";
		value03.text = increase03 + "";
		value04.text = increase04 + "";
		
	}

}
