using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrazyGIS.Graphical.Models;


namespace CrazyGIS.Graphical
{
	/// <summary>
	/// 坐标转换类
	/// </summary>
	public class CoordinateTransform
    {
		// 经度：lng,纬度：lat
		// WGS-84：是国际标准，GPS坐标（Google Earth使用、或者GPS模块）
		// GCJ-02：中国坐标偏移标准，Google Map、高德、腾讯使用
		// BD-09：百度坐标偏移标准，Baidu Map使用
		// lnglat: 以经纬度表示坐标的坐标系统，包括以上三种
		// mercator: 墨卡托投影坐标，如webMeractor

		private double x_pi;

		public CoordinateTransform()
		{
			x_pi = Math.PI * 3000.0 / 180.0;
		}


		#region 公共方法

		/// <summary>
		/// 计算两个坐标之间的距离
		/// </summary>
		/// <param name="start">起点</param>
		/// <param name="end">终点</param>
		/// <returns>距离(米)</returns>
		public double Distance(ICoordinate start, ICoordinate end)
		{
			return this.distance(start, end);
		}

		/// <summary>
		/// 计算面积
		/// </summary>
		/// <param name="coordinates">顶点集合(顺序或倒序排列)</param>
		/// <returns>面积(平方米)</returns>
		public double Area(List<ICoordinate> coordinates)
		{
			return this.area(coordinates);
		}

		/// <summary>
		/// WGS84坐标转火星坐标
		/// </summary>
		/// <param name="wgs84Point"></param>
		/// <returns></returns>
		public SpherePoint WGS84_To_GCJ02(SpherePoint wgs84Point)
		{
			double wgs84Lng = wgs84Point.lng;
			double wgs84Lat = wgs84Point.lat;
			double gcj02Lng = 0;
			double gcj02Lat = 0;

			this.wgs84_to_gcj02(wgs84Lng, wgs84Lat, out gcj02Lng, out gcj02Lat);

			SpherePoint gcj02Point = new SpherePoint(gcj02Lng, gcj02Lat);
			return gcj02Point;
		}

		/// <summary>
		/// 火星坐标转WGS84坐标
		/// </summary>
		/// <param name="gcj02Point"></param>
		/// <returns></returns>
		public SpherePoint GCJ02_To_WGS84(SpherePoint gcj02Point)
		{
			double gcj02Lng = gcj02Point.lng;
			double gcj02Lat = gcj02Point.lat;
			double wgs84Lng = 0;
			double wgs84Lat = 0;

			this.gcj02_to_wgs84(gcj02Lng, gcj02Lat, out wgs84Lng, out wgs84Lat);

			SpherePoint wgs84Point = new SpherePoint(wgs84Lng, wgs84Lat);
			return wgs84Point;
		}

		/// <summary>
		/// 火星坐标转WGS84坐标(精确)
		/// </summary>
		/// <param name="gcj02Point"></param>
		/// <returns></returns>
		public SpherePoint GCJ02_To_WGS84_EXACT(SpherePoint gcj02Point)
		{
			double gcj02Lng = gcj02Point.lng;
			double gcj02Lat = gcj02Point.lat;
			double wgs84Lng = 0;
			double wgs84Lat = 0;

			this.gcj02_to_wgs84_exact(gcj02Lng, gcj02Lat, out wgs84Lng, out wgs84Lat);

			SpherePoint wgs84Point = new SpherePoint(wgs84Lng, wgs84Lat);
			return wgs84Point;
		}

		/// <summary>
		/// 火星坐标转百度坐标
		/// </summary>
		/// <param name="gcj02Point"></param>
		/// <returns></returns>
		public SpherePoint GCJ02_To_BD09(SpherePoint gcj02Point)
		{
			double gcj02Lng = gcj02Point.lng;
			double gcj02Lat = gcj02Point.lat;
			double bd09Lng = 0;
			double bd09Lat = 0;

			this.gcj02_to_bd09(gcj02Lng, gcj02Lat, out bd09Lng, out bd09Lat);

			SpherePoint bd09Point = new SpherePoint(bd09Lng, bd09Lat);
			return bd09Point;
		}

