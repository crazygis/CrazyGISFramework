
namespace CrazyGIS.CoordinateConversion.Models
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

		public PlanePoint(double x, double y, double z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
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

		public double z
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
			return new PlanePoint(this.x, this.y, this.z);
		}
	}
}
