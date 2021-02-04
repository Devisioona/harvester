using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devisioona.Harvest.CLI.Helpers
{
	public static class SelectionHelper
	{
		private static readonly string[] labels = new string[] {
			"1", "2","3", "4", "5", "6", "7", "8",
			"9", "A", "B","C", "D", "E", "F" };

		public static T Select<T> ( IList<T> selectables, Action<T> printer )
		{
			Console.WriteLine("Choose from list, or enter Q to abort and N to fetch next results");
			int selectableIndex = 0;
			PrintCurrentSelectables();

			while ( true )
			{
				Console.Write("> ");
				var input = Console.ReadLine().ToUpper();
				if ( input.Length == 1 && Array.IndexOf(labels, input) != -1 )
				{
					int userSelection = Array.IndexOf(labels, input.ToUpper());
					return selectables[selectableIndex + userSelection];
				}
				else if ( input == "Q") 
				{ 
					return default(T); 
				}
				else if ( input == "N")
				{
					selectableIndex += labels.Length;
					if ( selectableIndex >= selectables.Count )
					{
						Console.WriteLine("No more items to fetch. Restarting.");
						selectableIndex = 0;
					}
					PrintCurrentSelectables();
				}
				else if (input == "R")
				{
					selectableIndex = 0;
				}
				else
				{
					Console.WriteLine("Choose from list, or enter Q to abort and N to fetch next results");
				}
			}

			void PrintCurrentSelectables()
			{
				for (int i = 0; i < labels.Length; ++i)
				{
					if (i + selectableIndex >= selectables.Count) break;
					Console.Write($"{labels[i]}. ");
					printer(selectables[i + selectableIndex]);
				}
			}
		}
	}
}
