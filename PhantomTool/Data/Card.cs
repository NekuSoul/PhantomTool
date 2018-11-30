﻿using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace NekuSoul.PhantomTool.Data
{
	public class Card
	{
		public int Id;
		public string Name;
		public string Set;
		public string CollectorNumber;
		public string SubType;
		public string CardType;
		public string Text;
		public string Cost;
		public int ConvertedCost;

		public override string ToString()
			=> $"{Id}: {Name} ({Set}) {CollectorNumber}";

		public string GetDescription()
		{
			StringBuilder description = new StringBuilder();

			description.AppendLine($"{Name} - {Cost.Replace("o", string.Empty)}");

			description.Append(CardType);

			if (SubType != null)
				description.Append($" - {SubType}");

			if (Text != null)
			{
				description.AppendLine();
				description.AppendLine();

				int length = 0;
				foreach (string word in GetCleanText().Split(' '))
				{
					if (length + word.Length > 40)
					{
						description.AppendLine();
						length = 0;
					}

					description.Append(word);
					description.Append(' ');

					length += word.Length;
				}
			}

			return description.ToString();
		}

		private string GetCleanText()
			=> Regex.Replace(Text, @"{.+}", m => m.Value.Replace("o", string.Empty))
				.Replace("<i>", string.Empty)
				.Replace("</i>", string.Empty);
	}
}