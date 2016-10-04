using System;
using CrazyGIS.CoordinateConversion.Models;

namespace CrazyGIS.CoordinateConversion.Transform
{
	/// <summary>
	/// 大地坐标系与空间直角坐标系的转换
	/// </summary>
	public class GeodeticTransform
	{
		private double a = 0;  // 长半轴
		private double b = 0;  // 短半轴
		private double e = 0;  // 第一偏心率
		private double ep = 0; // 第二偏心率
		private double centerMeridian = 0;  // 中央子午线

		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="sourceCoordinateSystem">坐标系统</param>
		/// <param name="centerMeridian">中央子午线</param>
		public GeodeticTransform(CoordinateSystemType sourceCoordinateSystem, double centerMeridian)
		{
			EllipsoidalParameters parameters = EllipsoidalType.GetEllipsoidalParametersByTpye(sourceCoordinateSystem);
			a = parameters.SemiMajorAxis;
			b = parameters.SemiMinorAxis;
			e = Math.Sqrt(a * a - b * b) / a;
			ep = Math.Sqrt(a * a - b * b) / b;

			this.centerMeridian = centerMeridian;
		}

		#region 接口

		/// <summary>
		/// 大地坐标转空间直角坐标
		/// </summary>
		/// <param name="coordinate">大地坐标</param>
		/// <returns></returns>
		public PlanePoint GeodeticToThreeDimensions(SpherePoint coordinate)
		{
			double B = coordinate.lng;
			double L = coordinate.lat;
			double H = coordinate.hgt;
			double X = 0;
			double Y = 0;
			double Z = 0;
			this.BLHtoXYZ(B, L, H, out X, out Y, out Z);
			PlanePoint result = new PlanePoint(X, Y, Z);
			return result;
		}

		/// <summary>
		/// 空间直角坐标转大地坐标
		/// </summary>
		/// <param name="coordinate">空间直角坐标</param>
		/// <returns></returns>
		public SpherePoint ThreeDimensionsToGeodetic(PlanePoint coordinate)
		{
			double X = coordinate.x;
			double Y = coordinate.y;
			double Z = coordinate.z;
			double B = 0;
			double L = 0;
			double H = 0;
			this.XYZtoBLH(X, Y, Z, out B, out L, out H);
			SpherePoint result = new SpherePoint(B, L);
			return result;
		}


		#endregion

		#region 私有方法
		/// <summary>
		/// 大地坐标系转空间直角坐标系(经纬度单位为度)
		/// </summary>
		/// <param name="B">纬度</param>
		/// <param name="L">经度</param>
		/// <param name="H">高程</param>
		/// <param name="X">纵坐标</param>
		/// <param name="Y">横坐标</param>
		/// <param name="Z">竖坐标</param>
		private void BLHtoXYZ(double B, double L, double H, out double X, out double Y, out double Z)
		{
			// 转换公式
			// X = (N+H)*cosB*cosL，Y = (N+H)*cosB*sinL，Z = [N*(1-e*e)+H]*sinB
			// N = a/W，W = sqrt(1-e*e*sinB*sinB)

			//度转弧度
			B = (B * Math.PI) / 180;
			L = (L * Math.PI) / 180;
			double N = a / Math.Sqrt(1 - Math.Pow(e, 2) * Math.Sin(B) * Math.Sin(B));
			X = 0; Y = 0; Z = 0;
			X = (N + H) * Math.Cos(B) * Math.Cos(L);
			Y = (N + H) * Math.Cos(B) * Math.Sin(L);
			Z = (N * (1 - Math.Pow(e, 2)) + H) * Math.Sin(B);
		}

		/// <summary>
		/// 空间直角坐标系转大地坐标系（经纬度单位为度）
		/// </summary>
		/// <param name="X">纵坐标</param>
		/// <param name="Y">横坐标</param>
		/// <param name="Z">竖坐标</param>
		/// <param name="B">纬度</param>
		/// <param name="L">经度</param>
		/// <param name="H">高程</param>
		private void XYZtoBLH(double X, double Y, double Z, out double B, out double L, out double H)
		{
			B = 0; L = 0; H = 0;
			L = Math.Atan(Y / X);
			// 迭代法求B
			double initB = Math.Atan(Z / Math.Sqrt(X * X + Y * Y));
			double initN = a / Math.Sqrt(1 - Math.Pow(e, 2) * Math.Sin(B) * Math.Sin(B));
			iterationB(initB, initN, X, Y, Z, ref B);
			// 根据B求H
			double N = a / Math.Sqrt(1 - Math.Pow(e, 2) * Math.Sin(B) * Math.Sin(B));
			H = Math.Sqrt(X * X + Y * Y) * Math.Cos(B) + Z * Math.Sin(B) - N * (1 - Math.Pow(e, 2) * Math.Sin(B) * Math.Sin(B));
			// 弧度转化为度
			B = B * 180 / Math.PI;
			// 如果中央经线>=90, 则B= 180-B;反之，B=B
			if(centerMeridian >= 90)
			{
				B = 180 - B;
			}

			L = L * 180 / Math.PI;
			if (L < 0)
				L += 180;
		}

		private void iterationB(double B, double N, double X, double Y, double Z, ref double finalB)
		{
			double tempB = Math.Atan((Z + N * Math.Pow(e, 2) * Math.Sin(B)) / Math.Sqrt(X * X + Y * Y));
			finalB = tempB;
			if (Math.Abs(tempB - B) < 0.000000000000001)
			{
				return;
			}
			
			double tempN = a / Math.Sqrt(1 - Math.Pow(e, 2) * Math.Sin(tempB) * Math.Sin(tempB));
			iterationB(tempB, tempN, X, Y, Z, ref finalB);
		}

		#endregion
	}
}
