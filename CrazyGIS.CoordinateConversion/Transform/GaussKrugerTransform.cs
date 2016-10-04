using System;
using CrazyGIS.CoordinateConversion.Models;

namespace CrazyGIS.CoordinateConversion.Transform
{
	/// <summary>
	/// 高斯-克吕格 转换
	/// </summary>
	public class GaussKrugerTransform
	{
		private double a = 0;  // 长半轴
		private double b = 0;  // 短半轴
		private double e = 0;  // 第一偏心率
		private double ep = 0; // 第二偏心率
		private double e2 = 0; // 第一偏心率平方
		private double ep2 = 0; // 第二偏心率平方

		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="sourceCoordinateSystem">源坐标系统</param>
		public GaussKrugerTransform(CoordinateSystemType sourceCoordinateSystem)
		{
			EllipsoidalParameters parameters = EllipsoidalType.GetEllipsoidalParametersByTpye(sourceCoordinateSystem);
			a = parameters.SemiMajorAxis;
			b = parameters.SemiMinorAxis;
			e = Math.Sqrt(a * a - b * b) / a;
			ep = Math.Sqrt(a * a - b * b) / b;
			e2 = Math.Pow(e, 2);
			ep2 = Math.Pow(ep, 2);
		}

		#region 接口

		/// <summary>
		/// 高斯-克吕格 投影正算(球面坐标转换为平面坐标)
		/// </summary>
		/// <param name="coordinate">球面坐标</param>
		/// <param name="centerMeridian">中央经线</param>
		/// <returns>平面坐标</returns>
		public PlanePoint GaussKrugerForward(SpherePoint coordinate, double centerMeridian)
		{
			double lng = coordinate.lng;
			double lat = coordinate.lat;
			double x = 0;
			double y = 0;
			this.BLtoxy(lng, lat, centerMeridian, out x, out y);

			PlanePoint result = new PlanePoint(x, y);
			return result;
		}

		/// <summary>
		/// 高斯-克吕格 投影反算(平面坐标转换为球面坐标)
		/// </summary>
		/// <param name="coordinate">平面坐标</param>
		/// <param name="centerMeridian">中央经线</param>
		/// <returns>球面坐标</returns>
		public SpherePoint GaussKrugerReverse(PlanePoint coordinate, double centerMeridian)
		{
			double x = coordinate.x;
			double y = coordinate.y;
			double lng = 0;
			double lat = 0;
			this.xytoBL(x, y, centerMeridian, out lng, out lat);
			SpherePoint result = new SpherePoint(lng, lat);
			return result;
		}

		/// <summary>
		/// 高斯坐标转平面坐标(y+500Km;x,y交换)
		/// </summary>
		/// <param name="gaussianPoint"></param>
		/// <returns></returns>
		public PlanePoint GaussToPlane(PlanePoint gaussianPoint)
		{
			PlanePoint result = new PlanePoint();
			result.x = gaussianPoint.y + 500000;
			result.y = gaussianPoint.x;
			result.z = gaussianPoint.z;

			return result;
		}

		/// <summary>
		/// 平面坐标转高斯坐标(x,y交换;y-500Km)
		/// </summary>
		/// <param name="planePoint"></param>
		/// <returns></returns>
		public PlanePoint PlaneToGauss(PlanePoint planePoint)
		{
			PlanePoint result = new PlanePoint();
			result.x = planePoint.y;
			result.y = planePoint.x - 500000;
			result.z = planePoint.z;

			return result;
		}

		#endregion

		#region 高斯正算

