# Delete Objects

`CMAppObjectService` offers a few ways to remove objects by ID. `DeleteObject`, `DeleteObjects` and `DeleteAllObjects` are the methods used to remove objects from CloudMine.

## Deleting an object

Calling the `DeleteObject` method takes the ID of the object to be deleted.

```csharp
Task<CMObjectResponse> delResponse =
	cmAppObjectService.DeleteObject ("fe232f...");
```

## Deleting multiple objects

Deleting multiple objects is done in the same way except the `DeleteObjects` method is leveraged with a list of ID to be deleted.

```csharp
Task<CMObjectResponse> delResponse =
	cmAppObjectService.DeleteObjects ( 
		new string[] {"fe232f...", "hoi78..."});
```

Much like other functions in the SDK, if an ID is not present it will be in the `Errors` dictionary with a reason why deleting wasn't possible.