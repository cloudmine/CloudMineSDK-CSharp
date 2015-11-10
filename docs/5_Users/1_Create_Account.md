# Create User

{{note 'If you want to create a user account using a social network such as Facebook, Twitter or Instagram you should head over to [User Login with Social Network](#/android_and_java#user-login-with-social-network) and ignore this section as it does not apply.'}}

You can create a new user by instantiating a new `CMUser` object with an optional `CMUserprofile` and calling `Create` on the `CMUserService`.

The below example includes a custom user profile object as well. 

```csharp
CMUser<CMUserProfileMock> user = new CMUser<CMUserProfileMock> (
			"test2", 
			"test2@cloudmine.me", 
			"testpass", 
			new CMUserProfileMock () {
				FavoriteCafe = "CloudMine Coffee to Go"
		});

Task<CMUserResponse> userResponse = userService.Create (user);
```	  

### Custom user classes

The above example includes a custom user profile object as well. `CMUserProfileMock` derives from `CMUserProfile` and is persisted as the profile of the user.
