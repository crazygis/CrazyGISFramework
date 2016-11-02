using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyGIS.TilePackage.Models
{
	public class Scale
	{
		private static double minScale = 590995186.1175;

		public static double GetScaleByLevel(int level)
		{
			if(level < 0 || level > 19)
			{
				return 0;
			}

			return minScale / Math.Pow(2, level);
		}
	}
}
