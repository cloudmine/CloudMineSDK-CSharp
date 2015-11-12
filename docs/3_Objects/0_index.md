# Application Data

Application objects can be seen and modified by anyone using the application, so long as the API key used to access the data has permissions to do so.

When it comes to modeling your data in CloudMine, you have two options.

The first option is strongly creating model classes that inherit from the base `CMObject` class. This is the recommended approach, as `CMObject` has a well-defined structure which makes it easier to work with as your app evolves over time. `CMObject`-derived classes also have the luxury of working with similarly defined objects across the other CloudMine offered SDKs.

The second option is to work with dynamic C# objects. This is nice for stubbing data for use in integration tests, testing functionality out, or rapidly prototyping a part of your app before you're certain of the exact data model.


## CMObject

Start by defining a class that extends from `CMObject`. The default empty constructor on the base class will automatically generate a new `Guid` for the key and automatically specify the correct value for the `__class__` field. This is used to deserialize over-the-wire objects into their proper type. **All derived objects should call the base constructor must call the base constructor such that the `CMObject` constructor is called last in the chain.**

CloudMine objects are deserialized to a specific class based on their `__class__` property. When serializing, `GetType().Name` is used as the default value.

### Overriding Property Names

If you are working with objects from a prior design, or that are consumed by other platforms and have different names, and wish to override how they are named when stored on CloudMine you can use [Newtonsoft JSON](http://www.newtonsoft.com/json/help/html/SerializationGuide.htm) attributes such as:

```csharp
[JsonProperty("loc", NullValueHandling=NullValueHandling.Ignore)]
public CMLocation Location { get; set; }
```
In this example, the `Location` property on this class will be serialized into a property named `loc` when saved to CloudMine. Similarly, the `loc` field will be mapped to `Location` locally when deserialized from CloudMine. See the documentation for [JsonProperty](http://www.newtonsoft.com/json/help/html/T_Newtonsoft_Json_JsonPropertyAttribute.htm) for more information.

### Example CMObject Subclass

Let's take a look at an example of a healthcare provider class definition:

```csharp
using System;
using Newtonsoft.Json;
using CloudmineSDK.Model;

namespace HealthcareApp
{
	[JsonObject(MemberSerialization.OptIn)]
	public class CareProvider: CMObject
	{
		[JsonProperty("ProviderName", NullValueHandling=NullValueHandling.Ignore)]
		public string ProviderName { get; set; }

		[JsonProperty("ProviderAddress", NullValueHandling=NullValueHandling.Ignore)]
		public string ProviderAddress { get; set; }

		[JsonProperty("ProviderEmployeeCount", NullValueHandling=NullValueHandling.Ignore)]
		public int ProviderEmployeeCount { get; set; }

		public CareProvider (): base()
		{
		}
	}
}

```

Notice that we've set the `MemberSerialization` of the `CareProvider ` class to `MemberSerialization.OptIn`. For more information on this, see the documentation for [JsonObject](http://www.newtonsoft.com/json/help/html/T_Newtonsoft_Json_JsonObjectAttribute.htm) and the [MemberSerialization](http://www.newtonsoft.com/json/help/html/T_Newtonsoft_Json_MemberSerialization.htm).
