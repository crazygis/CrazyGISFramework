using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyGIS.TilePackage.Models
{
	public class Constant
	{
		public static int TILE_SIZE = 256;

		public static int MIN_ZOOM_LEVEL = 0;
		public static int MAX_ZOOM_LEVEL = 20;

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

		public static int DPI_3857 = 96;
		public static int DPI_4326 = 96;
		public static int DPI_4490 = 96;

		public static double MIN_SCALE_3857 = 591658710.909131;
		public static double MIN_SCALE_4326 = 591658710.909131;
		public static double MIN_SCALE_4490 = 591658710.909131;

		public static double MAX_RESOLUTION_3857 = 156543.033928;
		public static double MAX_RESOLUTION_4326 = 1.40625;
		public static double MAX_RESOLUTION_4490 = 1.40625;
	}
}
