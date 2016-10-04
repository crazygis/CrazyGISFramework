using System;

namespace CrazyGIS.CoordinateConversion.Transform
{
	/// <summary>
	/// 角度转换
	/// </summary>
	public class AngleTransform
	{
		/// <summary>
		/// 度转弧度
		/// </summary>
		/// <param name="degree"></param>
		/// <returns></returns>
		public static double DegreeToRadian(double degree)
		{
			return degree * Math.PI / 180;
		}

		/// <summary>
		/// 弧度转度
		/// </summary>
		/// <param name="radian"></param>
		/// <returns></returns>
		public static double RadianToDegree(double radian)
		{
			return radian * 180 / Math.PI;
		}
	}
}
