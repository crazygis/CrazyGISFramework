
namespace CrazyGIS.CoordinateConversion.Models
{
	/// <summary>
	/// 坐标系统椭球参数
	/// </summary>
	public class EllipsoidalParameters
	{
		/// <summary>
		/// 长半轴
		/// </summary>
		public double SemiMajorAxis { get; set; }

		/// <summary>
		/// 短半轴
		/// </summary>
		public double SemiMinorAxis { get; set; }
	}
}
