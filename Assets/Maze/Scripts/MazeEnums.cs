namespace MazeCore.enums
{
    public enum CellType 
    {
        WALL, PATH, ENTRY, EXIT, DOOR, VINE
    }

    public enum Direction 
    {
        LEFT, RIGHT, UP, DOWN
    }

    public enum JunctionType
    {
        DeadEnd,
        LeftJunction,
        RightJunction,
        StraightJunction,
        Cross,
        TJunction,
        Fork,
        None
    }


}