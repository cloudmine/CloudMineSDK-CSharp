# User Logout
Once a user has a valid session token, it can be invalidated by logging the user out.

```csharp
// assume this user exists and has been logged in
Task<CMLogoutResponse> logoutResponse = userService.Logoff (user);
```
Calling the logout method has a `ContinueWith` that will automatically invalidate the session on the passed user object. 