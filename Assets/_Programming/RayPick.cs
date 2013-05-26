using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RayPick : MonoBehaviour{
	public const int X = 0;
	public const int Y = 1;
	
	public Camera cameraRef;
	
	public Plane tilePlane;
	public Vector4 bounds;
	
	public LayerMask pickFromLayer;
	
	private CellManager manager;
	
	public void Awake(){
		//manager = GetComponent<CellManagement>();
		tilePlane = new Plane(Vector3.up, Vector3.zero);
		manager = GetComponent<CellManager>();
		manager.OnGenerateLevel  += OnGenerateLevel;
		
	}

	public void Start(){
		
		
		
	}
	private void OnDrawGizmos()
	{
		Vector3 p;
		if(PickPointOnPlane(cameraRef.ScreenPointToRay(Input.mousePosition), out p))
		{
			p = SnapToGridCorner(p);
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(p, 1);
		}
	}
	public bool PickWithCamera(out int[]coordinates){
		int[]toGoOut = null;
		if(PickWithRay(cameraRef.ScreenPointToRay(Input.mousePosition), out toGoOut)){
			coordinates = toGoOut;
			return true;
		}
		//raycast did not hit the floor. no coordinates.
		coordinates = null;
		return false;
		
	}
	public bool PickCornerPointCamera(out Vector3 p)
	{
		p = Vector3.zero;
		if(PickPointOnPlane(cameraRef.ScreenPointToRay(Input.mousePosition), out p))
		{
			p = SnapToGridCorner(p);
			return true;
		}
		else{
			return false;	
		}
		
	}
	public void OnGenerateLevel(){
		Debug.Log("Rescaled bounds for raypicking");
		bounds = new Vector4(0, 0, manager.width * Tile.DIMENSION.x, manager.height * Tile.DIMENSION.z);
	}

	public bool PickWithRay(Ray ray, out int[]coordinates){
		//if the ray hits a floor tile this will return true aand the coorinates are the width and height of the cell.
		/*
		RaycastHit hitInfo;
		if(Physics.Raycast(ray, out hitInfo, Mathf.Infinity,pickFromLayer.value)){
			coordinates = MathPick(hitInfo.point);
			
			return true;
		}
		*/
		Vector3 p;
		if(PickPointOnPlane(ray, out p)){
			
//			Debug.Log(p);
			coordinates = MathPick(p);
			
//			Debug.Log(bounds);
//			Debug.Log(Helper.ArrToString(coordinates));
//			Debug.Log(coordinates[X] > bounds.x && coordinates[X] < bounds.z && coordinates[Y] > bounds.y && coordinates[Y] < bounds.w);
			if(coordinates[X] >= bounds.x && coordinates[X] < bounds.z && coordinates[Y] >= bounds.y && coordinates[Y] < bounds.w){
				return true;
			}
		}
		coordinates = null;
		return false;
	}
	private bool PickPointOnPlane(Ray ray, out Vector3 point)
	{
		float enter;
		if(tilePlane.Raycast(ray, out enter))
		{
			point = ray.GetPoint(enter);
			return true;
		}
		else{
			point = Vector3.zero;
			return false;
		}
	}
	private int[] MathPick(Vector3 pos){
		//round down the results, since the cells are 1 x 1 this will give their location.
		int x = (int) pos.x;
		int z = (int) pos.z;
		return new int[2]{x, z};
	}
	private Vector3 SnapToGridCorner(Vector3 pos)
	{
		Vector3 snap = Vector3.zero;
		snap.x = pos.x + ((Mathf.Round(pos.x / Tile.DIMENSION.x) - pos.x / Tile.DIMENSION.x) * Tile.DIMENSION.x);
		snap.z = pos.z + ((Mathf.Round(pos.z / Tile.DIMENSION.z) - pos.z / Tile.DIMENSION.z) * Tile.DIMENSION.z);
		return snap;
	}
	
}