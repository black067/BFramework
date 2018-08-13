using System;

namespace BFramework
{

    /// <summary>
    /// 三维浮点型向量
    /// </summary>
    [Serializable]
    public struct Vector
    {
        /// <summary>
        /// x值
        /// </summary>
        public float x;

        /// <summary>
        /// y值
        /// </summary>
        public float y;

        /// <summary>
        /// z值
        /// </summary>
        public float z;

        /// <summary>
        /// 构建一个三维浮点型向量并设置其 x, y, z
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public Vector(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        /// <summary>
        /// 构建一个三维浮点型向量并设置其 x, y
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Vector(float x, float y) : this(x, y, 0) { }

        /// <summary>
        /// 通过一个现有的向量构建一个三维浮点型向量
        /// </summary>
        /// <param name="vector"></param>
        public Vector(Vector vector) : this(vector.x, vector.y, vector.z) { }
        
        /// <summary>
        /// 取得一个零向量 (0, 0, 0)
        /// </summary>
        public static Vector Zero
        {
            get
            {
                return new Vector(0, 0, 0);
            }
        }

        /// <summary>
        /// 取得一个一向量 (1, 1, 1)
        /// </summary>
        public static Vector One
        {
            get
            {
                return new Vector(1, 1, 1);
            }
        }

        /// <summary>
        /// 取得向前的单位向量 (0, 0, 1)
        /// </summary>
        public static Vector Forward
        {
            get
            {
                return new Vector(0, 0, 1);
            }
        }

        /// <summary>
        /// 取得向后的单位向量 (0, 0, -1)
        /// </summary>
        public static Vector Back
        {
            get
            {
                return new Vector(0, 0, -1);
            }
        }

        /// <summary>
        /// 取得向上的单位向量 (0, 1, 0)
        /// </summary>
        public static Vector Up
        {
            get
            {
                return new Vector(0, 1, 0);
            }
        }

        /// <summary>
        /// 取得向下的单位向量 (0, -1, 0)
        /// </summary>
        public static Vector Down
        {
            get
            {
                return new Vector(0, -1, 0);
            }
        }

        /// <summary>
        /// 取得向右的单位向量 (1, 0, 0)
        /// </summary>
        public static Vector Right
        {
            get
            {
                return new Vector(1, 0, 0);
            }
        }

        /// <summary>
        /// 取得向左的单位向量 (-1, 0, 0)
        /// </summary>
        public static Vector Left
        {
            get
            {
                return new Vector(-1, 0, 0);
            }
        }

        /// <summary>
        /// 输出为字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("Vector({0:0.000}, {1:0.000}, {2:0.000})", x, y, z);
        }

        /// <summary>
        /// 设置向量的 x
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public Vector SetX(float x)
        {
            this.x = x;
            return this;
        }

        /// <summary>
        /// 设置向量的 y
        /// </summary>
        /// <param name="y"></param>
        /// <returns></returns>
        public Vector SetY(float y)
        {
            this.y = y;
            return this;
        }

        /// <summary>
        /// 设置向量的 z
        /// </summary>
        /// <param name="z"></param>
        /// <returns></returns>
        public Vector SetZ(float z)
        {
            this.z = z;
            return this;
        }

        /// <summary>
        /// 设置向量的 x, y
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public Vector SetXY(float x, float y)
        {
            this.x = x;
            this.y = y;
            return this;
        }

        /// <summary>
        /// 设置向量的 x, z
        /// </summary>
        /// <param name="x"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public Vector SetXZ(float x, float z)
        {
            this.x = x;
            this.z = z;
            return this;
        }

        /// <summary>
        /// 设置向量的 y, z
        /// </summary>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public Vector SetYZ(float y, float z)
        {
            this.y = y;
            this.z = z;
            return this;
        }

