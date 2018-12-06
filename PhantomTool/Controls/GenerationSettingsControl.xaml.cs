using System;
using System.Collections.Generic;
using System.Linq;
using NekuSoul.PhantomTool.Data;
using NekuSoul.PhantomTool.Generator;

namespace NekuSoul.PhantomTool
{
	/// <summary>
	/// Interaction logic for GenerationSettingsControl.xaml
	/// </summary>
	public partial class GenerationSettingsControl
	{
		public GenerationSettingsControl()
		{
			InitializeComponent();

			foreach (var set in GameData.PlayableSets)
			{
				SetCheckListBox.Items.Add(set);
			}
		}

		public GeneratorSettings GetSettings()
		{
			List<CardRestiction> restictions = new List<CardRestiction>();

			bool CommonRestriction(Card c) => c.CardTypes.All(t => t != CardType.Land) && c.CardRarity == CardRarity.Common;
			bool UncommonRestriction(Card c) => c.CardTypes.All(t => t != CardType.Land) && c.CardRarity == CardRarity.Uncommon;
			bool RareMythicRestriction(Card c) => c.CardTypes.All(t => t != CardType.Land) && c.CardRarity == CardRarity.Rare || c.CardRarity == CardRarity.Mythic;
			bool LandRestriction(Card c) => c.CardTypes.Any(t => t == CardType.Land);
			bool AnyRestriction(Card c) => true;

			int multiplier = BoosterIntegerUpDown.Value ?? 0;

			restictions.AddRange(Enumerable.Repeat((CardRestiction)CommonRestriction, (CommonIntegerUpDown.Value ?? 0) * multiplier));
			restictions.AddRange(Enumerable.Repeat((CardRestiction)UncommonRestriction, (UncommonIntegerUpDown.Value ?? 0) * multiplier));
			restictions.AddRange(Enumerable.Repeat((CardRestiction)RareMythicRestriction, (RareMythicIntegerUpDown.Value ?? 0) * multiplier));
			restictions.AddRange(Enumerable.Repeat((CardRestiction)LandRestriction, (LandIntegerUpDown.Value ?? 0) * multiplier));
			restictions.AddRange(Enumerable.Repeat((CardRestiction)AnyRestriction, (AnyIntegerUpDown.Value ?? 0) * multiplier));

			return new GeneratorSettings
			{
				Amount = restictions.Count,
				PerPickRestrictions = restictions.ToArray(),
				Seed = string.IsNullOrWhiteSpace(SeedTextBox.Text) ? null : SeedTextBox.Text,
				Sets = (from s in SetCheckListBox.SelectedItems.Cast<string>() select s).ToArray()
			};
		}
	}
}
