using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyGIS.TilePackage.Database
{
	public class TilePackageCreator
	{
		public TilePackageCreator()
		{

		}

		/// <summary>
		/// 创建瓦片包
		/// </summary>
		/// <param name="dbFileName">数据库文件完整路径(后缀名为.tpkg)</param>
		/// <returns></returns>
		public void Create(string dbFileName)
		{
			bool created = this.createTilePackage(dbFileName);
			if(!created)
			{
				return;
			}

			SQLiteConnection connection = this.getConnection(dbFileName);
			this.createTableAndView(connection);
		}

		/// <summary>
		/// 创建瓦片包
		/// </summary>
		/// <param name="dbFileName">数据库文件完整路径</param>
		/// <returns></returns>
		private bool createTilePackage(string dbFileName)
		{
			if(!checkDbFileName(dbFileName))
			{
				return false;
			}

			try
			{
				SQLiteConnection.CreateFile(dbFileName);
				return true;
			}
			catch
			{
				return false;
			}
			
		}

		/// <summary>
		/// 获取数据库连接对象
		/// </summary>
		/// <param name="dbFileName">数据库文件完整路径</param>
		/// <returns></returns>
		private SQLiteConnection getConnection(string dbFileName)
		{
			SQLiteConnection sqliteConnection = null;
			try
			{
				string connectionString = "Data Source=" + dbFileName + ";Pooling=true;FailIfMissing=false;";
				sqliteConnection = new SQLiteConnection(connectionString);
			}
			catch
			{
				sqliteConnection = null;
			}

			return sqliteConnection;
		}

		/// <summary>
		/// 检查数据库文件名称(后缀名为.tpkg)
		/// </summary>
		/// <param name="dbFileName"></param>
		/// <returns></returns>
		private bool checkDbFileName(string dbFileName)
		{
			if(dbFileName == null || dbFileName.Length < 6)
			{
				return false;
			}

			int lastPointIndex = dbFileName.LastIndexOf(".");
			if(dbFileName.Substring(lastPointIndex) == ".tpkg")
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// 创建表和视图
		/// </summary>
		/// <param name="connection"></param>
		private void createTableAndView(SQLiteConnection connection)
		{
			if (connection == null)
			{
				return;
			}

			if (connection.State == ConnectionState.Closed)
			{
				connection.Open();
			}

			using (SQLiteCommand command = connection.CreateCommand())
			{
				command.CommandType = CommandType.Text;
				StringBuilder sql = new StringBuilder();
				// table:spatial_ref_sys
				sql.Append("CREATE TABLE spatial_ref_sys (");
				sql.Append("    srs_name                 TEXT    NOT NULL,");
				sql.Append("    srs_id                   INTEGER NOT NULL");
				sql.Append("                                     PRIMARY KEY,");
				sql.Append("    organization             TEXT    NOT NULL,");
				sql.Append("    organization_coordsys_id INTEGER NOT NULL,");
				sql.Append("    definition               TEXT    NOT NULL,");
				sql.Append("    description              TEXT");
				sql.Append(");");

				// table-data:4326
				sql.Append("INSERT INTO spatial_ref_sys VALUES(");
				sql.Append("	'GCS_WGS_1984',");
				sql.Append("	4326,");
				sql.Append("	'EPSG',");
				sql.Append("	4326,");
				sql.Append("	'GEOGCS[\"GCS_WGS_1984\",DATUM[\"D_WGS_1984\",SPHEROID[\"WGS_1984\",6378137.0,298.257223563]],PRIMEM[\"Greenwich\",0.0],UNIT[\"Degree\",0.0174532925199433]]',");
				sql.Append("	'WGS 1984'");
				sql.Append(");");
				// table-data:3857
				sql.Append("INSERT INTO spatial_ref_sys VALUES(");
				sql.Append("	'WGS_1984_Web_Mercator_Auxiliary_Sphere',");
				sql.Append("	3857,");
				sql.Append("	'EPSG',");
				sql.Append("	3857,");
				sql.Append("	'PROJCS[\"WGS_1984_Web_Mercator_Auxiliary_Sphere\",GEOGCS[\"GCS_WGS_1984\",DATUM[\"D_WGS_1984\",SPHEROID[\"WGS_1984\",6378137.0,298.257223563]],PRIMEM[\"Greenwich\",0.0],UNIT[\"Degree\",0.0174532925199433]],PROJECTION[\"Mercator_Auxiliary_Sphere\"],PARAMETER[\"False_Easting\",0.0],PARAMETER[\"False_Northing\",0.0],PARAMETER[\"Central_Meridian\",0.0],PARAMETER[\"Standard_Parallel_1\",0.0],PARAMETER[\"Auxiliary_Sphere_Type\",0.0],UNIT[\"Meter\",1.0]]',");
				sql.Append("	'WGS 1984 Web Mercator Major Auxiliary Sphere'");
				sql.Append(");");
				// table-data:4490
				sql.Append("INSERT INTO spatial_ref_sys VALUES(");
				sql.Append("	'GCS_China_Geodetic_Coordinate_System_2000',");
				sql.Append("	4490,");
				sql.Append("	'EPSG',");
				sql.Append("	4490,");
				sql.Append("	'GEOGCS[\"GCS_China_Geodetic_Coordinate_System_2000\",DATUM[\"D_China_2000\",SPHEROID[\"CGCS2000\",6378137.0,298.257222101]],PRIMEM[\"Greenwich\",0.0],UNIT[\"Degree\",0.0174532925199433]]',");
				sql.Append("	'China Geodetic Coordinate System 2000'");
				sql.Append(");");

				//// table:images
				//sql.Append("CREATE TABLE images (");
				//sql.Append("    tile_data BLOB,");
				//sql.Append("    tile_id   TEXT");
				//sql.Append(");");

				//// table:map
				//sql.Append("CREATE TABLE map (");
				//sql.Append("    zoom_level  INTEGER,");
				//sql.Append("    tile_column INTEGER,");
				//sql.Append("    tile_row    INTEGER,");
				//sql.Append("    tile_id     TEXT");
				//sql.Append(");");

				//// view:tiles
				//sql.Append("CREATE VIEW tiles AS");
				//sql.Append("    SELECT map.ROWID AS id,");
				//sql.Append("           map.zoom_level AS zoom_level,");
				//sql.Append("           map.tile_column AS tile_column,");
				//sql.Append("           map.tile_row AS tile_row,");
				//sql.Append("           images.tile_data AS tile_data");
				//sql.Append("      FROM map");
				//sql.Append("           JOIN");
				//sql.Append("           images ON images.tile_id = map.tile_id;");

				// table:cache_scheme
				sql.Append("CREATE TABLE cache_scheme (");
				sql.Append("    srs_id           INTEGER  NOT NULL");
				sql.Append("					          PRIMARY KEY,");
				sql.Append("    max_resolution	 DOUBLE,");
				sql.Append("    min_scale        DOUBLE,");
				sql.Append("    min_level        INTEGER,");
				sql.Append("    max_level        INTEGER");
				sql.Append(");");

				// table-data:cache_scheme
				sql.Append("INSERT INTO cache_scheme VALUES(4326,1.40625,590995186.11750006,0,20);");
				sql.Append("INSERT INTO cache_scheme VALUES(3857,156543.033928,591657527.591555,0,20);");
				sql.Append("INSERT INTO cache_scheme VALUES(4490,1.40625,590995186.11750006,0,20);");

				// table:cache_scheme_level
				sql.Append("CREATE TABLE cache_scheme_level (");
				sql.Append("    srs_id           INTEGER,");
				sql.Append("    zoom_level       INTEGER,");
				sql.Append("    zoom_resolution	 DOUBLE,");
				sql.Append("    zoom_scale       DOUBLE,");
				sql.Append("    row_number       INTEGER,");
				sql.Append("    column_number    INTEGER");
				sql.Append(");");

				// table-data:cache_scheme_level
				// EPSG_4326
				sql.Append("INSERT INTO cache_scheme_level VALUES(4326,0,1.40625,590995186.11750006,1,1);");
				sql.Append("INSERT INTO cache_scheme_level VALUES(4326,1,0.703125,295497593.05875003,1,2);");
				sql.Append("INSERT INTO cache_scheme_level VALUES(4326,2,0.3515625,147748796.52937502,2,4);");
				sql.Append("INSERT INTO cache_scheme_level VALUES(4326,3,0.17578125,73874398.26468751,4,8);");
				sql.Append("INSERT INTO cache_scheme_level VALUES(4326,4,0.087890625,36937199.132343754,8,16);");
				sql.Append("INSERT INTO cache_scheme_level VALUES(4326,5,0.0439453125,18468599.566171877,16,32);");
				sql.Append("INSERT INTO cache_scheme_level VALUES(4326,6,0.02197265625,9234299.783085939,32,64);");
				sql.Append("INSERT INTO cache_scheme_level VALUES(4326,7,0.010986328125,4617149.891542969,64,128);");
				sql.Append("INSERT INTO cache_scheme_level VALUES(4326,8,0.0054931640625,2308574.9457714846,128,256);");
				sql.Append("INSERT INTO cache_scheme_level VALUES(4326,9,0.00274658203125,1154287.4728857423,256,512);");
				sql.Append("INSERT INTO cache_scheme_level VALUES(4326,10,0.001373291015625,577143.7364428712,512,1024);");
				sql.Append("INSERT INTO cache_scheme_level VALUES(4326,11,0.0006866455078125,288571.8682214356,1024,2048);");
				sql.Append("INSERT INTO cache_scheme_level VALUES(4326,12,0.00034332275390625,144285.9341107178,2048,4096);");
				sql.Append("INSERT INTO cache_scheme_level VALUES(4326,13,0.000171661376953125,72142.9670553589,4096,8192);");
				sql.Append("INSERT INTO cache_scheme_level VALUES(4326,14,0.0000858306884765625,36071.48352767945,8192,16384);");
				sql.Append("INSERT INTO cache_scheme_level VALUES(4326,15,0.00004291534423828125,18035.741763839724,16384,32768);");
				sql.Append("INSERT INTO cache_scheme_level VALUES(4326,16,0.000021457672119140625,9017.870881919862,32768,65536);");
				sql.Append("INSERT INTO cache_scheme_level VALUES(4326,17,0.000010728836059570312,4508.935440959931,65536,131072);");
				sql.Append("INSERT INTO cache_scheme_level VALUES(4326,18,0.000005364418029785156,2254.4677204799655,131072,262144);");
				sql.Append("INSERT INTO cache_scheme_level VALUES(4326,19,0.000002682209014892578,1127.2338602399827,262144,524288);");
				sql.Append("INSERT INTO cache_scheme_level VALUES(4326,20,0.000001341104507446289,563.6169301199914,524288,1048576);");
				// EPSG_4490
				sql.Append("INSERT INTO cache_scheme_level VALUES(4490,0,1.40625,590995186.11750006,1,1);");
				sql.Append("INSERT INTO cache_scheme_level VALUES(4490,1,0.703125,295497593.05875003,1,2);");
				sql.Append("INSERT INTO cache_scheme_level VALUES(4490,2,0.3515625,147748796.52937502,2,4);");
				sql.Append("INSERT INTO cache_scheme_level VALUES(4490,3,0.17578125,73874398.26468751,4,8);");
				sql.Append("INSERT INTO cache_scheme_level VALUES(4490,4,0.087890625,36937199.132343754,8,16);");
				sql.Append("INSERT INTO cache_scheme_level VALUES(4490,5,0.0439453125,18468599.566171877,16,32);");
				sql.Append("INSERT INTO cache_scheme_level VALUES(4490,6,0.02197265625,9234299.783085939,32,64);");
				sql.Append("INSERT INTO cache_scheme_level VALUES(4490,7,0.010986328125,4617149.891542969,64,128);");
				sql.Append("INSERT INTO cache_scheme_level VALUES(4490,8,0.0054931640625,2308574.9457714846,128,256);");
				sql.Append("INSERT INTO cache_scheme_level VALUES(4490,9,0.00274658203125,1154287.4728857423,256,512);");
				sql.Append("INSERT INTO cache_scheme_level VALUES(4490,10,0.001373291015625,577143.7364428712,512,1024);");
				sql.Append("INSERT INTO cache_scheme_level VALUES(4490,11,0.0006866455078125,288571.8682214356,1024,2048);");
				sql.Append("INSERT INTO cache_scheme_level VALUES(4490,12,0.00034332275390625,144285.9341107178,2048,4096);");
				sql.Append("INSERT INTO cache_scheme_level VALUES(4490,13,0.000171661376953125,72142.9670553589,4096,8192);");
				sql.Append("INSERT INTO cache_scheme_level VALUES(4490,14,0.0000858306884765625,36071.48352767945,8192,16384);");
				sql.Append("INSERT INTO cache_scheme_level VALUES(4490,15,0.00004291534423828125,18035.741763839724,16384,32768);");
				sql.Append("INSERT INTO cache_scheme_level VALUES(4490,16,0.000021457672119140625,9017.870881919862,32768,65536);");
				sql.Append("INSERT INTO cache_scheme_level VALUES(4490,17,0.000010728836059570312,4508.935440959931,65536,131072);");
				sql.Append("INSERT INTO cache_scheme_level VALUES(4490,18,0.000005364418029785156,2254.4677204799655,131072,262144);");
				sql.Append("INSERT INTO cache_scheme_level VALUES(4490,19,0.000002682209014892578,1127.2338602399827,262144,524288);");
				sql.Append("INSERT INTO cache_scheme_level VALUES(4490,20,0.000001341104507446289,563.6169301199914,524288,1048576);");
				// EPSG_3857
				sql.Append("INSERT INTO cache_scheme_level VALUES(3857,0,156543.033928,591657527.591555,1,1);");
				sql.Append("INSERT INTO cache_scheme_level VALUES(3857,1,78271.516964,295828763.795777,2,2);");
				sql.Append("INSERT INTO cache_scheme_level VALUES(3857,2,39135.758482,147914381.897889,4,4);");
				sql.Append("INSERT INTO cache_scheme_level VALUES(3857,3,19567.879241,73957190.948944,8,8);");
				sql.Append("INSERT INTO cache_scheme_level VALUES(3857,4,9783.939621,36978595.474472,16,16);");
				sql.Append("INSERT INTO cache_scheme_level VALUES(3857,5,4891.969810,18489297.737236,32,32);");
				sql.Append("INSERT INTO cache_scheme_level VALUES(3857,6,2445.984905,9244648.868618,64,64);");
				sql.Append("INSERT INTO cache_scheme_level VALUES(3857,7,1222.992453,4622324.434309,128,128);");
				sql.Append("INSERT INTO cache_scheme_level VALUES(3857,8,611.496226,2311162.217155,256,256);");
				sql.Append("INSERT INTO cache_scheme_level VALUES(3857,9,305.748113,1155581.108577,512,512);");
				sql.Append("INSERT INTO cache_scheme_level VALUES(3857,10,152.874057,577790.554289,1024,1024);");
				sql.Append("INSERT INTO cache_scheme_level VALUES(3857,11,76.437028,288895.277144,2048,2048);");
				sql.Append("INSERT INTO cache_scheme_level VALUES(3857,12,38.218514,144447.638572,4096,4096);");
				sql.Append("INSERT INTO cache_scheme_level VALUES(3857,13,19.109257,72223.819286,8192,8192);");
				sql.Append("INSERT INTO cache_scheme_level VALUES(3857,14,9.554629,36111.909643,16384,16384);");
				sql.Append("INSERT INTO cache_scheme_level VALUES(3857,15,4.777314,18055.954822,32768,32768);");
				sql.Append("INSERT INTO cache_scheme_level VALUES(3857,16,2.388657,9027.977411,65536,65536);");
				sql.Append("INSERT INTO cache_scheme_level VALUES(3857,17,1.194329,4513.988705,131072,131072);");
				sql.Append("INSERT INTO cache_scheme_level VALUES(3857,18,0.597164,2256.994353,262144,262144);");
				sql.Append("INSERT INTO cache_scheme_level VALUES(3857,19,0.298582,1128.497176,524288,524288);");
				sql.Append("INSERT INTO cache_scheme_level VALUES(3857,20,0.149291,564.248588,1048576,1048576);");

				// table:tiles
				sql.Append("CREATE TABLE tiles (");
				sql.Append("    zoom_level  INTEGER NOT NULL,");
				sql.Append("    tile_column INTEGER NOT NULL,");
				sql.Append("    tile_row    INTEGER NOT NULL,");
				sql.Append("    tile_data   BLOB    NOT NULL,");
				sql.Append("    PRIMARY KEY (zoom_level,tile_column,tile_row)");
				sql.Append(");");

				// execute
				command.CommandText = sql.ToString();
				command.ExecuteNonQuery();
				
			}
		}
	}
}