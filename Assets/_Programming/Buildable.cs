using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Buildable : MonoBehaviour {

	public float cost = 0;
	
	private int _meshKey = -1;
	
	public CellLayer layer;
	public bool batch = true;
	
	public int buildableId = -1; //needs to be public so that it is copied when a instantiated.
	
	private static Dictionary<int, int> alreadyBatched = new Dictionary<int, int>();
	
	public void Start(){
		if(buildableId == -1)Debug.LogError("BuildableID not assigned");
		if(meshKey == -1 && batch == true){
			//this buildable has no mesh key, get one from the mesh combine class.
			if(alreadyBatched.ContainsKey(buildableId)){
				_meshKey = alreadyBatched[buildableId];
			}
			else{
				_meshKey = MeshCombine.Instance().GetNewMeshKey(gameObject);
				alreadyBatched[buildableId] = _meshKey;
			}
			//queue this mesh for combining
			MeshCombine.Instance().AddCombineMesh(this);
		}
	}
	/*
	public int buildableId{
		get{return _buildableId;}
		set{_buildableId = value;}
	}
	*/
	public int meshKey{
		get{return _meshKey;}
	}
	public void OnDestroy(){
		if(MeshCombine.Instance() != null) MeshCombine.Instance().RemoveCombineMesh(this);
	}
}
