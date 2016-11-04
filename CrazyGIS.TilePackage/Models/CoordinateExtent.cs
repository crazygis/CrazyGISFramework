using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyGIS.TilePackage.Models
{
	public class CoordinateExtent
	{
		public CoordinateExtent() { }
		public CoordinateExtent(ProjectionType projType, double minX, double minY, double maxX, double maxY)
		{
			this.ProjType = projType;
			this.MinX = minX;
			this.MinY = minY;
			this.MaxX = maxX;
			this.MaxY = maxY;
		}

		public ProjectionType ProjType { get; set; }

		public double MinX { get; set; }

		public double MinY { get; set; }

		public double MaxX { get; set; }

		public double MaxY { get; set; }
	}
}
