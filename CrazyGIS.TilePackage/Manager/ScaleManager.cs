using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrazyGIS.TilePackage.Models;

namespace CrazyGIS.TilePackage.Manager
{
	public class ScaleManager
	{
		/// <summary>
		/// 获取比例尺
		/// </summary>
		/// <param name="projType">投影类型</param>
		/// <param name="zoomLevel">缩放等级</param>
		public static double GetScale(ProjectionType projType, int zoomLevel)
		{
			if(!ZoomManager.CheckZoomLevel(zoomLevel))
			{
				return 0;
			}

			double minScale = 0;
			switch (projType)
			{
				case ProjectionType.EPSG_3857:
					minScale = Constant.MIN_SCALE_3857;
					break;
				case ProjectionType.EPSG_4326:
					minScale = Constant.MIN_SCALE_4326;
					break;
				case ProjectionType.EPSG_4490:
					minScale = Constant.MIN_SCALE_4490;
					break;
				default:
					break;
			}

			return minScale / Math.Pow(2, zoomLevel);
		}
	}
}
