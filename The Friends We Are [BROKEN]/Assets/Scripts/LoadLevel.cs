using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour {

	private void Start() {
		 SceneManager.LoadScene("DrivexStory");
	}
}
