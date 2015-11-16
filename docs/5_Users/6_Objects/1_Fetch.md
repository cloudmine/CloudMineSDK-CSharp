# Fetch User Objects

This section refers to retrieving an object given its ID. If you're interested in retrieving objects by searching the contents of its fields, see the [Search](#search-for-user-objects) section below.

`CMUserService` offers a few ways to retrieve objects by ID. `GetUserObject` and `GetUserObjects` are the methods that will be leveraged to retrieve these objects. Setting up the `CMUserService` is done as follows:

```csharp
CMApplication app = new CMApplication (appID, apiKey);
IRestWrapper api = new PCLRestWrapper ();
cmUserService = new CMUserService (app, api);
```

Use the `GetUserObject` methods to fetch objects that exist on the server. Prior to loading user objects, a user must be logged in. In the following examples, we assume `user` is a valid instance of `CMUser` and is already logged in.

## Fetching all objects

To fetch all the objects owned by the given user, just call on the `GetUserObjects` method with an empty set for the `keys` parameter.

```csharp
Task<CMObjectFetchResponse<PatientRecord>> getResponse = 
	cmUserService.GetUserObjects<PatientRecord> (
		user, new string[] { });
```

## Fetching a Single Object by ID

Fetching a specific key is done by calling the `GetUserObject` method on `CMUserService`. Specify the ID of the object to be retrieved as well as the type of the object. Remember that this type must be derived from `CMObject`.

```csharp
Task<CMObjectFetchResponse<PatientRecord>> getResponse = 
	cmUserService.GetUserObject<PatientRecord> (user, "abf14872d");
```

## Fetching Multiple Objects by ID

Fetching multiple objects by their IDs requires that you specify the IDs to retrieve in an array. In this example, we are fetching two objects.

```csharp
Task<CMObjectFetchResponse<PatientRecord>> getResponse = 
	userService.GetUserObjects <PatientRecord> (
		user, new string[] { "abf14872d", cdb1432ba" });
```

**Note:** When fetching multiple IDs, if an ID isn't present within CloudMine the result will still be a successful `HttpStatusCode`. However, the response object's `HasErrors` method will return `true`. IDs that had retrieval issues will be present in the `Errors` dictionary on the response object with a message regarding why each specific object failed to be retrieved.PatientRecord