
namespace CrazyGIS.CoordinateConversion.Models
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

		public SpherePoint(double lng, double lat, double hgt)
		{
			this.lng = lng;
			this.lat = lat;
			this.hgt = hgt;
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

		public double hgt
		{
			get
			{
				return zAxis;
			}

			set
			{
				zAxis = value;
			}
		}

		public double xAxis { get; set; }

		public double yAxis { get; set; }

		public double zAxis { get; set; }

		public CoordinateUnit unit { get; set; }

		public ICoordinate Clone()
		{
			return new SpherePoint(this.lng, this.lat, this.hgt);
		}
	}
}
