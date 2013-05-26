using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class MeshCombine : MonoBehaviour {
	bool rebatchQueued = false;
	CellManager manager;
	
	private List<Buildable> toCombine = new List<Buildable>();
	private HashSet<int> toRebatch = new HashSet<int>();
	private static Dictionary<int, MeshFilter> combined = new Dictionary<int, MeshFilter>();
	
	//how many meshes must be queued before the queue is combined with the batched meshes
	public int rebatchThresshold = 10;
	public float rebatchDelay = 2.0f;
	private static MeshCombine instance = null;
	
	
	private float rebatchCalledTime = 0.0f; //time when the last call to rebatchmeshes was made.
	// Use this for initialization
	private void Awake(){

		manager = GameObject.FindWithTag("MainObject").GetComponent<CellManager>();
		manager.OnGenerateLevel += DiscardCombined;
	}
	
	public void AddCombineMesh(Buildable buildable){
		toCombine.Add(buildable);
		//Debug.Log("Added " + buildable + " to be combined ");
		if(toCombine.Count >= rebatchThresshold && !rebatchQueued){
			//enough new buildable are in the scene to warrent a rebatching of the old meshes
			
			RebatchMeshes();
		}
	}
	public void RemoveCombineMesh(Buildable buildable){
		if(toCombine.Remove(buildable)){
			//the mesh was in the queue to be combined. it was destroyed before it happend. So remove from the queue and done.
			Debug.Log("Removed " + buildable + "from mesh combine queu", buildable);
		}
		else{
			//the mesh was not in the queue. It must already be combined.
			//the mesh must be removed from combined mesh.
			
			
			//find in what layer we have to look for this buildable
			
			toRebatch.Add(buildable.meshKey);		//rebatch queue is emptied at the end of this update, meshes will be regenerated
			
		}
	}
	public void LateUpdate(){
		foreach(int meshKey in toRebatch){
			ClearMesh(meshKey);
			
			//find all remaining objects with this meshkey
			
		}
		if(toRebatch.Count > 0){
			List<Buildable> allBuildables = manager.GetAllBuildables();
			toCombine.AddRange(allBuildables.FindAll(x => toRebatch.Contains(x.meshKey)));
			//toCombine.ForEach(x => x.ActivateChildren()); //activate all gameobject children so that the mesh filter can be found.
			RebatchMeshes(true);
		}
		toRebatch.Clear();
	}
	private void DiscardCombined(){
		//when  a new level is generated it must unload all combined meshes
		foreach(KeyValuePair<int, MeshFilter> safedMesh in combined){
			ClearMesh(safedMesh.Key);
		}
	}
	private void ClearMesh(int meshKey_){
		combined[meshKey_].mesh = new Mesh();
	}
	private void OnApplicationQuit(){
		
		DiscardCombined();
	}
	private void RebatchMeshes(bool imidiateMode = false){
		rebatchQueued = true;
		if(imidiateMode){
			//yield return new WaitForSeconds(rebatchDelay);//wait a little to allow big blocks off objects to complete up. 
			//while(manager.state != CellManager.CellManagerState.Ready)yield return null;//for loading levels
			RebatchMeshesConcrete();	
			rebatchQueued = false;
		}
		else{
			rebatchCalledTime = Time.time;
		}
		
	}
	private void Update(){
		if(rebatchQueued){
			if(rebatchCalledTime + rebatchDelay <Time.time){
				RebatchMeshesConcrete();
				rebatchQueued = false;
			}
		}
	}
	public void RebatchMeshesConcrete(){
		Debug.Log("Batching " + toCombine.Count + " meshes " );
		
		Dictionary<int, List<Buildable> > seperated = new Dictionary<int, List<Buildable> >();
		for(int i = toCombine.Count -1; i >= 0; i--){
			//seperate the buildables based on there meshkey
			Buildable b = toCombine[i];
			
			if(seperated.ContainsKey(b.meshKey)){
				//the key is in the dictionary. Add this buildable to the list of its kind.
				seperated[b.meshKey].Add(b);
			}
			else{
				//the key is not yet in the dictionary. Make a new entry and add the buildable.
				seperated[b.meshKey] = new List<Buildable>();
				seperated[b.meshKey].Add(b);
			}
		}
		//all meshes that remain to be combined are in the dictionary, we can clear the tocombine queue again.
		toCombine.Clear();
		
		
		foreach(KeyValuePair<int, List<Buildable> > toCombinePair in seperated){
			//now we start making combine instances for each type of buildable.
			CombineInstance[] cbi = new CombineInstance[toCombinePair.Value.Count + 1]; // also make room for the exsiting mesh
			int meshKey = toCombinePair.Key;
			
			for(int i = 0; i < toCombinePair.Value.Count; i++){
				Buildable b = toCombinePair.Value[i];
				
				//get the meshfilter from the buildable we are currenty combining
				MeshFilter mf = b.GetComponentInChildren<MeshFilter>();
				if(mf == null)Debug.LogError("Meshfilter == null on Buildable" + i, b);
				else{
					cbi[i].mesh = mf.sharedMesh;
					cbi[i].transform = mf.transform.localToWorldMatrix;
					
					//the renderer that this mesh has can now be disabled, it will be renderered because its mesh is part of the combined mesh
					mf.gameObject.renderer.enabled = false;
				}
			}
			
			//adding the existing mesh to the combinemesh instances
			
			cbi[cbi.Length -1].mesh = combined[meshKey].mesh;
			cbi[cbi.Length -1].transform = combined[meshKey].transform.localToWorldMatrix;
			
			//create the new mesh
			Mesh m = new Mesh();
			m.CombineMeshes(cbi);
			combined[meshKey].mesh = m;
			
		}
		//finished. new blocks can now start a new timer for the meshcombine
	}
	
	public static MeshCombine Instance(){
		if(instance == null)instance = (MeshCombine)FindObjectOfType(typeof(MeshCombine));
		return instance;
	}
	public int GetNewMeshKey(GameObject go){
		//this will create a new batch mesh when called. It should be called if a completely new mesh / material combo is created.
		
		MeshFilter mf = new GameObject("MeshHolder " + combined.Count).AddComponent<MeshFilter>(); //a gameobject that will hold the mesh and render it.
		mf.mesh = new Mesh();
		Renderer rend = mf.gameObject.AddComponent<MeshRenderer>();		//give the new go a renderer to render the batched mesh
		mf.transform.parent = this.transform;							//organize the mesh as a child of this.
		rend.materials = go.GetComponentInChildren<Renderer>().materials; //copy the materials over to the newly created renderer
		
		combined[combined.Count] = mf;
		return combined.Count -1;
	}
	
}
