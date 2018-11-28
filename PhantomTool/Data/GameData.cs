using System.Linq;
using NekuSoul.PhantomTool.Importer;

namespace NekuSoul.PhantomTool.Data
{
	public static class GameData
	{
		public static Card[] Cards { get; }
		public static string[] Sets { get; }
		public static string[] PlayableSets { get; }

		static GameData()
		{
			Cards = DataImporter.ImportCards();
			Sets = (from c in Cards select c.Set).Distinct().OrderBy(s => s).ToArray();
			PlayableSets = Sets.Except(new[] {"ANA", "ArenaSUP"}).ToArray();
		}
	}
}
