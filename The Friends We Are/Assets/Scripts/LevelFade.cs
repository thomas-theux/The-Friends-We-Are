using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelFade : MonoBehaviour {

	public Animator animator;
	private string levelToLoad;


	private void Awake() {
		DontDestroyOnLoad(this.gameObject);
	}


	public void FadeToLevel(string levelName) {
		levelToLoad = levelName;
		animator.SetTrigger("Fading");
	}


	public void OnFadeCompletes() {
		SceneManager.LoadScene(levelToLoad);
	}

}
