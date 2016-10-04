using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyGIS.Graphical.Models
{
	/// <summary>
	/// 球面坐标(以经纬度表示的坐标)
	/// </summary>
	public class SpherePoint: ICoordinate
	{
		public SpherePoint()
		{
			this.unit = CoordinateUnit.Degree;
		}

		public SpherePoint(double lng, double lat)
		{
			this.lng = lng;
			this.lat = lat;
			this.unit = CoordinateUnit.Degree;
		}

		public double lng
		{
			get
			{
				return xAxis;
			}

			set
			{
				xAxis = value;
			}
		}

		public double lat
		{
			get
			{
				return yAxis;
			}

			set
			{
				yAxis = value;
			}
		}

		public double xAxis { get; set; }

		public double yAxis { get; set; }

		public CoordinateUnit unit { get; set; }
	}
}
