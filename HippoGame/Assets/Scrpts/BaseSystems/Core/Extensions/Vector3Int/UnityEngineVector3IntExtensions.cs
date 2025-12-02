using UnityEngine;

namespace Scripts.BaseSystems.Core
{
    public static class UnityEngineVector3IntExtensions 
    {
        public static int EncodPointByMatrixSize(this Vector3Int point, Vector3Int matrixSize)
        {
            return EncodePoint(
                point.x,
                point.y,
                point.z,
                matrixSize.x,
                matrixSize.y,
                matrixSize.z
                );
        }

        public static int EncodPointByMatrixSize(this Vector3Int point, int matrixSizeX, int matrixSizeY, int matrixSizeZ)
        {
            return EncodePoint(
                point.x,
                point.y,
                point.z,
                matrixSizeX,
                matrixSizeY,
                matrixSizeZ
                );
        }

        public static int EncodeByPoint(this Vector3Int size, int pointX, int pointY, int pointZ)
        {
            return EncodePoint(
                pointX,
                pointY,
                pointZ,
                size.x,
                size.y,
                size.z
                );
        }

        /// <summary>
        ///     Current vector is treated as matrix size vector.
        /// </summary>
        /// <param name="size"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static int EncodeByPoint(this Vector3Int size, Vector3Int point)
        {
            return EncodePoint(
                point.x,
                point.y,
                point.z,
                size.x,
                size.y,
                size.z
                );
        }

        public static int EncodePoint(int pointX, int pointY, int pointZ, int sizeX, int sizeY, int sizeZ)
        {
            return pointX + pointY * sizeZ + pointZ * sizeZ * sizeY;
        }

        public static Vector3Int DecodePointPosition(this Vector3Int matrixSize, int index)
        {
            int multiplication = (matrixSize.z * matrixSize.y);
            int z = index / multiplication;
            int remainder = index % multiplication;
            int y = remainder / matrixSize.z;
            int x = remainder % matrixSize.z;
            return new Vector3Int(x, y, z);
        }
    }
}

