using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;


using MazeCore.enums;
namespace MazeCore.types
{
    public struct MazeCell
    {
        public int x;
        public int y;
        public CellType type;
        public bool isOnPathToExit;
        // optional, if cell is a path, 
        // does it exist at a dead end, 
        // left junction, right junction, or straight junction, 
        //cross, or t-junction, fork, or none of the above
        public JunctionType junctionType;
        public Quaternion rotation; // Rotation of the cell

    }

    public class Maze
    {
        public int width;
        public int height;
        public List<MazeCell> cells;

        public Maze(int width, int height)
        {
            this.width = width;
            this.height = height;
            this.cells = new List<MazeCell>();
        }
    }

}