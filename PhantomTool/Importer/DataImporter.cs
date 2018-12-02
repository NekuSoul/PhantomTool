using System;
using System.IO;
using System.Linq;
using System.Text;
using NekuSoul.PhantomTool.Data;
using Newtonsoft.Json;
using Type = NekuSoul.PhantomTool.Data.Type;

namespace NekuSoul.PhantomTool.Importer
{
	internal static class DataImporter
	{
		internal static Card[] ImportCards()
		{
			var assetDirectory = new DirectoryInfo(Path.Combine(Helper.GetInstallPath(), @"MTGA_Data\Downloads\AssetBundle"));
			var cardFile = assetDirectory.EnumerateFiles("data_cards*").First();

			StringBuilder jsonExcerpt = new StringBuilder();

			JsonLocalization[] jsonLocalizations = GetLocalizations();

			const string startLine = @"  {";
			const string endLine = @"  }";

			bool isJsonPart = false;

			jsonExcerpt.AppendLine("{ cards: [");
			foreach (var line in File.ReadLines(cardFile.FullName))
			{
				if (line == startLine)
					isJsonPart = true;

				if (isJsonPart)
					jsonExcerpt.AppendLine(line);

				if (line == endLine)
					isJsonPart = false;
			}

			jsonExcerpt.AppendLine("] }");

			var cards = JsonConvert.DeserializeObject<JsonCardFile>(jsonExcerpt.ToString()).JsonCards;

			return (from jsonCard in cards
					select new Card
					{
						CollectorNumber = jsonCard.CollectorNumber,
						Id = jsonCard.Id,
						Name = jsonLocalizations.First(l => l.Id == jsonCard.LocalizationId).Text,
						CardType = jsonLocalizations.First(l => l.Id == jsonCard.CardTypeId).Text,
						SubType = jsonLocalizations.FirstOrDefault(l => l.Id == jsonCard.SubTypeId)?.Text,
						Set = jsonCard.Set,
						Text = string.Join(Environment.NewLine, from a in jsonCard.Abilities select jsonLocalizations.First(l => l.Id == a.TextId).Text),
						Cost = jsonCard.Cost,
						ConvertedCost = jsonCard.ConvertedCost,
						Collectible = jsonCard.IsCollectible,
						Craftable = jsonCard.IsCraftable,
						Rarity = (Rarity)jsonCard.Rarity,
						Types = jsonCard.Types.Select(t => (Type)t).ToArray(),
						CardArt = jsonCard.ArtUrl
					}).ToArray();
		}

		private static JsonLocalization[] GetLocalizations()
		{
			var assetDirectory = new DirectoryInfo(Path.Combine(Helper.GetInstallPath(), @"MTGA_Data\Downloads\AssetBundle"));
			var localizationFile = assetDirectory.EnumerateFiles("data_loc*").First();

			StringBuilder jsonExcerpt = new StringBuilder();

			const string startLine = @"  {";
			const string endLine = @"  }";

			bool isJsonPart = false;

			jsonExcerpt.AppendLine("{ languages: [");
			foreach (var line in File.ReadLines(localizationFile.FullName))
			{
				if (line == startLine)
					isJsonPart = true;

				if (isJsonPart)
					jsonExcerpt.AppendLine(line);

				if (line == endLine)
					isJsonPart = false;
			}

			jsonExcerpt.AppendLine("] }");

			return JsonConvert.DeserializeObject<JsonLocalizationFile>(jsonExcerpt.ToString()).JsonLanguages.First(l => l.Key == "EN").JsonLocalizations;
		}

		[JsonObject(MemberSerialization.OptIn)]
		private class JsonCardFile
		{
			[JsonProperty("cards")]
			public JsonCard[] JsonCards = new JsonCard[0];
		}

		[JsonObject(MemberSerialization.OptIn)]
		private class JsonLocalizationFile
		{
			[JsonProperty("languages")]
			public JsonLanguage[] JsonLanguages = new JsonLanguage[0];
		}

		[JsonObject(MemberSerialization.OptIn)]
		private class JsonLanguage
		{
			[JsonProperty("langkey")]
			public string Key = string.Empty;

			[JsonProperty("keys")]
			public JsonLocalization[] JsonLocalizations = new JsonLocalization[0];
		}

		[JsonObject(MemberSerialization.OptIn)]
		private class JsonAbility
		{
			[JsonProperty("abilityId")]
			public int Id = -1;

			[JsonProperty("textId")]
			public int TextId = -1;
		}

		[JsonObject(MemberSerialization.OptIn)]
		private class JsonLocalization
		{
			[JsonProperty("id")]
			public int Id = -1;

			[JsonProperty("Text")]
			public string Text = string.Empty;
		}

		[JsonObject(MemberSerialization.OptIn)]
		private class JsonCard
		{
			[JsonProperty("grpid")]
			public int Id = -1;

			[JsonProperty("titleId")]
			public int LocalizationId = -1;

			[JsonProperty("cardtypeTextId")]
			public int CardTypeId = -1;

			[JsonProperty("isCollectible")]
			public bool IsCollectible;

			[JsonProperty("isCraftable")]
			public bool IsCraftable;

			[JsonProperty("abilities")]
			public JsonAbility[] Abilities;

			[JsonProperty("types")]
			public int[] Types;

			[JsonProperty("castingcost")]
			public string Cost;

			[JsonProperty("cmc")]
			public int ConvertedCost;

			[JsonProperty("artUrl")]
			public string ArtUrl = string.Empty;

			[JsonProperty("subtypeTextId")]
			public int SubTypeId = -1;

			[JsonProperty("rarity")]
			public int Rarity = -1;

			[JsonProperty("set")]
			public string Set = string.Empty;

			[JsonProperty("CollectorNumber")]
			public string CollectorNumber = string.Empty;
		}
	}
}
