using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BFramework.TetrisGame
{
    public class Tetromino
    {
        public enum SHAPE
        {
            Z,
            S,
            I,
            L,
            J,
            O,
            T
        }
        
        public SHAPE Shape { get; set; }
        public Square[] Squares { get; set; }
        public int[,,] SquarePositions { get; set; }
    }
}
