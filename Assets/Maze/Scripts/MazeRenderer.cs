using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.AI; 

using MazeCore.types;
using MazeCore.enums; 

public class MazeRenderer : MonoBehaviour
{

    public GameObject EntryPrefab;
    public GameObject ExitPrefab;
    public GameObject WallPrefab;

    public GameObject MazeDeadEndCell;
    public GameObject MazeForkCell;
    public GameObject MazeHallCell;
    public GameObject MazeCornerCell;
    public GameObject MazeCrossCell;

    public NavMeshSurface navMeshSurface;



    private float spacing_x = 8.0f;
    private float spacing_y = 7.99f;

    public void RenderMaze(Maze maze)
    {
        foreach (MazeCell cell in maze.cells)
        {
            switch(cell.type)
            {
                case CellType.PATH:
                    GameObject prefab;
                    DeterminePrefabAndRotationForPath(cell, maze, out prefab);
  
                    Instantiate(prefab, new Vector3(cell.x * spacing_x, 0, cell.y * spacing_y), Quaternion.identity);
                    break;
                case CellType.WALL:
                    Instantiate(WallPrefab, new Vector3(cell.x * spacing_x, 0, cell.y * spacing_y), Quaternion.identity);
                    break;
                case CellType.ENTRY:
                    Instantiate(EntryPrefab, new Vector3(cell.x * spacing_x, 0, cell.y * spacing_y), Quaternion.identity);
                    break;
                case CellType.EXIT:
                    Instantiate(ExitPrefab, new Vector3(cell.x * spacing_x, 0, cell.y * spacing_y), Quaternion.identity);
                    break;
            }
        }

        BakeNavMesh();
    }

    private void BakeNavMesh()
    {
        if (navMeshSurface != null)
        {
            navMeshSurface.BuildNavMesh();
        }
        else
        {
            Debug.LogError("NavMeshSurface reference is not set.");
        }
    }

    private void DeterminePrefabAndRotationForPath(MazeCell cell, Maze maze, out GameObject prefab)
    {
        prefab = MazeHallCell;

        bool hasLeftNeighbor = HasNeighbor(cell, maze, Direction.LEFT);
        bool hasRightNeighbor = HasNeighbor(cell, maze, Direction.RIGHT);
        bool hasUpNeighbor = HasNeighbor(cell, maze, Direction.UP);
        bool hasDownNeighbor = HasNeighbor(cell, maze, Direction.DOWN);
        


        int count = (hasLeftNeighbor ? 1 : 0) + (hasRightNeighbor ? 1 : 0) + (hasUpNeighbor ? 1 : 0) + (hasDownNeighbor ? 1 : 0);

        switch(count)
        {
            case 1: // Dead end
            if (cell.type == CellType.ENTRY || cell.type == CellType.EXIT || IsOnBoundary(cell, maze))
            {
                prefab = MazeHallCell;
            }
            else
            {
                prefab = MazeDeadEndCell;
            }
            break;

            case 2: // Hall or Corner
                if (hasLeftNeighbor && hasRightNeighbor)
                {
                    prefab =  MazeHallCell;
                }
                else if (hasUpNeighbor && hasDownNeighbor)
                {
                    prefab =   MazeHallCell;
                }
                else
                {
                    prefab =  MazeCornerCell;
                }
                break;

            case 3: // T-Junction or Fork
                prefab =  MazeForkCell;
                break;

            case 4: // Cross
                prefab =  MazeCrossCell;
                break;
        }

    }


    private bool IsOnBoundary(MazeCell cell, Maze maze)
    {
        return cell.x == 0 || cell.x == maze.width - 1 || cell.y == 0 || cell.y == maze.height - 1;
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
}
