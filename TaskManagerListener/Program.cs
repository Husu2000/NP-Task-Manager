using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using TaskManagerListener;

var ip = IPAddress.Loopback;
var port = 27001;
var listener = new TcpListener(ip, port);
listener.Start();

while (true)
{
	var client = listener.AcceptTcpClient();

	var stream = client.GetStream();
	var br = new BinaryReader(stream);
	var bw = new BinaryWriter(stream);

	while (true)
	{
		var input = br.ReadString();
		var command = JsonSerializer.Deserialize<Command>(input);

		Console.WriteLine(command.Text);
		Console.WriteLine(command.Param);

		var text = command.Text.ToUpper();

		switch (text)
		{
			case Command.processList:
				var proceses = Process.GetProcesses();
				var processNames = JsonSerializer.Serialize
					(proceses.Select(p => p.ProcessName));
				bw.Write(processNames);
				break;
			case Command.run:
				Process.Start(command.Param);
				bw.Write("App started.");
				break;
			case Command.kill:
				var processes = Process.GetProcessesByName(command.Param);
				foreach (var proc in processes)
				{
					proc.Kill();
				}
				bw.Write($"App(s) killed.");
				break;
			default:
				break;
		}
	}

}