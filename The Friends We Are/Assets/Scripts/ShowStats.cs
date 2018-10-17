using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowStats : MonoBehaviour {

	public Text value01;
	public Text text01;

	public Text value02;
	public Text text02;

	public Text value03;
	public Text text03;

	public Text value04;
	public Text text04;


	private void Start() {
		value01.text = StatsManager.transferredValue01 + "";
		text01.text = StatsManager.transferredText01;

		value02.text = StatsManager.transferredValue02 + "";
		text02.text = StatsManager.transferredText02;

		value03.text = StatsManager.transferredValue03 + "";
		text03.text = StatsManager.transferredText03;

		value04.text = StatsManager.transferredValue04 + "";
		text04.text = StatsManager.transferredText04;
	}

}
