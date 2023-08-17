using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dijkstra : MonoBehaviour
{
    public static List<Vector3> Pathfind(Node fromNode, Node toNode)
    {
        List<Vector3> waypoints = new List<Vector3>();

        List<PathfindingNode> openList = new List<PathfindingNode>();
        List<PathfindingNode> closedList = new List<PathfindingNode>();

        Dictionary<Node, PathfindingNode> pathfindingNodes = new Dictionary<Node, PathfindingNode>();

        pathfindingNodes.Add(fromNode, new PathfindingNode (fromNode));
        openList.Add(new PathfindingNode(fromNode));

        while (openList.Count > 0 && !DoesListContainNode(toNode, closedList))
        {
            openList.Sort();
            PathfindingNode smallestCostSoFar = openList[0];

            foreach(Node connectedNode in smallestCostSoFar.graphNode.connections.Keys)
            {
                if (!connectedNode.obstacle)
                {
                    if (!DoesListContainNode(connectedNode, closedList))
                    {
                        if (!DoesListContainNode(connectedNode, openList))
                        {
                            float costToConnectedNode = smallestCostSoFar.costSoFar + smallestCostSoFar.graphNode.connections[connectedNode];
                            PathfindingNode predecessor = smallestCostSoFar;

                            pathfindingNodes.Add(connectedNode, new PathfindingNode(connectedNode, costToConnectedNode, predecessor));
                            openList.Add(pathfindingNodes[connectedNode]);
                        }
                        else
                        {
                            // Is the connection from the currently processing node faster
                            // than the existing connection to this target node?
                            // If so, update the target node.

                            float currentCostToTarget = pathfindingNodes[connectedNode].costSoFar;
                            float costToTargetThroughCurrentNode = smallestCostSoFar.costSoFar + smallestCostSoFar.graphNode.connections[connectedNode];

                            if (costToTargetThroughCurrentNode < currentCostToTarget)
                            {
                                pathfindingNodes[connectedNode].costSoFar = costToTargetThroughCurrentNode;
                                pathfindingNodes[connectedNode].predecessor = smallestCostSoFar;
                            }
                        }
                    }
                }
            }
            closedList.Add(smallestCostSoFar);
            openList.Remove(smallestCostSoFar);

        }
        // Pathfinding complete

        // Destination node is on closed list
        // All of its predecessors are also on the closed list

        MonoBehaviour.print("Pathfinding nodes is size: " + pathfindingNodes.Count);
        
        for (PathfindingNode waypoint = pathfindingNodes[toNode]; waypoint != null; waypoint = waypoint.predecessor)
        {
            waypoints.Add(waypoint.graphNode.position);
        }

        waypoints.Reverse();

        return waypoints;
    }

    public static List<Vector3> PathfindAStar(Node fromNode, Node toNode)
    {
        List<Vector3> waypoints = new List<Vector3>();

        List<PathfindingNode> openList = new List<PathfindingNode>();
        List<PathfindingNode> closedList = new List<PathfindingNode>();

        Dictionary<Node, PathfindingNode> pathfindingNodes = new Dictionary<Node, PathfindingNode>();

        pathfindingNodes.Add(fromNode, new PathfindingNode(fromNode));
        openList.Add(new PathfindingNode(fromNode));

        while (openList.Count > 0 && !DoesListContainNode(toNode, closedList))
        {
            openList.Sort();
            PathfindingNode smallestCostSoFar = openList[0];

            foreach (Node connectedNode in smallestCostSoFar.graphNode.connections.Keys)
            {
                if (!connectedNode.obstacle)
                {
                    if (!DoesListContainNode(connectedNode, closedList))
                    {
                        if (!DoesListContainNode(connectedNode, openList))
                        {
                            float costToConnectedNode = smallestCostSoFar.costSoFar + smallestCostSoFar.graphNode.connections[connectedNode] + Vector3.Distance(connectedNode.position, toNode.position);
                            PathfindingNode predecessor = smallestCostSoFar;

                            pathfindingNodes.Add(connectedNode, new PathfindingNode(connectedNode, costToConnectedNode, predecessor));
                            openList.Add(pathfindingNodes[connectedNode]);
                        }
                        else
                        {
                            // Is the connection from the currently processing node faster
                            // than the existing connection to this target node?
                            // If so, update the target node.

                            float currentCostToTarget = pathfindingNodes[connectedNode].costSoFar + Vector3.Distance(connectedNode.position, toNode.position);
                            float costToTargetThroughCurrentNode = smallestCostSoFar.costSoFar + smallestCostSoFar.graphNode.connections[connectedNode];

                            if (costToTargetThroughCurrentNode < currentCostToTarget)
                            {
                                pathfindingNodes[connectedNode].costSoFar = costToTargetThroughCurrentNode;
                                pathfindingNodes[connectedNode].predecessor = smallestCostSoFar;
                            }
                        }
                    }
                }
            }
            closedList.Add(smallestCostSoFar);
            openList.Remove(smallestCostSoFar);

        }
        // Pathfinding complete

        // Destination node is on closed list
        // All of its predecessors are also on the closed list

        print("Pathfinding nodes is size: " + pathfindingNodes.Count);

        for (PathfindingNode waypoint = pathfindingNodes[toNode]; waypoint != null; waypoint = waypoint.predecessor)
        {
            waypoints.Add(waypoint.graphNode.position);
        }

        waypoints.Reverse();

        return waypoints;
    }

    private static bool DoesListContainNode(Node searchedNode, List<PathfindingNode> pathfindingNodeList)
    {
        foreach (PathfindingNode pathfindingNode in pathfindingNodeList)
            if (pathfindingNode.graphNode == searchedNode)
                return true;
        return false;
    }

}

public class PathfindingNode : System.IComparable<PathfindingNode>
{
    public Node graphNode;
    public float costSoFar;

    public PathfindingNode predecessor;

    public int CompareTo(PathfindingNode other)
    {
        return costSoFar.CompareTo(other.costSoFar);
    }

    public PathfindingNode(Node graphNode, float costSoFar, PathfindingNode predecessor)
    {
        this.graphNode = graphNode;
        this.costSoFar = costSoFar;
        this.predecessor = predecessor;
    }

    public PathfindingNode(Node graphNode)
    {
        this.graphNode = graphNode;
        costSoFar = 0f;
        predecessor = null;
    }
}

public class TileGraph
{
    public List<Vector3> points = new List<Vector3>();
}

public class Node
{
    public Node(Vector3 pos)
    {
        position = pos;
    }

    public Vector3 position;

    public Dictionary<Node, float> connections = new Dictionary<Node, float>();

    public bool obstacle;

}
