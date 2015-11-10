# Fetch User Objects

Use the `GetUserObject` methods to fetch objects that exist on the server. Prior to loading user objects, a user must be logged in.

### Fetching an individual key

Fetching a specific key is done by calling the `GetUserObject` method on `CMUserService`. Specify the key to be found, user object with valid session,  as well as the type of the object which should derive from `CMObject`.

```csharp
Task<CMObjectFetchResponse<HCPMock>> getResponse = 
	userService.GetUserObject<HCPMock> (user, "fe232f...");
```

### Fetching multiple keys

Fetching multiple keys is the same as fetching all keys except a list or array of IDs is specified

```csharp
Task<CMObjectFetchResponse<HCPMock>> getResponse = 
	userService.GetUserObjects <HCPMock> (user, 
		new string[] { "fe232f...", j9jlh..." });
```

Note: When fetching multiple IDs, if an ID isn't present at CloudMine the result will still be a successful HttpStatusCode. However, the response object HasErrors method should return true. IDs which had retrieval issues will be present in the Errors dictionary on the response with a message regarding why the call specifically failed. 