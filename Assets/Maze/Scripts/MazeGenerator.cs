using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MazeCore.types;
using MazeCore.enums;
using System.Linq;

public class MazeGenerator : MonoBehaviour
{

    public Maze CreateEmptyBox(int width, int height, Vector2Int entrance, Vector2Int? exit)
    {
        Maze maze = new Maze(width, height);
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (x == entrance.x && y == entrance.y)
                {
                    maze.cells.Add(new MazeCell { x = x, y = y, type = CellType.ENTRY });
                }
                else if (exit.HasValue && x == exit.Value.x && y == exit.Value.y)
                {
                    maze.cells.Add(new MazeCell { x = x, y = y, type = CellType.EXIT });
                }
                else if (x == 0 || y == 0 || x == width - 1 || y == height - 1)
                {
                    maze.cells.Add(new MazeCell { x = x, y = y, type = CellType.WALL });
                }
                else
                {
                    maze.cells.Add(new MazeCell { x = x, y = y, type = CellType.PATH });
                }
            }
        }

        // Set entrance path
        int entranceIndexBehind = entrance.x + (entrance.y - 1) * width;
        if(entrance.y > 0) // Ensure we don't go out of bounds
        {
            MazeCell entranceCellBehind = maze.cells[entranceIndexBehind];
            entranceCellBehind.type = CellType.PATH;
            maze.cells[entranceIndexBehind] = entranceCellBehind;
        }

        // Set exit path
        if (exit.HasValue)
        {
            int exitIndexBehind = exit.Value.x + (exit.Value.y + 1) * width;
            if (exit.Value.y < height - 1) // Ensure we don't go out of bounds
            {
                MazeCell exitCellBehind = maze.cells[exitIndexBehind];
                exitCellBehind.type = CellType.PATH;
                maze.cells[exitIndexBehind] = exitCellBehind;
            }
        }

        return maze;
    }
    public Maze GenerateMaze(int width, int height, Vector2Int entrance, Vector2Int? exit)
    {
        Maze maze = new Maze(width, height);
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                maze.cells.Add(new MazeCell { x = x, y = y, type = CellType.WALL });
            }
        }

        Stack<MazeCell> stack = new Stack<MazeCell>();
        Dictionary<string, bool> visited = new Dictionary<string, bool>();

        bool IsInside(int x, int y)
        {
            return x >= 0 && x < width && y >= 0 && y < height;
        }

        MazeCell? GetNeighbor(MazeCell cell)
        {
            List<Direction> directions = new List<Direction> { Direction.LEFT, Direction.RIGHT, Direction.UP, Direction.DOWN };
            directions.Sort((a, b) => Random.value > 0.5 ? 1 : -1);

            foreach (var direction in directions)
            {
                int nx = cell.x;
                int ny = cell.y;
                switch (direction)
                {
                    case Direction.LEFT: nx -= 2; break;
                    case Direction.RIGHT: nx += 2; break;
                    case Direction.UP: ny -= 2; break;
                    case Direction.DOWN: ny += 2; break;
                }

                string key = $"{nx},{ny}";
                if (IsInside(nx, ny) && !visited.ContainsKey(key))
                {
                    return new MazeCell { x = nx, y = ny, type = CellType.PATH };
                }
            }
            return null;
        }
        


        void MarkCellAsVisited(MazeCell cell)
        {
            int index = maze.cells.FindIndex(c => c.x == cell.x && c.y == cell.y);
            maze.cells[index] = cell;
            visited[$"{cell.x},{cell.y}"] = true;
        }

        void RemoveWallBetween(MazeCell cellA, MazeCell cellB)
        {
            int x = (cellA.x + cellB.x) / 2;
            int y = (cellA.y + cellB.y) / 2;
            MarkCellAsVisited(new MazeCell { x = x, y = y, type = CellType.PATH });
        }

        stack.Push(new MazeCell { x = entrance.x, y = entrance.y, type = CellType.ENTRY });
        MarkCellAsVisited(new MazeCell { x = entrance.x, y = entrance.y, type = CellType.ENTRY });

        while (stack.Count > 0)
        {
            MazeCell current = stack.Pop();
            MazeCell? neighbor = GetNeighbor(current);

            if (neighbor.HasValue)
            {
                stack.Push(current);
                RemoveWallBetween(current, neighbor.Value);
                stack.Push(neighbor.Value);
                MarkCellAsVisited(neighbor.Value);
            }
        }

        if (exit.HasValue) MarkCellAsVisited(new MazeCell { x = exit.Value.x, y = exit.Value.y, type = CellType.EXIT });

        MarkCellAsVisited(new MazeCell { x = entrance.x, y = entrance.y - 1, type = CellType.PATH });
        if (exit.HasValue) MarkCellAsVisited(new MazeCell { x = exit.Value.x, y = exit.Value.y + 1, type = CellType.PATH });

        maze = DetermineJunctionTypes(maze);
        maze = DetermineCellRotation(maze);
        if (exit.HasValue) maze = DeterinePathToExit(maze, exit.Value);
        return maze;
    }

    public Maze DetermineJunctionTypes(Maze maze)
    {
        for (int i = 0; i < maze.cells.Count; i++)
        {
            MazeCell cell = maze.cells[i];
            if (cell.type != CellType.PATH)
                continue;

            int pathCount = 0;
            bool hasLeftNeighbor = HasNeighbor(cell, maze, Direction.LEFT);
            bool hasRightNeighbor = HasNeighbor(cell, maze, Direction.RIGHT);
            bool hasUpNeighbor = HasNeighbor(cell, maze, Direction.UP);
            bool hasDownNeighbor = HasNeighbor(cell, maze, Direction.DOWN);
            


            pathCount = (hasLeftNeighbor ? 1 : 0) + (hasRightNeighbor ? 1 : 0) + (hasUpNeighbor ? 1 : 0) + (hasDownNeighbor ? 1 : 0);
            // TODO: BROKEN
            switch (pathCount)
            {
                case 1:
                    cell.junctionType = JunctionType.DeadEnd;
                    break;
                case 2:
                    // todo handle left and right junctions

                    cell.junctionType = JunctionType.StraightJunction;
                    break;
                case 3:
                    cell.junctionType = JunctionType.TJunction;
                    break;
                case 4:
                    cell.junctionType = JunctionType.Cross;
                    break;
                default:
                    cell.junctionType = JunctionType.None;
                    break;
            }
            // cell.junctionType = JunctionType.StraightJunction;
            maze.cells[i] = cell; 
        }

        return maze;
    }


    public Maze DetermineCellRotation(Maze maze) {
        //TODO: later, just set cell rotations to Quaternion.identity
        for (int i = 0; i < maze.cells.Count; i++)
        {
            var cell = maze.cells[i];
            cell.rotation = Quaternion.identity;
        }

        return maze;
    }
    
    public Maze DeterinePathToExit(Maze maze, Vector2Int exit)
    {
        Stack<MazeCell> stack = new Stack<MazeCell>();
        HashSet<string> visited = new HashSet<string>();

        MazeCell exitCell = maze.cells.Find(cell => cell.x == exit.x && cell.y == exit.y);
        stack.Push(exitCell);
        visited.Add($"{exitCell.x},{exitCell.y}");

        while (stack.Count > 0)
        {
            MazeCell current = stack.Pop();
            current.isOnPathToExit = true;
            int index = maze.cells.FindIndex(c => c.x == current.x && c.y == current.y);
            maze.cells[index] = current;

            foreach (var direction in new[] { Direction.LEFT, Direction.RIGHT, Direction.UP, Direction.DOWN })
            {
                int nx = current.x, ny = current.y;
                switch (direction)
                {
                    case Direction.LEFT: nx--; break;
                    case Direction.RIGHT: nx++; break;
                    case Direction.UP: ny++; break;
                    case Direction.DOWN: ny--; break;
                }

                if (IsInside(nx, ny, maze.width, maze.height) && IsPath(maze, nx, ny))
                {
                    string key = $"{nx},{ny}";
                    if (!visited.Contains(key))
                    {
                        stack.Push(new MazeCell { x = nx, y = ny, type = CellType.PATH });
                        visited.Add(key);
                    }
                }
            }
        }

        return maze;
    }

    private bool IsPath(Maze maze, int x, int y)
    {
        return maze.cells.Any(cell => cell.x == x && cell.y == y && cell.type == CellType.PATH);
    }
    private bool HasNeighbor(MazeCell cell, Maze maze, Direction direction)
    {
        int neighborX = cell.x;
        int neighborY = cell.y;
        switch(direction)
        {
            case Direction.LEFT: neighborX--; break;
            case Direction.RIGHT: neighborX++; break;
            case Direction.UP: neighborY++; break;
            case Direction.DOWN: neighborY--; break;
        }
        return maze.cells.Any(c => c.x == neighborX && c.y == neighborY && c.type != CellType.WALL);
    }
    
    private bool IsInside(int x, int y, int width, int height)
    {
        return x >= 0 && x < width && y >= 0 && y < height;
    }

    public void AddRandomDoors(Maze maze, int numberOfDoors)
    {
        List<MazeCell> wallCells = maze.cells.FindAll(cell => cell.type == CellType.WALL);
        List<MazeCell> validDoorLocations = new List<MazeCell>();

        foreach (MazeCell wall in wallCells)
        {
            // skip walls on the border
            if (wall.x == 0 || wall.y == 0 || wall.x == maze.width - 1 || wall.y == maze.height - 1)
            {
                continue;
            }

            // count neighboring path cells
            int pathNeighbors = 0;
            if (maze.cells[wall.x + 1 + wall.y * maze.width].type == CellType.PATH) pathNeighbors++;
            if (maze.cells[wall.x - 1 + wall.y * maze.width].type == CellType.PATH) pathNeighbors++;
            if (maze.cells[wall.x + (wall.y + 1) * maze.width].type == CellType.PATH) pathNeighbors++;
            if (maze.cells[wall.x + (wall.y - 1) * maze.width].type == CellType.PATH) pathNeighbors++;

            // Skip if there are 3 or more neighboring path cells
            if (pathNeighbors >= 3)
            {
                continue;
            }

            // valid horizontal door location
            if (maze.cells[wall.x + 1 + wall.y * maze.width].type == CellType.PATH &&
                maze.cells[wall.x - 1 + wall.y * maze.width].type == CellType.PATH)
            {
                validDoorLocations.Add(wall);
                continue;
            }

            // valid vertical door location
            if (maze.cells[wall.x + (wall.y + 1) * maze.width].type == CellType.PATH &&
                maze.cells[wall.x + (wall.y - 1) * maze.width].type == CellType.PATH)
            {
                validDoorLocations.Add(wall);
                continue;
            }
        }

        for (int i = 0; i < numberOfDoors; i++)
        {
            if (validDoorLocations.Count == 0)
            {
                Debug.LogWarning("Not enough valid locations to convert to doors.");
                return;
            }

            int randomIndex = Random.Range(0, validDoorLocations.Count);
            MazeCell doorCell = validDoorLocations[randomIndex];
            doorCell.type = CellType.DOOR;
            maze.cells[doorCell.x + doorCell.y * maze.width] = doorCell;
            validDoorLocations.RemoveAt(randomIndex);
        }
    }

    public void AddRandomVines(Maze maze, int numberOfVines)
    {
        List<MazeCell> pathCells = maze.cells.FindAll(cell => cell.type == CellType.PATH);
        for (int i = 0; i < numberOfVines; i++)
        {
            if (pathCells.Count == 0)
            {
                Debug.LogWarning("Not enough valid locations to add vines.");
                return;
            }

            int randomIndex = Random.Range(0, pathCells.Count);
            MazeCell vineCell = pathCells[randomIndex];
            vineCell.type = CellType.VINE;
            maze.cells[vineCell.x + vineCell.y * maze.width] = vineCell;
            pathCells.RemoveAt(randomIndex); 
        }
    }

    
}
