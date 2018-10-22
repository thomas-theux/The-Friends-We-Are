using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointSpawner : MonoBehaviour {

	public GameObject point;
	private int spawnAmount = 200;

	private float spawnX = 5;
	private float spawnY = 2;
	private float spawnZ = 20;
	private float tempSpawnZ;

	private void Start() {
		for (int i = 0; i < spawnAmount; i++) {
			GameObject newPoint = Instantiate(point, Vector3.zero, point.transform.rotation);

			spawnX = Random.Range(-5, 16);
			newPoint.transform.position = new Vector3(spawnX, spawnY, spawnZ);
			spawnZ = Random.Range(1, 24) + tempSpawnZ;
			tempSpawnZ = spawnZ;
			newPoint.transform.SetParent(this.gameObject.transform);
		} 
	}
}
