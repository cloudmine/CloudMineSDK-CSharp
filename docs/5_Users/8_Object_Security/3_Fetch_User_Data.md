# Fetching Shared User Data

Once you've granted access to an object with a `CMAccessList`, other authorized users with can fetch, modify, or delete the shared object depending the permissions you've set in the ACL.

If you are requesting the shared object with its ID explicitly, the object will be automatically fetched.

```csharp
Task<CMObjectFetchResponse<PIIMock>> getResponse = 
	userService.GetUserObject<PIIMock> (user, SHARED_OBJECT_ID);
```

However, if you want to load it through a search, it will not be returned UNLESS you use the proper query string parameters in `CMRequestOptions` when creating your request.

```csharp
CMRequestOptions opts = new CMRequestOptions () {
	Parameters = new System.Collections.Generic.Dictionary<string, string> () {
		{ "shared", "true" },
		{ "shared_only", "false" }
	}
};

Task<CMObjectSearchResponse<PIIMock>> searchResponse = 
	userService.SearchUserObjects<PIIMock> (@"[__class__=""PIIMock"", FirstName=""Jane""]", opts);
```	

The above query will search for a shared objects matching the first name but not limit the search to only shared objects.