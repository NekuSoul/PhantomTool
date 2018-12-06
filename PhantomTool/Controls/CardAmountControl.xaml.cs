using System.Threading.Tasks;
using System.Windows.Media;
using NekuSoul.PhantomTool.Data;
using NekuSoul.PhantomTool.Importer;

namespace NekuSoul.PhantomTool.Controls
{
	/// <summary>
	/// Interaction logic for CardAmountControl.xaml
	/// </summary>
	public partial class CardAmountControl
	{
		public CardAmount CardAmount { get; }

		public CardAmountControl(CardAmount cardAmount)
		{
			InitializeComponent();

			CardAmount = cardAmount;
			NameLabel.Text = CardAmount.Card.Name;
			ToolTip = CardAmount.Card.GetDescription();

			Task.Run(() => ApplyCardArt());

			switch (CardAmount.Card.CardRarity)
			{
				case CardRarity.Common:
					RarityLabel.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
					break;
				case CardRarity.Uncommon:
					RarityLabel.Background = new SolidColorBrush(Color.FromRgb(185, 185, 185));
					break;
				case CardRarity.Rare:
					RarityLabel.Background = new SolidColorBrush(Color.FromRgb(204, 153, 0));
					break;
				case CardRarity.Mythic:
					RarityLabel.Background = new SolidColorBrush(Color.FromRgb(255, 102, 0));
					break;
			}

			CostLabel.Content = CardAmount.Card.Cost.Replace("o", string.Empty);

			UpdateAmount();
		}

		public void UpdateAmount()
		{
			AmountLabel.Content = $"{CardAmount.Amount}x";
		}

		public void ApplyCardArt()
		{
			var cardArt = CardArtImporter.GetCardArt(CardAmount.Card);
			Dispatcher.Invoke(() => ThumbnailImage.Source = cardArt.ToBitmapImage());
		}
	}
}
