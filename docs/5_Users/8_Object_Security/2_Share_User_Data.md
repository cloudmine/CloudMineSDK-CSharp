# Sharing User Data

To share data between different users, we need to grant a `CMObject` access to a `CMAccessList`. This is done using the optional `Access` member on the base `CMObject`.

This assumes that we've already created an `CMAccessList`.

```csharp
CMObject obj = new CMObject () {
	AccessListIDs = new string[] { acl.ID }
};
```

Once the access list has been attached to an object, members of that list will have the permissions enumerated in the access list.