		/// <summary>
		/// 百度坐标转火星坐标
		/// </summary>
		/// <param name="bd09Point"></param>
		/// <returns></returns>
		public SpherePoint BD09_To_GCJ02(SpherePoint bd09Point)
		{
			double bd09Lng = bd09Point.lng;
			double bd09Lat = bd09Point.lat;
			double gcj02Lng = 0;
			double gcj02Lat = 0;

			this.bd09_to_gcj02(bd09Lng, bd09Lat, out gcj02Lng, out gcj02Lat);

			SpherePoint gcj02Point = new SpherePoint(gcj02Lng, gcj02Lat);
			return gcj02Point;
		}

		/// <summary>
		/// WGS84坐标转百度坐标
		/// </summary>
		/// <param name="wgs84Point"></param>
		/// <returns></returns>
		public SpherePoint WGS84_To_BD09(SpherePoint wgs84Point)
		{
			double wgs84Lng = wgs84Point.lng;
			double wgs84Lat = wgs84Point.lat;
			double gcj02Lng = 0;
			double gcj02Lat = 0;
			double bd09Lng = 0;
			double bd09Lat = 0;

			this.wgs84_to_gcj02(wgs84Lng, wgs84Lat, out gcj02Lng, out gcj02Lat);
			this.gcj02_to_bd09(gcj02Lng, gcj02Lat, out bd09Lng, out bd09Lat);

			SpherePoint bd09Point = new SpherePoint(bd09Lng, bd09Lat);
			return bd09Point;
		}

		/// <summary>
		/// 百度坐标转WGS84坐标
		/// </summary>
		/// <param name="bd09Point"></param>
		/// <returns></returns>
		public SpherePoint BD09_To_WGS84(SpherePoint bd09Point)
		{
			double bd09Lng = bd09Point.lng;
			double bd09Lat = bd09Point.lat;
			double gcj02Lng = 0;
			double gcj02Lat = 0;
			double wgs84Lng = 0;
			double wgs84Lat = 0;

			this.bd09_to_gcj02(bd09Lng, bd09Lat, out gcj02Lng, out gcj02Lat);
			this.gcj02_to_wgs84(gcj02Lng, gcj02Lat, out wgs84Lng, out wgs84Lat);

			SpherePoint wgs84Point = new SpherePoint(wgs84Lng, wgs84Lat);
			return wgs84Point;
		}

		/// <summary>
		/// 经纬度坐标转Web墨卡托坐标
		/// </summary>
		/// <param name="lnglat"></param>
		/// <returns></returns>
		public PlanePoint LngLat_To_WebMercator(SpherePoint lnglat)
		{
			double lng = lnglat.lng;
			double lat = lnglat.lat;
			double x = 0;
			double y = 0;

			this.lnglat_to_webmercator(lng, lat, out x, out y);

			PlanePoint xy = new PlanePoint(x, y);
			return xy;
		}

		/// <summary>
		/// Web墨卡托坐标转经纬度坐标
		/// </summary>
		/// <param name="xy"></param>
		/// <returns></returns>
		public SpherePoint WebMercator_To_LngLat(PlanePoint xy)
		{
			double x = xy.x;
			double y = xy.y;
			double lng = 0;
			double lat = 0;

			this.webmercator_to_lnglat(x, y, out lng, out lat);

			SpherePoint lnglat = new SpherePoint(lng, lat);
			return lnglat;
		}

		#endregion

		#region 私有方法

		private double degreeToRadian(double degree)
		{
			return degree * Math.PI / 180;
		}

		private double radianToDegree(double radian)
		{
			return radian * 180 / Math.PI;
		}

		private double transformLng(double lng, double lat)
		{
			var x = lng;
			var y = lat;

			var ret = 300.0 + x + 2.0 * y + 0.1 * x * x + 0.1 * x * y + 0.1 * Math.Sqrt(Math.Abs(x));
			ret += (20.0 * Math.Sin(6.0 * x * Math.PI) + 20.0 * Math.Sin(2.0 * x * Math.PI)) * 2.0 / 3.0;
			ret += (20.0 * Math.Sin(x * Math.PI) + 40.0 * Math.Sin(x / 3.0 * Math.PI)) * 2.0 / 3.0;
			ret += (150.0 * Math.Sin(x / 12.0 * Math.PI) + 300.0 * Math.Sin(x / 30.0 * Math.PI)) * 2.0 / 3.0;
			return ret;
		}

