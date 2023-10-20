using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MazeCore.types;
using MazeCore.enums;

public class MazeManager : MonoBehaviour
{
    public MazeRenderer mazeRenderer;
    public MazeGenerator mazeGenerator;  // This is your reference to the MazeGenerator

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


    void Start() 
    {
        // Logic for generating mazes and boxes similar to your React component
        Maze maze1 = mazeGenerator.GenerateMaze(WIDTH, HEIGHT, new Vector2Int(1, TOP), new Vector2Int((int) Mathf.Floor(WIDTH / 2.0f) - 1, BOTTOM));
        //... generate other mazes and boxes
        mazeRenderer.RenderMaze(maze1);
        //... render other mazes and boxes
    }
}
