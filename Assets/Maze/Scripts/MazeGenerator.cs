using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MazeCore.types;
using MazeCore.enums;

public class MazeGenerator : MonoBehaviour
{


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static Maze CreateEmptyBox(int width, int height, Vector2Int entrance, Vector2Int? exit)
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

        MazeCell entranceCell = maze.cells[entrance.x + (entrance.y - 1) * width];
        entranceCell.type = CellType.PATH;
        maze.cells[entrance.x + (entrance.y - 1) * width] = entranceCell;

        if (exit.HasValue) 
        {
            MazeCell exitCell = maze.cells[exit.Value.x + (exit.Value.y + 1) * width];
            exitCell.type = CellType.PATH;
            maze.cells[exit.Value.x + (exit.Value.y + 1) * width] = exitCell;
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

        return maze;
    }
}
