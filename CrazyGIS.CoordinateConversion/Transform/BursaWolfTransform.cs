using System;
using CrazyGIS.CoordinateConversion.Models;

namespace CrazyGIS.CoordinateConversion.Transform
{
	/// <summary>
	/// 布尔萨-沃尔夫 转换
	/// </summary>
	public class BursaWolfTransform
	{
		private double rx; // X轴旋转角度(弧度)
		private double ry; // Y轴旋转角度(弧度)
		private double rz; // Z轴旋转角度(弧度)
		private double dx; // X轴平移长度(米)
		private double dy; // Y轴平移长度(米)
		private double dz; // Z轴平移长度(米)
		private double m;  // 尺度参数

		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="parameters">七参数</param>
		public BursaWolfTransform(SevenParameters parameters)
		{
			if(parameters == null)
			{
				throw new Exception("转换七参数为空");
			}

			this.rx = parameters.XaxisRotateRadian;
			this.ry = parameters.YaxisRotateRadian;
			this.rz = parameters.ZaxisRotateRadian;
			this.dx = parameters.XaxisDeviation;
			this.dy = parameters.YaxisDeviation;
			this.dz = parameters.ZaxisDeviation;
			this.m = parameters.ScaleParameter;
		}

		/// <summary>
		/// 根据七参数对坐标进行转换
		/// </summary>
		/// <param name="coordinate">转换前坐标</param>
		/// <returns>转换后坐标</returns>
		public ICoordinate Transform(ICoordinate coordinate)
		{
			if(coordinate == null)
			{
				return null;
			}
			double inputX = coordinate.xAxis;
			double inputY = coordinate.yAxis;
			double inputZ = coordinate.zAxis;
			double outputX = 0;
			double outputY = 0;
			double outputZ = 0;
			this.sevenParamTransform(inputX, inputY, inputZ, out outputX, out outputY, out outputZ);

			ICoordinate result = new PlanePoint(outputX, outputY, outputZ);
			return result;
		}

		/// <summary>
		/// 七参数转换
		/// </summary>
		/// <param name="inputX"></param>
		/// <param name="inputY"></param>
		/// <param name="inputZ"></param>
		/// <param name="outputX"></param>
		/// <param name="outputY"></param>
		/// <param name="outputZ"></param>
		private void sevenParamTransform(double inputX, double inputY, double inputZ, 
			out double outputX, out double outputY, out double outputZ)
		{
			double[,] rotationMatrix = MatrixTransform.RotationMatrix(rx, ry, rz);
			double[,] originalCoord = new double[3, 1];
			originalCoord[0, 0] = inputX;
			originalCoord[1, 0] = inputY;
			originalCoord[2, 0] = inputZ;

			double[,] resCoord = MatrixTransform.MatrixProduct(rotationMatrix, originalCoord);

			outputX = resCoord[0, 0] * (m + 1) + dx;
			outputY = resCoord[1, 0] * (m + 1) + dy;
			outputZ = resCoord[2, 0] * (m + 1) + dz;
		}
	}
}
