using System.Collections.Generic;

namespace BFramework.World
{

    /// <summary>
    /// 平面上长方形边界信息
    /// </summary>
    public struct FixedBounds2D
    {

        /// <summary>
        /// 长方形的锚点, 即左下顶点
        /// </summary>
        public VectorInt pivot;

        /// <summary>
        /// 长方形的尺寸, x : 宽, y : 0, z : 高
        /// </summary>
        public VectorInt size;

        /// <summary>
        /// 长方形的所有顶点
        /// </summary>
        public VectorInt[] vertices;

        /// <summary>
        /// 判断点在长方形中时使用的偏移量
        /// </summary>
        public int boxOffset;

        /// <summary>
        /// 长方形边界上的点集
        /// </summary>
        public VectorInt[] boundries;

        /// <summary>
        /// 长方形外边界上的点集
        /// </summary>
        public VectorInt[] boundriesOutside;

        /// <summary>
        /// 对角点
        /// </summary>
        public VectorInt Diagonal
        {
            get { return pivot + size; }
        }

        /// <summary>
        /// 左上方顶点
        /// </summary>
        public VectorInt UpLeft
        {
            get { return new VectorInt(pivot.x, 0, pivot.z + size.z); }
        }

        /// <summary>
        /// 右下方顶点
        /// </summary>
        public VectorInt BottomRight
        {
            get { return new VectorInt(pivot.x + size.x, 0, pivot.z); }
        }
        
        /// <summary>
        /// 通过锚点坐标与尺寸构造一个边界信息
        /// </summary>
        /// <param name="position"></param>
        /// <param name="size"></param>
        public FixedBounds2D(VectorInt position, VectorInt size)
        {
            pivot = position;
            this.size = size;
            List<VectorInt> vectors = new List<VectorInt>
            {
                pivot,
                pivot + new VectorInt(size.x, 0, size.z),
                pivot + new VectorInt(size.x, 0, 0),
                pivot + new VectorInt(0, 0, size.z)
            };
            List<VectorInt> boundriesList = new List<VectorInt>();
            List<VectorInt> boundriesOutsideList = new List<VectorInt>();
            for (int i = 0, length = size.x; i <= length; i++)
            {
                boundriesList.Add(pivot + new VectorInt(i, pivot.y, 0));
                boundriesList.Add(pivot + new VectorInt(i, pivot.y, size.z));
                boundriesOutsideList.Add(pivot + new VectorInt(i, pivot.y, -1));
                boundriesOutsideList.Add(pivot + new VectorInt(i, pivot.y, size.z + 1));
            }
            for (int j = 0, length = size.z; j <= length; j++)
            {
                boundriesList.Add(pivot + new VectorInt(0, pivot.y, j));
                boundriesList.Add(pivot + new VectorInt(size.x, pivot.y, j));
                boundriesOutsideList.Add(pivot + new VectorInt(-1, pivot.y, j));
                boundriesOutsideList.Add(pivot + new VectorInt(size.x + 1, pivot.y, j));
            }

            boundries = boundriesList.ToArray();
            boundriesOutside = boundriesOutsideList.ToArray();
            vertices = vectors.ToArray();
            boxOffset = 1;
        }

        /// <summary>
        /// 判断点是否在边界内部(无视点在 y 方向上的坐标)
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool IsPointInBounds(VectorInt point)
        {
            if (point.x > pivot.x - boxOffset &&
                point.x < Diagonal.x + boxOffset &&
                point.z > pivot.z - boxOffset &&
                point.z < Diagonal.z + boxOffset)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 判断另一个边界是否与此有重叠
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool IsIntersectWith(FixedBounds2D other)
        {
            bool pivotIn = IsPointInBounds(other.pivot);
            bool diagonalIn = IsPointInBounds(other.Diagonal);
            bool bottomRightIn = IsPointInBounds(other.BottomRight);
            bool upLeftlIn = IsPointInBounds(other.UpLeft);
            if ((bottomRightIn || upLeftlIn || pivotIn || diagonalIn))
            {
                return true;
            }
            if (!bottomRightIn && !upLeftlIn && !pivotIn && !diagonalIn)
            {
                pivotIn = other.IsPointInBounds(pivot);
                diagonalIn = other.IsPointInBounds(Diagonal);
                bottomRightIn = other.IsPointInBounds(BottomRight);
                upLeftlIn = other.IsPointInBounds(UpLeft);
                if ((bottomRightIn || upLeftlIn || pivotIn || diagonalIn))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