		private void BLtoxy(double srjd, double srwd, double zhyjd, out double scx, out double scy)
		{
			double jd, wd = 0;//经度、纬度和中央子午线经度
			double bt0, bt2, bt4, bt6, bt8, temc, rou, zwxcd, myqbj, xl, t, ytf, temx1, temx2, temx3, temy1, temy2, temy3;
			wd = srwd * Math.PI / 180; jd = srjd * Math.PI / 180;
			rou = 180 / Math.PI * 3600;
			// 中央经度转弧度
			zhyjd = zhyjd * Math.PI / 180;

			bt0 = 1 - 3 * ep2 / 4 + 45 * (ep2 * ep2) / 64 - 175 * (ep2 * ep2 * ep2) / 256 + 11025 * (ep2 * ep2 * ep2 * ep2) / 16384;
			bt2 = bt0 - 1;
			bt4 = 15 * (ep2 * ep2) / 32 - 175 * (ep2 * ep2 * ep2) / 384 + 3675 * (ep2 * ep2 * ep2 * ep2) / 8192;
			bt6 = -35 * (ep2 * ep2 * ep2) / 96 + 735 * (ep2 * ep2 * ep2 * ep2) / 2048;
			bt8 = 315 * (ep2 * ep2 * ep2 * ep2) / 1024;
			temc = a / Math.Sqrt(1 - e2);

			zwxcd = temc * (bt0 * wd + (bt2 * Math.Cos(wd) + bt4 * Math.Cos(wd) * Math.Cos(wd) * Math.Cos(wd) + bt6 * Math.Cos(wd) * Math.Cos(wd) * Math.Cos(wd) * Math.Cos(wd) * Math.Cos(wd) + bt8 * Math.Cos(wd) * Math.Cos(wd) * Math.Cos(wd) * Math.Cos(wd) * Math.Cos(wd) * Math.Cos(wd) * Math.Cos(wd)) * Math.Sin(wd));
			myqbj = a / Math.Sqrt(1 - e2 * Math.Sin(wd) * Math.Sin(wd));
			xl = (jd - zhyjd) * rou;
			t = Math.Tan(wd); ytf = ep2 * Math.Cos(wd) * Math.Cos(wd);
			temx1 = xl * xl * myqbj * Math.Sin(wd) * Math.Cos(wd) / (2 * rou * rou);
			temx2 = xl * xl * xl * xl * myqbj * Math.Sin(wd) * Math.Cos(wd) * Math.Cos(wd) * Math.Cos(wd) * (5 - t * t + 9 * ytf + 4 * ytf * ytf) / (24 * rou * rou * rou * rou);
			temx3 = xl * xl * xl * xl * xl * xl * myqbj * Math.Sin(wd) * Math.Cos(wd) * Math.Cos(wd) * Math.Cos(wd) * Math.Cos(wd) * Math.Cos(wd) * (61 - 58 * t * t + t * t * t * t) / (720 * rou * rou * rou * rou * rou * rou);
			scx = zwxcd + temx1 + temx2 + temx3;
			temy1 = xl * myqbj * Math.Cos(wd) / rou;
			temy2 = xl * xl * xl * myqbj * Math.Cos(wd) * Math.Cos(wd) * Math.Cos(wd) * (1 - t * t + ytf) / (6 * rou * rou * rou);
			temy3 = xl * xl * xl * xl * xl * myqbj * Math.Cos(wd) * Math.Cos(wd) * Math.Cos(wd) * Math.Cos(wd) * Math.Cos(wd) * (5 - 18 * t * t + t * t * t * t + 14 * ytf - 58 * ytf * t * t) / (120 * rou * rou * rou * rou * rou);
			scy = temy1 + temy2 + temy3;
		}

		#endregion

		#region 高斯反算

		private void xytoBL(double srx, double sry, double zhyjd, out double scjd, out double scwd)
		{
			double Bf, Mf, Nf;//底点纬度,底点纬度对应的子午线的曲率半径,及其对应的卯酉圈半径
			double wf, tf, ytf, jdc, temx1, temx2, temx3, temy1, temy2, temy3;

			//rou=180/Math.PI*3600;
			Bf = ddwd(a, e2, srx);
			wf = Math.Sqrt(1 - e2 * Math.Sin(Bf) * Math.Sin(Bf));

			Mf = (a * (1 - e2)) / (wf * wf * wf);
			Nf = a / wf;
			tf = Math.Tan(Bf);
			ytf = ep2 * Math.Cos(Bf) * Math.Cos(Bf);

			temx1 = sry * sry * tf / (2 * Mf * Nf);
			temx2 = (sry * sry * sry * sry * tf * (5 + 3 * tf * tf + ytf - 9 * tf * tf * ytf)) / (24 * Mf * Nf * Nf * Nf);
			temx3 = (sry * sry * sry * sry * sry * sry * tf * (61 + 90 * tf * tf + 45 * tf * tf * tf * tf)) / (720 * Mf * Nf * Nf * Nf * Nf * Nf);
			scwd = (Bf - temx1 + temx2 - temx3) * 180 / Math.PI; // 反算的纬度，单位为度

			temy1 = sry / (Nf * Math.Cos(Bf));
			temy2 = (sry * sry * sry * (1 + 2 * tf * tf + ytf)) / (6 * Nf * Nf * Nf * Math.Cos(Bf));
			temy3 = (sry * sry * sry * sry * sry * (5 + 28 * tf * tf + 24 * tf * tf * tf * tf + 6 * ytf + 8 * ytf * tf * tf)) / (120 * Nf * Nf * Nf * Nf * Nf * Math.Cos(Bf));
			jdc = temy1 - temy2 + temy3;
			scjd = zhyjd + jdc * 180 / Math.PI; // 反算的经度，单位为度
		}

		//高斯反算过程中的求底点纬度的子函数，输入椭球长半轴a、第一偏心率的平方e2和x坐标srx（直接解求法）
		private double ddwd(double a, double e2, double srx)
		{
			double e4, e6, e8, a0, b0, b1, k0, k2, k4, k6;//临时变量
			double fhz = 0;//解算的底点纬度(以弧段为单位)

			e4 = e2 * e2; e6 = e2 * e4; e8 = e2 * e6;
			a0 = 1 + 3 * e2 / 4 + 45 * e4 / 64 + 350 * e6 / 512 + 11025 * e8 / 16384;
			b0 = srx / (a * (1 - e2) * a0);
			b1 = Math.Sin(b0) * Math.Sin(b0);
			k0 = (a0 - 1) / 2;
			k2 = -(63 * e4 / 64 + 1108 * e6 / 512 + 58239 * e8 / 16384) / 3;
			k4 = (604 * e6 / 512 + 68484 * e8 / 16384) / 3;
			k6 = -26328 * e8 / (16384 * 3);
			fhz = b0 + (Math.Sin(2 * b0)) * (k0 + b1 * (k2 + b1 * (k4 + k6 * b1)));
			return fhz;

		}
		#endregion

	}
}
