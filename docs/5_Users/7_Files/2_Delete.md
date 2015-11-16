# Delete User File

Deleting user files is done the same way as deleting an object given the key of the file to remove and the user object with a valid session. 

## Deleting a file

Calling the `DeleteUserObject` method takes the key of the file as a parameter to be deleted as well as the user object with a valid session.

```csharp
Task<CMObjectResponse> delResponse =
	userService.DeleteUserObject ("fe232f...", user);
```

## Deleting multiple files

Deleting multiple files is done in the same way except the `DeleteUserObjects` method is used with an array or list of IDs to be deleted along with the user object with a valid session.

```csharp
Task<CMObjectResponse> delResponse =
	userService.DeleteUserObjects ( 
		new string[] {"fe232f...", "hoi78..."}, user);
```

Much like other functions in the SDK, if an ID is not present it will be in the `Errors` dictionary with a reason why deleting wasn't possible.