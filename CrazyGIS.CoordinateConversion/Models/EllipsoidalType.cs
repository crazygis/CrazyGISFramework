
namespace CrazyGIS.CoordinateConversion.Models
{
	public class EllipsoidalType
	{
		public static EllipsoidalParameters GetEllipsoidalParametersByTpye(CoordinateSystemType type)
		{
			EllipsoidalParameters parameters = null;
			switch(type)
			{
				case CoordinateSystemType.WGS84:
					parameters = new EllipsoidalParameters();
					parameters.SemiMajorAxis = 6378137;
					parameters.SemiMinorAxis = 6356752.3142;
					break;
				case CoordinateSystemType.BJ54:
					parameters = new EllipsoidalParameters();
					parameters.SemiMajorAxis = 6378245;
					parameters.SemiMinorAxis = 6356863.019;
					break;
				case CoordinateSystemType.XIAN80:
					parameters = new EllipsoidalParameters();
					parameters.SemiMajorAxis = 6378140;
					parameters.SemiMinorAxis = 6356755.2882;
					break;
				case CoordinateSystemType.CGCS2000:
					parameters = new EllipsoidalParameters();
					parameters.SemiMajorAxis = 6378137;
					parameters.SemiMinorAxis = 6356752.31414;
					break;
				default:
					break;
			}

			return parameters;
		}
	}
}