        /// <summary>
        /// 设置向量的 x, y, z
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public Vector Set(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            return this;
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

        /// <summary>
        /// 将向量归一化
        /// </summary>
        public Vector Normalized
        {
            get
            {
                return this / Magnitude;
            }
        }

        /// <summary>
        /// 与另一个向量相加得到一个新的向量
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public Vector Add(Vector other)
        {
            Vector result = new Vector(this);
            result.x += other.x;
            result.y += other.y;
            result.z += other.z;
            return result;
        }

        /// <summary>
        /// 与浮点数相乘得到一个新的向量
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public Vector Multiply(float other)
        {
            Vector result = new Vector(this);
            result.x *= other;
            result.y *= other;
            result.z *= other;
            return result;
        }

        /// <summary>
        /// 取得两个向量的点积
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public float Dot(Vector other)
        {
            return x * other.x + y * other.y + z * other.z;
        }

        /// <summary>
        /// 取得两个向量的叉积
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public Vector Cross(Vector other)
        {
            return new Vector(this)
            {
                x = y * other.z - z * other.y,
                y = z * other.x - x * other.z,
                z = x * other.y - y * other.x
            };
        }

        /// <summary>
        /// 取得两个向量的和向量
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static Vector operator +(Vector lhs, Vector rhs)
        {
            return lhs.Add(rhs);
        }

        /// <summary>
        /// 取得两个向量的差向量
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static Vector operator -(Vector lhs, Vector rhs)
        {
            return lhs.Add(-rhs);
        }

        /// <summary>
        /// 取得向量的逆向量
        /// </summary>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static Vector operator -(Vector rhs)
        {
            return rhs.Inverse;
        }

        /// <summary>
        /// 取得两个向量的点积
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static float operator *(Vector lhs, Vector rhs)
        {
            return lhs.Dot(rhs);
        }

        /// <summary>
        /// 取得向量与一个浮点数的乘积
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static Vector operator *(Vector lhs, float rhs)
        {
            return lhs.Multiply(rhs);
        }

        /// <summary>
        /// 取得向量与一个浮点数的商
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static Vector operator /(Vector lhs, float rhs)
        {
            return lhs.Multiply(1 / rhs);
        }

        /// <summary>
        /// 转化为整数型向量
        /// </summary>
        /// <param name="vector"></param>
        public static implicit operator VectorInt(Vector vector)
        {
            return new VectorInt((int)Math.Round(vector.x), (int)Math.Round(vector.y), (int)Math.Round(vector.z));
        }
    }

    /// <summary>
    /// 三维整数型向量
    /// </summary>
    [Serializable]
    public struct VectorInt
    {
        /// <summary>
        /// x值
        /// </summary>
        public int x;

        /// <summary>
        /// y值
        /// </summary>
        public int y;

        /// <summary>
        /// z值
        /// </summary>
        public int z;

        /// <summary>
        /// 构建一个三维整数型向量并设置其 x, y, z
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public VectorInt(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        /// <summary>
        /// 构建一个三维整数型向量并设置其 x, y
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public VectorInt(int x, int y) : this(x, y, 0) { }

        /// <summary>
        /// 构建一个三维整数型向量并设置其 x
        /// </summary>
        /// <param name="x"></param>
        public VectorInt(int x) : this(x, 0, 0) { }

        /// <summary>
        /// 通过一个现有的向量构建一个相等的向量
        /// </summary>
        /// <param name="vector"></param>
        public VectorInt(VectorInt vector) : this(vector.x, vector.y, vector.z) { }

        /// <summary>
        /// 取得零向量 (0, 0, 0)
        /// </summary>
        public static VectorInt Zero
        {
            get
            {
                return new VectorInt(0, 0, 0);
            }
        }

        /// <summary>
        /// 取得一向量 (1, 1, 1)
        /// </summary>
        public static VectorInt One
        {
            get
            {
                return new VectorInt(1, 1, 1);
            }
        }

        /// <summary>
        /// 取得前向量 (0, 0, 1)
        /// </summary>
        public static VectorInt Forward
        {
            get
            {
                return new VectorInt(0, 0, 1);
            }
        }

        /// <summary>
        /// 取得后向量 (0, 0, -1)
        /// </summary>
        public static VectorInt Back
        {
            get
            {
                return new VectorInt(0, 0, -1);
            }
        }

        /// <summary>
        /// 取得上向量 (0, 1, 0)
        /// </summary>
        public static VectorInt Up
        {
            get
            {
                return new VectorInt(0, 1, 0);
            }
        }

        /// <summary>
        /// 取得下向量 (0, -1, 0)
        /// </summary>
        public static VectorInt Down
        {
            get
            {
                return new VectorInt(0, -1, 0);
            }
        }

        /// <summary>
        /// 取得右向量 (1, 0, 0)
        /// </summary>
        public static VectorInt Right
        {
            get
            {
                return new VectorInt(1, 0, 0);
            }
        }

        /// <summary>
        /// 取得左向量 (-1, 0, 0)
        /// </summary>
        public static VectorInt Left
        {
            get
            {
                return new VectorInt(-1, 0, 0);
            }
        }

        /// <summary>
        /// 输出为字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("VectorInt({0}, {1}, {2})", x, y, z);
        }

