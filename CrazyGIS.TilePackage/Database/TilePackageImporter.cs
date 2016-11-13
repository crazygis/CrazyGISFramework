using CrazyGIS.TilePackage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyGIS.TilePackage.Database
{
	public class TilePackageImporter
	{
		private string tpkgFileFullName = string.Empty;
		private TileSource tileSource = null;

		public TilePackageImporter(string tpkgFileFullName, TileSource tileSource)
		{
			this.tpkgFileFullName = tpkgFileFullName;
			this.tileSource = tileSource;
		}

		public void ProcessTpkg()
		{

		}

		public void ProcessTileSource()
		{

		}
	}
}
