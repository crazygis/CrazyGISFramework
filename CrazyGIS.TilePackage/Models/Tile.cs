using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyGIS.TilePackage.Models
{
	public class Tile
	{
		public int Level { get; set; }

		public int Row { get; set; }

		public int Column { get; set; }

		public byte[] Data { get; set; }

		public Tile()
		{

		}

		public Tile(int level, int row, int column, byte[] data)
		{
			this.Level = level;
			this.Row = row;
			this.Column = column;
			this.Data = data;
		}
	}
}
