using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class EventAnnouncer : MonoBehaviour {

	public GameObject rulesInterfaceGO;
	public GameObject questionManagerGO;
	public GameObject radioInterface;
	public GameObject timeInterface;

	public static bool writeLevelsInArr = false;

	private StoryContent storyContentsScript;

	// To start with the driving level set this to false		--> Later needed when game is finished
	public static bool firstLevelPlayed = false;
	public static bool randomizeRole;
	private int randStoryIndex;
	public static List<string> storyArr = new List<string>();
	
	public Text[] taskTexts;
	public Image[] taskImages;

	public Text storyTitleText;
	public Text storyTextText;
	public Text storyGoalText;
	public Text storyTaskAText;
	public Text storyTaskBText;
	public Image storyRuleAImage;
	public Image storyRuleBImage;

	private Vector2[] textPos = {Vector2.zero, Vector2.zero};
	private Vector2[] imagePos = {Vector2.zero, Vector2.zero};

	private LevelFade levelFadeScript;

	private string nextStoryName;

	public AudioSource hissSound;
	public AudioSource radioBGMusic;
	public AudioSource[] welcomeToDayVoice;
	public AudioSource welcomeBackVoice;
	public AudioSource letsSeeVoice;
	public AudioSource welcomeFirstDayVoice;
	public AudioSource getStartedVoice;
	public AudioSource howItWorksVoice;
	public AudioSource nextUpVoice;


	private void Awake() {
		levelFadeScript = GameObject.Find("LevelFader").GetComponent<LevelFade>();
		timeInterface = GameObject.Find("TimeInterface").gameObject;
		storyContentsScript = GameObject.Find("GameManager").GetComponent<StoryContent>();

		radioInterface.SetActive(true);
		timeInterface.SetActive(true);
		
		// Activate time
		TimeManager.isDay = true;

		// Get all names of the stories in the Story folder
		if (!writeLevelsInArr) {
			string mapsFolder = Application.dataPath + "/Scenes/6 Stories";
			var mapsDirInfo = new DirectoryInfo(mapsFolder);
			var allMapsFileInfos = mapsDirInfo.GetFiles("*.unity", SearchOption.AllDirectories);
			foreach (var fileInfo in allMapsFileInfos) {
				storyArr.Add(Path.GetFileNameWithoutExtension(@fileInfo.Name));
			}

			writeLevelsInArr = true;
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


	private void OnEnable() {
		// Disable question manager
		questionManagerGO.SetActive(false);

		StartCoroutine(LaunchRadio());
	}


	IEnumerator LaunchRadio() {
		yield return new WaitForSeconds(0.5f);
		hissSound.Play();
		yield return new WaitForSeconds(2.4f);
		radioBGMusic.Play();
		yield return new WaitForSeconds(0.3f);

		// Check if the first level has been played already
		if (TimeManager.isDay) {
			if (firstLevelPlayed) {
				if (TimeManager.dayJustStarted) {
					// Voice: "Welcome to day x!"
					welcomeToDayVoice[DayDisplayer.currentDay-1].Play();
					// yield return new WaitForSeconds(2.0f);
					yield return new WaitForSeconds(0.5f);
					TimeManager.dayJustStarted = false;
				} else {
					// Voice: "Welcome back guys!"
					welcomeBackVoice.Play();
					// yield return new WaitForSeconds(2.0f);
					yield return new WaitForSeconds(0.5f);
				}
				// Voice: "Let's see what's going to happen next!"
				letsSeeVoice.Play();
				// yield return new WaitForSeconds(2.0f);
				yield return new WaitForSeconds(0.5f);
				PickRandomEvent();
			} else {
				// Voice: "Welcome to the first day of our road trip!"
				welcomeFirstDayVoice.Play();
				yield return new WaitForSeconds(0.5f);
				StartCoroutine(FirstDay());
			}
		} else {
			// Radio voice saying that the day is over

			// Loading the "New Day" scene
			TimeManager.currentTime = TimeManager.dayStartTime;
			levelFadeScript.FadeToLevel("7 New Day");
		}
		
	}


	private void Update() {
		if (GameManager.enableNavigation) {
			if (GameManager.playerDark.GetButtonDown("X")) {
				if (firstLevelPlayed) {
					// Load chosen scene
					levelFadeScript.FadeToLevel(nextStoryName);
				} else {
					// Load driving level
					levelFadeScript.FadeToLevel("0 DrivexStory");
				}
			}
		}
	}


	// Introduce the first day
	IEnumerator FirstDay() {
		// Voice: "Let's get started with our first story!"
		getStartedVoice.Play();
		yield return new WaitForSeconds(2.5f);

		// Voice: "And this is how it works:"
		howItWorksVoice.Play();
		yield return new WaitForSeconds(2.0f);

		ShowRules();

		GameManager.enableNavigation = true;
	}


	private void PickRandomEvent() {
		int randomEvent = Random.Range(0, 100);

		// Start radio event
		if (randomEvent >= 0 && randomEvent < GameManager.questionChance) {
			questionManagerGO.SetActive(true);
		}

		// Start story event
		if (randomEvent >= GameManager.questionChance && randomEvent < 100) {
			StartCoroutine(StartStory());
		}
	}


	IEnumerator StartStory() {
		// Voice: "Next up is a little story!"
		nextUpVoice.Play();
		yield return new WaitForSeconds(2.0f);

		// Voice: "And this is how it works:"
		howItWorksVoice.Play();
		yield return new WaitForSeconds(2.0f);

		PickStory();

		ShowRules();
	}


	private void PickStory() {
		// Get random number between 0 and storyArr.Length -1
		randStoryIndex = Random.Range(0, storyArr.Count -1);

		// Save name of chosen (by index) story level in string
		nextStoryName = storyArr[randStoryIndex];
	}


	// Randomize the roles for the minigame
	private void RandomizeRoles() {
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


	private void ShowRules() {
		// Delete this story level from storyArr
		storyArr.RemoveAt(randStoryIndex);

		RandomizeRoles();

		// Show rules interface
		rulesInterfaceGO.SetActive(true);

		// Show the according texts and images
		storyTitleText.text = storyContentsScript.storyTitles[randStoryIndex];
		storyTextText.text = storyContentsScript.storyTexts[randStoryIndex];
		storyGoalText.text = storyContentsScript.storyGoals[randStoryIndex];
		storyTaskAText.text = storyContentsScript.storyTasksA[randStoryIndex];
		storyTaskBText.text = storyContentsScript.storyTasksB[randStoryIndex];
		storyRuleAImage.sprite = storyContentsScript.storyRulesA[randStoryIndex];
		storyRuleBImage.sprite = storyContentsScript.storyRulesB[randStoryIndex];

		// Remove the selected elements from the arrays
		storyContentsScript.storyTitles.RemoveAt(randStoryIndex);
		storyContentsScript.storyTexts.RemoveAt(randStoryIndex);
		storyContentsScript.storyGoals.RemoveAt(randStoryIndex);
		storyContentsScript.storyTasksA.RemoveAt(randStoryIndex);
		storyContentsScript.storyTasksB.RemoveAt(randStoryIndex);
		storyContentsScript.storyRulesA.RemoveAt(randStoryIndex);
		storyContentsScript.storyRulesB.RemoveAt(randStoryIndex);

		// Enable navigation, so players can click to get started
		GameManager.enableNavigation = true;
	}


}
