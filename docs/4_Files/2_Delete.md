# Delete File

Deleting files is done the same way as deleting an object given the key of the file to remove. 

### Deleting an object

Calling the DeleteObject method takes the key of the object as a parameter which will be deleted.

```csharp
Task<CMObjectResponse> delResponse =
	cmAppObjectService.DeleteObject ("fe232f...");
```

### Deleting multiple objects

Deleting multiple objects is done in the same way except the DeleteObjects method is leveraged with an array or list of keys to be deleted.

```csharp
Task<CMObjectResponse> delResponse =
	cmAppObjectService.DeleteObjects ( 
		new string[] {"fe232f...", "hoi78..."});
```

Much like other functions in the SDK, if an ID is not present it will be in the Errors dictionary with a reason why deleting wasn't possible such as: "key does not exist"
