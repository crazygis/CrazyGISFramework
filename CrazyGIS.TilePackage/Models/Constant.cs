using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyGIS.TilePackage.Models
{
	public class Constant
	{
		public static int EPSG_3857_TILE_SIZE = 256;
		public static int EPSG_4326_TILE_SIZE = 256;
		public static int EPSG_4490_TILE_SIZE = 256;

		public static int EPSG_3857_MIN_ZOOM_LEVEL = 0;
		public static int EPSG_4326_MIN_ZOOM_LEVEL = 0;
		public static int EPSG_4490_MIN_ZOOM_LEVEL = 0;

		public static int EPSG_3857_MAX_ZOOM_LEVEL = 20;
		public static int EPSG_4326_MAX_ZOOM_LEVEL = 20;
		public static int EPSG_4490_MAX_ZOOM_LEVEL = 20;

		public static double EPSG_3857_MIN_X = -20037508.342787;
		public static double EPSG_3857_MIN_Y = -20037508.342787;
		public static double EPSG_3857_MAX_X = 20037508.342787;
		public static double EPSG_3857_MAX_Y = 20037508.342787;

		public static double EPSG_4326_MIN_X = -180;
		public static double EPSG_4326_MIN_Y = -90;
		public static double EPSG_4326_MAX_X = 180;
		public static double EPSG_4326_MAX_Y = 90;

		public static double EPSG_4490_MIN_X = -180;
		public static double EPSG_4490_MIN_Y = -90;
		public static double EPSG_4490_MAX_X = 180;
		public static double EPSG_4490_MAX_Y = 90;

		public static int EPSG_3857_DPI = 96;
		public static int EPSG_4326_DPI = 96;
		public static int EPSG_4490_DPI = 96;

		public static double EPSG_3857_MIN_SCALE = 591658710.909131;
		public static double EPSG_4326_MIN_SCALE = 591658710.909131;
		public static double EPSG_4490_MIN_SCALE = 591658710.909131;

		public static double EPSG_3857_MAX_RESOLUTION = 156543.033928;
		public static double EPSG_4326_MAX_RESOLUTION = 1.40625;
		public static double EPSG_4490_MAX_RESOLUTION = 1.40625;
	}
}
