# Update User Profiles

When using a custom user profile class, fields can be added which will be persisted as that user's "profile". The main purpose of this profile is discoverability. Using this profile, users of your app can find other users to share their data with.

It's important to remember that profile information is public. For this reason, by default, we don't save any fields on the user's profile except for their unique identifier.

To add fields to a user's profile, first create a custom user profile class.

```csharp
[JsonObject(MemberSerialization.OptIn)]
public class CMUserProfileMock : CMUserProfile
{
	[JsonProperty("favorite_cafe")]
	public string FavoriteCafe { get; set; }

	public CMUserProfileMock () { }
}

// an instantiation of a user with custom profile
CMUser<CMUserProfileMock> user = 
	new CMUser<CMUserProfileMock> (
		"test", 
		"test@cloudmine.me", 
		"testpass", 
		new CMUserProfileMock () {
			FavoriteCafe = "CloudMine Coffee to Go"
	});
```

To replace the user profile with the current state use the `UpdateUserProfile` method which takes an instance of the `CMUser` with a `CMUserprofile` type parameter.

```csharp
// assuming the user object instantiated in the prior code block
userService.UpdateUserProfile<CMUserProfileMock>(user);
```

To merge the user profile at the server with the current state use the `MergeUserProfile` method which takes an instance of the `CMUser` with a `CMUserprofile` type parameter.

```csharp
// assuming the user object instantiated in the prior code block
userService.MergeUserProfile<CMUserProfileMock>(user);
```



