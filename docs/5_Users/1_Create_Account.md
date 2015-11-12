# Create User

{{note 'If you want to create a user account using an OAuth-based service such as Facebook, Twitter or Instagram you should head over to [User Login with Social Network](#/android_and_java#user-login-with-social-network) and ignore this section as it does not apply.'}}

You can create a new user by instantiating a new `CMUser` object with an optional `CMUserProfile` and calling `Create` on `CMUserService`. You can supply either a username or an email address, or both, but both are not required. If you don't need one or the other, just set the parameter value to `null`.

Creating a new instance of `CMUser` does **not** necessarily mean you are trying to create a new user account. For example, you must also do this in order to log in as an existing user. See the next section for more information.

The below example includes a custom user profile object as well. 

```csharp
CMUser<MyUserProfile> user = new CMUser<MyUserProfile> (
    "testusername", 
    "test2@cloudmine.me", 
    "testpassword", 
    new MyUserProfile () {
        FavoriteCafe = "CloudMine Coffee to Go"
    }
);

Task<CMUserResponse> userResponse = userService.Create (user);
```	  

### Custom User Profile

The above example includes a custom user profile object as well. `MyUserProfile` derives from `CMUserProfile` and is persisted as the profile of the user.
