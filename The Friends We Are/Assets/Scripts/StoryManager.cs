using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class StoryManager : MonoBehaviour {

	public static List<string> storyLevel = new List<string>();


	private void Awake() {
		// Get all names of the stories in the Story folder
		string mapsFolder = Application.dataPath + "/Scenes/5 Stories";
		var mapsDirInfo = new DirectoryInfo(mapsFolder);
		var allMapsFileInfos = mapsDirInfo.GetFiles("*.unity", SearchOption.AllDirectories);
		foreach (var fileInfo in allMapsFileInfos) {
			storyLevel.Add(Path.GetFileNameWithoutExtension(@fileInfo.Name));
		}
	}

}
