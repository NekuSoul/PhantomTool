using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhantomFriend.Data
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
