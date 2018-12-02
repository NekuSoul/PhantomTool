using System;
using NekuSoul.PhantomTool.Data;

namespace NekuSoul.PhantomTool.Generator
{
	public delegate bool CardRestiction(Card card);

	public class GeneratorSettings
	{
		public string Seed;
		public int Amount;
		public string[] Sets = new string[0];
		public CardRestiction[] PerPickRestrictions = new CardRestiction[0];
	}
}
