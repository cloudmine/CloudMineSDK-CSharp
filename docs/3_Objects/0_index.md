# APPLICATION DATA

When it comes to modeling your data in CloudMine, you have two options. First is strongly creating you model classes which inherit from the base CMObject class. This is recommended as CMObject has alot of the makings for a complete and manageable model at the server. CMObject derived classes also have the luxury of working with similarly defined objects across the other CloudMine offered SDKs. It is also possible to work with dynamic C# objects which is nice for stubbing data for use in integration tests or just testing functionality out. 


### CMObject

This approach is more powerful and saves you from having to add and extract fields manually. You start by defining a class that extends from CMObject. The default empty constructor on the base class will automatically generate a new Guid for the key and specify the type (__class__). Derived objects should call the base constructor or implement one as deemed appropriate.

CloudMine objects are deserialized to a specific class based on their __class__ property. The C# SDK uses `GetType().Name` as the default. 

If you are working with objects from a prior design and wish to rename them you can specify how a property or class should be serialized and deserialized with Newtonsoft JSON attributes such as:

```csharp
[JsonProperty("loc", NullValueHandling=NullValueHandling.Ignore)]
public CMLocation Location { get; set; }
```
The above example illustrates how the Location property on an object will get mapped to and from '__loc__' on serialization and deserialization.

Below is an example of a healthcare provider class definition: 

```csharp
using System;
using Newtonsoft.Json;
using CloudmineSDK.Model;

namespace HealthcareApp
{
	[JsonObject(MemberSerialization.OptIn)]
	public class HCPMock: CMObject
	{
		[JsonProperty("ProviderName", NullValueHandling=NullValueHandling.Ignore)]
		public string ProviderName { get; set; }

		[JsonProperty("ProviderAddress", NullValueHandling=NullValueHandling.Ignore)]
		public string ProviderAddress { get; set; }

		[JsonProperty("ProviderEmployeeCount", NullValueHandling=NullValueHandling.Ignore)]
		public int ProviderEmployeeCount { get; set; }

		public HCPMock (): base()
		{
		}
	}
}

```

Now that you know how to make your models, let's look at doing something with them.
