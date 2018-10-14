using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionSpawner : MonoBehaviour {

	public GameObject section;
	public GameObject worldParent;
	private int spawnAmount = 10;

	private void Start() {
		for (int i = 0; i < spawnAmount; i++) {
			GameObject newSection = Instantiate(section, Vector3.zero, Quaternion.identity);
			newSection.transform.position = new Vector3(0, 0, i * 100);
			newSection.transform.SetParent(worldParent.transform);
		}
	}
}
