# Create Access List

Create a new access list by instantiating a `CMAccessList`. You must have a previously created user with a valid session to create a new access list.

Permissions are controlled by adding `CMAccessListPermission` enums to the access list array `Permissions` on a `CMAccessList` instance. This gives you the option to allow create, read, update, or delete permissions.

Add users to this access list by using the `Members` string array on the `CMAccessList` object. Values of the members should be the unique string identifer of the user. 

`AccessListService` is for communicating the creation and modification of an access list to CloudMine.

```csharp
var acl = new CMAccessList () {
	Permissions = new CMAccessListPermission[] {
		CMAccessListPermission.r, 
		CMAccessListPermission.u
	},
	Members =  new string[] { "hfiw2...", "fhre6..." }
};

IAccessListService accessService = new AccessListService ();
Task<CMResponse> createAccessListTask = accessService.CreateAccessList (user, acl);

acl.Permissions = new CMAccessListPermission[] {
	CMAccessListPermission.r
};

Task<CMResponse> modifyAccessListTask = accessService.ModifyAccessList (user, acl);
```	

The above example shows how to create and modify an access list permissions. In this case the access list initially grants members both read and update privelages with the `CreateAccessList` method. Update permission is then revoked with the second call to the `ModifyAccessList` method.