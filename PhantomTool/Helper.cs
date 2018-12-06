using System;
using System.IO;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace NekuSoul.PhantomTool
{
	internal static class Helper
	{
		internal static string GetInstallPath()
		{
			const string registryPath = @"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Wizards of the Coast\MTGArena";
			const string registryValue = "Path";
			return Registry.GetValue(registryPath, registryValue, null).ToString();
		}

		internal static string GetAppDataPath() => Path.Combine(
			Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
			"NekuSoul",
			"PhantomTool");

		public static BitmapImage ToBitmapImage(this System.Drawing.Bitmap bitmap)
		{         
			MemoryStream memoryStream = new MemoryStream();
			bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Bmp);
			BitmapImage image = new BitmapImage();
			memoryStream.Seek(0, SeekOrigin.Begin);
			image.BeginInit();
			image.StreamSource = memoryStream;
			image.EndInit();

			return image;
		}
	}
}
