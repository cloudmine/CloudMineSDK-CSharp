# USER DATA

Up until this point, we have been focusing on Application level objects. Application objects can be seen and modified by anyone using the application. CloudMine also supports the concept of users, and user level objects. User level objects can only be accessed by a logged in user, and typically only by the user which created the object. The exception to this is a user may explicitly grant specific users access to their data; this is covered in more detail below. For now, lets just start with what users are and how to work with them.

A user consists of a method of logging in and an optional profile. There are two ways for a user to log in; either with an e-mail or a username and password, or through a social service, such as Facebook or Twitter. The former requires that the user be created before they are allowed to log in, while the latter will create the `CMUser` upon log in if the account does not already exist. A `CMUser` works just like a `CMObject` when it comes to serialization, so you may also extend the user profile `CMUserProfile` on the `CMUser` object. This allows you to add additional information, such as the user's birthday or favorite color. The difference between a `CMUser` and a `CMObject` is `CMCredentials` object which include email, username, and password fields for a `CMUser`. 

{{caution "Note that user ID is populated after either the user is created or the user logs in."}}

By default, user objects do not have any profile information, so we'll create a new class that does have profile information.

```csharp
[JsonObject(MemberSerialization.OptIn)]
	public class CMUserProfileMock : CMUserProfile
	{
		[JsonProperty("favorite_cafe")]
		public string FavoriteCafe { get; set; }

		public CMUserProfileMock () {}
	}
```

Notice that just like CMObject, we can define which fields to opt in and what those fields will be represented like at the server. Of course you could just label the `JsonProperty` without overriding the name of the member stored at the server. In this case, the name of the field "FavoriteCafe" would be the key value rendered at the server.
