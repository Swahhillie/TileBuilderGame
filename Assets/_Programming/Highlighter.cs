using UnityEngine;
using System.Collections;

public class Highlighter : MonoBehaviour {
	
	 const int X = 0;
	 const int Y = 1;
	 
	// Use this for initialization
	private Vector3 bounds;
	public bool forWalls = false;
	//public Material mat;
	
	CellManager manager;
	
	void Awake(){
		manager = GameObject.FindGameObjectWithTag("MainObject").GetComponent<Main>().GetCellManager();
		manager.OnGenerateLevel += (Rescale);
	}
	
	private void Rescale(){
		Debug.Log("Rescaled the highlighter");
		transform.position = new Vector3(manager.width / 2.0f, 0.05f, manager.height / 2.0f); //center
		transform.localScale = new Vector3(manager.width, manager.height , 1); 				//scale
		mat.mainTextureScale = new Vector2(manager.width, manager.height);		//scale of texture
		mat.SetVector("_Bounds", new Vector4(0,0,0,0));			//start limits	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void Highlight(int[][] lineSelected){
		//highlight a line of tiles
		int[] start = lineSelected[0];
		int[] end = lineSelected[lineSelected.Length -1];
		
		Helper.RearangeForSelection(ref start, ref end);
		Vector2 offset = new Vector2(0,0);
		Vector4 bounds = new Vector4(start[X],start[Y],end[X]+Tile.DIMENSION.x,end[Y]+Tile.DIMENSION.z);
		if(forWalls) bounds -= new Vector4(Tile.DIMENSION.x / 2, Tile.DIMENSION.z /2, Tile.DIMENSION.x /2, Tile.DIMENSION.z /2);
		mat.SetVector("_Bounds", bounds);
		
		mat.mainTextureOffset = offset;
	}
	
	public void Highlight(int[] coordinates){
		//highlight a single tile
		Vector4 bounds = new Vector4(
			coordinates[X],
			coordinates[Y],
			coordinates[X]+(Tile.DIMENSION.x),
			coordinates[Y]+(Tile.DIMENSION.z));
			
		if(forWalls) bounds -= new Vector4(Tile.DIMENSION.x / 2, Tile.DIMENSION.z /2, Tile.DIMENSION.x /2, Tile.DIMENSION.z /2);
		mat.SetVector("_Bounds", bounds);
		Vector2 offset = Vector2.zero;
		mat.mainTextureOffset = offset;
		
	}
	public void Highlight(int[] start, int[]end){
		//highligt a group of tiles
		Vector4 bounds = new Vector4(start[X],start[Y],end[X]+Tile.DIMENSION.x,end[Y]+Tile.DIMENSION.z);
		
		mat.SetVector("_Bounds", bounds);
		
		//mat.SetVector("_Bounds", new Vector4(fromPos[X],fromPos[Y],toPos[X]+Tile.DIMENSION.x,toPos[Y]+Tile.DIMENSION.z));
		//mat.mainTextureOffset = Vector2.zero;
	}
	public void ClearHighlight(){
		mat.SetVector("_Bounds", Vector4.zero);
		mat.mainTextureOffset = Vector2.zero;
	}
	private Material mat{
		get{return renderer.material;}
	}
	
}
