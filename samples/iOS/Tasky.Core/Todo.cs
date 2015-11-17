using System;
using CloudmineSDK.Model;
using Newtonsoft.Json;

namespace Tasky.Core {
	/// <summary>
	/// Task business object
	/// </summary>
	public class Todo : CMObject {
		public Todo ()
		{
		}

		[JsonProperty("Name")]
		public string Name { get; set; }
		[JsonProperty("Notes")]
		public string Notes { get; set; }
		[JsonProperty("Done")]
		public bool Done { get; set; }	// TODO: add this field to the user-interface
	}
}