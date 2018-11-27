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
			OutputListBox.Items.Clear();

			foreach (var cardAmount in sealedDeck)
			{
				OutputListBox.Items.Add(new ListBoxItem {Content = cardAmount, ToolTip = cardAmount.Card.Name});
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

		private void ClipBoardButtonClicked(object sender, RoutedEventArgs e)
		{
			StringBuilder deckExport = new StringBuilder();

			foreach (ListBoxItem item in OutputListBox.Items)
			{
				deckExport.AppendLine((item.Content as CardAmount)?.ToDeckImportFormat());
			}

			Clipboard.SetText(deckExport.ToString());
		}
	}
}