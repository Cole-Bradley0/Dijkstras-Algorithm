using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerAi : MonoBehaviour {

	public float gravity = 1f;
	CharacterController controller;
	public TileGenerator tileGenerator;

	public Vector3 target;
	public bool seek = false;
	public float moveSpeed = 2f;
	public float satisfactionRadius = 0.25f;

	// Pathfinding
	public List<Vector3> waypoints;
	public int currentWaypoint;
	public int xPos;
	public int yPos;
	public OrderedPair location;

	void Start () {
		controller = GetComponent<CharacterController> ();
		location = new OrderedPair(0,0);
	}

	void Update () {
		// Gravity
		controller.Move (new Vector3(0f, -gravity * Time.deltaTime, 0f));

		// Follow path
		if (seek) {
			if (!(currentWaypoint > waypoints.Count - 1)) {
				target = waypoints[currentWaypoint];
				Vector3 offset = (target - transform.position);
                offset.y = 0;
				controller.Move ((offset.normalized * moveSpeed) * Time.deltaTime);
				if (offset.magnitude < satisfactionRadius) {
					currentWaypoint++;
					location = new OrderedPair (Mathf.RoundToInt(target.x), Mathf.RoundToInt(target.z));
				}
			} else {
				seek = false;
				location = new OrderedPair (Mathf.RoundToInt(target.x), Mathf.RoundToInt(target.z));
			}
		}

		xPos = location.x;
		yPos = location.y;
	}

	public void FollowPath(List<Vector3> path){
		seek = false;
		currentWaypoint = 0;
		waypoints = path;
		seek = true;
	}
}

public class OrderedPair{
	public int x = 0;
	public int y = 0;

	public OrderedPair(int inputX, int inputY){
		x = inputX;
		y = inputY;
	}
}