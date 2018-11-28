using System.IO;
using System.Linq;
using AssetStudio;

namespace NekuSoul.PhantomTool.Importer
{
	public static class CardArtImporter
	{
		public static void ImportCardArt(params string[] sets)
		{
			string cardArtPath = Path.Combine(Helper.GetAppDataPath(), "CardArt");

			Directory.CreateDirectory(cardArtPath);

			var assetDirectory = new DirectoryInfo(Path.Combine(Helper.GetInstallPath(), @"MTGA_Data\Downloads\AssetBundle"));

			var assetsManager = new AssetsManager();

			var files =
				from set in sets
				select assetDirectory.EnumerateFiles().FirstOrDefault(f => f.Name.StartsWith($"{set.ToLower()}_cardart_"))
				into file
				where file != null
				select file.FullName;

			assetsManager.LoadFiles(files.ToArray());

			foreach (var serializedFile in assetsManager.assetsFileList)
			{
				foreach (var obj in serializedFile.Objects.Values)
				{
					if (!(obj is Texture2D texture2D))
						continue;

					var texture2DConverter = new Texture2DConverter(texture2D);
					var bitmap = texture2DConverter.ConvertToBitmap(true);
					bitmap.Save(Path.Combine(cardArtPath, $"{texture2D.m_Name}.png"));
				}
			}
		}
	}
}
