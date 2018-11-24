using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayers : MonoBehaviour {

	public GameObject player;
	public GameObject[] spawns;
	public Material[] materials;


	private void Start() {
		for (int i = 0; i < 2; i++) {
			GameObject newPlayer = Instantiate(player, Vector3.zero, Quaternion.identity);
			newPlayer.transform.position = spawns[i].transform.position;
			
			// Set color/material for spawned player
			newPlayer.GetComponent<Renderer>().material = materials[i];

			// Set ID of the spawned player
			newPlayer.GetComponent<PlayerController>().playerID = i;
		}
	}

}
