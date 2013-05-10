using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[RequireComponent(typeof(LineRenderer))]

public class DebugAstar : MonoBehaviour {
	
	// Use this for initialization
	private LineRenderer lineRenderer;
	public static List<PathNode> path;
	public static float distanceBetweenNodes = 10;
	
	void Start () {
		lineRenderer = GetComponent<LineRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		if(path != null){
			lineRenderer.SetVertexCount(path.Count);
			for(int i = 0; i < path.Count; i ++){
				PathNode pn = path[i];
				Tile tl = (Tile) pn;
				Vector3 pos = tl.center + Vector3.up;
				lineRenderer.SetPosition(i, pos);
			}
		}	
	}
}
