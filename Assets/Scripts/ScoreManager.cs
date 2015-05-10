using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

	public static int score;
	public static bool gameOver;
	public GameObject gameOverCanvas;
	Text text;

	void Awake(){
		gameOver = false;
		// Set up the reference.
		text = GetComponent<Text>();
		gameOverCanvas.SetActive(false);
		score = 0;
	}
	
	// Update is called once per frame
	void Update () {
		// Set the displayed text to be the word "Score" followed by the score value.
		text.text = "Scores : " + score;
		if(gameOver){
			showGameOverDialog();
		}
	}

	void showGameOverDialog ()
	{
		gameOverCanvas.SetActive(true);
	}
}
