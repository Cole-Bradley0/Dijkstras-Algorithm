using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilesByGeneration : MonoBehaviour {

    public int gridWidth = 10;
    public int gridHeight = 10;

    public GameObject tileTemplate;

    public Dictionary<Node, GameObject> tilesToNode = new Dictionary<Node, GameObject>();

	public Dictionary<Vector3, Node> nodesByPosition = new Dictionary<Vector3, Node>();

	void Start () {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int z = 0; z < gridHeight; z++)
            {
                GameObject newTile = Instantiate(tileTemplate);
                newTile.transform.position = new Vector3(x, 0, z);
				if ((x+ (z%2)) % 2 == 0) {
					newTile.GetComponent<Renderer> ().material.color = Color.gray;
				}

                Node tileNode = new Node(newTile.transform.position);
                if((x > 1 || z > 1) && Random.Range(0, 11) <= 1)
                {
                    newTile.GetComponent<Renderer>().material.color = Color.red;
                    tileNode.obstacle = true;
                }
                nodesByPosition.Add(tileNode.position, tileNode);

                tilesToNode.Add(tileNode, newTile);
            }
        }

        foreach(Vector3 nodePosition in nodesByPosition.Keys)
        {
            Node currentNode = nodesByPosition[nodePosition];
            Dictionary<Node, float> weightedConnections = currentNode.connections;

            Node right = LookupNode(nodePosition + Vector3.right);
            if (right != null)
                currentNode.connections.Add(right, 1);

            Node left = LookupNode(nodePosition + Vector3.left);
            if (left != null)
                currentNode.connections.Add(left, 1);

            Node up = LookupNode(nodePosition + Vector3.forward);
            if (up != null)
            {
                currentNode.connections.Add(up, 1);
            }

            Node down = LookupNode(nodePosition + Vector3.back);
            if (down != null)
                currentNode.connections.Add(down, 1);
        }
	}

    public Node LookupNode(Vector3 lookup)
    {
		if (!nodesByPosition.ContainsKey (lookup)) {
			return null;
		} else {
			return nodesByPosition [lookup];
		}
    }

    public GameObject LookupTile(Vector3 lookup)
    {
        if (!tilesToNode.ContainsKey(LookupNode(lookup)))
        {
            return null;
        }
        else
        {
            return tilesToNode[LookupNode(lookup)];
        }
    }

	public List<Vector3> PathfindWithVectors(Vector3 start, Vector3 end){
		return Dijkstra.PathfindAStar(LookupNode(start), LookupNode(end));
	}
}
