using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System;
using Newtonsoft.Json;

namespace WebService.Tests
{
	[TestClass]
	public class JsonParserTests
	{
		[TestMethod]
		public void CheckStructureConversion()
		{
			PayloadStructure payload = new PayloadStructure();
			payload.drm = true;
			payload.episodeCount = 5;
			payload.image.showImage = "some/image/url";
			payload.slug = "some/url";
			payload.title = "Show Title";

			ResponseContainer container = new ResponseContainer();
			container.response.Add(new ResponseStructure(payload));
			Assert.AreEqual(container.response.Count, 1);
			Assert.AreEqual(payload.image.showImage, container.response[0].image);
			Assert.AreEqual(payload.slug, container.response[0].slug);
			Assert.AreEqual(payload.title, container.response[0].title);

			try
			{
				payload.image = null;
				container.response.Add(new ResponseStructure(payload));
			}
			catch (NullReferenceException)
			{
				Assert.AreEqual(container.response.Count, 1);
			}
		}

		[TestMethod]
		public void CheckEmptyStringFailsToParse()
		{
			string output;
			Assert.IsFalse(JsonParser.TryParseRequest("", out output));

			StreamReader reader = new StreamReader(@"Resources\FailedParseError.json");
			ErrorStructure expectedError = JsonConvert.DeserializeObject<ErrorStructure>(reader.ReadToEnd());
			reader.Close();
			ErrorStructure receivedError = JsonConvert.DeserializeObject<ErrorStructure>(output);

			Assert.AreEqual(expectedError.error, receivedError.error);
		}

		[TestMethod]
		public void CheckExampleJsonParses()
		{
			string input, output;

			StreamReader reader = new StreamReader(@"Tests\TestData.json");
			input = reader.ReadToEnd();
			reader.Close();

			Assert.IsTrue(JsonParser.TryParseRequest(input, out output));

			reader = new StreamReader(@"Tests\ExpectedResponse.json");
			ResponseContainer expectedResponse = JsonConvert.DeserializeObject<ResponseContainer>(reader.ReadToEnd());
			reader.Close();
			ResponseContainer receivedResponse = JsonConvert.DeserializeObject<ResponseContainer>(output);

			Assert.IsTrue(receivedResponse.response.Count == expectedResponse.response.Count);
			for (int i = 0; i < receivedResponse.response.Count; i++)
			{
				Assert.AreEqual(receivedResponse.response[i].image, expectedResponse.response[i].image);
				Assert.AreEqual(receivedResponse.response[i].slug, expectedResponse.response[i].slug);
				Assert.AreEqual(receivedResponse.response[i].title, expectedResponse.response[i].title);
			}
		}

		[TestMethod]
		public void CheckBrokenJsonFails()
		{
			string input, output;
			StreamReader reader = new StreamReader("Tests/BrokenData.json");
			input = reader.ReadToEnd();
			reader.Close();

			Assert.IsFalse(JsonParser.TryParseRequest(input, out output));

			reader = new StreamReader(@"Resources\FailedParseError.json");
			ErrorStructure expectedError = JsonConvert.DeserializeObject<ErrorStructure>(reader.ReadToEnd());
			reader.Close();
			ErrorStructure receivedError = JsonConvert.DeserializeObject<ErrorStructure>(output);

			Assert.AreEqual(expectedError.error, receivedError.error);
		}
	}
}
