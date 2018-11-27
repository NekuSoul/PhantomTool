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

		public GeneratorSettings GetSettings() => new GeneratorSettings
		{
			Amount = (int)AmountSlider.Value,
			Seed = string.IsNullOrWhiteSpace(SeedTextBox.Text) ? null : SeedTextBox.Text,
			Sets = (from s in SetCheckListBox.SelectedItems.Cast<string>() select s).ToArray()
		};
	}
}
