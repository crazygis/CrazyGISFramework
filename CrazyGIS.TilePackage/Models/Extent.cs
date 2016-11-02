using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyGIS.TilePackage.Models
{
	public class Extent
	{
		public Extent() { }
		public Extent(double minX, double minY, double maxX, double maxY)
		{
			this.MinX = minX;
			this.MinY = minY;
			this.MaxX = maxX;
			this.MaxY = maxY;
		}

		public double MinX { get; set; }
		public double MinY { get; set; }
		public double MaxX { get; set; }
		public double MaxY { get; set; }
	}
}
