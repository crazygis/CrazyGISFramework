
namespace CrazyGIS.CoordinateConversion.Models
{
	/// <summary>
	/// 坐标转换参数
	/// </summary>
	public class ConversionParameters
	{
		/// <summary>
		/// 转换七参数
		/// </summary>
		public SevenParameters ConversionSevenParams { get; set; }

		/// <summary>
		/// 源坐标系统
		/// </summary>
		public CoordinateSystemType SourceCoordinateSystem { get; set; } = CoordinateSystemType.WGS84;

		/// <summary>
		/// 目标坐标系统
		/// </summary>
		public CoordinateSystemType TargetCoordinateSystem { get; set; } = CoordinateSystemType.WGS84;

		/// <summary>
		/// 源坐标系统中央经线
		/// </summary>
		public double SourceCenterMeridian { get; set; } = 0;
		/// <summary>
		/// 目标坐标系统中央经线
		/// </summary>
		public double TargetCenterMeridian { get; set; } = 0;

		/// <summary>
		/// 源坐标类型
		/// </summary>
		public CoordinateType SourceCoordinateType { get; set; } = CoordinateType.Sphere;

		/// <summary>
		/// 目标坐标类型
		/// </summary>
		public CoordinateType TargetCoordinateType { get; set; } = CoordinateType.Plane;
	}
}
