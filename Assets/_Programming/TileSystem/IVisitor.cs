using UnityEngine;
using System.Collections;

public interface IVisitor {
	void Visit(WallTile element);
	void Visit(FloorTile element);
	void Visit(BlockTile element);
}
