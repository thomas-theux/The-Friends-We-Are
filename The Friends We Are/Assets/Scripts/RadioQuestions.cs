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