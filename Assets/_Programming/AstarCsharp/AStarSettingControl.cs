using UnityEngine;
using System.Collections;

public class AStarSettingControl : MonoBehaviour {

	
	public AStarSettings settings = new AStarSettings();
	
	public void Start()
	{
		AStarPathFinding.settings = settings;
	}
}
