using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MobileController : MonoBehaviour {
	public Button upArrow;
	public Button downArrow;
	public Button leftArrow;
	public Button rightArrow;
	public Button fallDownArrow;
	public GameController gameController;
	GameObject mobileController;


	void Awake(){
//		mobileController = GetComponent<GameObject>();
//
//		mobileController.SetActive(Constants.useMobileController);
		InitButtonsClicks();
	}

	void InitButtonsClicks ()
	{
		upArrow.onClick.AddListener(MoveUp);
		downArrow.onClick.AddListener(MoveDown);
		leftArrow.onClick.AddListener(MoveLeft);
		rightArrow.onClick.AddListener(MoveRight);
		fallDownArrow.onClick.AddListener(FallQuick);
	}

	void MoveDown(){
		gameController.Move(MoveDirection.DOWN);
	}

	void MoveUp(){
		gameController.Move(MoveDirection.UP);
	}

	void MoveLeft(){
		gameController.Move(MoveDirection.LEFT);
	}

	void MoveRight(){
		gameController.Move(MoveDirection.RIGHT);
	}

	void FallQuick(){
		gameController.Move(MoveDirection.FALL_QUICK);
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
