using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnTriggers : MonoBehaviour {

	public GameObject trigger;
	public GameObject triggerCanvas;
	public GameObject triggerParent;

	public Material[] materials;

	private Color32[] colors = {
		new Color32(41, 177, 204, 255),		// Beverages
		new Color32(41, 204, 177, 255),		// Dairy
		new Color32(41, 82, 204, 255),		// Frozen
		new Color32(204, 106, 41, 255),		// Fruits
		new Color32(204, 41, 41, 255),		// Meat
		new Color32(204, 182, 41, 255),		// Snacks
		new Color32(204, 41, 163, 255),		// Sundries
		new Color32(54, 204, 41, 255),		// Vegetables
	};

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

	private string[] departmentNames = {
		"Beverages",
		"Dairy",
		"Frozen",
		"Fruits",
		"Meat",
		"Snacks",
		"Sundries",
		"Vegetables",
	};


	private void Start() {
		for (int i = 0; i < FoodChecklist.departments; i++) {
			// Instantiate new department trigger
			GameObject newTrigger = Instantiate(trigger, Vector3.zero, Quaternion.Euler(new Vector3(0, 40, 0)));
			newTrigger.transform.SetParent(triggerParent.transform);
			newTrigger.transform.localPosition = new Vector3(posX[i], 0.5f, posZ[i]);
			newTrigger.transform.localScale = new Vector3(scaleX[i], 1, scaleZ[i]);
			newTrigger.GetComponent<Renderer>().material = materials[i];

			newTrigger.GetComponent<FoodTrigger>().triggerID = i;

			// Instantiate new trigger canvas
			GameObject newCanvas = Instantiate(triggerCanvas, Vector3.zero, Quaternion.Euler(new Vector3(0, -50, 0)));
			newCanvas.transform.SetParent(triggerParent.transform);
			newCanvas.transform.localPosition = newTrigger.transform.localPosition;
			newCanvas.transform.GetChild(0).GetComponent<Image>().color = colors[i];
			newCanvas.transform.GetChild(1).gameObject.GetComponent<Text>().text = departmentNames[i];
		}
	}
}