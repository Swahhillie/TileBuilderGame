using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildableSelectVisitor : IVisitor {
	
	public List<Buildable> selection;
	
	CellLayer layer;
	int buildableId;
	int meshKey;
	
	public BuildableSelectVisitor(Buildable b){
		layer = b.layer;
		buildableId = b.buildableId;
		meshKey = b.meshKey;
	}
	
	public void Visit(WallTile wallTile){
		if(layer == CellLayer.Wall)CheckBuildables(wallTile);
	}
	public void Visit(BlockTile blockTile){
		if(layer == CellLayer.Block)CheckBuildables(blockTile);
	}
	public void Visit(FloorTile floorTile){
		if(layer == CellLayer.Floor)CheckBuildables(floorTile);
	}
	
	private void CheckBuildables(TileComponent t){
		foreach(Buildable b in  t.buildables){
			if(b.meshKey == meshKey){
				selection.Add(b);
			}
		}
	}
}
