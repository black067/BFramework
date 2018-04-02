using System;

namespace BFramework.ExpandedMath
{
    public class Vector
    {
        public float x;
        public float y;
        public float z;

        public static Vector Zero = new Vector(0, 0, 0);

        public Vector(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector(float x, float y) : this(x, y, 0) { }

        public Vector() : this(0, 0, 0) { }

        public Vector(Vector vector):this (vector.x, vector.y, vector.z) { }

        public static Vector One
        {
            get
            {
                return new Vector(1, 1, 1);
            }
        }

        public override string ToString()
        {
            return string.Format("Vector({0:0.000}, {1:0.000}, {2:0.000})", x, y, z);
        }

        public Vector Inverse
        {
            get
            {
                return new Vector(-x, -y, -z);
            }
        }
        public float NormSquare
        {
            get
            {
                return x * x + y * y + z * z;
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
            result.x += addition.x;
            result.y += addition.y;
            result.z += addition.z;
            return result;
        }
        public Vector Multiply(float multiplier)
        {
            Vector result = new Vector(this);
            result.x *= multiplier;
            result.y *= multiplier;
            result.z *= multiplier;
            return result;
        }
        public Vector Dot(Vector multiplier)
        {
            Vector result = new Vector(this);
            result.x *= multiplier.x;
            result.y *= multiplier.y;
            result.z *= multiplier.z;
            return result;
        }
        public Vector Cross(Vector multiplier)
        {
            return new Vector(this)
            {
                x = y * multiplier.z - z * multiplier.y,
                y = z * multiplier.x - x * multiplier.z,
                z = x * multiplier.y - y * multiplier.x
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
    }
}
