using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyGIS.Graphical.Models
{
	/// <summary>
	/// 平面坐标(以XY表示的坐标)
	/// </summary>
    public class PlanePoint: ICoordinate
    {
		public PlanePoint()
		{
			this.unit = CoordinateUnit.Meter;
		}

		public PlanePoint(double x, double y)
		{
			this.x = x;
			this.y = y;
			this.unit = CoordinateUnit.Meter;
		}

		public double x
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

		public double y
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
