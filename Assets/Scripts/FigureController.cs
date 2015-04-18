using UnityEngine;
using System.Collections;

[System.Serializable]
public class Boundary
{
	public float xMin, xMax, zMin, zMax;
}

public class FigureController : MonoBehaviour {

	public float speed;
	public Boundary boundary;
	public bool controlledByPlayer = true;
	public bool shouldFall = true;
	public float fallSpeed = 10.0f;
	public GameObject [] cubes;
	public GameController controller;

	void FixedUpdate()
	{
		if(controlledByPlayer)
		{
			HorizontalMovement();
			RotaionMovement();
		}
		if(shouldFall)
		{
			FallFigureDown();
		}
	}

	private void HorizontalMovement(){
		float zPosition = 0.0f;
		float xPosition = 0.0f;

		if(Input.GetKeyDown(KeyCode.UpArrow))
			zPosition = -Constants.MOVE_STEP;
		if(Input.GetKeyDown(KeyCode.DownArrow))
			zPosition = Constants.MOVE_STEP;
		if(Input.GetKeyDown(KeyCode.LeftArrow))
			xPosition = Constants.MOVE_STEP;
		if(Input.GetKeyDown(KeyCode.RightArrow))
			xPosition = -Constants.MOVE_STEP;

		if(zPosition == 0.0f && xPosition == 0.0f)
			return;

		Vector3 currentPosition = gameObject.transform.position;

		Vector3 movePosition = new Vector3(
			Mathf.Clamp(currentPosition.x + xPosition, boundary.xMin, boundary.xMax), 
			currentPosition.y,
			Mathf.Clamp(currentPosition.z + zPosition, boundary.zMin, boundary.zMax));

		gameObject.transform.position = movePosition;
	}

	void RotaionMovement ()
	{
		float xRotation = 0.0f;
		float zRotation = 0.0f;
		if(Input.GetKeyDown(KeyCode.W))
			zRotation = Constants.ROTATION_STEP;
		if(Input.GetKeyDown(KeyCode.S))
			zRotation = -Constants.ROTATION_STEP;
		if(Input.GetKeyDown(KeyCode.A))
			xRotation = -Constants.ROTATION_STEP;
		if(Input.GetKeyDown(KeyCode.D))
			xRotation = Constants.ROTATION_STEP;
		if(zRotation == 0.0f && xRotation == 0.0f)
			return;

		Quaternion currentRotation = gameObject.transform.rotation;
		gameObject.transform.Rotate(xRotation, currentRotation.y, zRotation);
	}

	IEnumerator FallFigureDown()
	{
		while(shouldFall)
		{
			Vector3 currentPosition = gameObject.transform.position;
			if(currentPosition.y <= 15.0f || controller != null && controller.canFallDown(cubes)){
				shouldFall = false;
				controlledByPlayer = false;
				if(controller != null){
					controller.InstantiateFigure();
				}
				break;
			}
			gameObject.transform.position = new Vector3(
				currentPosition.x,
				currentPosition.y - Constants.MOVE_STEP,
				currentPosition.z);
			yield return new WaitForSeconds (fallSpeed);
		}
	}

	// Use this for initialization
	void Start () {
		StartCoroutine(FallFigureDown());
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
