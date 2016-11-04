﻿using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace CrazyGIS.Toolkit
{
    public class ImageTool
    {
		/// <summary>
		/// Convert Image to Byte[]
		/// </summary>
		/// <param name="imageFullName"></param>
		/// <returns></returns>
		public static byte[] ImageToBytes(string imageFullName)
		{
			try
			{
				Image image = Image.FromFile(imageFullName);
				return imageToBytes(image);
			}
			catch
			{
				return null;
			}
		}

		/// <summary>
		/// Convert Image to Byte[]
		/// </summary>
		/// <param name="image"></param>
		/// <returns></returns>
		public static byte[] ImageToBytes(Image image)
		{
			if(image == null)
			{
				return null;
			}
			return imageToBytes(image);
		}

		/// <summary>
		/// Convert Byte[] to Image
		/// </summary>
		/// <param name="buffer"></param>
		/// <returns></returns>
		public static Image BytesToImage(byte[] buffer)
		{
			MemoryStream ms = new MemoryStream(buffer);
			Image image = Image.FromStream(ms);
			return image;
		}

		/// <summary>
		/// Convert Byte[] to a picture and Store it in file
		/// </summary>
		/// <param name="fileName"></param>
		/// <param name="buffer"></param>
		/// <returns></returns>
		public static string CreateImageFromBytes(string fileName, byte[] buffer)
		{
			string file = fileName;
			Image image = BytesToImage(buffer);
			ImageFormat format = image.RawFormat;
			if (format.Equals(ImageFormat.Jpeg))
			{
				file += ".jpeg";
			}
			else if (format.Equals(ImageFormat.Png))
			{
				file += ".png";
			}
			else if (format.Equals(ImageFormat.Bmp))
			{
				file += ".bmp";
			}
			else if (format.Equals(ImageFormat.Gif))
			{
				file += ".gif";
			}
			else if (format.Equals(ImageFormat.Icon))
			{
				file += ".icon";
			}
			FileInfo info = new FileInfo(file);
			Directory.CreateDirectory(info.Directory.FullName);
			File.WriteAllBytes(file, buffer);
			return file;
		}

		#region private

		private static byte[] imageToBytes(Image image)
		{
			ImageFormat format = image.RawFormat;
			using (MemoryStream ms = new MemoryStream())
			{
				if (format.Equals(ImageFormat.Jpeg))
				{
					image.Save(ms, ImageFormat.Jpeg);
				}
				else if (format.Equals(ImageFormat.Png))
				{
					image.Save(ms, ImageFormat.Png);
				}
				else if (format.Equals(ImageFormat.Bmp))
				{
					image.Save(ms, ImageFormat.Bmp);
				}
				else if (format.Equals(ImageFormat.Gif))
				{
					image.Save(ms, ImageFormat.Gif);
				}
				else if (format.Equals(ImageFormat.Icon))
				{
					image.Save(ms, ImageFormat.Icon);
				}
				byte[] buffer = new byte[ms.Length];
				//Image.Save()会改变MemoryStream的Position，需要重新Seek到Begin
				ms.Seek(0, SeekOrigin.Begin);
				ms.Read(buffer, 0, buffer.Length);
				return buffer;
			}
		}

		#endregion
	}
}
