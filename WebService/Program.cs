using System;

namespace WebService
{
	class Program
	{
		static void Main(string[] args)
		{
			HttpListenerService server = new HttpListenerService("http://webservice-44.apphb.com/");
			server.RunServer();
			Console.WriteLine("Server running. Press 'Q' to quit.");
			while (true)
			{
				if (Console.ReadKey().KeyChar == 'q')
					break;
			}
			server.StopServer();
		}
	}
}
