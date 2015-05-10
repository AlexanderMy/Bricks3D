using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameOverButton : MonoBehaviour {

	Button gameOverButton;
	void Awake(){
		gameOverButton = GetComponent<Button>();
		gameOverButton.onClick.AddListener(startNewGame);
	}

	void startNewGame(){
		ScoreManager.gameOver = false;
		Application.LoadLevel(Application.loadedLevel);
	}
}
