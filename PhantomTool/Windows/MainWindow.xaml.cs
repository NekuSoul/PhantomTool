using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using NekuSoul.PhantomTool.Controls;
using NekuSoul.PhantomTool.Data;
using NekuSoul.PhantomTool.Generator;
using NekuSoul.PhantomTool.Importer;
using NekuSoul.PhantomTool.Windows;

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
				OutputListBoxA.Items.Add(new CardAmountControl(cardAmount));
			}

			UpdateCounters();
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
            GC.Collect();
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

			foreach (CardAmountControl item in listBox.Items)
			{
				deckExport.AppendLine(item.CardAmount.ToDeckImportFormat());
			}

			Clipboard.SetText(deckExport.ToString());
		}

		private void MoveLeftButton_Click(object sender, RoutedEventArgs e)
		{
			MoveCard(OutputListBoxB, OutputListBoxA);
			UpdateCounters();
		}

		private void MoveRightButton_Click(object sender, RoutedEventArgs e)
		{
			MoveCard(OutputListBoxA, OutputListBoxB);
			UpdateCounters();
		}

		private void OutputListBoxA_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			MoveCard(OutputListBoxA, OutputListBoxB);
			UpdateCounters();
		}

		private void OutputListBoxB_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			MoveCard(OutputListBoxB, OutputListBoxA);
			UpdateCounters();
		}

		private static void MoveCard(ListBox source, ListBox target)
		{
			if (source.SelectedItem == null)
				return;

			var cardAmountControl = (CardAmountControl)source.SelectedItem;
			var cardAmount = cardAmountControl.CardAmount;

			if (cardAmount.Amount == 1)
				source.Items.Remove(source.SelectedItem);
			else
			{
				cardAmount.Amount--;
				cardAmountControl.UpdateAmount();
			}

			if (target.Items.Cast<CardAmountControl>().FirstOrDefault(lb => lb.CardAmount.Card == cardAmount.Card) is CardAmountControl cac)
			{
				cac.CardAmount.Amount++;
				cac.UpdateAmount();
			}

			else
			{
				var tempList = target.Items.Cast<CardAmountControl>().ToList();
				target.Items.Clear();
				tempList.Add(new CardAmountControl(new CardAmount { Amount = 1, Card = cardAmount.Card }));
				tempList = (from c in tempList orderby c.CardAmount.Card.ConvertedCost, c.CardAmount.Card.Name select c).ToList();

				foreach (var control in tempList)
					target.Items.Add(control);
			}
		}

		private void UpdateCounters()
		{
			CounterALabel.Content = $"{(from c in OutputListBoxA.Items.Cast<CardAmountControl>() select c.CardAmount).Sum(ca => ca.Amount)} cards";
			CounterBLabel.Content = $"{(from c in OutputListBoxB.Items.Cast<CardAmountControl>() select c.CardAmount).Sum(ca => ca.Amount)} cards";
		}

		private void MenuItemAbout_Click(object sender, RoutedEventArgs e)
		{
			new AboutWindow { Owner = this }.ShowDialog();
		}
	}
}