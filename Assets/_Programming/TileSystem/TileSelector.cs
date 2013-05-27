using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Selections. Stores points that are relevant to selection.
/// </summary>
public class Selections
{
	/// <summary>
	/// The coordinates that the player is currently hovering his mouse over.
	/// </summary>
	public int[] current;
	/// <summary>
	/// The coordinates that the player pressed his LMB down on.
	/// </summary>
	public int[] clicked;
	/// <summary>
	/// The corner point that the player was hovering his mouse over when he pressed the LMB down.
	/// </summary>
	public Vector3 cornerPointClicked; // corner near where the mouse was clicked down
	public Vector3 cornerPointCurrent; // corner near where the mouse is now
	
	public List<int[]> painted = new List<int[]> ();
	
	public Selections ()
	{
		current = null;
		clicked = null;
	}

	public int[][] ClickAndDragSelection {
		get {
			//clicked and current must be set, otherwise it will default to 0,0 and that is not actually the case
			if (clicked == null || current == null)
			{
				return null;
			}
			
			//selection is an array of 2 intger arrays. the start click and the current hover
			int[][] selection = new int[2][]{new int[2]{clicked [0], clicked [1]},new int[2]{current [0], current [1]}}; //deep copy into new arrays
			
			//sort the selection so that it goes from top left to bottem right
			Helper.RearangeForSelection (ref selection [0], ref selection [1]);
			
			return selection;
		}
	}
	/// <summary>
	/// Gets the line selection. If the
	/// </summary>
	/// <example>
	/// <code>
	/// 	line selection is a single line of tiles
	///		the 2d array
	///		[[0,0,0,0],
	///		 [0,0,0,0],
	///		 [0,0,0,0],
	///		 [0,0,0,0]]
	///		
	///		the selected nodes, 1 = clicked, 2 = hover
	///		[[0,2,0,0],
	///		 [0,0,0,0],
	///		 [0,0,0,0],
	///		 [0,0,1,0]]
	///		
	///		the result, 1 = start, 2 = end;
	///		[[0,1,0,0],
	///		 [0,0,0,0],
	///		 [0,0,0,0],
	///		 [0,2,0,0]]*/
	///</code>
	///</example>
	/// <value>
	/// 2 Arrays of size 2. The first being the coordinates of the start position and the second is the end position.
	/// </value>
	public int[][] LineSelection {
		get {
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
			if (clicked == null || current == null)
			{
				return null;
			}
			
			//selection is an array of 2 intger arrays. the start click and the current hover
			int[][] selection = new int[2][]{new int[2]{clicked [0], clicked [1]}, new int[2]{current [0] ,current [1]}};//deep copy into new arrays
			
			Helper.RearangeForLine (ref selection [0], ref selection [1]);
			
			return selection;
			
		}
	}
	
	public bool WallLine (out int[] startCoord, out int[] endCoord, out Wall.Side side)
	{
		//early out if the user is not holding down the button, no cornerpoint clicked will exsist
		if (cornerPointClicked == Vector3.zero)
		{
			startCoord = null;
			endCoord = null;
			side = Wall.Side.North;
			return false;			
		}
		
		//determine if wall is north south or east west
		
		
		//EW = east <-> west
		//NS = north <-> south
		
		Vector3 startToEndLine = cornerPointCurrent - cornerPointClicked;
		
		Vector3 startPoint = cornerPointClicked;
		Vector3 endPoint = cornerPointCurrent;
		
		
		if (Mathf.Abs (startToEndLine.x) > Mathf.Abs (startToEndLine.z))
		{
			//the longest distance was covered in the x direction, set they z to the start.
			endPoint.z = cornerPointClicked.z;
			side = Wall.Side.North;
		} else
		{
			//the longest distance was equal or in the z direction
			endPoint.x = cornerPointClicked.x;
			side = Wall.Side.West;
		}
		Debug.DrawLine (startPoint + Vector3.up, endPoint + Vector3.up, Color.cyan);
		
		//getting tiles that are on the line
		startCoord = new int[]{Mathf.FloorToInt (startPoint.x), Mathf.FloorToInt (startPoint.z)};
		endCoord = new int[]{Mathf.FloorToInt (endPoint.x), Mathf.FloorToInt (endPoint.z)};
		
		
		Helper.RearangeForLine (ref startCoord, ref endCoord);
		
		Debug.DrawLine (new Vector3 (startCoord [0], 0, startCoord [1]), new Vector3 (endCoord [0], 0, endCoord [1]), Color.magenta);
		
		return true;
		
	}

