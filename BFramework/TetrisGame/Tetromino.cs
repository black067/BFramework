using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BFramework.ExpandedMath;

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
        public static readonly Dictionary<SHAPE, Vector[]> SquareOffsets = new Dictionary<SHAPE, Vector[]>
        {
            {SHAPE.Z, new Vector[]{new Vector(-1, 0), new Vector(0, 0), new Vector(0, -1), new Vector(1, -1)} },
            {SHAPE.S, new Vector[]{new Vector(1, 0), new Vector(0, 0), new Vector(0, -1), new Vector(-1, -1)} },
            {SHAPE.I, new Vector[]{new Vector(0, 1), new Vector(0, 0), new Vector(0, -1), new Vector(0, -2)} },
            {SHAPE.L, new Vector[]{new Vector(1, 0), new Vector(0, 0), new Vector(0, 1), new Vector(0, 2)} },
            {SHAPE.J, new Vector[]{new Vector(-1, 0), new Vector(0, 0), new Vector(0, 1), new Vector(0, 2)} },
        };
        
        public SHAPE Shape { get; set; }
        public Square[] Squares { get; set; }
        public int[,,] SquarePositions { get; set; }
    }
}
