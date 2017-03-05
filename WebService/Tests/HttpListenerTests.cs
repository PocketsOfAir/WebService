using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WebService.Tests
{
	[TestClass]
	public class HttpListenerTests
	{
		[TestMethod]
		public void CheckParameterlessListenerFails()
		{
			try
			{
				HttpListenerService service = new HttpListenerService();
			}
			catch (ArgumentException e)
			{
				Assert.AreEqual(e.Message, "No prefixes");
			}
		}

		[TestMethod]
		public void CheckListenerStates()
		{
			HttpListenerService listener = new HttpListenerService("http://localhost:8080/");
			try
			{
				Assert.IsFalse(listener.isListening);
				listener.RunServer();
				Assert.IsTrue(listener.isListening);
				listener.RunServer();
			}
			catch(InvalidOperationException e)
			{
				Assert.AreEqual(e.Message, "Server is already running.");
			}
			finally
			{
				listener.StopServer();
				Assert.IsFalse(listener.isListening);
			}
		}
	}
}
