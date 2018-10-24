using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioQuestions : MonoBehaviour {

	private void Start() {

<<<<<<< HEAD
<<<<<<< HEAD
			{ new RadioQuestion(
				"What is Player One's best attribute?",
				new List<string> {
					"Punctuality",
					"Discipline",
					"Wisdom",
					"Tolerance"
				})
			},

			{ new RadioQuestion(
				"Who was better back in school?",
				new List<string> {
					"Player One",
					"Player Two",
					"We were both awful..",
					"We both rocked!"
				})
			},

			{ new RadioQuestion(
				"Which one of you is the party dude/dudess?",
				new List<string> {
					"Player One",
					"Player Two",
					"Both of us",
					"No one"
				})
			},

		};
=======
=======
>>>>>>> parent of e564554... Improved radio interface and reworked displaying answers
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
			"Summer" + "\n" +
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
		
<<<<<<< HEAD
>>>>>>> parent of e564554... Improved radio interface and reworked displaying answers
=======
>>>>>>> parent of e564554... Improved radio interface and reworked displaying answers
	}

}
