namespace NekuSoul.PhantomTool.Data
{
	public class Card
	{
		public int Id;
		public string Name;
		public string Set;
		public string CollectorNumber;

		public override string ToString()
		{
			return $"{Id}: {Name} ({Set}) {CollectorNumber}";
		}
	}
}
