# User Login with Password

Once we have an instance of a user, and that instance has been created server-side, we can log in as that user. Responses to login will contain a session token if the credentials were valid. The `Task` will continue by setting the token and expiration values on the `CMUser` object passed into the login method.

{{note 'You do not need to specify both a username and an email address to log in, even if you specified both during account creation. Only one is required and the other may be set to `null`.' }}

This request clears the password field on the user regardless of whether or not login was successful. A new session token is generated every time a user logs in, and remains valid for up to six months of inactivity.

```csharp
// instantiate a CMUser instance with a username, email and password
CMUser<MyUserProfile> user = new CMUser<MyUserProfile> (
			null, /* you can also set this field to the username, and the email parameter to null */
			"test@cloudmine.me", 
			"testpass);
		
Task<CMUserResponse<MyUserProfile>> loginResponse = userService.Login<MyUserProfile> (user);
// Check user.LoggedIn for truth to confirm a successful login
```

In the above example the type of the derived profile object class (in this case, `MyUserProfile`) is passed as a type parameter to the method. This is done to help the response parse the proper type. If no type is present then the default `CMUser` object will be parsed and it will be up to the developer implementation to handle the user profile.