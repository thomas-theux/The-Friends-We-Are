using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryContent : MonoBehaviour {

	public List<string> storyTitles = new List<string> {
		"Driving",
		"Shopping"
	};

	public List<string> storyTexts = new List<string> {
		"Leaving home with this 'borrowed' school bus is the first of many adventures.",
		"If you're going on a road trip you need to eat and drink, right?"
	};

	public List<string> storyGoals = new List<string> {
		"Try and collect as much Road Experience as you can to increase your friends score.",
		"Various grocery items will pop up on your list and you want to get as many as you can."
	};

	public List<string> storyTasksA = new List<string> {
		"Accelerating & Braking",
		"Walking & Collecting"
	};

	public List<string> storyTasksB = new List<string> {
		"Steering",
		""
	};

	public List<Sprite> storyRulesA;
	public List<Sprite> storyRulesB;

}
