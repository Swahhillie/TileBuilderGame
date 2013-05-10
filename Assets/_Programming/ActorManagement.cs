using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActorManagement : MonoBehaviour{
	//const
	const int X = 0;
	const int Y = 1;
	const int Z = 1;
	
	//links
	private CellManager manager;
	
	//all actors
	private List<Actor> selectedActors;
	private List<Actor> actors;
	
	public void Awake(){
		
		actors = new List<Actor>();
		selectedActors = new List<Actor>();
	}
	public void Start(){
		manager = GameObject.FindGameObjectWithTag("MainObject").GetComponent<Main>().GetCellManager();
	}
	public void AddActor(Actor theActor){
		actors.Add(theActor);
	}
	public void RemoveActor(Actor theActor){
		actors.Remove(theActor);
	}
	public bool MoveSelectedActors(int[] destination){
		if(selectedActors.Count > 0){
			Tile dest = manager.GetTile(destination);
			for(int i= 0; i < selectedActors.Count; i++){
				selectedActors[i].StartMoveTo(dest);
			}
			return true;
			
		}
		else{
			return false;
		}		
	}
	public bool SelectActors(int[] topLeft, int[] bottemRight){
		DeselectActors();
		
		bool foundActor = false;
		for(int i = 0; i < actors.Count; i++){
			Actor actor = actors[i];
			int x = actor.coords[X];
			int y = actor.coords[Y];
			
			if(x >= topLeft[X] && x <= bottemRight[X] && y >= topLeft[Y] && y <= bottemRight[Y]){
				//found the actor at coordinates
				selectedActors.Add(actor);
				actor.selected = true;
				foundActor = true;
				
			}
		}
		return foundActor;
		
	}
	public bool SelectActors(int[][] beginAndEnd){
		if(beginAndEnd == null){
			return false;
		}
		if(beginAndEnd.Length != 2 || beginAndEnd[0].Length != 2)
			 	Debug.LogError("FEEDING WRONG SELECTION INTO SELECT ACTORS");
			 	
		return SelectActors(beginAndEnd[0],beginAndEnd[1]);
	}
	public void DeselectActors(){
		foreach(Actor actor in selectedActors) actor.selected = false;
		selectedActors.Clear();
	}
}
