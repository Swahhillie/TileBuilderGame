using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {

	// Use this for initialization
	private ActorManagement _actorManager;
	private CellManager _cellManager;
	
	void Awake(){
		_actorManager = GetComponent<ActorManagement>();
		_cellManager = GetComponent<CellManager>();
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public ActorManagement GetActorManager(){
		return _actorManager;
	}
	public CellManager GetCellManager(){
		return _cellManager;
	}
}
