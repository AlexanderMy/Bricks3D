using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum MoveDirection{
	LEFT, RIGHT, UP, DOWN, FALL, FALL_QUICK
}
public enum RotateDirection{
	PLUS_X, MINUS_X, PLUS_Z, MINUS_Z
}

public class GameController : MonoBehaviour {
	public GameObject [] figures;
	GameObject currentFigure;
	GameObject [] currentFigureCubes;
	int scores = 0;
	Vector3 _spawnPosition = new Vector3(
		Constants.X_CELLS * Constants.X_CELL_SIZE / 2,
		Constants.Y_CELLS * Constants.Y_CELL_SIZE + 2,
		Constants.Z_CELLS * Constants.Z_CELL_SIZE / 2);

	float _gameSpeed = 0.5f;
	float _quickFallSpeed = 0.005f;
	bool _quickFall;
	float _nextMove;
	float _nextQuckMove;
	public GameObject [,,] cubes = new GameObject[Constants.X_CELLS, Constants.Y_CELLS, Constants.Z_CELLS];
	IList presentFigures = new ArrayList();

	// Use this for initialization
	void Start () {
		InstantiateFigure();
	}

	void InstantiateFigure(){
		int val = (int)Random.Range(0.0f, figures.Length - 0.1f);
		currentFigure = (GameObject)Instantiate(figures[val], _spawnPosition, Quaternion.identity);
		presentFigures.Add(currentFigure);
		currentFigureCubes = currentFigure.GetComponent<Figure>().cubes;
	}
	
	// Update is called once per frame
	void Update () {
		if(!ScoreManager.gameOver){
			if(calculateShouldFall()){
				FallFigureStep();
			}
			HorizontalMove();
			Rotate();
		}
	}

	bool calculateShouldFall(){
		if(_quickFall){
			if(Time.time > _nextQuckMove){
				_nextQuckMove = Time.time + _quickFallSpeed;
				return true;
			}
		}
		if(Time.time > _nextMove){
			_nextMove = Time.time + _gameSpeed;
			return true;
		}
		return false;
	}

	#region Moving
	void HorizontalMove ()
	{
		if(Input.GetKeyDown(KeyCode.DownArrow))
			Move(MoveDirection.DOWN);
		if(Input.GetKeyDown(KeyCode.UpArrow))
			Move(MoveDirection.UP);
		if(Input.GetKeyDown(KeyCode.LeftArrow))
			Move(MoveDirection.LEFT);
		if(Input.GetKeyDown(KeyCode.RightArrow))
			Move(MoveDirection.RIGHT);
		if(Input.GetKeyDown(KeyCode.Space))
			Move(MoveDirection.FALL_QUICK);
	}
	
	public void Move(MoveDirection moveDirection){
		if(canMove(moveDirection)){
			Vector3 position = currentFigure.transform.position;
			Vector3 movePosition = position;
			switch(moveDirection){
			case MoveDirection.DOWN:
				position.x += Constants.X_CELL_SIZE;
				break;
			case MoveDirection.UP:
				position.x -= Constants.X_CELL_SIZE;
				break;
			case MoveDirection.RIGHT:
				position.z += Constants.X_CELL_SIZE;
				break;
			case MoveDirection.LEFT:
				position.z -= Constants.X_CELL_SIZE;
				break;
			case MoveDirection.FALL_QUICK:
				_quickFall = true;
				break;
			}
			currentFigure.transform.position = position;
		}
	}
	#endregion
	#region Rotation
	void Rotate(){
		if(Input.GetKeyDown(KeyCode.W))
			Rotate(RotateDirection.PLUS_Z);			
		if(Input.GetKeyDown(KeyCode.S))
			Rotate(RotateDirection.MINUS_Z);			
		if(Input.GetKeyDown(KeyCode.A))
			Rotate(RotateDirection.PLUS_X);
		if(Input.GetKeyDown(KeyCode.D))
			Rotate(RotateDirection.MINUS_X);
	}

	//TODO if needed
	void Rotate(RotateDirection direction){
		return;
		if(!canRotate(direction))
			return;
		float xRotation = 0.0f;
		float zRotation = 0.0f;
		float yRotation = 0.0f;

		switch (direction){
		case RotateDirection.PLUS_X:
			xRotation += Constants.ROTATION_STEP;
			break;
		case RotateDirection.MINUS_X:
			xRotation -= Constants.ROTATION_STEP;
			break;
		case RotateDirection.PLUS_Z:
			zRotation += Constants.ROTATION_STEP;
			break;
		case RotateDirection.MINUS_Z:
			zRotation -= Constants.ROTATION_STEP;
			break;
		}
		currentFigure.transform.Rotate(xRotation, currentFigure.transform.rotation.y, zRotation);
	}
	bool canRotate(RotateDirection direction){
		return true;
	}