	public void AddToPainted (int[] coords)
	{
		if (coords != null)
		{
			for (int i = 0; i < painted.Count; i++)
			{
				int[] toCheck = painted [i];
				if (toCheck [0] == coords [0] && toCheck [1] == coords [1])
				{
					return;
				}
				//the coord is already in the painted list
			}
			painted.Add (coords);
		}
	}

	public void ClearPainted ()
	{
		painted.Clear ();
	}

}


/// <summary> 
/// Tile selector. Manages tile selection based on controller input.
/// </summary>
public class TileSelector : MonoBehaviour
{
	
	private CellManager manager;
	private RayPick picker;
	private Selections _selections = new Selections ();
	public Highlighter highlighter;
	
	void Awake ()
	{
		GameObject highlighterGo = Instantiate (highlighter.gameObject) as GameObject;
		highlighter = highlighterGo.GetComponent<Highlighter> ();
	}

	void Start ()
	{
		manager = GameObject.FindGameObjectWithTag ("MainObject").GetComponent<Main> ().GetCellManager ();
		picker = GetComponent<RayPick> ();
		
		Controller.onLeftMouseDownFunctions.Add (OnStartSelect);
		Controller.onLeftMouseHoldFunctions.Add (OnSelectionHold);
		Controller.onLeftMouseReleaseFunctions.Add (OnSelectRelease);
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		//update all the selections and highlight where neccessary
		
		CleanSelections ();
		int[] coords = null;
		if (picker.PickWithCamera (out coords))
		{
			_selections.current = coords;						
		}
		Vector3 p;
		if (picker.PickCornerPointCamera (out p))
		{
			_selections.cornerPointCurrent = p;
		}
		HighlightSelection ();
		
		
		int [] start;
		int [] end;
		Wall.Side side;
		if (selections.WallLine (out start, out end, out side))
		{
			if (manager.IsCoordInBound (start) && manager.IsCoordInBound (end))
			{
				Debug.DrawLine (manager.GetTile (start).corner + Vector3.up , manager.GetTile (end).corner + Vector3.up, Color.green);
			}
		}
		

		
	}

	public void OnStartSelect ()
	{
		_selections.clicked = _selections.current;
		_selections.cornerPointClicked = _selections.cornerPointCurrent;
	}

	public void OnSelectionHold ()
	{
		_selections.AddToPainted (_selections.current);
	}

	public void OnSelectRelease ()
	{
		_selections.ClearPainted ();
		_selections.clicked = null;
		_selections.cornerPointClicked = Vector3.zero;
		//selections are still valid here, after this function they will be cleaned up.
		//foreach(int[] coord in _selections.painted) manager.GetTile(coord).model.transform.GetChild(0).renderer.material.color = Color.blue; //debugging the painted selection
	}

	private void HighlightSelection ()
	{
		//highlight based on the current selection
		
		if (_selections.current != null)
		{
			int[][] dragSelection = _selections.ClickAndDragSelection;
			if (dragSelection != null)
			{
				if (_selections.clicked == null)
				{
					Debug.LogError ("Should not be here");
				}
				highlighter.Highlight (dragSelection [0], dragSelection [1]);
			} else
			{
				highlighter.Highlight (_selections.current);
			}
		}
		
	}

	private void CleanSelections ()
	{
		_selections.current = null;
	}
	
	//----------- access ---------------
	public Selections selections {
		get{ return _selections;}
	}
}
