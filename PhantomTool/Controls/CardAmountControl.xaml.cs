using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using NekuSoul.PhantomTool.Data;

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

			string imagePath = GetImagePath();
			if (string.IsNullOrEmpty(imagePath))
				ThumbnailImage.Width = 0;
			else
				ThumbnailImage.Source = new BitmapImage(new Uri(GetImagePath(), UriKind.Absolute));

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

		private string GetImagePath()
		{
			string imagePath = Path.Combine(Helper.GetAppDataPath(), "CardArt", $"{CardAmount.Card.CardArt}_AIF.png");

			return File.Exists(imagePath) ? imagePath : string.Empty;
		}

		public void UpdateAmount()
		{
			AmountLabel.Content = $"{CardAmount.Amount}x";
		}

		private string GetCleanText(string text)
			=> Regex.Replace(text, @"{.+}", m => m.Value.Replace("o", string.Empty))
				.Replace("<i>", string.Empty)
				.Replace("</i>", string.Empty);
	}
}
