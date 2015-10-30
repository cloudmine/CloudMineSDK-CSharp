using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace CloudmineSDK.Model
{
	/// <summary>
	/// CMObject is the base class for CloudMine objects to be synced to and from the
	/// server. Derives from the CMPropertyChange class for tracking fields which have
	/// changed. One serializing to update the server a client can send the CMObject having
	/// only serialized the properties which have changed which effectively ensures
	/// simultaneous changes on other clients haven't been lost.
	/// </summary>
	[JsonObject(MemberSerialization.OptIn)]
	public abstract class CMObject : INotifyPropertyChanged
	{
		/// <summary>
		/// Unique identifier for indexing and fast reference from the server.
		/// </summary>
		[JsonProperty("__id__")]
		public string ID { get; set; }
		/// <summary>
		/// Collection name of the object. Defaults to the name of the class implementing
		/// CMObject. 
		/// </summary>
		[JsonProperty("__class__")] // default to reflection on type
		public string Class { get; set; } // public to allow for custom definition
		/// <summary>
		/// List of access list ID's for object sharing. Will ignore serialization if not
		/// instantiated and sending to the server. Not applicable for Application level
		/// objects. 
		/// </summary>
		[JsonProperty("__access__", NullValueHandling=NullValueHandling.Ignore)]
		public string[] AccessListIDs { get; set; }

		#region Property Change Tracking Members
		// Flags the class to track or not track changes
		private bool trackChanges = false;

		// Changes to the object
		public Dictionary<string, object> Changes { get; private set; }

		// A check for object changes which exist beyond the creation of the object
		public bool IsObjectDirty
		{
			get { return Changes.Count > 0; }
			set { ; }
		}

		// Event required for INotifyPropertyChanged
		public event PropertyChangedEventHandler PropertyChanged;

		// TODO: Needs a ToUpdateJson extension method to only serialize changed properties on the object
		#endregion

		public CMObject()
		{
			ID = Guid.NewGuid().ToString();//.Remove('-'); // default id construction
			Class = this.GetType().Name; // default type construction

			// Change tracking is enabled by default default
			trackChanges = true;
			Changes = new Dictionary<string, object>();
		}

		public CMObject(string id, string type)
		{
			ID = id;
			Class = type;

			// Change tracking is enabled by default default
			trackChanges = true;
			Changes = new Dictionary<string, object>();
		}

		#region Property Change Tracking
		// Clear change history
		public void ClearChanges()
		{
			Changes.Clear();
		}

		// Begin/Resume tracking changes
		public void StartChangeTracking()
		{
			trackChanges = true;
		}


		// Stop tracking changes
		public void StopChangeTracking()
		{
			trackChanges = false;
		}
		/// <summary>
		/// Change the property if required and throw event. Change will be 
		/// added to to the underlying property tracking data structure. 
		/// Properties intended to fire change events should add this to the
		/// property setter and not expose the underlying property as public 
		/// to prevent missing change history.
		/// </summary>
		/// <param name="variable"></param>
		/// <param name="property"></param>
		/// <param name="value"></param>
		public void ApplyPropertyChange<T, F>(ref F field,
					Expression<Func<T, object>> property, F value)
		{
			// Only do this if the value changes
			if (field == null || !field.Equals(value))
			{
				// Get the property
				var propertyExpression = GetMemberExpression(property);
				if (propertyExpression == null)
					throw new InvalidOperationException("You must specify a property");
				// Property name
				string propertyName = propertyExpression.Member.Name;
				// Set the value
				field = value;
				// If change tracking is enabled, we can track the changes...
				if (trackChanges)
				{
					Changes[propertyName] = value;
					NotifyPropertyChanged(propertyName);
				}
			}
		}

		/// <summary>
		/// Get member expression
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="expression"></param>
		/// <returns></returns>
		public MemberExpression GetMemberExpression<T>(Expression<Func<T,
								object>> expression)
		{
			// Default expression
			MemberExpression memberExpression = null;
			// Convert
			if (expression.Body.NodeType == ExpressionType.Convert)
			{
				var body = (UnaryExpression)expression.Body;
				memberExpression = body.Operand as MemberExpression;
			}
			// Member access
			else if (expression.Body.NodeType == ExpressionType.MemberAccess)
			{
				memberExpression = expression.Body as MemberExpression;
			}
			// Not a member access
			if (memberExpression == null)
				throw new ArgumentException("Not a member access",
											"expression");
			// Return the member expression
			return memberExpression;
		}

		/// <summary>
		/// Raise property change event
		/// </summary>
		/// <param name="propertyName"></param>
		private void NotifyPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

		public string ChangesToXml()
		{
			// Prepare base objects
			//XDeclaration declaration = new XDeclaration("1.0",
			//							   Encoding.UTF8.HeaderName, String.Empty);
			//XElement root = new XElement("Changes");
			//// Create document
			//XDocument document = new XDocument(declaration, root);
			//// Add changes to the document
			//// TODO: If it's an object, maybe do some other things
			//foreach (KeyValuePair<string, object> change in Changes)
			//	root.Add(new XElement(change.Key, change.Value));
			//// Get the XML
			//return document.Document.ToString();
			return string.Empty;
		}
		#endregion
	}
}
