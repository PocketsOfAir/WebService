using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebService
{
	public class PayloadStructure
	{
		public PayloadStructure()
		{
			image = new ImageStructure();
		}

		public ImageStructure image { get; set; }
		public string slug { get; set; }
		public string title { get; set; }
		public bool drm { get; set; }
		public float episodeCount { get; set; }
	}

	public class ImageStructure
	{
		public string showImage { get; set; }
	}

	public class ResponseContainer
	{
		public ResponseContainer()
		{
			response = new List<ResponseStructure>();
		}

		public List<ResponseStructure> response { get; set; }
	}

	public class ResponseStructure
	{
		public string image { get; set; }
		public string slug { get; set; }
		public string title { get; set; }

		public ResponseStructure(PayloadStructure source)
		{
			image = source.image.showImage;
			slug = source.slug;
			title = source.title;
		}
	}
}
