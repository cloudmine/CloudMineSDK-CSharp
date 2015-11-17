# Sharing User Data

To share data between different users, we need to grant a `CMObject` access to a `CMAccessList`. This is done using the `Access` member on the base `CMObject`.

This assumes that we've already created and configured an instance of `CMAccessList` and that it is called `acl` in the example below.

```csharp
// Assume `obj` is of type MyObjectType which extends CMObject
// Assume userObjSrvc is configured and is of type `CMUserService`
// Assume `user` is a version of `CMUser` that has a valid session
obj.AccessListIDs = new string[] { acl.ID }
Task<CMUserResponse> objResponse = userObjSrvc.SetUserObject<MyObjectType> (cp, user);
objResponse.Wait ();
```

Once the access list has been attached to an object and that object has been saved, members of that list will have the permissions enumerated in the access list.
