using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Selections{
	public int[] current;
	public int[] clicked;
	public Vector3 cornerPointClicked; // corner near where the mouse was clicked down
	public Vector3 cornerPointCurrent; // corner near where the mouse is now
	
	public List<int[]> painted = new List<int[]>();
	
	
	public Selections(){
		current = null;
		clicked = null;
	}
	public int[][] clickAndDragSelection{
		get{
			//clicked and current must be set, otherwise it will default to 0,0 and that is not actually the case
			if(clicked == null || current == null) return null;
			
			//selection is an array of 2 intger arrays. the start click and the current hover
			int[][] selection = new int[2][]{new int[2]{clicked[0], clicked[1]},new int[2]{current[0], current[1]}}; //deep copy into new arrays
			
			//sort the selection so that it goes from top left to bottem right
			Helper.RearangeForSelection(ref selection[0], ref selection[1]);
			
			return selection;
		}
	}
	
	public int[][] lineSelection{
		get{
			//line selection is a single line of tiles
			
			/*
			the 2d array
			[[0,0,0,0],
			 [0,0,0,0],
			 [0,0,0,0],
			 [0,0,0,0]]
			
			the selected nodes, 1 = clicked, 2 = hover
			[[0,2,0,0],
			 [0,0,0,0],
			 [0,0,0,0],
			 [0,0,1,0]]
			
			the result, 1 = start, 2 = end;
			[[0,1,0,0],
			 [0,0,0,0],
			 [0,0,0,0],
			 [0,2,0,0]]
			*/
			
			//clicked and current must be set, otherwise it will default to 0,0 and that is not actually the case
			if(clicked == null || current == null) return null;
			
			//selection is an array of 2 intger arrays. the start click and the current hover
			int[][] selection = new int[2][]{new int[2]{clicked[0], clicked[1]}, new int[2]{current[0] ,current[1]}};//deep copy into new arrays
			
			Helper.RearangeForLine(ref selection[0], ref selection[1]);
			
			return selection;
			
		}
	}
	public void AddToPainted(int[] coords){
		if(coords != null){
			for(int i = 0; i < painted.Count; i++){
				int[] toCheck = painted[i];
				if(toCheck[0] == coords[0] && toCheck[1] == coords[1])return;
				//the coord is already in the painted list
			}
			painted.Add(coords);
		}
	}
	public void ClearPainted(){
		painted.Clear();
	}

}


public class TileSelector : MonoBehaviour {
	
	private CellManager manager;
	
	
	private RayPick picker;
	private Selections _selections = new Selections();
	
	public Highlighter highlighter;
	
	void Awake(){
		GameObject highlighterGo = Instantiate(highlighter.gameObject) as GameObject;
		highlighter = highlighterGo.GetComponent<Highlighter>();
	}
	void Start () {
		manager = GameObject.FindGameObjectWithTag("MainObject").GetComponent<Main>().GetCellManager();
		picker = GetComponent<RayPick>();
		
		Controller.onLeftMouseDownFunctions.Add(OnStartSelect);
		Controller.onLeftMouseHoldFunctions.Add(OnSelectionHold);
		Controller.onLeftMouseReleaseFunctions.Add(OnSelectRelease);
		
	}
	
	// Update is called once per frame
	void Update () {
		//update all the selections and highlight where neccessary
		
		CleanSelections();
		int[] coords = null;
		if(picker.PickWithCamera(out coords)){
			_selections.current = coords;						
		}
		Vector3 p;
		if(picker.PickCornerPointCamera(out p))
		{
			_selections.cornerPointCurrent = p;
		}
		HighlightSelection();
		

		
	}
	public void OnStartSelect(){
		_selections.clicked = _selections.current;
		_selections.cornerPointClicked = _selections.cornerPointCurrent;
	}
	public void OnSelectionHold(){
		_selections.AddToPainted(_selections.current);
	}
	public void OnSelectRelease(){
		_selections.ClearPainted();
		_selections.clicked = null;
		//selections are still valid here, after this function they will be cleaned up.
		//foreach(int[] coord in _selections.painted) manager.GetTile(coord).model.transform.GetChild(0).renderer.material.color = Color.blue; //debugging the painted selection
	}
	private void HighlightSelection(){
		//highlight based on the current selection
		
		if(_selections.current != null){
			int[][] dragSelection = _selections.clickAndDragSelection;
			if(dragSelection != null){
				if(_selections.clicked == null) Debug.LogError("Should not be here");
				highlighter.Highlight(dragSelection[0], dragSelection[1]);
			}
			else{
				highlighter.Highlight(_selections.current);
			}
		}
		
	}
	private void CleanSelections(){
		_selections.current = null;
	}
	
	//----------- access ---------------
	public Selections selections{
		get{ return _selections;}
	}
}
