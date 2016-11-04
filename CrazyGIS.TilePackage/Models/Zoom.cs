using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyGIS.TilePackage.Models
{
	public class Zoom
	{
		public ProjectionType ProjType { get; set; }

		public int ZoomLevel { get; set; }

		public double ZoomResolution { get; set; }

		public double ZoomScale { get; set; }

		public int RowNumber { get; set; }

		public int ColumnNumber { get; set; }

	}
}
