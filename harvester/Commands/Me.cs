using Newtonsoft.Json;
using System;
using System.CommandLine;
using System.CommandLine.Invocation;

namespace Devisioona.Harvest.CLI.Commands {
	public static class Me {
		public static Command GetCommand() {
			var cmd = new Command("me", "Get current user") {
			};

			cmd.Handler = CommandHandler.Create<bool>(Execute);

			return cmd;
		}

		private static void Execute(bool outputJson) {
			var client = Program.GetHarvestClient();
			var user = client.GetCurrentUser().Result;

			if (outputJson) {
				Console.WriteLine(JsonConvert.SerializeObject(user, Formatting.Indented));
			}
			else {
				Console.ForegroundColor = ConsoleColor.Gray;
				Console.Write($"Hello Harvest User: ");
				Console.ForegroundColor = ConsoleColor.White;
				Console.Write($"{user.FirstName} {user.LastName}");
				Console.ForegroundColor = ConsoleColor.Gray;
				Console.Write($" - ");
				Console.ForegroundColor = ConsoleColor.Cyan;
				Console.Write($"{user.Email}");
				Console.Write($" - ");
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.Write($"{user.Id}");
				Console.ResetColor();
				Console.WriteLine();

			}
		}

	}
}
