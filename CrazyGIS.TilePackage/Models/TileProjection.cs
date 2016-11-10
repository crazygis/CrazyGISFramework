using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyGIS.TilePackage.Models
{
	public class TileProjection
	{
		public ProjectionType ProjType { get; set; }

		public int MinZoomLevel { get; set; }

		public int MaxZoomLevel { get; set; }

		public CoordinateExtent CoordExtent { get; set; }

		public IDictionary<int, TileExtent> TileExtentCollection { get; set; }
	}
}
