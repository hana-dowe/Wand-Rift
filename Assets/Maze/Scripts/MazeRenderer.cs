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
    public GameObject DoorPrefab;
    public GameObject VinePrefab;

    public GameObject MazeDeadEndCell;
    public GameObject MazeForkCell;
    public GameObject MazeHallCell;
    public GameObject MazeCornerCell;
    public GameObject MazeCrossCell;

    public GameObject Foliage1Prefab;
    public GameObject Foliage2Prefab;
    public GameObject Foliage3Prefab;
    public GameObject Foliage4Prefab;

    public GameObject crossFoliagePrefab;
    public GameObject turnFoliagePrefab;


    public float spacing_x = 8.0f;
    public float spacing_y = 7.99f;

    public void RenderMaze(Maze maze, Vector3 offset = default)
    {
        for(int i = 0; i < maze.cells.Count; i++)
        {
            MazeCell cell = maze.cells[i];
            Vector3 cellPosition = new Vector3(cell.x * spacing_x, 4.1f, cell.y * spacing_y) + offset;  // Add the offset to the calculated position
            GameObject instantiatedPrefab = null;

            switch(cell.type)
            {
                case CellType.PATH:
                    GameObject prefab;
                    Quaternion rotation;
                    DeterminePrefabAndRotationForPath(cell, maze, out prefab, out rotation);
                    Instantiate(prefab, cellPosition, rotation);
                    break;
                case CellType.WALL:
                    Instantiate(WallPrefab, cellPosition, Quaternion.identity);
                    break;
                case CellType.ENTRY:
                    Instantiate(EntryPrefab, cellPosition, Quaternion.identity);
                    break;
                case CellType.EXIT:
                    Instantiate(ExitPrefab, cellPosition, Quaternion.identity);
                    break;
                case CellType.DOOR:
                    Quaternion doorRotation = DetermineDoorRotation(cell, maze);
                    Instantiate(DoorPrefab, cellPosition, doorRotation);
                    break;

                // case CellType.VINE:
                //     Quaternion vineRotation = DetermineDoorRotation(cell, maze); // Same rotation as door
                //     Instantiate(VinePrefab, cellPosition, vineRotation);
                //     break;

            }
            Debug.Log("cell type: " + cell.type + " cell junction type: " + cell.junctionType);
             if (instantiatedPrefab != null)
            {
                MazeCellComponent cellComponent = instantiatedPrefab.GetComponent<MazeCellComponent>();
                
                if (cellComponent != null)
                {   
                    cellComponent.x = cell.x;
                    cellComponent.y = cell.y;
                    cellComponent.cellType = cell.type;
                    cellComponent.junctionType = cell.junctionType;
                    cellComponent.isOnPathToExit = cell.isOnPathToExit;
                    cellComponent.rotation = cell.rotation;
                    cellComponent.worldPosition = cellPosition;
                }
            }
        }

    }

    public void RenderEmptyBox(Maze maze, Vector3 offset = default)
    {
        int width = maze.width;
        int height = maze.height;
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {

                Vector3 cellPosition = new Vector3(x * spacing_x, 0, y * spacing_y) + offset;
                if (x == 0 || y == 0 || x == width - 1 || y == height - 1)
                {
                    Instantiate(WallPrefab, cellPosition, Quaternion.identity);
                }
            }
        }
    }

    
    private void DeterminePrefabAndRotationForPath(MazeCell cell, Maze maze, out GameObject prefab, out Quaternion rotation)
    {
        // Default to a simple hall cell
        prefab = MazeHallCell;
        rotation = cell.rotation;
        switch (cell.junctionType)
        {
            case JunctionType.DeadEnd:
                if (cell.type == CellType.ENTRY || cell.type == CellType.EXIT || IsOnBoundary(cell, maze))
                {
                    prefab = MazeHallCell;
                }
                else
                {
                    prefab = MazeDeadEndCell;
                }
                break;

            case JunctionType.StraightJunction:
                prefab = MazeHallCell;
                break;

            case JunctionType.RightJunction:
            case JunctionType.LeftJunction:
                prefab = MazeCornerCell;
                break;
            

            case JunctionType.TJunction:
            case JunctionType.Fork:
                prefab = MazeForkCell;
                break;

            default:
                prefab = MazeHallCell;
                break;
        }
    }
    
    private void DetermineFoliageAndRotation(MazeCell cell, bool hasLeft, bool hasRight, bool hasUp, bool hasDown, out GameObject foliagePrefab)
    {
        foliagePrefab = null;

        int count = (hasLeft ? 1 : 0) + (hasRight ? 1 : 0) + (hasUp ? 1 : 0) + (hasDown ? 1 : 0);

        Quaternion foliageRotation = Quaternion.identity;

        switch (count)
        {
            case 0: // Dead end
                foliagePrefab = ChooseFoliagePrefab();
                break;

            case 2: // Hall or Corner
                if (hasLeft && hasRight)
                {
                    foliagePrefab = ChooseFoliagePrefab();
                    // No rotation needed for X axis hall
                }
                else if (hasUp && hasDown)
                {
                    foliagePrefab = ChooseFoliagePrefab();
                    foliageRotation = Quaternion.Euler(0, 90, 0); // Rotate for Z axis hall
                }
                else if (hasLeft && hasDown)
                {
                    foliagePrefab = ChooseFoliagePrefab();
                    foliageRotation = Quaternion.Euler(0, -90, 0);
                }
                else if (hasRight && hasDown)
                {
                    foliagePrefab = ChooseFoliagePrefab();
                    foliageRotation = Quaternion.Euler(0, 180, 0);
                }
                else if (hasLeft && hasUp)
                {
                    foliagePrefab = turnFoliagePrefab; // Use turn foliage for this condition
                    foliageRotation = Quaternion.Euler(0, 0, 0);
                }
                else if (hasRight && hasUp)
                {
                    foliagePrefab = ChooseFoliagePrefab();
                    foliageRotation = Quaternion.Euler(0, 90, 0);
                }
                break;

            case 3: // T-Junction or Fork
            case 4: // Cross
                break;
        }

        if (foliagePrefab != null)
        {
            Instantiate(foliagePrefab, new Vector3(cell.x * spacing_x, 0, cell.y * spacing_y), foliageRotation);
        }
    }


    private Quaternion DetermineDoorRotation(MazeCell cell, Maze maze)
    {
        bool hasLeftNeighbor = HasNeighbor(cell, maze, Direction.LEFT);
        bool hasRightNeighbor = HasNeighbor(cell, maze, Direction.RIGHT);
        bool hasUpNeighbor = HasNeighbor(cell, maze, Direction.UP);
        bool hasDownNeighbor = HasNeighbor(cell, maze, Direction.DOWN);

        // List to hold possible rotations
        List<Quaternion> possibleRotations = new List<Quaternion>();

        // Handle opposing sides (left-right, up-down)
        if (hasLeftNeighbor && hasRightNeighbor)
        {
            // Choose between left and right randomly or based on a specific rule
            possibleRotations.Add(Quaternion.Euler(0, 90, 0)); // Left
            possibleRotations.Add(Quaternion.Euler(0, -90, 0)); // Right
        }
        else if (hasUpNeighbor && hasDownNeighbor)
        {
            // Choose between up and down randomly or based on a specific rule
            possibleRotations.Add(Quaternion.Euler(0, 180, 0)); // Up
            possibleRotations.Add(Quaternion.Euler(0, 0, 0)); // Down
        }
        else
        {
            // Handle single direction cases
            if (hasLeftNeighbor)
                possibleRotations.Add(Quaternion.Euler(0, 90, 0));
            if (hasRightNeighbor)
                possibleRotations.Add(Quaternion.Euler(0, -90, 0));
            if (hasUpNeighbor)
                possibleRotations.Add(Quaternion.Euler(0, 180, 0));
            if (hasDownNeighbor)
                possibleRotations.Add(Quaternion.Euler(0, 0, 0));
        }

        // Choose a rotation randomly if there are multiple possibilities
        if (possibleRotations.Count > 0)
            return possibleRotations[Random.Range(0, possibleRotations.Count)];

        // Default rotation if no conditions are met
        return Quaternion.Euler(0, Random.Range(0, 4) * 90, 0); // Randomly choose between 0, 90, 180, 270 degrees
    }

    private GameObject ChooseFoliagePrefab()
    {
        GameObject[] foliageOptions = { Foliage1Prefab, Foliage2Prefab, Foliage3Prefab, Foliage4Prefab };
        int randomIndex = Random.Range(0, foliageOptions.Length);
        return foliageOptions[randomIndex];
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
