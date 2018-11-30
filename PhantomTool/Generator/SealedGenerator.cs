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

			// Start the pool with all collected cards.
			var baseFilteredCards = (from c in collection.CollectedCards where c.Amount > 0 select c.Card).ToList();

			// Filter by selected sets, if any sets are selected.
			if (settings.Sets.Any())
				baseFilteredCards.RemoveAll(c => !settings.Sets.Contains(c.Set));

			baseFilteredCards = baseFilteredCards.ToList();

			while (cards.Count < settings.Amount)
			{
				// Check if any choices for valid cards are left.
				if (baseFilteredCards.Count == 0)
					return new CardAmount[0];

				// Randomly choose next card.
				var selectedCard = baseFilteredCards[random.Next(baseFilteredCards.Count)];
				cards.Add(selectedCard);

				// Remove selected card from cardpool if maximum available amount has been added.
				if (cards.Count(c => c == selectedCard) == collection.CollectedCards.First(c => c.Card == selectedCard).Amount)
					baseFilteredCards.Remove(selectedCard);
			}

			// Group cards and return result.
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