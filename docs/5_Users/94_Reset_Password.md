# Reset User Password

If the user has forgotten their current password, use `ResetPasswordRequest` instead. `ResetPasswordRequest` takes the email address of the user for whom you want to reset. 

```csharp
Task<CMResponse> changeResponse = 
	userService.ResetPasswordRequest (emailAddressString);
```

A link to reset the password will be sent to the user email address. CloudMine also supports customization and self hosting of the reset form for proper branding. By default, this link will take the user to an auto-generated password reset page where they can enter their new password. However, you can also use your own custom web page or a deep link into the app to perform the reset yourself.

## Reset via Custom Web Page

If you choose to use a Hosted Site within CloudMine, or any other URL as the password reset link in the email body, you will need to retrieve the password reset token and send it via the CloudMine REST API in order to complete the reset operation. See the REST API or JavaScript SDK documentation for information on how to accomplish this in a web page.

## Reset via Deep Link into App

If you choose to use a deep link so the user can reset their password from within your app, then you can use the `ResetPassword` method in this SDK to perform the actual reset. Be sure you embed the reset token in your deep link so you can pass it to the method as shown below.

```csharp
Task<CMResponse> changeResponse = 
	userService.ResetPassword ("RESETPASSWORDTOKEN", "NEWPASSWORD")'
```