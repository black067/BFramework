using System;

namespace BFramework.ExpandedMath
{
    public class Vector
    {
        public float x;
        public float y;
        public float z;

        public Vector(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector(float x, float y) : this(x, y, 0) { }

        public Vector() : this(0, 0, 0) { }

        public Vector(Vector vector) : this(vector.x, vector.y, vector.z) { }


        public static Vector Zero
        {
            get
            {
                return new Vector(0, 0, 0);
            }
        }

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

        /// <summary>
        /// 逆向量
        /// </summary>
        public Vector Inverse
        {
            get
            {
                return new Vector(-x, -y, -z);
            }
        }

        /// <summary>
        /// 向量长度的平方(不开根号)
        /// </summary>
        public float SqrMagnitude
        {
            get
            {
                return x * x + y * y + z * z;
            }
        }

        /// <summary>
        /// 向量的长度
        /// </summary>
        public float Magnitude
        {
            get
            {
                return (float)Math.Sqrt(SqrMagnitude);
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
        public float Dot(Vector multiplier)
        {
            return x * multiplier.x + y * multiplier.y + z * multiplier.z;
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

        public static Vector operator +(Vector lhs, Vector rhs)
        {
            return lhs.Add(rhs);
        }
        public static Vector operator -(Vector lhs, Vector rhs)
        {
            return lhs.Add(-rhs);
        }
        public static Vector operator -(Vector rhs)
        {
            return rhs.Inverse;
        }
        public static float operator *(Vector lhs, Vector rhs)
        {
            return lhs.Dot(rhs);
        }
        public static Vector operator *(Vector lhs, float rhs)
        {
            return lhs.Multiply(rhs);
        }
        public static Vector operator /(Vector lhs, float rhs)
        {
            return lhs.Multiply(1 / rhs);
        }

        public static implicit operator VectorInt(Vector vector)
        {
            return new VectorInt((int)Math.Round(vector.x), (int)Math.Round(vector.y), (int)Math.Round(vector.z));
        }
    }

    public class VectorInt
    {
        public int x;
        public int y;
        public int z;

        public VectorInt(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public VectorInt(int x, int y) : this(x, y, 0) { }

        public VectorInt() : this(0, 0, 0) { }

        public static VectorInt Zero
        {
            get
            {
                return new VectorInt(0, 0, 0);
            }
        }

        public VectorInt(VectorInt vector) : this(vector.x, vector.y, vector.z) { }

        public static VectorInt One
        {
            get
            {
                return new VectorInt(1, 1, 1);
            }
        }

        public override string ToString()
        {
            return string.Format("VectorInt({0}, {1}, {2})", x, y, z);
        }

        /// <summary>
        /// 逆向量
        /// </summary>
        public VectorInt Inverse
        {
            get
            {
                return new VectorInt(-x, -y, -z);
            }
        }

        /// <summary>
        /// 向量长度的平方(不开根号)
        /// </summary>
        public int SqrMagnitude
        {
            get
            {
                return x * x + y * y + z * z;
            }
        }

        /// <summary>
        /// 向量的长度
        /// </summary>
        public float Magnitude
        {
            get
            {
                return (float)Math.Sqrt(SqrMagnitude);
            }
        }

        public VectorInt Add(VectorInt addition)
        {
            VectorInt result = new VectorInt(this);
            result.x += addition.x;
            result.y += addition.y;
            result.z += addition.z;
            return result;
        }

        public VectorInt Multiply(int multiplier)
        {
            VectorInt result = new VectorInt(this);
            result.x *= multiplier;
            result.y *= multiplier;
            result.z *= multiplier;
            return result;
        }

        /// <summary>
        /// 向量的点积
        /// </summary>
        /// <param name="multiplier"></param>
        /// <returns></returns>
        public int Dot(VectorInt multiplier)
        {
            return x * multiplier.x + y * multiplier.y + z * multiplier.z;
        }

        /// <summary>
        /// 向量叉积, 返回一个与这两个向量垂直的向量,
        /// </summary>
        /// <param name="multiplier"></param>
        /// <returns></returns>
        public VectorInt Cross(VectorInt multiplier)
        {
            return new VectorInt(this)
            {
                x = y * multiplier.z - z * multiplier.y,
                y = z * multiplier.x - x * multiplier.z,
                z = x * multiplier.y - y * multiplier.x
            };
        }

        public static VectorInt operator +(VectorInt lhs, VectorInt rhs)
        {
            return lhs.Add(rhs);
        }
        public static VectorInt operator -(VectorInt lhs, VectorInt rhs)
        {
            return lhs.Add(-rhs);
        }
        public static VectorInt operator -(VectorInt rhs)
        {
            return rhs.Inverse;
        }
        public static int operator *(VectorInt lhs, VectorInt rhs)
        {
            return lhs.Dot(rhs);
        }
        public static VectorInt operator *(VectorInt lhs, int rhs)
        {
            return lhs.Multiply(rhs);
        }
        public static VectorInt operator /(VectorInt lhs, int rhs)
        {
            return lhs.Multiply(1 / rhs);
        }

        public static implicit operator Vector(VectorInt vectorInt)
        {
            return new Vector(vectorInt.x, vectorInt.y, vectorInt.z);
        }
    }
}
