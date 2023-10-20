using System.Collections;
using System.Collections.Generic;
using MazeCore.enums;
namespace MazeCore.types
{
    public struct MazeCell
    {
        public int x;
        public int y;
        public CellType type;
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