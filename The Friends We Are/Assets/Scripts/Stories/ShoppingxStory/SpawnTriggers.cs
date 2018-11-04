using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTriggers : MonoBehaviour {

	public GameObject trigger;
	public GameObject triggerParent;

	private float[] posX = {
		10.0f,
		-6.0f,
		-11.5f,
		-8.75f,
		10.0f,
		19.75f,
		12.25f,
		-6.0f
	};

	private float[] posZ = {
		22.5f,
		17.5f,
		2.5f,
		-21.75f,
		-15.75f,
		12.0f,
		2.0f,
		-11.0f
	};

	private float[] scaleX = {
		6.0f,
		6.0f,
		6.0f,
		4.5f,
		9.0f,
		9.5f,
		3.5f,
		6.0f
	};

	private float[] scaleZ = {
		4.0f,
		4.0f,
		4.0f,
		5.5f,
		5.5f,
		5.0f,
		6.0f,
		4.0f
	};


	private void Start() {
		for (int i = 0; i < FoodChecklist.departments; i++) {
			GameObject newTrigger = Instantiate(trigger, Vector3.zero, Quaternion.Euler(new Vector3(0, 40, 0)));
			newTrigger.transform.SetParent(triggerParent.transform);
			newTrigger.transform.localPosition = new Vector3(posX[i], 0.5f, posZ[i]);
			newTrigger.transform.localScale = new Vector3(scaleX[i], 1, scaleZ[i]);

			newTrigger.GetComponent<FoodTrigger>().triggerID = i;
		}
	}
}
