using System;

namespace CrazyGIS.CoordinateConversion.Transform
{
	/// <summary>
	/// 矩阵转换
	/// </summary>
	public class MatrixTransform
	{
		/// <summary>
		/// 根据旋转角度求旋转矩阵
		/// </summary>
		/// <param name="xRotation">X轴旋转角度(弧度值)</param>
		/// <param name="yRotation">Y轴旋转角度(弧度值)</param>
		/// <param name="zRotation">Z轴旋转角度(弧度值)</param>
		/// <returns></returns>
		public static double[,] RotationMatrix(double xRotation, double yRotation, double zRotation)
		{
			double[,] fhjz = new double[3, 3]; // 复合矩阵
			double[,] jzx = new double[3, 3], jzy = new double[3, 3], jzz = new double[3, 3];
			double[,] temp;

			//下面是徐邵铨《GPS测量原理及应用》P25上的公式
			jzx[0, 0] = 1; jzx[0, 1] = 0; jzx[0, 2] = 0;
			jzx[1, 0] = 0; jzx[1, 1] = Math.Cos(xRotation); jzx[1, 2] = Math.Sin(xRotation);
			jzx[2, 0] = 0; jzx[2, 1] = -Math.Sin(xRotation); jzx[2, 2] = Math.Cos(xRotation);

			jzy[0, 0] = Math.Cos(yRotation); jzy[0, 1] = 0; jzy[0, 2] = -Math.Sin(yRotation);
			jzy[1, 0] = 0; jzy[1, 1] = 1; jzy[1, 2] = 0;
			jzy[2, 0] = Math.Sin(yRotation); jzy[2, 1] = 0; jzy[2, 2] = Math.Cos(yRotation);

			jzz[0, 0] = Math.Cos(zRotation); jzz[0, 1] = Math.Sin(zRotation); jzz[0, 2] = 0;
			jzz[1, 0] = -Math.Sin(zRotation); jzz[1, 1] = Math.Cos(zRotation); jzz[1, 2] = 0;
			jzz[2, 0] = 0; jzz[2, 1] = 0; jzz[2, 2] = 1;

			temp = MatrixProduct(jzz, jzy);
			fhjz = MatrixProduct(temp, jzx);

			return fhjz;
		}

		/// <summary>
		/// 矩阵乘积
		/// </summary>
		/// <param name="matrix1"></param>
		/// <param name="matrix2"></param>
		/// <returns></returns>
		public static double[,] MatrixProduct(double[,] matrix1, double[,] matrix2)
		{
			int m1, n1, m2, n2;
			double temp;
			m1 = matrix1.GetLength(0); n1 = matrix1.GetLength(1);
			m2 = matrix2.GetLength(0); n2 = matrix2.GetLength(1);

			double[,] result = new double[m1, n2];

			for (int i = 0; i < m1; i++)
			{
				for (int j = 0; j < n2; j++)
				{
					temp = 0;
					for (int k = 0; k < n1; k++)
					{
						temp = temp + matrix1[i, k] * matrix2[k, j];
					}
					result[i, j] = temp;
				}
			}

			return result;
		}
	}
}
