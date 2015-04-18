using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	public float speed = 10.0f;
	public GameObject figureLine;

	private Vector3 spawnPosition = new Vector3(35.0f, 155.0f, 35.0f);

	public GameObject[,,] cubes = new GameObject[7, 10, 7];
	// Use this for initialization
	void Start () {
		InstantiateFigure();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate()
	{

	}

	public void InstantiateFigure()
	{
		GameObject gObject = (GameObject)Instantiate(figureLine, spawnPosition, Quaternion.identity);
		FigureController figureController = gObject.GetComponent<FigureController>();
		figureController.controller = this;
	}

	public bool canFallDown(GameObject [] cubes){
		foreach(var cube in cubes){
			Debug.Log(cube.transform.position);
			Vector3 cubePosition = cube.transform.position;

		}
		return false;
	}
}
