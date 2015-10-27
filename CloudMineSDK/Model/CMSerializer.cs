using Newtonsoft.Json;
using System.IO;
using System.Text;

namespace CloudmineSDK.Model
{
	public class CMSerializer
	{
		private CMSerializer()
		{
		}

		public static T To<T>(string data, Encoding enc = null)
		{
			if (enc == null) enc = Encoding.UTF8;
			return To<T>(new MemoryStream(enc.GetBytes(data)));
		}

		public static T To<T>(Stream data)
		{
			using (JsonTextReader reader = new JsonTextReader(new StreamReader(data)))
			{
				JsonSerializer serilizer = new JsonSerializer();
				return serilizer.Deserialize<T>(reader);
			}
		}

		public static Stream ToStream(object o, Stream stream = null)
		{
			if (stream == null) stream = new MemoryStream();
			using (JsonWriter writer = new JsonTextWriter(new StreamWriter(stream)))
			{
				JsonSerializer serilizer = new JsonSerializer();
				serilizer.Serialize(writer, o);
			};

			return stream;
		}

		public static string ToString(object o, Stream stream = null)
		{
			if (stream == null) stream = new MemoryStream();

			using (JsonWriter writer = new JsonTextWriter(new StreamWriter(stream)))
			{
				JsonSerializer serilizer = new JsonSerializer();
				serilizer.Serialize(writer, o);
				stream.Seek(0, SeekOrigin.Begin);
				return new StreamReader(stream).ReadToEnd();
			}
		}
	}
}
