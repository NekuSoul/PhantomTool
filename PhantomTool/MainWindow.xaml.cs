using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
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
			OutputListBoxA.Items.Clear();

			foreach (var cardAmount in sealedDeck)
			{
				OutputListBoxA.Items.Add(new ListBoxItem {Content = cardAmount, ToolTip = cardAmount.Card.GetDescription()});
			}
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

		private void ClipBoardButtonAClicked(object sender, RoutedEventArgs e)
		{
			SetClipboardText(OutputListBoxA);
		}

		private void ClipBoardButtonBClicked(object sender, RoutedEventArgs e)
		{
			SetClipboardText(OutputListBoxB);
		}

		private void SetClipboardText(ListBox listBox)
		{
			StringBuilder deckExport = new StringBuilder();

			foreach (ListBoxItem item in listBox.Items)
			{
				deckExport.AppendLine((item.Content as CardAmount)?.ToDeckImportFormat());
			}

			Clipboard.SetText(deckExport.ToString());
		}

		private void MenuItemExtractCardArt_Click(object sender, RoutedEventArgs e)
		{
			CardArtImporter.ImportCardArt(GameData.Sets);
		}
	}
}