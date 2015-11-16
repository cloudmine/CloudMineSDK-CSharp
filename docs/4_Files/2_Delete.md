# Delete File

Deleting files is done the same way as deleting an object given the key of the file to remove. 

## Deleting a file

Calling the `DeleteObject` method takes the ID of the file to be deleted as the only parameter.

```csharp
Task<CMObjectResponse> delResponse =
	cmAppObjectService.DeleteObject ("fe232f...");
```

## Deleting multiple files

Deleting multiple files is done in the same way except the `DeleteObjects` method is used with a list of ID to be deleted.

```csharp
Task<CMObjectResponse> delResponse =
	cmAppObjectService.DeleteObjects ( 
		new string[] {"fe232f...", "hoi78..."});
```

Much like other functions in the SDK, if an ID is not present it will be in the `Errors` dictionary with a reason why deleting wasn't possible.