# Reset User Password
If the user does not know their current password, use `ResetPasswordRequest` instead. `ResetPasswordRequest` takes the email address of the user for whom you want to reset. 

```csharp
Task<CMResponse> changeResponse = 
	userService.ResetPasswordRequest (emailAddressString);
```

{{note 'A link to reset the password will be sent to the user email address. CloudMine also supports customization and self hosting of the reset form for proper branding.'}}

In some cases the reset password redirect could be to a CloudMine or self hosted site as well as a deep link into your app. If the token to reset a password is retrieved the `ResetPassword` method can be leveraged. 

```csharp
Task<CMResponse> changeResponse = 
	userService.ResetPassword ("fweaih...", "NEWPASSWORD")'
```