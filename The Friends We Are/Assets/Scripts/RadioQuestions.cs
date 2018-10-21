using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioQuestions : MonoBehaviour {

	private void Start() {

		Radio.questionsArr.AddRange(new string[] {
			"If you find out that a very close friend has incurable cancer – what would you do?",
			"Which is your favorite season?",
			"Which one of you is the party dude/dudess?",
			"Who was better back in school?",
			"What fruit do you prefer?"
		});

		Radio.answersArr.AddRange(new string[] {
			"Spend more time with her/him." + "\n" +
			"Don’t spend any time anymore." + "\n" +
			"Slowly distance myself from her/him." + "\n" +
			"I would not change anything.",

			"Spring" + "\n" +
			"Sommer" + "\n" +
			"Fall" + "\n" +
			"Winter",

			"Player One" + "\n" +
			"Player Two" + "\n" +
			"Both of us" + "\n" +
			"No one",

			"Player One" + "\n" +
			"Player Two" + "\n" +
			"We were both awful.." + "\n" +
			"We both rocked!",

			"Apples" + "\n" +
			"Bananas" + "\n" +
			"Strawberries" + "\n" +
			"Melons"
		});
		
	}

}
