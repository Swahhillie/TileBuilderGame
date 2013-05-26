using UnityEngine;
using System.Collections;

public class TileEditor : MonoBehaviour {
	
	//targets and tools
	private CellLayer _toolTarget;
	private ToolType  _toolType;
	
	// links
	TileSelector tileSelector;
	ActorManagement actorManagement;
	CellManager cellManager;
	Menu menu;
	
	void Awake(){
		
		Controller.onLeftMouseReleaseFunctions.Add(OnLeftMouseRelease);
		Controller.onRightMouseReleaseFunctions.Add(OnRightMouseRelease);
	
	}
	void Start () {
		
		cellManager = GetComponent<CellManager>();
		tileSelector = GetComponent<TileSelector>();
		actorManagement = GetComponent<ActorManagement>();
		menu = GetComponent<Menu>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnRightMouseRelease(){
		
		/*Tile[] ts = cellManager.GetAccessableNeighbours(cellManager.GetTile(tileSelector.selections.current));
		Color c = new Color(Random.value, Random.value, Random.value, 1.0f);
		if(ts != null){
			for(int i = 0 ; i < ts.Length; i++){
				
				if(ts[i] != null)ts[i].DebugColor(c);
			}
		}
		else Debug.Log("ts is null");
		*/
		
		switch(_toolType){
			case ToolType.Pathing:
				MoveActors();
				
				break;
			case ToolType.Builder:
				
				break;
			case ToolType.Remover:
				
				break;
		}
		
	}
	void OnLeftMouseRelease(){
		switch(_toolType){
			case ToolType.Pathing:
				SelectActors();
				break;
			case ToolType.Builder:
				BuildSomething();
				break;
			case ToolType.Remover:
				RemoveSomething();
				break;
		}
	}
	
	void SelectActors(){
		actorManagement.SelectActors(tileSelector.selections.clickAndDragSelection);
		//cellManager.GetTile(tileSelector.selections.current).DebugColor(Color.red, true);
	}
	void MoveActors(){
		if(tileSelector.selections.current != null)
			actorManagement.MoveSelectedActors(tileSelector.selections.current);
	}
	void BuildSomething(){
		SendBuildVisitor();
//		Tile targetTile = cellManager.GetTile(tileSelector.selections.current);
//		if(targetTile != null)SendBuildVisitor(targetTile, toolTarget);
	}
	void RemoveSomething(){
		Tile targetTile = cellManager.GetTile(tileSelector.selections.current);
		SendRemoveVisitor();
		//the editor does not know how to handle the building of all objects.the implementation depends on the object itself
	}
	
	/*
		switch(toolTarget){
			case CellLayer.Block:
				BuildBlock();
				break;
			case CellLayer.Floor:
				BuildFloor();
				break;
			case CellLayer.Wall:
				BuildWall();
				break;
		}
		switch(toolTarget){
			case CellLayer.Block:
				RemoveBlock();
				break;
			case CellLayer.Floor:
				RemoveFloor();
				break;
			case CellLayer.Wall:
				RemoveWall();
				break;
		}
	
	//building stuff
	void BuildBlock(){
	
	}
	void BuildFloor(){
	
	}
	void BuildWall(){
		
	}
	//removing stuff
	void RemoveBlock(){
	
	}
	void RemoveFloor(){
	
	}
	void RemoveWall(){
	
	}
	*/
	//---------------- current target and tools
	private void SendBuildVisitor(){
		BuildVisitor bv = new BuildVisitor(tileSelector.selections, this, cellManager, menu);
		//populate the bv
		
		//send the bv on its way;
	}
	private void SendRemoveVisitor(){
		RemoveVisitor rv = new RemoveVisitor(tileSelector.selections, this, cellManager, menu);
		//populate the remove vistor
	}
	public CellLayer toolTarget{
		get{return _toolTarget;}
		set{
			Debug.Log("changed toolTarget to " + value.ToString());
			_toolTarget = value;
			}
	}
	public ToolType toolType{
		get{return _toolType;}
		set{
			//Debug.Log("changed tooltype to " + value.ToString());
			_toolType = value;
		}
	}
}
