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

			var cardRestictions = new Stack<CardRestiction>(settings.PerPickRestrictions);
			while (cards.Count < settings.Amount)
			{
				// Get next card restriction.
				var cardRestiction = cardRestictions.Count > 0 ? cardRestictions.Pop() : null;

				// Filter by current card restriction.
				var perCardFilteredCards = cardRestiction != null
					? baseFilteredCards.Where(c => cardRestiction(c)).ToList()
					: baseFilteredCards;

				// Check if any choices for valid cards are left.
				if (perCardFilteredCards.Count == 0)
					return new[] { new CardAmount { Amount = 1, Card = GetWhiskers() } };

				// Randomly choose next card.
				var selectedCard = perCardFilteredCards[random.Next(perCardFilteredCards.Count)];
				cards.Add(selectedCard);

				// Remove selected card from cardpool if maximum available amount has been added.
				if (cards.Count(c => c == selectedCard) ==
					collection.CollectedCards.First(c => c.Card == selectedCard).Amount)
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

		private static Card GetWhiskers()
		{
			return new Card
			{
				Cost = "G",
				Name = "Whiskers, Master of missing Cards",
				CardRarity = CardRarity.Mythic,
				CollectorNumber = "🐱",
				CardType = "Legendary Cat",
				CardTypes = new[] { CardType.Creature },
				CardArt = "403322",
				Set = "CAT",
				SubType = "Stray cat",
				Text = "Couldn't find enough cards with the currently active filters. Sorry, have this cat instead."
			};
		}
	}
}