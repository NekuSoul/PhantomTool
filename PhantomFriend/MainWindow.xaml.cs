using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using PhantomFriend.Data;
using PhantomFriend.Importer;

namespace PhantomFriend
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private static readonly RNGCryptoServiceProvider Random = new RNGCryptoServiceProvider();
		protected internal CardCollection Collection;
		protected internal Card[] Cards;

		public MainWindow()
		{
			InitializeComponent();
			Cards = CardImporter.ImportCards();
			Collection = CollectionImporter.ImportCollection(Cards);
			
			OutputCardList(GetSealedList());
		}

		private void ButtonClicked(object sender, RoutedEventArgs e)
		{
			OutputCardList(GetSealedList());
		}

		private void OutputCardList(CardAmount[] sealedDeck)
		{
			sealedDeck = GetSealedList();

			StringBuilder deckExport = new StringBuilder();

			foreach (var cardAmount in sealedDeck)
			{
				deckExport.AppendLine(cardAmount.ToDeckImportFormat());
			}

			OutputTextBox.Text = deckExport.ToString();
			OutputTextBox.SelectionStart = 0;
			OutputTextBox.SelectionLength = OutputTextBox.Text.Length;
			OutputTextBox.Focus();
		}

		private CardAmount[] GetSealedList()
		{
			List<Card> cards = new List<Card>();

			while (cards.Count < 90)
			{
				var inputBytes = new byte[1024];
				Random.GetBytes(inputBytes);

				for (int i = 0; i < inputBytes.Length; i += 2)
				{
					int position = inputBytes[i] + (inputBytes[i + 1] << 8);

					if (position >= Cards.Length)
						continue;

					Card selectedCard = Cards[position];

					CardAmount collectionAmount = Collection.CollectedCards.FirstOrDefault(ca => ca.Card == selectedCard);

					if (collectionAmount == null)
						continue;

					if (cards.Count(c => c == selectedCard) >= collectionAmount.Amount)
						continue;

					cards.Add(selectedCard);

					if (cards.Count == 90)
						break;
				}
			}

			var sealedDeck =
				from card in cards
				group card by card
				into groupedCards
				select new CardAmount {Amount = groupedCards.Count(), Card = groupedCards.Key};
			return sealedDeck.ToArray();
		}
	}
}
