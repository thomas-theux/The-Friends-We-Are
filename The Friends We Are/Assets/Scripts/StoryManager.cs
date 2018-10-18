using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;


public class StoryManager : MonoBehaviour {

	private LevelFade levelFadeScript;

	public static List<string> storyArr = new List<string>();
	public GameObject hideStats;
	public AudioSource acceptBtn;


	private void Awake() {
		DontDestroyOnLoad(this.gameObject);

		// Get all names of the stories in the Story folder
		string mapsFolder = Application.dataPath + "/Scenes/5 Stories";
		var mapsDirInfo = new DirectoryInfo(mapsFolder);
		var allMapsFileInfos = mapsDirInfo.GetFiles("*.unity", SearchOption.AllDirectories);
		foreach (var fileInfo in allMapsFileInfos) {
			storyArr.Add(Path.GetFileNameWithoutExtension(@fileInfo.Name));
		}
	}


	private void Start() {
		levelFadeScript = GameObject.Find("LevelFader").GetComponent<LevelFade>();
	}


	private void Update() {
		GetInput();
	}


	private void GetInput() {
		// Get input from player one (dark)
		if(GameManager.playerDark.GetButtonDown("X")) {
			acceptBtn.Play();
			StartCoroutine(Continue());
		}
	}


	// Chosing a random number to check if the next event is a Story or Radio
	private void RandomEvent() {
		int randomEvent = Random.Range(0, 100);

		// Start radio event
		if (randomEvent >= 0 && randomEvent < 30) {
			print("Radio!");
			StartRadio();
		}

		// Start story event
		if (randomEvent >= 30 && randomEvent < 100) {
			print("Story!");
			NextStory();
		}
	}


	// Starting the Radio
	private void StartRadio() {
		// Hide stats interface
		hideStats.SetActive(false);

		// Call radio script
	}


	// Continuing with the next Story
	private void NextStory() {
		// Get random number between 0 and storyArr.Length -1
		int randStoryIndex = Random.Range(0, storyArr.Count -1);

		// Save name of chosen (by index) story level in string
		string nextStoryName = storyArr[randStoryIndex];

		// Delete this story level from storyArr
		storyArr.RemoveAt(randStoryIndex);

		// Load chosen scene
		levelFadeScript.FadeToLevel(nextStoryName);
	}


	// When player hits continue it calls a random event
	private IEnumerator Continue() {
		yield return new WaitForSeconds(0.5f);
		RandomEvent();
	}

}
