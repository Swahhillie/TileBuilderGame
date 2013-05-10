public enum ToolType{ Builder, Remover, Pathing};
public enum CellLayer{Floor = 0, Wall = 1, Block = 2, Ceiling = 3, Path = 4};

[System.FlagsAttribute()]public enum WallState{none = 0, North = 1<<1, East = 1<<2, South = 1<<3, West = 1<<4};
public enum FloorState{Build, Open, Disabled};