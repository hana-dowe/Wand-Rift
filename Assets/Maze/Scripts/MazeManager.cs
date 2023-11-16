using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.AI; 

using MazeCore.types;
using MazeCore.enums;

public class MazeManager : MonoBehaviour
{
    public MazeRenderer mazeRenderer;
    public MazeGenerator mazeGenerator;
    public NavMeshSurface navMeshSurface;

    // Game CONSTANTS
    private const int WIDTH = 21;
    private const int HEIGHT = 21;

    private const int TOP = 1;
    private const int BOTTOM = HEIGHT - 2;

    //GROSS!
    private int GetRandomOddInt(int min, int max) 
    {
        min = Mathf.CeilToInt(min);
        max = Mathf.FloorToInt(max);

        if (min % 2 == 0) min++;
        if (max % 2 == 0) max--;

        return Mathf.FloorToInt(Random.Range(min / 2.0f, (max - min) / 2.0f + 1)) * 2 + min;
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

    void Start() 
    {
        int PORTAL1, PORTAL2, PORTAL3;
        PORTAL1 = 1;
        PORTAL2 = (int) Mathf.Floor(WIDTH / 2.0f) - 1;
        PORTAL3 = GetRandomOddInt(1, WIDTH - 2);

        Maze maze1 = mazeGenerator.GenerateMaze(
            WIDTH, HEIGHT, 
            new Vector2Int(PORTAL1, TOP), 
            new Vector2Int(PORTAL2, BOTTOM)
        );

        mazeGenerator.AddRandomDoors(maze1, 5);
        mazeGenerator.AddRandomVines(maze1, 5);
        mazeRenderer.RenderMaze(maze1);

        // // offset for the second maze.
        // Vector3 offset = new Vector3(0, 0, (HEIGHT) * mazeRenderer.spacing_y);

        // Maze maze2 = mazeGenerator.CreateEmptyBox(
        //     WIDTH, HEIGHT, 
        //     new Vector2Int(PORTAL2, TOP), 
        //     new Vector2Int(PORTAL3, BOTTOM)
        // );
        // mazeRenderer.RenderEmptyBox(maze2, offset);

        BakeNavMesh();
    }
}
