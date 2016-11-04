using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrazyGIS.TilePackage.Models;

namespace CrazyGIS.TilePackage.Manager
{
	public class ResolutionManager
	{
		private static double MAX_RESOLUTION_4326 = 1.40625;
		private static double MAX_RESOLUTION_4490 = 1.40625;
		private static double MAX_RESOLUTION_3857 = 156543.033928;

		/// <summary>
		/// 获取分辨率
		/// </summary>
		/// <param name="projType">投影类型</param>
		/// <param name="zoomLevel">缩放等级</param>
		/// <returns></returns>
		public static double GetResolution(ProjectionType projType, int zoomLevel)
		{
			if(!ZoomManager.CheckZoomLevel(zoomLevel))
			{
				return 0;
			}

			double maxResolution = 0;

			switch (projType)
			{
				case ProjectionType.EPSG_3857:
					maxResolution = MAX_RESOLUTION_3857;
					break;
				case ProjectionType.EPSG_4326:
					maxResolution = MAX_RESOLUTION_4326;
					break;
				case ProjectionType.EPSG_4490:
					maxResolution = MAX_RESOLUTION_4490;
					break;
				default:
					break;
			}

			return maxResolution / Math.Pow(2, zoomLevel);
		}
	}
}
