using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile {
	public int x;
	public int y;

	public GameObject tileObject;

	public bool isObstacle = false;

	public Tile(int inputX, int inputY, GameObject inputTile, bool obstacle){
		x = inputX;
		y = inputY;
		tileObject = inputTile;
		isObstacle = obstacle;
		if (obstacle == true) {
			tileObject.GetComponent<Renderer> ().material.color = Color.red;
		}
	}
}

public class TileGenerator : MonoBehaviour {

	public int x;
	public int y;

	public List<Tile> tiles = new List<Tile>();

	private int currentX;
	private int currentY;

	public GameObject tile;

	void Start () {
		SpawnTile (currentX, currentY, false);
		currentX++;
		while (currentY < y) {
			while (currentX < x) {
				SpawnTile (currentX, currentY, Random.Range(0, 10) < 1);
				currentX++;
			}
			currentX = 0;
			currentY++;
		}
	}

	void SpawnTile(int inputX, int inputY, bool isObstacle){
		tiles.Add (new Tile(inputX, inputY, Instantiate (tile, new Vector3((float)inputX,0f,(float)inputY),
			Quaternion.Euler(new Vector3(90f,0f,0f))), isObstacle));
	}

}
