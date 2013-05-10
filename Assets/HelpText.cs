using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GUIText))]

public class HelpText : MonoBehaviour {
	
	public bool isShown = false;
	
	public string shownText = "Help goes here";
	public string hideText = "[?]";
	public GUIText shadow;
	public void Start(){
		isShown = !isShown;
		Change();
	}
	private void OnMouseDown(){
		
		Change();
	}
	private void Change(){
		isShown = !isShown;
		if(isShown)ShowText();
		else HideText();
	}
	private void HideText(){
		guiText.text = hideText;
		shadow.text = guiText.text;
	}
	private void ShowText(){
		guiText.text = shownText;
		shadow.text = guiText.text;
	}
}

