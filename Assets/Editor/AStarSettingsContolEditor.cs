using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(AStarSettingControl))]
public class AStarSettingsContolEditor : Editor
{

	override public void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();
		EditorGUILayout.BeginVertical ("Box");
		{
			AStarSettings settings = ((AStarSettingControl)target).settings;
			
			EditorGUILayout.BeginHorizontal ();
			{
				EditorGUILayout.PrefixLabel ("Heuristic");
				settings.Heuristic = (AStarPathFinding.Heuristic)EditorGUILayout.EnumPopup (settings.Heuristic);			
			}
			

			EditorGUILayout.EndHorizontal ();
			
			EditorGUILayout.BeginHorizontal ();
			{
				EditorGUILayout.PrefixLabel ("Travel Cost");
				settings.TravelCost = EditorGUILayout.FloatField (settings.TravelCost);
			}
			
			EditorGUILayout.EndHorizontal ();
			
			EditorGUILayout.BeginHorizontal ();
			{
				EditorGUILayout.PrefixLabel ("Heuristic Cost");
				settings.HeuristicCost = EditorGUILayout.FloatField (settings.HeuristicCost);
			}
			
			EditorGUILayout.EndHorizontal ();
			
			EditorGUILayout.BeginHorizontal ();
			{
				EditorGUILayout.PrefixLabel ("Tiebreak");
				settings.TieBreak = EditorGUILayout.FloatField (settings.TieBreak);
			}
			
			EditorGUILayout.EndHorizontal ();
		}
		EditorGUILayout.EndVertical ();

	}
	
}