		private double transformLat(double lng, double lat)
		{
			var x = lng;
			var y = lat;

			var ret = -100.0 + 2.0 * x + 3.0 * y + 0.2 * y * y + 0.1 * x * y + 0.2 * Math.Sqrt(Math.Abs(x));
			ret += (20.0 * Math.Sin(6.0 * x * Math.PI) + 20.0 * Math.Sin(2.0 * x * Math.PI)) * 2.0 / 3.0;
			ret += (20.0 * Math.Sin(y * Math.PI) + 40.0 * Math.Sin(y / 3.0 * Math.PI)) * 2.0 / 3.0;
			ret += (160.0 * Math.Sin(y / 12.0 * Math.PI) + 320 * Math.Sin(y * Math.PI / 30.0)) * 2.0 / 3.0;
			return ret;
		}

		private void delta(double lng, double lat, out double deltaLng, out double deltaLat)
		{
			// Krasovsky 1940
			//
			// a = 6378245.0, 1/f = 298.3
			// b = a * (1 - f)
			// ee = (a^2 - b^2) / a^2;
			double a = 6378245.0;
			double ee = 0.00669342162296594323;
			deltaLng = this.transformLng(lng - 105.0, lat - 35.0);
			deltaLat = this.transformLat(lng - 105.0, lat - 35.0);
			double radLat = lat / 180.0 * Math.PI;
			double magic = Math.Sin(radLat);
			magic = 1 - ee * magic * magic;
			double sqrtMagic = Math.Sqrt(magic);
			deltaLng = (deltaLng * 180.0) / (a / sqrtMagic * Math.Cos(radLat) * Math.PI);
			deltaLat = (deltaLat * 180.0) / ((a * (1 - ee)) / (magic * sqrtMagic) * Math.PI);
		}

		private bool outOfChina(double lng, double lat)
		{
			if ((lng < 72.004 || lng > 137.8347)
				&& (lat < 0.8293 || lat > 55.8271))
				return true;

			return false;
		}

		/// <summary>
		/// WGS84坐标转火星坐标
		/// </summary>
		/// <param name="wgs84Lng"></param>
		/// <param name="wgs84Lat"></param>
		/// <param name="gcj02Lng"></param>
		/// <param name="gcj02Lat"></param>
		private void wgs84_to_gcj02(double wgs84Lng, double wgs84Lat, out double gcj02Lng, out double gcj02Lat)
		{
			if (this.outOfChina(wgs84Lng, wgs84Lat))
			{
				gcj02Lng = wgs84Lng;
				gcj02Lat = wgs84Lat;
			}
			else
			{
				double deltaLng = 0;
				double deltaLat = 0;
				this.delta(wgs84Lng, wgs84Lat, out deltaLng, out deltaLat);

				gcj02Lng = wgs84Lng + deltaLng;
				gcj02Lat = wgs84Lat + deltaLat;
			}
		}

		/// <summary>
		/// 火星坐标转WGS84坐标
		/// </summary>
		/// <param name="gcj02Lng"></param>
		/// <param name="gcj02Lat"></param>
		/// <param name="wgs84Lng"></param>
		/// <param name="wgs84Lat"></param>
		private void gcj02_to_wgs84(double gcj02Lng, double gcj02Lat, out double wgs84Lng, out double wgs84Lat)
		{
			if (this.outOfChina(gcj02Lng, gcj02Lat))
			{
				wgs84Lng = gcj02Lng;
				wgs84Lat = gcj02Lat;
			}
			else
			{
				double deltaLng = 0;
				double deltaLat = 0;
				this.delta(gcj02Lng, gcj02Lat, out deltaLng, out deltaLat);

				wgs84Lng = gcj02Lng - deltaLng;
				wgs84Lat = gcj02Lat - deltaLat;
			}
		}

