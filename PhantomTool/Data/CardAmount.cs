namespace NekuSoul.PhantomTool.Data
{
	public class CardAmount
	{
		public Card Card;
		public int Amount;

		public string ToDeckImportFormat() 
			=> $"{Amount} {Card.Name} ({Card.Set}) {Card.CollectorNumber}";

		public override string ToString()
			=> $"[{Amount}] {Card.Id}: {Card.Name} ({Card.Set}) {Card.CollectorNumber}";
	}
}
