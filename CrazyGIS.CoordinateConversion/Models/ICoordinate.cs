
namespace CrazyGIS.CoordinateConversion.Models
{
	/// <summary>
	/// 坐标接口
	/// </summary>
	public interface ICoordinate
	{
		/// <summary>
		/// 横坐标
		/// </summary>
		double xAxis { get; set; }
		/// <summary>
		/// 纵坐标
		/// </summary>
		double yAxis { get; set; }

		/// <summary>
		/// 竖坐标
		/// </summary>
		double zAxis { get; set; }

		CoordinateUnit unit { get; set; }

		ICoordinate Clone();
	}
}
