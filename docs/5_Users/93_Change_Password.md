# Change User Password

If a user wishes to change their password and still know their current password, you can change it by using the `ChangePassword` method on the CMUserService.

```csharp
// assumes the user is instantiated with a valid session token
Task<CMResponse> changeResponse = userService.ChangePassword (user, "NEWPASSWORD");
```
