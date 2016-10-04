
namespace CrazyGIS.CoordinateConversion.Models
{
	/// <summary>
	/// 七参数
	/// </summary>
	public class SevenParameters
	{
		/// <summary>
		/// 参数反转
		/// </summary>
		/// <returns></returns>
		public SevenParameters Reverse()
		{
			SevenParameters result = new SevenParameters();
			result.XaxisRotateRadian = -this.XaxisRotateRadian;
			result.YaxisRotateRadian = -this.YaxisRotateRadian;
			result.ZaxisRotateRadian = -this.ZaxisRotateRadian;
			result.XaxisDeviation = -this.XaxisDeviation;
			result.YaxisDeviation = -this.YaxisDeviation;
			result.ZaxisDeviation = -this.ZaxisDeviation;
			result.ScaleParameter = -this.ScaleParameter;

			return result;
		}

		/// <summary>
		/// 坐标系统X轴旋转角度(弧度值)
		/// </summary>
		public double XaxisRotateRadian { get; set; } = 0;

		/// <summary>
		/// 坐标系统Y轴旋转角度(弧度值)
		/// </summary>
		public double YaxisRotateRadian { get; set; } = 0;

		/// <summary>
		/// 坐标系统Z轴旋转角度(弧度值)
		/// </summary>
		public double ZaxisRotateRadian { get; set; } = 0;

		/// <summary>
		/// 坐标系统X轴偏移(米)
		/// </summary>
		public double XaxisDeviation { get; set; } = 0;

		/// <summary>
		/// 坐标系统Y轴偏移(米)
		/// </summary>
		public double YaxisDeviation { get; set; } = 0;

		/// <summary>
		/// 坐标系统Z轴偏移(米)
		/// </summary>
		public double ZaxisDeviation { get; set; } = 0;

		/// <summary>
		/// 尺度参数
		/// </summary>
		public double ScaleParameter { get; set; } = 0;
	}
}
