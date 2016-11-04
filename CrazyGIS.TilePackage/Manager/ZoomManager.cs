using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrazyGIS.TilePackage.Manager;
using CrazyGIS.TilePackage.Models;

namespace CrazyGIS.TilePackage.Manager
{
	public class ZoomManager
	{
		/// <summary>
		/// 检查缩放等级,判断是否在可用范围内
		/// </summary>
		/// <param name="zoomLevel">缩放等级</param>
		/// <returns>true or false</returns>
		public static bool CheckZoomLevel(int zoomLevel)
		{
			if (zoomLevel >= Constant.MIN_ZOOM_LEVEL && zoomLevel <= Constant.MAX_ZOOM_LEVEL)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// 获取缩放等级信息
		/// </summary>
		/// <param name="projInfo">投影类型</param>
		/// <param name="zoomLevel">缩放等级</param>
		/// <returns></returns>
		public static Zoom GetZoomInfo(ProjectionType projType, int zoomLevel)
		{
			if(!CheckZoomLevel(zoomLevel))
			{
				return null;
			}

			Zoom result = null;
			switch (projType)
			{
				case ProjectionType.EPSG_3857:
					result = getZoomInfo_3857(zoomLevel);
					break;
				case ProjectionType.EPSG_4326:
					result = getZoomInfo_4326(zoomLevel);
					break;
				case ProjectionType.EPSG_4490:
					result = getZoomInfo_4490(zoomLevel);
					break;
				default:
					result = null;
					break;
			}

			return result;
		}

		private static Zoom getZoomInfo_3857(int zoomLevel)
		{
			Zoom zoom = new Zoom();
			zoom.ProjType = ProjectionType.EPSG_3857;
			zoom.ZoomLevel = zoomLevel;
			zoom.ZoomResolution = ResolutionManager.GetResolution(ProjectionType.EPSG_3857, zoomLevel);
			zoom.ZoomScale = ScaleManager.GetScale(ProjectionType.EPSG_3857, zoomLevel);
			if(zoomLevel == 0)
			{
				zoom.RowNumber = 1;
				zoom.ColumnNumber = 1;
			}
			else if(zoomLevel == 1)
			{
				zoom.RowNumber = 1;
				zoom.ColumnNumber = 2;
			}
			else
			{
				zoom.RowNumber = Convert.ToInt32(1 * Math.Pow(2, (zoomLevel - 1)));
				zoom.ColumnNumber = Convert.ToInt32(2 * Math.Pow(2, (zoomLevel - 1)));
			}

			return zoom;
		}

		private static Zoom getZoomInfo_4326(int zoomLevel)
		{
			Zoom zoom = new Zoom();
			zoom.ProjType = ProjectionType.EPSG_4326;
			zoom.ZoomLevel = zoomLevel;
			zoom.ZoomResolution = ResolutionManager.GetResolution(ProjectionType.EPSG_4326, zoomLevel);
			zoom.ZoomScale = ScaleManager.GetScale(ProjectionType.EPSG_4326, zoomLevel);
			zoom.RowNumber = Convert.ToInt32(Math.Pow(2, zoomLevel));
			zoom.ColumnNumber = Convert.ToInt32(Math.Pow(2, zoomLevel));

			return zoom;
		}

		private static Zoom getZoomInfo_4490(int zoomLevel)
		{
			Zoom zoom = new Zoom();
			zoom.ProjType = ProjectionType.EPSG_4490;
			zoom.ZoomLevel = zoomLevel;
			zoom.ZoomResolution = ResolutionManager.GetResolution(ProjectionType.EPSG_4490, zoomLevel);
			zoom.ZoomScale = ScaleManager.GetScale(ProjectionType.EPSG_4490, zoomLevel);
			zoom.RowNumber = Convert.ToInt32(Math.Pow(2, zoomLevel));
			zoom.ColumnNumber = Convert.ToInt32(Math.Pow(2, zoomLevel));

			return zoom;
		}
	}
}
