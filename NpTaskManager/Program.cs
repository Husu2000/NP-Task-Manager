using NpTaskManager;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;

var ip = IPAddress.Loopback;
var port = 27001;
var client = new TcpClient();
client.Connect(ip, port);

var stream = client.GetStream();
var br = new BinaryReader(stream);
var bw = new BinaryWriter(stream);

Command command = new Command();
string responce = null;
string str = null;

while (true)
{
	Console.WriteLine("Write any command or HELP");
	Console.Write("enter cmd: ");
	str = Console.ReadLine();

	var input = str.Split(' ');

	if (input[0].ToUpper() == "HELP")
	{
		Console.WriteLine();
		Console.WriteLine("Command List:");
		Console.WriteLine(Command.processList);
		Console.WriteLine($"\t\t\t{Command.run} <process_name>");
		Console.WriteLine($"\t\t\t{Command.kill} <process_name>");
		Console.WriteLine($"\t\t\tHELP");
		Console.WriteLine($"\t\t\tCLS");
		Console.ReadLine();
		Console.Clear();
	}
	else if (input[0].ToUpper() == "CLS")
	{
		Console.Clear();
	}

	var sCase = input[0].ToUpper();

	switch (sCase)
	{
		case Command.processList:
			command = new Command { Text = input[0] };
			bw.Write(JsonSerializer.Serialize(command));

			responce = br.ReadString();

			var processList = JsonSerializer.Deserialize<string[]>(responce);

			foreach (var proc in processList)
			{
				Console.WriteLine($"\t\t\t{proc}");
			}
			break;
		case Command.run:
			command = new Command { Text = input[0], Param = input[1] };
			bw.Write(JsonSerializer.Serialize(command));
			responce = br.ReadString();
			Console.WriteLine($"\t\t\t{responce}");
			break;
		case Command.kill:
			command = new Command { Text = input[0], Param = input[1] };
			bw.Write(JsonSerializer.Serialize(command));

			responce = br.ReadString();
			Console.WriteLine($"\t\t\t{responce}");
			break;
		default:
			break;
	}
}
