using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyGIS.Graphical.Models
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

		CoordinateUnit unit { get; set; }
	}
}
