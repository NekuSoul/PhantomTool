using System;
using System.Collections.Generic;
using System.Linq;
using NekuSoul.PhantomTool.Data;

namespace NekuSoul.PhantomTool.Generator
{
	public static class SealedGenerator
	{
		public static CardAmount[] GetSealedList(GeneratorSettings settings, CardCollection collection)
		{
			List<Card> cards = new List<Card>();
			Random random = settings.Seed == null ? new Random() : new Random(settings.Seed.GetHashCode());
			Card[] filteredCards = GameData.Cards;

			if (settings.Sets.Any())
				filteredCards = filteredCards.Where(c => settings.Sets.Contains(c.Set)).ToArray();

			int mercyKill = 100000;

			while (cards.Count < settings.Amount)
			{
				if (mercyKill-- == 0)
					return new CardAmount[0];

				int position = random.Next(filteredCards.Length);

				if (position >= GameData.Cards.Length)
					continue;

				Card selectedCard = filteredCards[position];

				CardAmount collectionAmount = collection.CollectedCards.FirstOrDefault(ca => ca.Card == selectedCard);

				if (collectionAmount == null)
					continue;

				if (cards.Count(c => c == selectedCard) >= collectionAmount.Amount)
					continue;

				cards.Add(selectedCard);
			}

			var sealedDeck =
				from card in cards
				group card by card
				into groupedCards
				select new CardAmount { Amount = groupedCards.Count(), Card = groupedCards.Key };
			return sealedDeck.ToArray();
		}
	}
}
