# User Login with Password

Once we have an instance of a user, and that instance has been created server-side, we can log in as that user. Responses to login will contain a session token value if the credentials were valid. The `Task` will continue by setting the token and expiration values on the `CMUser` object passed into the login method.

This request clears the password field on the user regardless of whether or not login was successful. A new session token is generated every time a user logs in, and remains valid for up to six months of inactivity.

```csharp
// instantiate a CMUser instance with an email and password (presumably from the UI)
CMUser<CMUserProfileMock> user = new CMUser<CMUserProfileMock> (
			"test", 
			"test@cloudmine.me", 
			"testpass", 
			new CMUserProfileMock () {
				FavoriteCafe = "CloudMine Coffee to Go"
		});
		
Task<CMUserResponse<CMUserProfileMock>> loginResponse = userService.Login<CMUserProfileMock> (user);
```

In the above example the type of the derived profile object is passed as a type parameter to the method. This is done to help the response parse the proper type. If no type is present then the default `CMUser` object will be parsed and it will be up to the developer implementation to handle the user profile.