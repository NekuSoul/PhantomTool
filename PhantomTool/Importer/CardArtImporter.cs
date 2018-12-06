using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using AssetStudio;
using NekuSoul.PhantomTool.Data;

namespace NekuSoul.PhantomTool.Importer
{
	public static class CardArtImporter
	{
		private static AssetsManager _assetsManager;
		private static Dictionary<string, Texture2D> _loadedAssets;

		public static void ImportCardArt(params string[] sets)
		{
			string cardArtPath = Path.Combine(Helper.GetAppDataPath(), "CardArt");

			Directory.CreateDirectory(cardArtPath);

			var assetDirectory = new DirectoryInfo(Path.Combine(Helper.GetInstallPath(), @"MTGA_Data\Downloads\AssetBundle"));

			_assetsManager = new AssetsManager();
			_loadedAssets = new Dictionary<string, Texture2D>();

			var files =
				from set in sets
				select assetDirectory.EnumerateFiles().FirstOrDefault(f => f.Name.StartsWith($"{set.ToLower()}_cardart_"))
				into file
				where file != null
				select file.FullName;

			_assetsManager.LoadFiles(files.ToArray());

			foreach (var serializedFile in _assetsManager.assetsFileList)
			{
				foreach (var obj in serializedFile.Objects.Values)
				{
					if (!(obj is Texture2D texture2D))
						continue;

					_loadedAssets[texture2D.m_Name] = (Texture2D)obj;
				}
			}
		}

		public static Bitmap GetCardArt(Card card)
		{
			var texture2DConverter = new Texture2DConverter(_loadedAssets[$"{card.CardArt}_AIF"]);
			return texture2DConverter.ConvertToBitmap(true);
		}
	}
}
