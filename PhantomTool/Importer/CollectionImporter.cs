using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using NekuSoul.PhantomTool.Data;

namespace NekuSoul.PhantomTool.Importer
{
	public static class CollectionImporter
	{
		public static CardCollection ImportCollection()
		{
			var assetDirectory = new DirectoryInfo(Path.Combine(Helper.GetInstallPath(), @"MTGA_Data\Logs\Logs"));
			foreach (var logFile in assetDirectory.EnumerateFiles().OrderByDescending(fi => fi.CreationTimeUtc))
			{
				const string startLine = @"<== PlayerInventory.GetPlayerCardsV";
				const string endLine = @"}</div></div>";

				bool isCollectionPart = false;

				List<CardAmount> collectedCards = new List<CardAmount>();

				var regex = new Regex(@"^  ""(\d+)"": (\d),?$");

				foreach (var line in File.ReadLines(logFile.FullName))
				{
					if (line.StartsWith(startLine))
						isCollectionPart = true;

					if (isCollectionPart)
					{
						var match = regex.Match(line);

						if (!match.Success)
							continue;

						collectedCards.Add(new CardAmount { Amount = int.Parse(match.Groups[2].Value), Card = GameData.Cards.First(c => c.Id == int.Parse(match.Groups[1].Value)) });
					}

					if (isCollectionPart && line == endLine)
						break;
				}

				if(collectedCards.Count==0)
					continue;

				return new CardCollection { CollectedCards = collectedCards.ToArray() };
			}

			return new CardCollection { CollectedCards = new CardAmount[0] };
		}
	}
}
