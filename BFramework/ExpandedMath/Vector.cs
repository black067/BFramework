using System;

namespace BFramework.ExpandedMath
{
    public class Vector
    {
        private float x;
        private float y;
        private float z;

        public static Vector Zero = new Vector(0, 0, 0);

        public Vector(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vector(float x, float y) : this(x, y, 0) { }

        public Vector() : this(0, 0, 0) { }

        public Vector(Vector vector):this (vector.X, vector.Y, vector.Z) { }

        public static Vector One
        {
            get
            {
                return new Vector(1, 1, 1);
            }
        }

        public override string ToString()
        {
            return string.Format("Vector({0:0.000}, {1:0.000}, {2:0.000})", X, Y, Z);
        }

        public Vector Inverse
        {
            get
            {
                return new Vector(-X, -Y, -Z);
            }
        }
        public float NormSquare
        {
            get
            {
                return X * X + Y * Y + Z * Z;
            }
        }

        public float Norm
        {
            get
            {
                return (float)Math.Sqrt(NormSquare);
            }
        }
        public Vector Add(Vector addition)
        {
            Vector result = new Vector(this);
            result.X += addition.X;
            result.Y += addition.Y;
            result.Z += addition.Z;
            return result;
        }
        public Vector Multiply(float multiplier)
        {
            Vector result = new Vector(this);
            result.X *= multiplier;
            result.Y *= multiplier;
            result.Z *= multiplier;
            return result;
        }
        public Vector Dot(Vector multiplier)
        {
            Vector result = new Vector(this);
            result.X *= multiplier.X;
            result.Y *= multiplier.Y;
            result.Z *= multiplier.Z;
            return result;
        }
        public Vector Cross(Vector multiplier)
        {
            return new Vector(this)
            {
                X = Y * multiplier.Z - Z * multiplier.Y,
                Y = Z * multiplier.X - X * multiplier.Z,
                Z = X * multiplier.Y - Y * multiplier.X
            };
        }

        public static Vector operator + (Vector lhs, Vector rhs)
        {
            return lhs.Add(rhs);
        }
        public static Vector operator - (Vector lhs, Vector rhs)
        {
            return lhs.Add(- rhs);
        }
        public static Vector operator - (Vector rhs)
        {
            return rhs.Inverse;
        }
        public static Vector operator * (Vector lhs, Vector rhs)
        {
            return lhs.Dot(rhs);
        }
        public static Vector operator * (Vector lhs, float rhs)
        {
            return lhs.Multiply(rhs);
        }
        public static Vector operator / (Vector lhs, float rhs)
        {
            return lhs.Multiply(1 / rhs);
        }

        public int XInt { get; private set; }
        public int YInt { get; private set; }
        public int ZInt { get; private set; }

        public float X
        {
            get { return x; }
            set
            {
                x = value;
                int r = (int)x;
                XInt = x - r >= 0.5f ? (r + 1) : r;
            }
        }
        public float Y
        {
            get { return y; }
            set
            {
                y = value;
                int r = (int)y;
                YInt = y - r >= 0.5f ? (r + 1) : r;
            }
        }
        public float Z
        {
            get { return z; }
            set
            {
                z = value;
                int r = (int)z;
                ZInt = z - r >= 0.5f ? (r + 1) : r;
            }
        }
    }
    
}