	#endregion
	bool canMove(MoveDirection direction){
		foreach(GameObject cube in currentFigureCubes){
			Vector3 cubePosition = cube.transform.position;
			int x = Mathf.FloorToInt(cubePosition.x / Constants.X_CELL_SIZE);
			int y = Mathf.FloorToInt(cubePosition.y / Constants.Y_CELL_SIZE);
			int z = Mathf.FloorToInt(cubePosition.z / Constants.Z_CELL_SIZE);
			Debug.Log("x=" + x + "; y=" + y + "; z=" + z);
			switch(direction){
				case MoveDirection.DOWN:
				{
					if(x == Constants.X_CELLS -1 || cubes[x, y - 2, z] != null)
						return false;
					break;
				}
				case MoveDirection.UP:
				{
					if(x == 0 || cubes[x-1, y, z] != null)
						return false;
					break;
				}
				case MoveDirection.RIGHT:
				{
					if(z == Constants.Z_CELLS -1 || cubes[x, y, z+1] != null)
						return false;					
					break;
				}
				case MoveDirection.LEFT:
				{
					if(z == 0 || cubes[x, y, z-1] != null)
						return false;					
					break;
				}
				case MoveDirection.FALL:
				{
					if(y == 0 || cubes[x, y-1, z] != null)
						return false;					
					break;
				}
			}
		}
		Debug.Log("");
		return true;
	}

	void FallFigureStep(){
		Vector3 currentPosition = currentFigure.transform.position;
		if(canMove(MoveDirection.FALL)){
			currentPosition.y -= Constants.Y_CELL_SIZE;
			currentFigure.transform.position = currentPosition;
		}else{
			_quickFall = false;
			AddCubesToMatrix();
			CheckLines();
			if(!ScoreManager.gameOver){
				InstantiateFigure();
			}
		}
	}

	void AddCubesToMatrix(){
		if(CheckGameOver())
			return;

		foreach(GameObject cube in currentFigureCubes){
			Vector3 cubePosition = cube.transform.position;
			int x = Mathf.FloorToInt(cubePosition.x / Constants.X_CELL_SIZE);
			int y = Mathf.FloorToInt(cubePosition.y / Constants.Y_CELL_SIZE);
			int z = Mathf.FloorToInt(cubePosition.z / Constants.Z_CELL_SIZE);
			cubes[x, y, z] = cube;
		}
	}

	void CheckLines(){
		int filledLines = 0;
		int bottomY = -1;
		for(int y = 0; y < Constants.Y_CELLS; y++){
			if(IsLinesFilled(y)){
				if(bottomY == -1)
					bottomY = y;
				filledLines ++;
			}
		}

		ShiftLinesDown(bottomY, filledLines);
		CheckGameOver();
	}

	bool CheckGameOver ()
	{
		for(int x = 0; x < Constants.X_CELLS; x++){
			for(int z = 0; z < Constants.Z_CELLS; z++){
				if(cubes[x, Constants.Y_CELLS - 1, z] != null){
					ScoreManager.gameOver = true;
					return true;
				}
			}
		}
		return false;
	}

	bool IsLinesFilled(int y){
		int count = 0;
		for(int x = 0; x < Constants.X_CELLS; x++){
			for(int z = 0; z < Constants.Z_CELLS; z++){
				if(cubes[x, y, z] != null)
					count++;
			}
		}
		Debug.Log("cubes on y = " + y + " - " + count + "; maxCubes = " + Constants.X_CELLS * Constants.Z_CELLS);
		if(count == Constants.X_CELLS * Constants.Z_CELLS){
			RemoveLine(y);
			return true;
//			checkLines();
		}
		return false;
	}

	void RemoveLine (int y)
	{
		for(int x = 0; x < Constants.X_CELLS; x++){
			for(int z = 0; z < Constants.Z_CELLS; z++){
				Destroy(cubes[x, y, z]);
			}
		}
		Debug.Log("Will be removed line: " + y);
//		RemoveEmptyFigures();
//		ShiftLinesDown(y);
		UpdateScores();
	}

	void ShiftLinesDown (int y, int count)
	{
		if(y == -1)
			return;
		for(int i = 0; i <= count; i++)
		{
			for(; y < Constants.Y_CELLS - 1; y++)
			{
				for(int x = 0; x < Constants.X_CELLS; x++)
				{
					for(int z = 0; z < Constants.Z_CELLS; z++)
					{
						GameObject shufleCube = cubes[x, y+1, z];
						cubes[x, y, z] = cubes[x, y+1, z];

						if(cubes[x, y, z] == null)
							continue;

						Vector3 pos = cubes[x, y, z].transform.position;
						pos.y -= Constants.Y_CELL_SIZE;
						cubes[x, y, z].transform.position = pos;
					}
				}
			}
		}
	}

	void RemoveEmptyFigures(){
		//TODO
		for(int i = 0; i < presentFigures.Count; i++){
			Figure fig = ((GameObject)presentFigures[i]).GetComponent<Figure>();
			bool isFigureEmpty = false;
			Debug.Log("check figure: " + fig);

			foreach(var cube in fig.cubes){
				Debug.Log("check cube: " + cube);
				if(cube != null){
					isFigureEmpty = false;
					break;
				}else{
					isFigureEmpty = true;
				}
			}

			if(isFigureEmpty){
				presentFigures.Remove(fig);
				Destroy(fig);
				Debug.Log("will be removed: " + fig);
			}
		}
	}

	void UpdateScores(){
		_gameSpeed -= ScoreManager.score / 50000;
		ScoreManager.score += 100;
	}
}
