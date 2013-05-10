using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum AIState{MovingToDestination, Idle};

public class Actor : MonoBehaviour {
	//state
	private AIState _currentState = AIState.Idle;
	public AIState stateReadOnly = AIState.Idle; //for seeing in the inspector
	private bool _selected = false;
	public GameObject _highlight;
	
	
	//pathing
	private List<Tile> waypointQ = new List<Tile>();
	private Tile lastPathNode;
	private Tile currentNode;
	private Tile currentTarget;
	private Tile destination;
	
	
	//moving
	public float moveSpeed = 3.0f;
	private Vector3 movementDirection = Vector3.zero;
	public float rotSmooth = .5f;
	
	//links
	private RayPick picker;
	private CellManager manager;
	private ActorManagement actorManager;
	
	void Start () {
		GameObject mainGo = GameObject.FindGameObjectWithTag("MainObject");
		CellManager.AddGenerateLevelCallback(ResetPosition);
		manager = mainGo.GetComponent<Main>().GetCellManager();
		picker = mainGo.GetComponent<RayPick>();
		actorManager = mainGo.GetComponent<Main>().GetActorManager();
		actorManager.AddActor(this);
		lastPathNode = FindCurrentNode();
		
	}
	
	private void Update () {
		
		currentNode = FindCurrentNode();
		if(currentNode == null){
			Debug.LogWarning("not on a node");
			return;
		}
		if(selected){
			_highlight.renderer.enabled = true;
		}
		else{
			_highlight.renderer.enabled = false;
		}
		if(currentNode != lastPathNode){		
			lastPathNode = currentNode;
		}
		UpdateBehaviour();
		stateReadOnly = _currentState;
	}
	private void UpdateBehaviour(){
		switch(_currentState){
			case AIState.Idle: 
				
				break;
			case AIState.MovingToDestination: MoveAlongQ(); break;
		}
	}
	private Tile FindCurrentNode(){
		int[] coord = null;
		Ray downRay = new Ray(transform.position + Vector3.up, Vector3.down);
		Debug.DrawRay(downRay.origin, downRay.direction, Color.red);
		if(picker.PickWithRay(downRay, out coord)){
			Tile tile = manager.GetTile(coord);
			return tile;
		}
		else{
			//should not happen, this means the actor is not over a tile
			Debug.LogWarning("! Find current node returned null. Something is wrong !");
			return null;
		}
		
		
	}
	private void ResetPosition(){
		transform.position = new Vector3(.5f,0,.5f);
	}
	private void MoveAlongQ(){
		currentTarget = waypointQ[0];
		//Tile currentTargetTile = (Tile)currentTarget; // cast as tile to get access to center property
		
		Vector3 dirToTarget = currentTarget.center - new Vector3(transform.position.x, 0, transform.position.z);
		if(dirToTarget.magnitude > moveSpeed * Time.deltaTime){
			MoveForwardAlong(dirToTarget);
		}
		else{
			//note. currently skips one frame of movement when getting next target
			GetNextTarget();
		}
	}
	private void MoveForwardAlong(Vector3 direction){
		direction = Vector3.Normalize(direction);
		movementDirection = direction;
		direction *= moveSpeed;
		transform.position += direction * Time.deltaTime;
		FaceForward(movementDirection);
	}
	private void FaceForward(Vector3 direction){
		transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(transform.forward - direction, Vector3.up), rotSmooth);
	}
	private void GetNextTarget(){
		waypointQ.RemoveAt(0);
		if(waypointQ.Count > 0){
			//make sure the next node is still accesable from the current node
			if(HasAccessTo(currentTarget, waypointQ[0])){ 
				currentTarget = waypointQ[0];

			}
			else{
				//there is no connection from the current node to the next node in the Q.
				StartMoveTo(destination); //create an alternative path
			}
			
		}
		else{
			//at the end of the Q.
			GoIdle();
		}		
	}
	private bool HasAccessTo(Tile fromTile, Tile destination){
		if(destination == null || fromTile == null)return false; //cant access null tiles
		//Debug.Log(System.Array.IndexOf(fromTile.neighbours, destination));
		if(System.Array.IndexOf(fromTile.neighbours, destination) == -1)return false; //destination is not a neighbour of fromTile
		return true;
		
	}
	private void GoIdle(){
		_currentState = AIState.Idle;
	}
	public void StartMoveTo(Tile newDestination){
		List<PathNode> pns = AStarPathFinding.FindPath(currentNode, newDestination, manager.GetAccessableNeighbours);
		DebugAstar.path = pns;
		waypointQ.Clear();
		if(pns != null){
			waypointQ = new List<Tile>();
			foreach(PathNode pn in pns){
				waypointQ.Add(PathNodeToTile(pn));
			}
			//remove the first element because we are (surely) probably already there
			if(waypointQ.Count > 0)waypointQ.RemoveAt(0);
			//waypointQ = pns.ConvertAll<Tile>( new System.Converter<PathNode, Tile>(PahtNodeToTile));
			 
			
			
			_currentState = AIState.MovingToDestination;
			destination = newDestination;
			waypointQ.Add(destination); // adding the last node manually.
			
		}
		else{
			
			GoIdle();
			
		}
	}
	private static Tile PathNodeToTile(PathNode pn){
		return (Tile)pn;
	}
	public int[] coords{
		get{return new int[2]{currentNode.column, currentNode.row};}
	}
	public bool selected{
		get{return _selected;}
		set{
		_selected = value;
		}
	}
	public void OnDestroy(){
		actorManager.RemoveActor(this);
	}
}
