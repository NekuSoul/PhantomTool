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
			var cards = new List<Card>();
			var random = settings.Seed == null ? new Random() : new Random(settings.Seed.GetHashCode());
			var baseFilteredCards = (from c in collection.CollectedCards where c.Amount > 0 select c.Card).ToList();

			if (settings.Sets.Any())
				baseFilteredCards.RemoveAll(c => !settings.Sets.Contains(c.Set));

			baseFilteredCards = baseFilteredCards.ToList();

			while (cards.Count < settings.Amount)
			{
				if (baseFilteredCards.Count == 0)
					return new CardAmount[0];

				int position = random.Next(baseFilteredCards.Count);

				var selectedCard = baseFilteredCards[position];

				cards.Add(selectedCard);

				if (cards.Count(c => c == selectedCard) == collection.CollectedCards.First(c => c.Card == selectedCard).Amount)
					baseFilteredCards.Remove(selectedCard);
			}

			var sealedDeck =
				from card in cards
				group card by card
				into groupedCards
				orderby groupedCards.Key.ConvertedCost, groupedCards.Key.Name
				select new CardAmount { Amount = groupedCards.Count(), Card = groupedCards.Key };
			return sealedDeck.ToArray();
		}
	}
}