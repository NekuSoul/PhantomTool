using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using NekuSoul.PhantomTool.Data;
using NekuSoul.PhantomTool.Generator;
using NekuSoul.PhantomTool.Importer;

namespace NekuSoul.PhantomTool
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private CardCollection _collection = new CardCollection();

		public MainWindow()
		{
			InitializeComponent();
			UpdateStatusLabel();
			RefreshCollection();
		}

		private void ButtonClicked(object sender, RoutedEventArgs e)
		{
			OutputCardList(SealedGenerator.GetSealedList(GenerationSettingsControl.GetSettings(), _collection));
		}

		private void OutputCardList(CardAmount[] sealedDeck)
		{
			StringBuilder deckExport = new StringBuilder();

			foreach (var cardAmount in sealedDeck)
			{
				deckExport.AppendLine(cardAmount.ToDeckImportFormat());
			}

			OutputTextBox.Text = deckExport.ToString();
		}

		private void MenuItemRefreshCollection_Click(object sender, RoutedEventArgs e)
		{
			RefreshCollection();
		}

		private void MenuItemExit_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void RefreshCollection()
		{
			_collection = CollectionImporter.ImportCollection();
			UpdateStatusLabel();
		}

		private void UpdateStatusLabel()
		{
			StatusLabel.Content = $"{_collection.CollectedCards.Sum(ca => ca.Amount)} cards out of {GameData.Cards.Length * 4} cards collected.";
		}

		private void ClipBoardButtonClicked(object sender, RoutedEventArgs e)
		{
			Clipboard.SetText(OutputTextBox.Text);
		}
	}
}