# Delete User Objects

`CMUserService` offers a few ways to remove objects by ID. `DeleteUserObject`, `DeleteUserObjects` and `DeleteAllUserObjects` are the methods used to remove objects from CloudMine.

## Deleting an object

Calling the `DeleteUserObject` method takes the ID of the object to be deleted and the logged in `CMUser` object representing the user that owns the object.

```csharp
Task<CMObjectResponse> delResponse = 
	userObjSrvc.DeleteObject ("h8h2hf...", user);
```

## Deleting multiple objects

Deleting multiple objects is done in the same way except the `DeleteUserObjects` method is leveraged with a list of ID to be deleted.

```csharp
Task<CMObjectResponse> delResponse = 
	userObjSrvc.DeleteObjects (new string[] {"h8h2hf...", "koi2f5..."}, user);
```
