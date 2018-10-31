using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;


public class EventManager : MonoBehaviour {

	public GameObject questionManagerGO;
	public GameObject radioInterface;
	public GameObject timeInterface;
	public GameObject rulesInterface;

	public Text[] taskTexts;
	public Image[] taskImages;

	private Vector2[] textPos = {Vector2.zero, Vector2.zero};
	private Vector2[] imagePos = {Vector2.zero, Vector2.zero};

	private LevelFade levelFadeScript;

	public static List<string> storyArr = new List<string>();
	public AudioSource btnContinue;

	public static bool firstLevelPlayed = false;
	public static bool randomizeRole;

	private string nextStoryName;

	public AudioSource firstStoryGameVoice;
	public AudioSource hissSound;
	public AudioSource radioBGMusic;
	public AudioSource[] nextEventVoice;


	private void Awake() {
		// Get all names of the stories in the Story folder
		string mapsFolder = Application.dataPath + "/Scenes/6 Stories";
		var mapsDirInfo = new DirectoryInfo(mapsFolder);
		var allMapsFileInfos = mapsDirInfo.GetFiles("*.unity", SearchOption.AllDirectories);
		foreach (var fileInfo in allMapsFileInfos) {
			storyArr.Add(Path.GetFileNameWithoutExtension(@fileInfo.Name));
		}

		// Save original positions of tasks and their image as default
		for (int i = 0; i < taskTexts.Length; i++) {
			textPos[i] = new Vector2(
				taskTexts[i].GetComponent<RectTransform>().anchoredPosition.x,
				taskTexts[i].GetComponent<RectTransform>().anchoredPosition.y
			);
			imagePos[i] = new Vector2(
				taskImages[i].GetComponent<RectTransform>().anchoredPosition.x,
				taskImages[i].GetComponent<RectTransform>().anchoredPosition.y
			);
		}
		
	}


	private void Start() {
		levelFadeScript = GameObject.Find("LevelFader").GetComponent<LevelFade>();
		// GameManager.enableNavigation = true;

		// Display radio interface and time
		radioInterface.SetActive(true);
		timeInterface.SetActive(true);
	}

	private void OnEnable() {
		// Disable question manager
		questionManagerGO.SetActive(false);

		if (firstLevelPlayed) {
			StartCoroutine(Continue());
		} else {
			StartCoroutine(LaunchRadio());
		}
	}


	private void Update() {
		if (GameManager.enableNavigation) {
			if (GameManager.playerDark.GetButtonDown("X")) {
				btnContinue.Play();
				// Disable navigation
				GameManager.enableNavigation = false;
				if (firstLevelPlayed) {
					// Load chosen scene
					levelFadeScript.FadeToLevel(nextStoryName);
				} else {
					// Load driving level
					levelFadeScript.FadeToLevel("4 DrivexStory");
				}
			}
		}
	}


	IEnumerator LaunchRadio() {
		yield return new WaitForSeconds(0.5f);
		hissSound.Play();
		yield return new WaitForSeconds(2.4f);
		radioBGMusic.Play();
		yield return new WaitForSeconds(0.3f);

		// Play random voice to introduce next event
		if (firstLevelPlayed) {
			int randomVoice = Random.Range(0, nextEventVoice.Length);
			nextEventVoice[randomVoice].Play();
		} else {
			// Voice introducing first game
			firstStoryGameVoice.Play();
			yield return new WaitForSeconds(7.5f);

			// Randomize and show rules for first game
			FirstLevel();
		}
	}


	private void FirstLevel() {
		// Randomize roles for first game
		RandomizeRoles();

		// Show rules interface
		rulesInterface.SetActive(true);
		btnContinue.Play();

		// Enable navigation, so players can click to get started
		GameManager.enableNavigation = true;
	}


	// When player hits continue it calls a random event
	private IEnumerator Continue() {
		yield return new WaitForSeconds(0.5f);
		RandomEvent();
	}


	// Chosing a random number to check if the next event is a Story or Radio
	private void RandomEvent() {

		int randomEvent = Random.Range(0, 100);

		// Start radio event
		if (randomEvent >= 0 && randomEvent < GameManager.questionChance) {
			QuestionManager();
		}

		// Start story event
		// if (randomEvent >= questionChance && randomEvent < 100) {
		// 	NextStory();
		// }
	}


	private void QuestionManager() {
		questionManagerGO.SetActive(true);
	}


	// Continuing with the next Story
	private void NextStory() {
		// Get random number between 0 and storyArr.Length -1
		int randStoryIndex = Random.Range(0, storyArr.Count -1);

		// Save name of chosen (by index) story level in string
		nextStoryName = storyArr[randStoryIndex];

		// Delete this story level from storyArr
		storyArr.RemoveAt(randStoryIndex);
	}


	private void RandomizeRoles() {
		// Randomize the roles so that the controls can change
		randomizeRole = (Random.value < 0.5);

		if (randomizeRole) {
			taskTexts[0].GetComponent<RectTransform>().anchoredPosition = textPos[0];
			taskImages[0].GetComponent<RectTransform>().anchoredPosition = imagePos[0];

			taskTexts[1].GetComponent<RectTransform>().anchoredPosition = textPos[1];
			taskImages[1].GetComponent<RectTransform>().anchoredPosition = imagePos[1];
		} else {
			taskTexts[0].GetComponent<RectTransform>().anchoredPosition = textPos[1];
			taskImages[0].GetComponent<RectTransform>().anchoredPosition = imagePos[1];

			taskTexts[1].GetComponent<RectTransform>().anchoredPosition = textPos[0];
			taskImages[1].GetComponent<RectTransform>().anchoredPosition = imagePos[0];
		}
	}

}
