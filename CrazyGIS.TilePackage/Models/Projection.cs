using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyGIS.TilePackage.Models
{
	public class Projection
	{
		public ProjectionType PROJECTION_TYPE { get; private set; } = ProjectionType.EPSG_4326;

		public int TILE_SIZE { get; private set; } = 256;

		public int MIN_ZOOM_LEVEL { get; private set; } = 0;

		public int MAX_ZOOM_LEVEL { get; private set; } = 20;

		public int DPI { get; private set; } = 96;

		public double MIN_SCALE { get; private set; } = 591658710.909131;

		public double MAX_RESOLUTION { get; private set; } = 1.40625;

		public double MIN_X { get; private set; } = -180;

		public double MIN_Y { get; private set; } = -90;

		public double MAX_X { get; private set; } = 180;

		public double MAX_Y { get; private set; } = 90;

		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="projType"></param>
		public Projection(ProjectionType projType)
		{
			switch (projType)
			{
				case ProjectionType.EPSG_3857:
					this.PROJECTION_TYPE = projType;
					this.TILE_SIZE = Constant.EPSG_3857_TILE_SIZE;
					this.MIN_ZOOM_LEVEL = Constant.EPSG_3857_MIN_ZOOM_LEVEL;
					this.MAX_ZOOM_LEVEL = Constant.EPSG_3857_MAX_ZOOM_LEVEL;
					this.DPI = Constant.EPSG_3857_DPI;
					this.MIN_SCALE = Constant.EPSG_3857_MIN_SCALE;
					this.MAX_RESOLUTION = Constant.EPSG_3857_MAX_RESOLUTION;
					this.MIN_X = Constant.EPSG_3857_MIN_X;
					this.MIN_Y = Constant.EPSG_3857_MIN_Y;
					this.MAX_X = Constant.EPSG_3857_MAX_X;
					this.MAX_Y = Constant.EPSG_3857_MAX_Y;
					break;
				case ProjectionType.EPSG_4326:
					this.PROJECTION_TYPE = projType;
					this.TILE_SIZE = Constant.EPSG_4326_TILE_SIZE;
					this.MIN_ZOOM_LEVEL = Constant.EPSG_4326_MIN_ZOOM_LEVEL;
					this.MAX_ZOOM_LEVEL = Constant.EPSG_4326_MAX_ZOOM_LEVEL;
					this.DPI = Constant.EPSG_4326_DPI;
					this.MIN_SCALE = Constant.EPSG_4326_MIN_SCALE;
					this.MAX_RESOLUTION = Constant.EPSG_4326_MAX_RESOLUTION;
					this.MIN_X = Constant.EPSG_4326_MIN_X;
					this.MIN_Y = Constant.EPSG_4326_MIN_Y;
					this.MAX_X = Constant.EPSG_4326_MAX_X;
					this.MAX_Y = Constant.EPSG_4326_MAX_Y;
					break;
				case ProjectionType.EPSG_4490:
					this.PROJECTION_TYPE = projType;
					this.TILE_SIZE = Constant.EPSG_4490_TILE_SIZE;
					this.MIN_ZOOM_LEVEL = Constant.EPSG_4490_MIN_ZOOM_LEVEL;
					this.MAX_ZOOM_LEVEL = Constant.EPSG_4490_MAX_ZOOM_LEVEL;
					this.DPI = Constant.EPSG_4490_DPI;
					this.MIN_SCALE = Constant.EPSG_4490_MIN_SCALE;
					this.MAX_RESOLUTION = Constant.EPSG_4490_MAX_RESOLUTION;
					this.MIN_X = Constant.EPSG_4490_MIN_X;
					this.MIN_Y = Constant.EPSG_4490_MIN_Y;
					this.MAX_X = Constant.EPSG_4490_MAX_X;
					this.MAX_Y = Constant.EPSG_4490_MAX_Y;
					break;
				default:
					break;
			}
		}

		/// <summary>
		/// 获取层级的分辨率
		/// </summary>
		/// <param name="zoomLevel"></param>
		/// <returns></returns>
		public double GetZoomResolution(int zoomLevel)
		{
			if(!this.CheckZoomLevel(zoomLevel))
			{
				return 0;
			}

			return this.MAX_RESOLUTION / Math.Pow(2, zoomLevel);
		}

		/// <summary>
		/// 获取层级的比例尺
		/// </summary>
		/// <param name="zoomLevel"></param>
		/// <returns></returns>
		public double GetZoomScale(int zoomLevel)
		{
			if (!this.CheckZoomLevel(zoomLevel))
			{
				return 0;
			}

			return this.MIN_SCALE / Math.Pow(2, zoomLevel);
		}

		/// <summary>
		/// 获取默认的坐标范围
		/// </summary>
		/// <returns></returns>
		public CoordinateExtent GetDefaultCoordinateExtent()
		{
			CoordinateExtent extent = new CoordinateExtent();
			extent.ProjType = this.PROJECTION_TYPE;
			extent.MinX = this.MIN_X;
			extent.MinY = this.MIN_Y;
			extent.MaxX = this.MAX_X;
			extent.MaxY = this.MAX_Y;

			return extent;
		}

		/// <summary>
		/// 获取默认瓦片范围
		/// </summary>
		/// <param name="zoomLevel"></param>
		/// <returns></returns>
		public TileExtent GetDefaultTileExtent(int zoomLevel)
		{
			if (!this.CheckZoomLevel(zoomLevel))
			{
				return null;
			}

			TileExtent result = null;
			switch (this.PROJECTION_TYPE)
			{
				case ProjectionType.EPSG_3857:
					result = this.getTileExtent_3857(zoomLevel);
					break;
				case ProjectionType.EPSG_4326:
					result = this.getTileExtent_4326(zoomLevel);
					break;
				case ProjectionType.EPSG_4490:
					result = this.getTileExtent_4490(zoomLevel);
					break;
				default:
					result = null;
					break;
			}

			return result;
		}

		/// <summary>
		/// 检查层级是否在显示范围内
		/// </summary>
		/// <param name="zoomLevel"></param>
		/// <returns></returns>
		public bool CheckZoomLevel(int zoomLevel)
		{
			if (zoomLevel >= this.MIN_ZOOM_LEVEL && zoomLevel <= this.MAX_ZOOM_LEVEL)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// 检查坐标范围
		/// </summary>
		/// <param name="extent"></param>
		/// <returns></returns>
		public bool CheckCoordinateExtent(CoordinateExtent coordinateExtent)
		{
			if(coordinateExtent == null)
			{
				return false;
			}

			if(coordinateExtent.MinX < this.MIN_X)
			{
				return false;
			}

			if(coordinateExtent.MinY < this.MIN_Y)
			{
				return false;
			}

			if(coordinateExtent.MaxX > this.MAX_X)
			{
				return false;
			}

			if(coordinateExtent.MaxY > this.MAX_Y)
			{
				return false;
			}

			return true;

		}

		/// <summary>
		/// 纠正坐标范围(如果在合理范围内,则返回原值;如果超出边界,则舍弃边界以外的部分。)
		/// </summary>
		/// <param name="coordinateExtent"></param>
		/// <returns></returns>
		public CoordinateExtent CorrectCoordianteExtent(CoordinateExtent coordinateExtent)
		{
			if(coordinateExtent == null)
			{
				return new CoordinateExtent(this.PROJECTION_TYPE, this.MIN_X, this.MIN_Y, this.MAX_X, this.MAX_Y);
			}

			if (coordinateExtent.MinX < this.MIN_X)
			{
				coordinateExtent.MinX = this.MIN_X;
			}

			if (coordinateExtent.MinY < this.MIN_Y)
			{
				coordinateExtent.MinY = this.MIN_Y;
			}

			if (coordinateExtent.MaxX > this.MAX_X)
			{
				coordinateExtent.MaxX = this.MAX_X;
			}

			if (coordinateExtent.MaxY > this.MAX_Y)
			{
				coordinateExtent.MaxY = this.MAX_Y;
			}

			return coordinateExtent;
		}


		private TileExtent getTileExtent_3857(int zoomLevel)
		{
			TileExtent tileExtent = new TileExtent();
			tileExtent.ProjType = this.PROJECTION_TYPE;
			tileExtent.ZoomLevel = zoomLevel;
			tileExtent.ZoomResolution = this.GetZoomResolution(zoomLevel);
			tileExtent.ZoomScale = this.GetZoomScale(zoomLevel);
			tileExtent.MinColumnIndex = 0;
			tileExtent.MinRowIndex = 0;

			tileExtent.RowNumber = Convert.ToInt32(Math.Pow(2, zoomLevel));
			tileExtent.ColumnNumber = Convert.ToInt32(Math.Pow(2, zoomLevel));

			tileExtent.MaxColumnIndex = tileExtent.ColumnNumber - 1;
			tileExtent.MaxRowIndex = tileExtent.RowNumber - 1;

			return tileExtent;
		}

		private TileExtent getTileExtent_4326(int zoomLevel)
		{
			TileExtent tileExtent = new TileExtent();
			tileExtent.ProjType = this.PROJECTION_TYPE;
			tileExtent.ZoomLevel = zoomLevel;
			tileExtent.ZoomResolution = this.GetZoomResolution(zoomLevel);
			tileExtent.ZoomScale = this.GetZoomScale(zoomLevel);
			tileExtent.MinColumnIndex = 0;
			tileExtent.MinRowIndex = 0;

			if (zoomLevel == 0)
			{
				tileExtent.RowNumber = 1;
				tileExtent.ColumnNumber = 1;

				
			}
			else if (zoomLevel == 1)
			{
				tileExtent.RowNumber = 1;
				tileExtent.ColumnNumber = 2;

				tileExtent.MaxColumnIndex = 0;
				tileExtent.MaxRowIndex = 0;
			}
			else
			{
				tileExtent.RowNumber = Convert.ToInt32(1 * Math.Pow(2, (zoomLevel - 1)));
				tileExtent.ColumnNumber = Convert.ToInt32(2 * Math.Pow(2, (zoomLevel - 1)));
			}

			tileExtent.MaxColumnIndex = tileExtent.ColumnNumber - 1;
			tileExtent.MaxRowIndex = tileExtent.RowNumber - 1;

			return tileExtent;
		}

		private TileExtent getTileExtent_4490(int zoomLevel)
		{
			TileExtent tileExtent = new TileExtent();
			tileExtent.ProjType = this.PROJECTION_TYPE;
			tileExtent.ZoomLevel = zoomLevel;
			tileExtent.ZoomResolution = this.GetZoomResolution(zoomLevel);
			tileExtent.ZoomScale = this.GetZoomScale(zoomLevel);
			tileExtent.MinColumnIndex = 0;
			tileExtent.MinRowIndex = 0;

			if (zoomLevel == 0)
			{
				tileExtent.RowNumber = 1;
				tileExtent.ColumnNumber = 1;


			}
			else if (zoomLevel == 1)
			{
				tileExtent.RowNumber = 1;
				tileExtent.ColumnNumber = 2;

				tileExtent.MaxColumnIndex = 0;
				tileExtent.MaxRowIndex = 0;
			}
			else
			{
				tileExtent.RowNumber = Convert.ToInt32(1 * Math.Pow(2, (zoomLevel - 1)));
				tileExtent.ColumnNumber = Convert.ToInt32(2 * Math.Pow(2, (zoomLevel - 1)));
			}

			tileExtent.MaxColumnIndex = tileExtent.ColumnNumber - 1;
			tileExtent.MaxRowIndex = tileExtent.RowNumber - 1;

			return tileExtent;
		}
	}
}
