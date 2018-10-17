using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionSpawner : MonoBehaviour {

	public GameObject section;
	public GameObject asphalt;
	public int spawnAmount = 50;

	private void Start() {
		for (int i = 0; i < spawnAmount; i++) {
			GameObject newSection = Instantiate(section, Vector3.zero, Quaternion.identity);
			newSection.transform.position = new Vector3(0, 0, i * 100);
			newSection.transform.SetParent(this.gameObject.transform);
		}

		GameObject newAsphalt = Instantiate(asphalt, Vector3.zero, Quaternion.identity);
		newAsphalt.transform.localScale = new Vector3(
			newAsphalt.transform.localScale.x,
			newAsphalt.transform.localScale.y,
			spawnAmount * 100);
		newAsphalt.transform.position = new Vector3(5, 0, (newAsphalt.transform.localScale.z / 2) - 20);
		newAsphalt.transform.SetParent(this.gameObject.transform);
	}
}
