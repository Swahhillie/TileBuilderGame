using UnityEngine;
using System.Collections;

public class AStarSettingControl : MonoBehaviour {

	// Use this for initialization
	public float heuristicCost = 1.0f;
	public float travelCost = 1.0f;
	
	public enum Heuristic{
		Manhatten,
		Diagonal,
		Euclidian	
	}
	
	public Heuristic heuristFunction;
	
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		AStarPathFinding.heuristicCost = heuristicCost;
		AStarPathFinding.travelCost = travelCost;
		
		switch(heuristFunction){
			case Heuristic.Manhatten:
				AStarPathFinding.heuristic = AStarPathFinding.ManhattenHeuristic;
				break;
			case Heuristic.Diagonal:
				AStarPathFinding.heuristic = AStarPathFinding.DiagonalHeuristic;
				break;
			case Heuristic.Euclidian:
				AStarPathFinding.heuristic = AStarPathFinding.EuclideanHeuristic;
				break;
		
		}
	}
}