		/// <summary>
		/// 火星坐标转WGS84坐标(精确)
		/// </summary>
		/// <param name="gcj02Lng"></param>
		/// <param name="gcj02Lat"></param>
		/// <param name="wgs84Lng"></param>
		/// <param name="wgs84Lat"></param>
		private void gcj02_to_wgs84_exact(double gcj02Lng, double gcj02Lat, out double wgs84Lng, out double wgs84Lat)
		{
			double initDelta = 0.01;
			double threshold = 0.000000001;
			double dLng = initDelta;
			double dLat = initDelta;
			double mLng = gcj02Lng - dLng;
			double mLat = gcj02Lat - dLat;
			double pLng = gcj02Lng + dLng;
			double pLat = gcj02Lat + dLat;

			int i = 0;
			while (true)
			{
				wgs84Lng = (mLng + pLng) / 2;
				wgs84Lat = (mLat + pLat) / 2;
				double tempLng = 0;
				double tempLat = 0;
				this.wgs84_to_gcj02(wgs84Lng, wgs84Lat, out tempLng, out tempLat);
				dLng = tempLng - gcj02Lng;
				dLat = tempLat - gcj02Lat;
				if ((Math.Abs(dLng) < threshold) && (Math.Abs(dLat) < threshold))
					break;

				if (dLng > 0)
					pLng = wgs84Lng;
				else
					mLng = wgs84Lng;
				if (dLat > 0)
					pLat = wgs84Lat;
				else
					mLat = wgs84Lat;

				if (++i > 10000) break;
			}
		}

		/// <summary>
		/// 火星坐标转百度坐标
		/// </summary>
		/// <param name="gcj02Lng"></param>
		/// <param name="gcj02Lat"></param>
		/// <param name="bd09Lng"></param>
		/// <param name="bd09Lat"></param>
		private void gcj02_to_bd09(double gcj02Lng, double gcj02Lat, out double bd09Lng, out double bd09Lat)
		{
			double x = gcj02Lng, y = gcj02Lat;
			var z = Math.Sqrt(x * x + y * y) + 0.00002 * Math.Sin(y * this.x_pi);
			var theta = Math.Atan2(y, x) + 0.000003 * Math.Cos(x * this.x_pi);
			bd09Lng = z * Math.Cos(theta) + 0.0065;
			bd09Lat = z * Math.Sin(theta) + 0.006;
		}

		/// <summary>
		/// 百度坐标转火星坐标
		/// </summary>
		/// <param name="bd09Lng"></param>
		/// <param name="bd09Lat"></param>
		/// <param name="gcj02Lng"></param>
		/// <param name="gcj02Lat"></param>
		private void bd09_to_gcj02(double bd09Lng, double bd09Lat, out double gcj02Lng, out double gcj02Lat)
		{
			double x = bd09Lng - 0.0065, y = bd09Lat - 0.006;
			var z = Math.Sqrt(x * x + y * y) - 0.00002 * Math.Sin(y * this.x_pi);
			var theta = Math.Atan2(y, x) - 0.000003 * Math.Cos(x * this.x_pi);
			gcj02Lng = z * Math.Cos(theta);
			gcj02Lat = z * Math.Sin(theta);
		}

		/// <summary>
		/// 经纬度坐标转web墨卡托坐标
		/// </summary>
		/// <param name="lng"></param>
		/// <param name="lat"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		private void lnglat_to_webmercator(double lng, double lat, out double x, out double y)
		{
			x = lng * 20037508.34 / 180;
			y = Math.Log(Math.Tan((90 + lat) * Math.PI / 360)) / (Math.PI / 180);
			y = y * 20037508.34 / 180;
		}

		/// <summary>
		/// web墨卡托坐标转经纬度坐标
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="lng"></param>
		/// <param name="lat"></param>
		private void webmercator_to_lnglat(double x, double y, out double lng, out double lat)
		{
			lng = x / 20037508.34 * 180;
			lat = y / 20037508.34 * 180;
			lat = 180 / Math.PI
			* (2 * Math.Atan(Math.Exp(lat * Math.PI / 180)) - Math.PI / 2);
		}

