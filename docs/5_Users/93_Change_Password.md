# Change User Password

If a user wishes to change their password and still knows their current password, you can change it by using the `ChangePassword` method on the `CMUserService`. To handle the case where the user has forgotten their password, see the Reset Password section below.

```csharp
// assumes the user is instantiated with a valid session token
Task<CMResponse> changeResponse = userService.ChangePassword (user, "OLDPASSWORD", "NEWPASSWORD");
```
