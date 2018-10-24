using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillAllPoints : MonoBehaviour {

	public void DestroyRemainingPoints() {
		Destroy(this.gameObject);
	}
}
