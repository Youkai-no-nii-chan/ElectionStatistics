using System;
using System.Linq;

namespace ElectionStatistics.ManagementConsole
{
	class Program
	{
		private static readonly Command[] commands = typeof(Command).Assembly
			.GetTypes()
			.Where(type => typeof(Command).IsAssignableFrom(type))
			.Where(type => !type.IsAbstract)
			.Select(type => (Command)Activator.CreateInstance(type))
			.ToArray();

		static void Main(string[] args)
		{
			try
			{
				if (args.Length < 1)
				{
					Console.WriteLine("Введите имя команды.");
					WriteHelp();
				}
				else
				{
					var command = commands.SingleOrDefault(c => c.Name.Equals(args[0], StringComparison.OrdinalIgnoreCase));
					if (command == null)
					{
						Console.WriteLine("Команда с именем {0} не найдена.", args[0]);
						WriteHelp();
					}
					else
					{
						command.Execute(args.Skip(1).ToArray());
						Console.WriteLine("Команда выполнена успешно.");
					}
				}
			}
			catch (Exception exception)
			{
				Console.WriteLine(exception);
			}
			Console.WriteLine("Нажмите любую клавишу для выхода...");
			Console.ReadKey();
		}

		private static void WriteHelp()
		{
			Console.WriteLine("Известные имена команд:");
			foreach (var command in commands)
			{
				Console.WriteLine(command.Name);
			}
		}
	}
}
