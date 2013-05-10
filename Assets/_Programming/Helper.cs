using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Helper {
	const int X = 0;
	const int Y = 1;
	// Use this for initialization
	public static void RearangeForSelection(ref int[]topLeft, ref int[]bottemRight){
		//this sets the top left corner and the bottem right corner.
		
		int[] copyTopLeft = new int[2]{topLeft[X], topLeft[Y]};
		int[] copyBottemRight = new int[2]{bottemRight[X], bottemRight[Y]};
		
		topLeft[X] = Mathf.Min(copyTopLeft[X], copyBottemRight[X]);
		topLeft[Y] = Mathf.Min(copyTopLeft[Y], copyBottemRight[Y]);
		
		bottemRight[X] = Mathf.Max(copyTopLeft[X], copyBottemRight[X]);
		bottemRight[Y] = Mathf.Max(copyTopLeft[Y], copyBottemRight[Y]);
		
		//Debug.Log("after (" + topLeft[X] + "," + topLeft[Y] + ")(" + bottemRight[X] + "," + bottemRight[Y] + ")");
		
	}
	public static void RearangeForLine(ref int[]topLeft, ref int[]bottemRight){
		
		int dx = Mathf.Abs(bottemRight[X] - topLeft[X]);
		int dy = Mathf.Abs(bottemRight[Y] - topLeft[Y]);
		
		//if(bottemRight[X] > bottemRight[Y]){
		if(dx > dy){
			//horizontal line
			bottemRight[Y] = topLeft[Y];
		}
		else{
			//vertical line
			bottemRight[X] = topLeft[X];
		}
		RearangeForSelection(ref topLeft, ref bottemRight);
		
	}
	
	public static string ArrToString<T>(System.Collections.Generic.IList<T> arr){
		string outStr = "[";
		for(int i = 0; i < arr.Count;i++){
			if(arr[i] == null)outStr += "null";
			else outStr += arr[i].ToString();
			if(i < arr.Count -1) outStr += ",";
		}
		outStr += "]";
		return outStr;
	}
	public static string IntsToString(int[] arr){
		string outStr = "(";
		for(int i = 0; i < arr.Length;i++){
			outStr += arr[i].ToString();
			if(i < arr.Length -1) outStr += ",";
		}
		outStr += ")";
		return outStr;
	}
	public static string IntsToString(int[][] arr){
		string outStr = "[";
		for(int i = 0; i < arr.Length;i++){
			outStr += "[";
			for(int j = 0; j < arr[i].Length; j++){
				outStr += i.ToString();
				if(j < arr[i].Length-1) outStr += ",";
			}
			outStr += "]";
			
		}
		outStr += "]";
		return outStr;
	}
	public static string IntsToString(int[,][] arr, int width, int height){
		string outStr = "[";
		for(int i =0; i < width; i++){
			for(int j =0; j < height; j++){
				int[]coord = arr[i,j];
				outStr += IntsToString(coord);
				if(j < height -1) outStr += ",\t";
			}
			if(i < width-1)outStr += "\n";
		}
		outStr += "]";
		return outStr;
	}
}