		/// <summary>
		/// 计算距离
		/// </summary>
		/// <param name="startLng"></param>
		/// <param name="startLat"></param>
		/// <param name="endLng"></param>
		/// <param name="endLat"></param>
		/// <returns>距离值(米)(</returns>
		private double distance(double startLng, double startLat, double endLng, double endLat)
		{
			double earthR = 6371000;
			double x = Math.Cos(startLat * Math.PI / 180) * Math.Cos(endLat * Math.PI / 180) * Math.Cos((startLng - endLng) * Math.PI / 180);
			double y = Math.Sin(startLat * Math.PI / 180) * Math.Sin(endLat * Math.PI / 180);
			double s = x + y;
			if (s > 1)
				s = 1;
			if (s < -1)
				s = -1;
			double alpha = Math.Acos(s);
			double distance = alpha * earthR;
			return distance;
		}

		/// <summary>
		/// 计算多个坐标点之间的总距离
		/// </summary>
		/// <param name="coordinates">坐标点集合(正序或倒序排列)</param>
		/// <returns></returns>
		private double distance(List<ICoordinate> coordinates)
		{
			if(coordinates == null || coordinates.Count < 2)
			{
				return 0;
			}

			double length = 0;
			int count = coordinates.Count;
			ICoordinate start = coordinates[0];
			for(int i = 1; i < count; i++)
			{
				ICoordinate end = coordinates[i];
				length += this.distance(start, end);
				start = end;
			}

			return length;
		}

		/// <summary>
		/// 计算两个坐标点之间的距离
		/// </summary>
		/// <param name="start">起点</param>
		/// <param name="end">终点</param>
		/// <returns></returns>
		private double distance(ICoordinate start, ICoordinate end)
		{
			if (start == null || end == null)
			{
				return 0;
			}

			if(start.unit == CoordinateUnit.Degree)
			{
				double radius = 6378137;
				double lat1 = this.degreeToRadian(start.yAxis);
				double lat2 = this.degreeToRadian(end.yAxis);
				double deltaLatBy2 = (lat2 - lat1) / 2;
				double deltaLonBy2 = this.degreeToRadian(end.xAxis - start.xAxis) / 2;
				double a = Math.Sin(deltaLatBy2) * Math.Sin(deltaLatBy2) +
					Math.Sin(deltaLonBy2) * Math.Sin(deltaLonBy2) *
					Math.Cos(lat1) * Math.Cos(lat2);
				return 2 * radius * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
			}
			else
			{
				double deltaX = start.xAxis - end.xAxis;
				double deltaY = start.yAxis - end.yAxis;
				return Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
			}
		}

		/// <summary>
		/// 计算多边形面积
		/// </summary>
		/// <param name="coordinates">多边形顶点集合(正序或倒序排列)</param>
		/// <returns></returns>
		private double area(List<ICoordinate> coordinates)
		{
			// 多边形的顶点必须大于等于3
			if (coordinates == null || coordinates.Count < 3)
			{
				return 0;
			}

			if(coordinates[0].unit == CoordinateUnit.Degree)
			{
				double radius = 6378137;
				double area = 0;
				int len = coordinates.Count;
				double x1 = coordinates[len - 1].xAxis;
				double y1 = coordinates[len - 1].yAxis;
				for (int i = 0; i < len; i++)
				{
					double x2 = coordinates[i].xAxis, y2 = coordinates[i].yAxis;
					area += this.degreeToRadian(x2 - x1) *
						(2 + Math.Sin(this.degreeToRadian(y1)) +
						Math.Sin(this.degreeToRadian(y2)));
					x1 = x2;
					y1 = y2;
				}
				return area * radius * radius / 2.0;
			}
			else
			{
				double area = 0;

				// 循环多边形的顶点
				for (int i = 0; i < coordinates.Count; i++)
				{
					ICoordinate p1 = coordinates[i];
					ICoordinate p2 = coordinates[i + 1];

					double subArea = p1.xAxis * p2.yAxis - p2.xAxis * p1.yAxis;
					area += subArea;
				}

				area /= 2;

				return area;
			}
		}
		#endregion
	}
}
