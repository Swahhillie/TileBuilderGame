using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class Controller : MonoBehaviour {

	// Use this for initialization
	TileSelector tileSelector;
	TileEditor tileEditor;
	
	public delegate void MouseFunction();
	public static List<MouseFunction> onLeftMouseDownFunctions = new List<MouseFunction>();
	public static List<MouseFunction> onLeftMouseHoldFunctions = new List<MouseFunction>();
	public static List<MouseFunction> onLeftMouseReleaseFunctions = new List<MouseFunction>();
	public static List<MouseFunction> onRightMouseDownFunctions = new List<MouseFunction>();
	public static List<MouseFunction> onRightMouseHoldFunctions = new List<MouseFunction>();
	public static List<MouseFunction> onRightMouseReleaseFunctions = new List<MouseFunction>();
	
	void Start () {
		tileSelector = GetComponent<TileSelector>();
		tileEditor = GetComponent<TileEditor>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0)){
			foreach(MouseFunction mf in onLeftMouseDownFunctions)mf();	
		}
		if(Input.GetMouseButton(0)){
			foreach(MouseFunction mf in onLeftMouseHoldFunctions)mf();
		}
		if(Input.GetMouseButtonUp(0)){
			foreach(MouseFunction mf in onLeftMouseReleaseFunctions)mf();
		}
		if(Input.GetMouseButtonDown(1)){
			foreach(MouseFunction mf in onRightMouseDownFunctions)mf();	
		}
		if(Input.GetMouseButton(1)){
			foreach(MouseFunction mf in onRightMouseHoldFunctions)mf();
		}
		if(Input.GetMouseButtonUp(1)){
			foreach(MouseFunction mf in onRightMouseReleaseFunctions)mf();
		}
	}
}
