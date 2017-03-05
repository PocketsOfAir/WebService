using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace WebService.Tests
{
	[TestClass]
	public class JsonParserTests
	{
		[TestMethod]
		public void CheckEmptyStringFailsToParse()
		{
			string output;
			Assert.IsTrue(!JsonParser.TryParseRequest("", out output));
		}

		[TestMethod]
		public void CheckExampleJsonParses()
		{
			string input, output;
			StreamReader file = new StreamReader("Tests/TestData1.json");
			input = file.ReadToEnd();
			Assert.IsTrue(JsonParser.TryParseRequest(input, out output));
			File.WriteAllText("output.json", output);
		}

		[TestMethod]
		public void CheckBrokenJsonFails()
		{
			string input, output;
			StreamReader file = new StreamReader("Tests/BrokenData1.json");
			input = file.ReadToEnd();
			Assert.IsTrue(!JsonParser.TryParseRequest(input, out output));
		}
	}
}
