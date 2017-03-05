using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace WebService
{
	public static class JsonParser
	{
		public static bool TryParseRequest(string input, out string output)
		{
			JObject parsedInput;
			try
			{
				parsedInput = JObject.Parse(input);
			}
			catch (JsonReaderException)
			{
				output = File.ReadAllText(@"Resources\FailedParseError.json");
				return false;
			}

			IList<JToken> entries = parsedInput["payload"].Children().ToList();
			ResponseContainer responses = new ResponseContainer();
			foreach(JToken entry in entries)
			{
				PayloadStructure p = JsonConvert.DeserializeObject<PayloadStructure>(entry.ToString());

				if(p.drm && p.episodeCount > 0)
					responses.response.Add(new ResponseStructure(p));
			}

			JsonSerializerSettings settings = new JsonSerializerSettings();
			settings.Formatting = Formatting.Indented;
			output = JsonConvert.SerializeObject(responses, settings);

			return true;
		}
	}
}