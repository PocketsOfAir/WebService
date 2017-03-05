using System;
using System.Net;
using System.Text;
using System.Threading;
using System.IO;

namespace WebService
{
	public class HttpListenerService
	{
		private HttpListener m_Listener;

		public bool running { get { return m_Listener != null && m_Listener.IsListening; } }

		public HttpListenerService(params string[] prefixes)
		{
			if (!HttpListener.IsSupported)
				throw new NotSupportedException("Windows XP SP2 or Server 2003 is required to use the HttpListener class.");

			if (prefixes == null || prefixes.Length == 0)
				throw new ArgumentException("No prefixes");

			m_Listener = new HttpListener();
			foreach (string s in prefixes)
				m_Listener.Prefixes.Add(s);
		}

		public void RunServer()
		{
			if (m_Listener.IsListening)
				throw new InvalidOperationException("Server is already running.");
			Thread listenerAsync = new Thread(Listen);
			listenerAsync.Start();
		}

		public void StopServer()
		{
			m_Listener.Stop();
			m_Listener.Close();
		}

		private void Listen()
		{
			m_Listener.Start();
			Console.WriteLine("Server Started...");
			while (m_Listener.IsListening)
			{
				HttpListenerContext context;
				//suppress errors caused by the service being stopped while we're waiting for a request
				try
				{
					context = m_Listener.GetContext();
				}
				catch (HttpListenerException)
				{
					continue;
				}

				if(context.Request.HttpMethod ==  null || context.Request.HttpMethod.ToUpper() != "POST")
				{
					using (StreamReader reader = new StreamReader(@"Resources\400BadRequest.html"))
					{
						ReturnBadRequestError(context, reader.ReadToEnd());
					}
					continue;
				}

				if (context.Request.InputStream == null)
				{
					using(StreamReader reader = new StreamReader(@"Resources\NoContentError.json"))
					{
						ReturnBadRequestError(context, reader.ReadToEnd());
					}
					continue;
				}

				string requestString, responseString;
				using (StreamReader reader = new StreamReader(context.Request.InputStream))
				{
					requestString = reader.ReadToEnd();
				}

				if (!JsonParser.TryParseRequest(requestString, out responseString))
				{
					ReturnBadRequestError(context, responseString);
					continue;
				}

				byte[] buffer = Encoding.UTF8.GetBytes(responseString);
				context.Response.ContentLength64 = buffer.Length;
				context.Response.OutputStream.Write(buffer, 0, buffer.Length);
				context.Response.OutputStream.Close();
			}
		}

		private void ReturnBadRequestError(HttpListenerContext context, string errorMessage)
		{
			context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
			byte[] buffer = Encoding.UTF8.GetBytes(errorMessage);
			context.Response.ContentLength64 = buffer.Length;
			context.Response.OutputStream.Write(buffer, 0, buffer.Length);
			context.Response.OutputStream.Close();
		}
	}
}
