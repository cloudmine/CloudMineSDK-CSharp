# User Data

Up until this point, we have been focusing on application-level objects. These objects can be seen and modified by anyone using the application, so long as the API key used to access the data has permissions to do so. CloudMine also supports the concept of users and user-level objects. Unlike application-level objects, access to user-level objects are governed not by API keys but by user accounts and access control lists. ACLs are covered later in this section.

A user consists of a profile and an authentication mechansim. Currently the two supported authentication mechanisms are: local username and password, and OAuth v2 to an external service (e.g. Facebook or Twitter). The former requires that the user be created before they are allowed to log in, while the latter will create the `CMUser` upon log in if the account does not already exist. A `CMUser` works just like a `CMObject` when it comes to serialization, so you may also extend the user profile `CMUserProfile` on the `CMUser` object. This allows you to add additional information, such as the user's birthday or favorite color. The difference between a `CMUser` and a `CMObject` is `CMCredentials` object which include email, username, and password fields for a `CMUser`. 

{{caution "Note that user ID is populated after either the user is created or the user logs in, and you should not attempt ot set it yourself"}}

By default, user objects do not have any profile information, so we'll create a new class that does have profile information.

```csharp
[JsonObject(MemberSerialization.OptIn)]
public class MyUserProfile : CMUserProfile
{
	[JsonProperty("favorite_cafe")]
	public string FavoriteCafe { get; set; }

	public MyUserProfile () {}
}
```

Notice that just like `CMObject`, we can define which fields to opt in and what those fields will be represented like at the server. Of course you could just label the `JsonProperty` without overriding the name of the member stored at the server. In this case, the exact name of the field `FavoriteCafe` would be the key value rendered at the server.
