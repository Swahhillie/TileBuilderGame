using UnityEngine;
using System.Collections;

[System.SerializableAttribute()]
public class AStarSettings
{
	private AStarPathFinding.Heuristic _heuristic;
	private float _travelCost = 1.5f;
	private float _heuristicCost = 1.0f;
	private float _tieBreak = 1.0001f;

	public AStarPathFinding.Heuristic Heuristic {
		get{ return _heuristic;}
		set {
			if(_heuristic != value)
			{
				Debug.Log(string.Format("Changed heuristic from {0} --> {1}", _heuristic.ToString(), value.ToString()));
			}
			_heuristic = value;
			
		}
	}

	public float TravelCost {
		get{ return _travelCost;}
		set{ _travelCost = value;}
	}

	public float HeuristicCost {
		get{ return _heuristicCost;}
		set{ _heuristicCost = value;}
	}

	public float TieBreak {
		get{ return _tieBreak;}
		set{ _tieBreak = value;}
	}
}