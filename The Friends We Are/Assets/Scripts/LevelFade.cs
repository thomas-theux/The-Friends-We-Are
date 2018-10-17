using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelFade : MonoBehaviour {

	public Animator animator;
	private string levelToLoad;


	private void Awake() {
		DontDestroyOnLoad(this.gameObject);
	}


	private void Update() {
		if (Input.GetMouseButtonDown(0)) {
			FadeToLevel("6 Summary");
		}
	}


	public void FadeToLevel(string levelName) {
		levelToLoad = levelName;
		animator.SetTrigger("Fading");
	}


	public void OnFadeCompletes() {
		SceneManager.LoadScene(levelToLoad);
	}

}
