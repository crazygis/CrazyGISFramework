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
			return getTileProjection(projType, null, null, null);
		}

		/// <summary>
		/// 获取瓦片的投影信息
		/// </summary>
		/// <param name="projType">投影类型</param>
		/// <param name="extent">坐标范围</param>
		/// <returns></returns>
		public static TileProjection GetTileProjection(ProjectionType projType, CoordinateExtent extent)
		{
			return getTileProjection(projType, extent, null, null);
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
			return getTileProjection(projType, null, minZoomLevel, maxZoomLevel);
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
			return getTileProjection(projType, extent, minZoomLevel, maxZoomLevel);
		}


		private static TileProjection getTileProjection(ProjectionType projType, CoordinateExtent extent, int? minZoomLevel, int? maxZoomLevel)
		{
			// 默认投影信息
			Projection projection = new Projection(projType);
			// 瓦片投影
			TileProjection tileProjection = new TileProjection();
			tileProjection.ProjType = projType;
			if(extent == null)
			{
				extent = projection.GetDefaultCoordinateExtent();
			}
			else
			{
				extent = projection.CorrectCoordianteExtent(extent);
			}
			tileProjection.CoordExtent = extent;
			if(minZoomLevel == null || !projection.CheckZoomLevel(minZoomLevel.Value))
			{
				tileProjection.MinZoomLevel = projection.MIN_ZOOM_LEVEL;
			}
			else
			{
				tileProjection.MinZoomLevel = minZoomLevel.Value;
			}
			if(maxZoomLevel == null || !projection.CheckZoomLevel(maxZoomLevel.Value))
			{
				tileProjection.MaxZoomLevel = projection.MAX_ZOOM_LEVEL;
			}
			else
			{
				tileProjection.MaxZoomLevel = maxZoomLevel.Value;
			}
			// TileExtent: key,level;value,TileExtent
			tileProjection.TileExtentCollection = new Dictionary<int, TileExtent>();
			// 遍历所有Level
			for (int i = tileProjection.MinZoomLevel; i <= tileProjection.MaxZoomLevel; i++)
			{
				TileExtent tileExtent = new TileExtent();
				tileExtent.ProjType = projType;
				tileExtent.ZoomLevel = i;
				// 计算当前范围
				double originX = projection.MIN_X;
				double originY = projection.MAX_Y;

				tileExtent.MinRowIndex = Convert.ToInt32(
					Math.Floor(
						(originY - extent.MaxY) / (projection.TILE_SIZE * projection.GetZoomResolution(i))
					));
				tileExtent.MinColumnIndex = Convert.ToInt32(
					Math.Floor(
						(originX - extent.MinX) / (projection.TILE_SIZE * projection.GetZoomResolution(i))
					));
				tileExtent.MaxRowIndex = Convert.ToInt32(
					Math.Ceiling(
						(originY - extent.MinY) / (projection.TILE_SIZE * projection.GetZoomResolution(i))
					));
				tileExtent.MaxColumnIndex = Convert.ToInt32(
					Math.Ceiling(
						(originX - extent.MaxX) / (projection.TILE_SIZE * projection.GetZoomResolution(i))
					));

				tileExtent.RowNumber = tileExtent.MaxRowIndex - tileExtent.MinRowIndex + 1;
				tileExtent.ColumnNumber = tileExtent.MaxColumnIndex - tileExtent.MinColumnIndex + 1;

				// 添加到集合
				tileProjection.TileExtentCollection.Add(i, tileExtent);
			}

			return tileProjection;
		}
	}
}
