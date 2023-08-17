using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickOnTile : MonoBehaviour {

	public CharacterControllerAi ai;

    public TilesByGeneration generatedTiles;

    void Start()
    {
		GameObject generatorObject = GameObject.FindGameObjectWithTag("generatorObject");
        generatedTiles = generatorObject.GetComponent<TilesByGeneration>();
    }

    void Update () {
		if (Input.GetMouseButtonDown (0)) {
			RaycastHit hit;
			if (Physics.Raycast (GetComponent<Camera> ().ScreenPointToRay(Input.mousePosition), out hit)) {
				if (hit.transform.tag == "tile") {
					Vector3 currentPosition = new Vector3 (ai.xPos, 0f, ai.yPos);
					ai.FollowPath(generatedTiles.PathfindWithVectors (currentPosition, hit.transform.position));
				}
			}
		}
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            if (Physics.Raycast(GetComponent<Camera>().ScreenPointToRay(Input.mousePosition), out hit))
            {
                if (hit.transform.tag == "tile")
                {
                    bool isObstacle = generatedTiles.LookupNode(hit.transform.position).obstacle;
                    generatedTiles.LookupNode(hit.transform.position).obstacle = !isObstacle;
                    if (!isObstacle)
                    {
                        generatedTiles.LookupTile(hit.transform.position).GetComponent<Renderer>().material.color = Color.red;
                    }
                    else
                    {
                        if ((generatedTiles.LookupNode(hit.transform.position).position.x + (generatedTiles.LookupNode(hit.transform.position).position.z % 2)) % 2 == 0)
                        {
                            generatedTiles.LookupTile(hit.transform.position).GetComponent<Renderer>().material.color = Color.gray;
                        }
                        else
                        {
                            generatedTiles.LookupTile(hit.transform.position).GetComponent<Renderer>().material.color = Color.white;
                        }
                    }
                }
            }
        }
    }

}