        /// <summary>
        /// 设置向量的 x
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public VectorInt SetX(int x)
        {
            this.x = x;
            return this;
        }

        /// <summary>
        /// 设置向量的 y
        /// </summary>
        /// <param name="y"></param>
        /// <returns></returns>
        public VectorInt SetY(int y)
        {
            this.y = y;
            return this;
        }

        /// <summary>
        /// 设置向量的 z
        /// </summary>
        /// <param name="z"></param>
        /// <returns></returns>
        public VectorInt SetZ(int z)
        {
            this.z = z;
            return this;
        }

        /// <summary>
        /// 设置向量的 x, y
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public VectorInt SetXY(int x, int y)
        {
            this.x = x;
            this.y = y;
            return this;
        }

        /// <summary>
        /// 设置向量的 x, z
        /// </summary>
        /// <param name="x"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public VectorInt SetXZ(int x, int z)
        {
            this.x = x;
            this.z = z;
            return this;
        }

        /// <summary>
        /// 设置向量的 y, z
        /// </summary>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public VectorInt SetYZ(int y, int z)
        {
            this.y = y;
            this.z = z;
            return this;
        }

        /// <summary>
        /// 设置向量的 x, y, z
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public VectorInt Set(int x, int y ,int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            return this;
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

        /// <summary>
        /// 取得两个向量的和
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public VectorInt Add(VectorInt other)
        {
            VectorInt result = new VectorInt(this);
            result.x += other.x;
            result.y += other.y;
            result.z += other.z;
            return result;
        }

        /// <summary>
        /// 将向量与一个整数相乘
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public VectorInt Multiply(int other)
        {
            VectorInt result = new VectorInt(this);
            result.x *= other;
            result.y *= other;
            result.z *= other;
            return result;
        }

        /// <summary>
        /// 向量的点积
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int Dot(VectorInt other)
        {
            return x * other.x + y * other.y + z * other.z;
        }

        /// <summary>
        /// 向量叉积, 返回一个与这两个向量垂直的向量
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public VectorInt Cross(VectorInt other)
        {
            return new VectorInt(this)
            {
                x = y * other.z - z * other.y,
                y = z * other.x - x * other.z,
                z = x * other.y - y * other.x
            };
        }

        /// <summary>
        /// 取得两个向量的和向量
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static VectorInt operator +(VectorInt lhs, VectorInt rhs)
        {
            return lhs.Add(rhs);
        }

        /// <summary>
        /// 取得两个向量的差向量
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static VectorInt operator -(VectorInt lhs, VectorInt rhs)
        {
            return lhs.Add(-rhs);
        }

        /// <summary>
        /// 取得向量的逆向量
        /// </summary>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static VectorInt operator -(VectorInt rhs)
        {
            return rhs.Inverse;
        }

        /// <summary>
        /// 取得两个向量的点积
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static int operator *(VectorInt lhs, VectorInt rhs)
        {
            return lhs.Dot(rhs);
        }

        /// <summary>
        /// 将向量与一个整数相乘
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static VectorInt operator *(VectorInt lhs, int rhs)
        {
            return lhs.Multiply(rhs);
        }

        /// <summary>
        /// 将向量与一个整数相除
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static VectorInt operator /(VectorInt lhs, int rhs)
        {
            return lhs.Multiply(1 / rhs);
        }

        /// <summary>
        /// 转化为浮点型向量
        /// </summary>
        /// <param name="vectorInt"></param>
        public static implicit operator Vector(VectorInt vectorInt)
        {
            return new Vector(vectorInt.x, vectorInt.y, vectorInt.z);
        }
    }
}
