using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioQuestions : MonoBehaviour {

	public struct RadioQuestion {
		public string question;
		public List<string> answers;

		public RadioQuestion(string question, List<string> answers) {
			this.question = question;
			this.answers= answers;
		}
	}

	private void Start() {
		RadioManager.questionsArr = new List<RadioQuestion> {

			{ new RadioQuestion(
				"What would you do when you find out that a close friend has incurable cancer?",
				new List<string> {
					"Spend more time with her/him.",
					"Don’t spend any more time.",
					"Slowly distance myself from her/him.",
					"I wouldn't change anything."
				})
			},

			{ new RadioQuestion(
				"What is your favorite season?",
				new List<string> {
					"Spring",
					"Summer",
					"Fall",
					"Winter"
				})
			},

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
				"Who do you think is the party dude/dudess among you?",
				new List<string> {
					"Player One",
					"Player Two",
					"Both of us",
					"No one"
				})
			},

			{ new RadioQuestion(
				"What is Player Two's preferred food?",
				new List<string> {
					"Italian",
					"Asian",
					"American",
					"German"
				})
			},

		};
	}
}







	// public static List<string[]> testList = new List<string[]>();


	// private void Start() {

	// 	for (int i = 0; i < 6; i++) {
	// 		Radio.questionsArr.Add(new List<string>());
	// 	}

	// 	Radio.questionsArr[0].AddRange(new string[] {
	// 		"If you find out that a very close friend has incurable cancer – what would you do?",
	// 		"Spend more time with her/him.",
	// 		"Don’t spend any time anymore.",
	// 		"Slowly distance myself from her/him.",
	// 		"I would not change anything."
	// 	});

	// 	Radio.questionsArr[1].AddRange(new string[] {
	// 		"Which is your favorite season?",
	// 		"Spring",
	// 		"Summer",
	// 		"Fall",
	// 		"Winter"
	// 	});

	// 	Radio.questionsArr[2].AddRange(new string[] {
	// 		"What is Player One's best attribute of the followoing?",
	// 		"Punctuality",
	// 		"Discipline",
	// 		"Wisdom",
	// 		"Tolerance"
	// 	});

	// 	Radio.questionsArr[3].AddRange(new string[] {
	// 		"Which one of you is the party dude/dudess?",
	// 		"Player One",
	// 		"Player Two",
	// 		"Both of us",
	// 		"No one"
	// 	});

	// 	Radio.questionsArr[4].AddRange(new string[] {
	// 		"Who was better back in school?",
	// 		"Player One",
	// 		"Player Two",
	// 		"We were both awful..",
	// 		"We both rocked!"
	// 	});

	// 	Radio.questionsArr[5].AddRange(new string[] {
	// 		"Who was better back in school?",
	// 		"Player One",
	// 		"Player Two",
	// 		"We were both awful..",
	// 		"We both rocked!"
	// 	});


	// 	// Radio.questionsArr.AddRange(new string[] {
	// 	// 	"If you find out that a very close friend has incurable cancer – what would you do?",
	// 	// 	"Which is your favorite season?",
	// 	// 	"Which one of you is the party dude/dudess?",
	// 	// 	"Who was better back in school?",
	// 	// 	"What fruit do you prefer?"
	// 	// });

	// 	// Radio.answersArr.AddRange(new string[] {
	// 	// 	"Spend more time with her/him." + "\n" +
	// 	// 	"Don’t spend any time anymore." + "\n" +
	// 	// 	"Slowly distance myself from her/him." + "\n" +
	// 	// 	"I would not change anything.",

	// 	// 	"Spring" + "\n" +
	// 	// 	"Summer" + "\n" +
	// 	// 	"Fall" + "\n" +
	// 	// 	"Winter",

	// 	// 	"Player One" + "\n" +
	// 	// 	"Player Two" + "\n" +
	// 	// 	"Both of us" + "\n" +
	// 	// 	"No one",

	// 	// 	"Player One" + "\n" +
	// 	// 	"Player Two" + "\n" +
	// 	// 	"We were both awful.." + "\n" +
	// 	// 	"We both rocked!",

	// 	// 	"Apples" + "\n" +
	// 	// 	"Bananas" + "\n" +
	// 	// 	"Strawberries" + "\n" +
	// 	// 	"Melons"
	// 	// });
		
	// }