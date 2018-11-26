using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Documents;
using PhantomFriend.Data;

namespace PhantomFriend.Importer
{
	public static class CollectionImporter
	{
		public static CardCollection ImportCollection(Card[] cards)
		{
			var assetDirectory = new DirectoryInfo(Path.Combine(Helper.GetInstallPath(), @"MTGA_Data\Logs\Logs"));
			var logFile = assetDirectory.EnumerateFiles().OrderByDescending(fi => fi.CreationTimeUtc).First();

			const string startLine = @"<== PlayerInventory.GetPlayerCardsV3(13)";
			const string endLine = @"}</div></div>";

			bool isCollectionPart = false;

			List<CardAmount> collectedCards = new List<CardAmount>();

			var regex = new Regex(@"^  ""(\d+)"": (\d),?$");

			foreach (var line in File.ReadLines(logFile.FullName))
			{
				if (line == startLine)
					isCollectionPart = true;

				if (isCollectionPart)
				{
					var match = regex.Match(line);

					if (!match.Success)
						continue;

					collectedCards.Add(new CardAmount { Amount = int.Parse(match.Groups[2].Value), Card = cards.First(c => c.Id == int.Parse(match.Groups[1].Value)) });
				}

				if (isCollectionPart && line == endLine)
					break;
			}

			return new CardCollection { CollectedCards = collectedCards.ToArray() };
		}
	}
}
