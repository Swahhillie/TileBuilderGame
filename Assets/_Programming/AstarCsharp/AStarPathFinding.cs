using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
///based on http://www.untoldentertainment.com/blog/2010/08/20/introduction-to-a-a-star-pathfinding-in-actionscript-3-as3-2/
///http://theory.stanford.edu/~amitp/GameProgramming/Heuristics.html
/// </summary>
public class AStarPathFinding
{
	
	public delegate float HeuristicDelegate (PathNode start,PathNode end,float cost);// = ManhattenHeuristic;
	public delegate PathNode[] NodeFinder (PathNode fromNode);

	public static AStarSettings settings;
	private static AStarPathFinding.HeuristicDelegate heuristic;
	
	public enum Heuristic
	{
		Manhatten,
		Diagonal,
		Euclidian	
	}
	
	public static List<PathNode> FindPath (PathNode firstNode, PathNode destinationNode, NodeFinder nodeFinder)
	{
	
		SetHeuristic (settings.Heuristic);
		List<PathNode> openNodes = new List<PathNode> ();
		List<PathNode> closedNodes = new List<PathNode> ();
		PathNode currentNode = firstNode;
		PathNode[] connectedNodes;
		float g;
		float h;
		float f;
		
		currentNode.g = 0;
		currentNode.h = heuristic (currentNode, destinationNode, settings.TravelCost);
		currentNode.f = currentNode.g + currentNode.h;

		if (destinationNode.traversable == false)
			return null; //early out
			
			
		while (currentNode != destinationNode) {
			connectedNodes = nodeFinder (currentNode);
			int l = connectedNodes.Length;
			for (int i = 0; i < l; i++) {
				PathNode testNode = connectedNodes [i];
				if (testNode == null)
					continue;
				if (testNode == currentNode || testNode.traversable == false)
					continue;
				g = currentNode.g + heuristic (currentNode, testNode, AStarPathFinding.settings.TravelCost);
				h = heuristic (testNode, destinationNode, AStarPathFinding.settings.HeuristicCost) * settings.TieBreak;
				f = g + h;
				if (openNodes.Contains (testNode) || closedNodes.Contains (testNode)) {
					if (testNode.f > f) {
						testNode.f = f;
						testNode.g = g;
						testNode.h = h;
						testNode.parentNode = currentNode;
					}
				} else {
					testNode.f = f;
					testNode.g = g;
					testNode.h = h;
					testNode.parentNode = currentNode;
					openNodes.Add (testNode);
				}
			}
			closedNodes.Add (currentNode);
			if (openNodes.Count == 0) {
				Debug.Log ("worked through " + closedNodes.Count + "before no more possible");
				return null; // no more open to test. There is no path.
			}
			//openNodes.Sort(Compare);
			openNodes.Sort ((x, y) => x.f.CompareTo (y.f));
			
			currentNode = openNodes [0];
			openNodes.RemoveAt (0);
		}
		return AStarPathFinding.BuildPath (destinationNode, firstNode);
	}
	
	public static float ManhattenHeuristic (PathNode currentNode, PathNode destinationNode, float cost)
	{
		return Mathf.Abs ((currentNode.column - destinationNode.column)) * cost + Mathf.Abs ((currentNode.row - destinationNode.row)) * cost;
	}

	public static float EuclideanHeuristic (PathNode currentNode, PathNode destinationNode, float cost)
	{
		Vector2 p1 = new Vector2 (currentNode.column, currentNode.row);
		Vector2 p2 = new Vector2 (destinationNode.column, destinationNode.row);
		return (p1 - p2).magnitude * cost;
	}

	public static float DiagonalHeuristic (PathNode currentNode, PathNode destinationNode, float cost)
	{
		float diagonalCost = Mathf.Sqrt (2) * cost;
		int dx = Mathf.Abs (currentNode.column - destinationNode.column);
		int dy = Mathf.Abs (currentNode.row - destinationNode.row);
		return cost * (dx + dy) + (diagonalCost - 2 * cost) * Mathf.Min (dx, dy);
		/*
		int straight = (Mathf.Abs(currentNode.column - destinationNode.column) + Mathf.Abs(currentNode.row - destinationNode.row));
		int diagonal = Mathf.Min(Mathf.Abs(currentNode.column - destinationNode.column), Mathf.Abs(currentNode.row - destinationNode.row));
		return diagCost * diagonal + cost * (straight - 2 * diagonal);
		*/
		//return cost * Mathf.Max(Mathf.Abs(currentNode.column-destinationNode.column), Mathf.Abs(currentNode.row-destinationNode.row));
	}

	public static List<PathNode> BuildPath (PathNode destination, PathNode firstNode)
	{
		List<PathNode> path = new List<PathNode> ();
		PathNode node = destination;
		while (node != firstNode) {
			node = node.parentNode;
			path.Insert (0, node);
		}
		return path;
	}

	static int Compare (PathNode nodeA, PathNode nodeB)
	{
		return nodeA.f.CompareTo (nodeB.f);
	}

	static void Test (List<PathNode> list)
	{
		string output = "";
		for (int k = 0; k < list.Count; k++) {
			PathNode nd = list [k];
			output += nd.traversable + ",";
		}
		Debug.Log (output);
	}
	
	private static void SetHeuristic (Heuristic h)
	{
			
		switch (h) {
		case Heuristic.Manhatten:
			heuristic = ManhattenHeuristic;
			break;
				
		case Heuristic.Diagonal:
			heuristic = DiagonalHeuristic;
			break;
				
		case Heuristic.Euclidian:
			heuristic = EuclideanHeuristic;
			break;
		}
	}
}

