using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

	public static int score;
	Text text;

	void Awake(){
		// Set up the reference.
		text = GetComponent<Text>();

		// Reset the score.
		score = 0;
	}
	
	// Update is called once per frame
	void Update () {
		// Set the displayed text to be the word "Score" followed by the score value.
		text.text = "Score : " + score;	
	}
}
