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

		private void AmountSlider_ValueChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<double> e)
		{
			if (AmountSlider == null || AmountIntegerUpDown == null)
				return;

			AmountIntegerUpDown.Value = (int)AmountSlider.Value;
		}

		private void AmountIntegerUpDown_ValueChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<object> e)
		{
			if (AmountSlider == null || AmountIntegerUpDown == null)
				return;

			AmountSlider.Value = AmountIntegerUpDown.Value ?? 90;
		}

		public GeneratorSettings GetSettings()
		{
			if (SealedRuleCheckBox.IsChecked ?? false)
			{
				List<CardRestiction> restrictions = new List<CardRestiction>();

				restrictions.AddRange(Enumerable.Repeat<CardRestiction>(c => c.CardTypes.All(t => t != CardType.Land) && c.CardRarity == CardRarity.Common, 10 * 6));
				restrictions.AddRange(Enumerable.Repeat<CardRestiction>(c => c.CardTypes.All(t => t != CardType.Land) && c.CardRarity == CardRarity.Uncommon, 3 * 6));
				restrictions.AddRange(Enumerable.Repeat<CardRestiction>(c => c.CardTypes.All(t => t != CardType.Land) && c.CardRarity == CardRarity.Rare || c.CardRarity == CardRarity.Mythic, 1 * 6));
				restrictions.AddRange(Enumerable.Repeat<CardRestiction>(c => c.CardTypes.Any(t => t == CardType.Land), 1 * 6));

				return new GeneratorSettings
				{
					Amount = 90,
					Seed = string.IsNullOrWhiteSpace(SeedTextBox.Text) ? null : SeedTextBox.Text,
					Sets = (from s in SetCheckListBox.SelectedItems.Cast<string>() select s).ToArray(),
					PerPickRestrictions = restrictions.ToArray()
				};
			}

			return new GeneratorSettings
			{
				Amount = (int)AmountSlider.Value,
				Seed = string.IsNullOrWhiteSpace(SeedTextBox.Text) ? null : SeedTextBox.Text,
				Sets = (from s in SetCheckListBox.SelectedItems.Cast<string>() select s).ToArray()
			};
		}

		private void SealedRuleCheckBox_Checked(object sender, System.Windows.RoutedEventArgs e)
		{
			AmountIntegerUpDown.IsEnabled = false;
			AmountSlider.IsEnabled = false;
		}

		private void SealedRuleCheckBox_Unchecked(object sender, System.Windows.RoutedEventArgs e)
		{
			AmountIntegerUpDown.IsEnabled = true;
			AmountSlider.IsEnabled = true;
		}
	}
}
