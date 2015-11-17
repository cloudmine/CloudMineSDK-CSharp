# USER DATA SECURITY

Ordinarily, only the user who created a user level object has access to it. Obviously, this is a problem for any site that wants to allow users to share information with their 'friends', or do any other kind of restrictive sharing. To help enable this sort of behavior, CloudMine supports the concept of Access Control Lists (ACLs).

ACLs consist of three pieces of information: an owning user, the permissions that are granted by the ACL, and a collection of users who are granted the permissions of the ACL. Every CloudMine object can have any number of ACLs attached to it, which will provide the ACL's users the permissions specified by the ACL on that object. Here is an example of how to create an ACL:

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
```

First we create a new ACL, belonging to user, with `READ` and `UPDATE` permissions. Now that our ACL has been persisted, lets add it to a `CMObject` so that it is shared.

```csharp
SomeCMObject obj = new SomeCMObject () {
	FirstName = "Ryan",
	AccessListIDs = new string[] { acl.ID }
};
```

Now `obj` is a user level object that belongs to user and it is shared with the users in the `acl.Members`. How do you (as one of those users) actually get access to it? Well, if you load it by `obj.ID`, it will be returned automatically:

```csharp
Task<CMObjectFetchResponse<PIIMock>> getResponse = 
	userService.GetUserObject<PIIMock> (user, SHARED_OBJECT_ID);
```

However, if you want to load it through a search or by loading all of the user objects, it will not be returned UNLESS you use the `CMRequestOptions` and set the `Parameters` when creating your search request.

```csharp
CMRequestOptions opts = new CMRequestOptions () {
	Parameters = new Dictionary<string, string> () {
		{ "shared", "true" },
		{ "shared_only", "false" }
	}
};
Task<CMObjectSearchResponse<PIIMock>> searchResponse = 
	userService.SearchUserObjects<PIIMock> (user, @"[__class__=""PIIMock"", FirstName=""Ryan""]");
```

Note: Only an object's owning user can modify the access list ids on a `CMObject`. This is done so access lists can be recycled which is important at scale. For instance you might want to have one access list designated for admin users who have `READ` access to all objects. In this case it would be wise to re-use that ID across objects as opposed to creating a new one for each object or user.