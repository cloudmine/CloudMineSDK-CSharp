# Delete User Objects

### Deleting a CMObject

Objects can be deleted with `DeleteUserObject` method on the user service which requires the object ID and  the user object of the owning and logged in user.

```csharp
Task<CMObjectResponse> delResponse = 
	appObjSrvc.DeleteObject ("h8h2hf...", user);
```

### Deleting multiple objects

Objects can also be deleted by passing their objectIds into the `DeleteUserObjects` method in the `CMUserService`. 

```csharp
Task<CMObjectResponse> delResponse = 
	appObjSrvc.DeleteObject (new string[] {"h8h2hf...", "koi2f5..."}, user);
```
