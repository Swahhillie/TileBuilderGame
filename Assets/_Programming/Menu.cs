using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {
	
	//private CellEditor cellEditor;
	private bool selectingPrefab = false;
	private CellLayer selectingPrefabType;
	
	private string[][] allPrefabNames;
	private int[] selectors;
	
	private CellManager manager;
	private ActorManagement actorManager;
	private TileEditor tileEditor;
	
	private string widthString;
	private string heightString;
	
	public Rect toolSelectRect;
	
	void Start(){
		GameObject mainGo = GameObject.FindGameObjectWithTag("MainObject");
		Main main = mainGo.GetComponent<Main>();
				
		manager = main.GetCellManager();
		widthString = manager.width.ToString();
		heightString = manager.height.ToString();
		actorManager = main.GetActorManager();
		tileEditor = GetComponent<TileEditor>();
		
		//cellEditor = GetComponent<CellEditor>();
		
		string[] walls = PrefabBank.GetKeys(CellLayer.Wall);
		string[] floors = PrefabBank.GetKeys(CellLayer.Floor);
		string[] blocks = PrefabBank.GetKeys(CellLayer.Block);
		selectors = new int[3]{0,0,0};
		
		
		allPrefabNames = new string[3][]{
			floors,
			walls,
			blocks
		};
		
	}
	private void Update(){
		ListenForModifiers();
	}
	private void ListenForModifiers(){
		if(Input.GetButtonDown("DeleteModifier")){
			//SwitchToolType();
		}
		if(Input.GetButtonUp("DeleteModifier")){
			//SwitchToolType();
		}
	}
	/*
	private void SwitchToolType(){
		switch(cellEditor.selectedTool){
			case ToolType.Builder:
				SelectTool(ToolType.Remover);
				break;
			case ToolType.Remover:
				SelectTool(ToolType.Builder);
				break;
		}
	}
	*/
	private void OnGUI(){
		ToolSelectionMenu();
		
	}
	private void ToolSelectionMenu(){
		GUILayout.BeginArea(toolSelectRect);
		GUILayout.BeginVertical("box");
		DrawGenerateBox();
		DrawCurrentToolBox();
		if(GUILayout.Button("Build")){
			SelectTool(ToolType.Builder);
		}
		if(GUILayout.Button("Remove")){
			selectingPrefab = false;
			SelectTool(ToolType.Remover);
		}
		if(GUILayout.Button("Pathing")){
			selectingPrefab = false;
			SelectTool(ToolType.Pathing);	
		}
		if(GUILayout.Button("Select Wall")){
			selectingPrefab = true;
			SelectTool(ToolType.Builder);
			SelectToolTarget(CellLayer.Wall);
			selectingPrefabType = CellLayer.Wall;
		}
		if(GUILayout.Button("Select Floor")){
			selectingPrefab = true;
			SelectTool(ToolType.Builder);
			SelectToolTarget(CellLayer.Floor);
			selectingPrefabType = CellLayer.Floor;
		}
		if(GUILayout.Button("Select Block")){
			selectingPrefab = true;
			SelectTool(ToolType.Builder);
			SelectToolTarget(CellLayer.Block);
			selectingPrefabType = CellLayer.Block;
		}
		
		GUILayout.Space(20.0f);
		if(selectingPrefab)PrefabSelectionMenu(selectingPrefabType);
		//GUILayout.Label(cellEditor.selectedTool.ToString());
		GUILayout.EndVertical();
		GUILayout.EndArea();
	}
	private void DrawGenerateBox(){
		GUILayout.BeginHorizontal("box");
		if(GUILayout.Button("Generate")){
			int w = 0;
			int h = 0;
			try{
				 w = System.Convert.ToInt32(widthString);
			}
			catch(System.FormatException e){
				
				w = manager.width;
				widthString = w.ToString();
			}
			try{
				h = System.Convert.ToInt32(heightString);
			}
			catch(System.FormatException e){
				
				h = manager.height;
				heightString = h.ToString();
			}
			
			manager.GenerateTiles(w,h);
		}
		widthString = GUILayout.TextField(widthString);
		heightString = GUILayout.TextField(heightString);
		GUILayout.EndHorizontal();
	}
	private void DrawCurrentToolBox(){
		GUILayout.Label("Current Tool: " + tileEditor.toolType + (tileEditor.toolType == ToolType.Pathing ? "" : " -> " + tileEditor.toolTarget));
	}
	private void PrefabSelectionMenu(CellLayer layerType){
		
		if(GUILayout.Button("Close Selector")){
			selectingPrefab = false;
		}
		selectors[(int)layerType] = GUILayout.SelectionGrid(selectors[(int)layerType], allPrefabNames[(int)layerType], 2, "button");
		
	}
	private void SelectTool(ToolType value){
		switch(value){
			case ToolType.Builder:
				tileEditor.toolType = ToolType.Builder;
				break;
			case ToolType.Remover: 
				tileEditor.toolType = ToolType.Remover;
				break;
			case ToolType.Pathing:
				tileEditor.toolType = ToolType.Pathing;
				break;
		}
	}
	private void SelectToolTarget(CellLayer value){
		switch(value){
			case CellLayer.Wall:
				tileEditor.toolTarget = CellLayer.Wall;
				actorManager.DeselectActors();
				break;
			case CellLayer.Floor: 
				tileEditor.toolTarget = CellLayer.Floor;
				actorManager.DeselectActors();
				break;
			case CellLayer.Block:
				tileEditor.toolTarget = CellLayer.Block;
				actorManager.DeselectActors();
				break;
		}	
	}
	public string GetSelected(CellLayer ofType){
		string str = allPrefabNames[(int)ofType][selectors[(int)ofType]];
		return str;
	}
}

