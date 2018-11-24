using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TestTimeManager : MonoBehaviour {

	public Text timer;
	private float current;


	private void Awake() {
		DontDestroyOnLoad(this.gameObject);
	}


	private void Update() {
		current += Time.deltaTime;
		timer.text = (current * 2).ToString("F0");

		if (GameManager.playerDark.GetButtonDown("X")) {
			SceneManager.LoadScene("DontDestroyTest2");
		}
	}
}
