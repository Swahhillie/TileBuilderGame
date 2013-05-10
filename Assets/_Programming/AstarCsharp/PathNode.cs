using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathNode {
		
	private int _column;
	private int _row;

	private float _g;
	private float _h;
	private float _f;
	
	private bool _traversable = true;
	
	
	public PathNode parentNode;
	
	public float g{
		get{return _g;}
		set{_g = value;}
	}
	public float h{
		get{return _h;}
		set{_h = value;}
	}
	public float f{
		get{return _f;}
		set{_f = value;}
	}
	
	public int column{
		get{return _column;}
		set{_column = value;}
	}
	public int row{
		get{ return _row ;}
		set{_row = value;}
	}
	
	virtual public bool traversable{
		get{ return _traversable;}
		set{ _traversable = value;}
	}

	override public string ToString(){
		//return "(" + g + ", " + h +", " + f + ")";
		return "(" + _column + ", " + _row + ")";
	}
}
