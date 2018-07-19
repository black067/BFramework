using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BFramework.TetrisGame
{
    public class Square
    {
        public float x;
        public float y;
        
        public STATE State;
        public enum STATE
        {
            EMPTY,
            PLACED,
            INMOTION
        }
        public Square()
        {

        }
    }
}
