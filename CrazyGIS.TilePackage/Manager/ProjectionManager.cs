using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrazyGIS.TilePackage.Models;

namespace CrazyGIS.TilePackage.Manager
{
	public class ProjectionManager
	{
		/// <summary>
		/// 获取瓦片的投影信息
		/// </summary>
		/// <param name="projType">投影类型</param>
		/// <returns></returns>
		public static TileProjection GetTileProjection(ProjectionType projType)
		{
			return null;
		}

		/// <summary>
		/// 获取瓦片的投影信息
		/// </summary>
		/// <param name="projType">投影类型</param>
		/// <param name="extent">坐标范围</param>
		/// <returns></returns>
		public static TileProjection GetTileProjection(ProjectionType projType, CoordinateExtent extent)
		{
			return null;
		}

		/// <summary>
		/// 获取瓦片的投影信息
		/// </summary>
		/// <param name="projType">投影类型</param>
		/// <param name="minZoomLevel">最小缩放等级</param>
		/// <param name="maxZoomLevel">最大缩放等级</param>
		/// <returns></returns>
		public static TileProjection GetTileProjection(ProjectionType projType, int minZoomLevel, int maxZoomLevel)
		{
			return null;
		}

		/// <summary>
		/// 获取瓦片的投影信息
		/// </summary>
		/// <param name="projType">投影类型</param>
		/// <param name="extent">坐标范围</param>
		/// <param name="minZoomLevel">最小缩放等级</param>
		/// <param name="maxZoomLevel">最大缩放等级</param>
		/// <returns></returns>
		public static TileProjection GetTileProjection(ProjectionType projType, CoordinateExtent extent, int minZoomLevel, int maxZoomLevel)
		{
			return null;
		}
	}
}
