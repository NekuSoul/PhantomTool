using System.Linq;

namespace NekuSoul.PhantomTool.Data
{
	public class CardCollection
	{
		public CardAmount[] CollectedCards = new CardAmount[0];

		public CardCollection Intersect(CardCollection otherCollection)
		{
			var cardCollection = new CardCollection
			{
				CollectedCards = CollectedCards
					.Union(otherCollection.CollectedCards)
					.GroupBy(cardAmount => cardAmount.Card)
					.Where(group => group.Any())
					.Select(group => group.OrderBy(amount => amount.Amount).First())
					.ToArray()
			};

			return cardCollection;
		}
	}
}
