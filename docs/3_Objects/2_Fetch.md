# Fetch Objects

This section refers to retrieving an object given its ID. If you're interested in retrieving objects by searching the contents of its fields, see the [Search](#search-for-objects) section below.

`CMAppObjectService` offers a few ways to retrieve objects by ID. `GetObject` and `GetObjects` are the methods that will be leveraged to retrieve these objects. Setting up the `CMAppObjectService` is done as follows:

```csharp
CMApplication app = new CMApplication (appID, apiKey);
IRestWrapper api = new PCLRestWrapper ();
cmAppObjectService = new CMAppObjectService (app, api);
```

## Fetching all objects

Fetching all objects stored for an application is easy. Just call on the the `GetObjects` method with an empty set for the `keys` parameter. Remember that this will only return objects that are not owned by a user and that your API Key has permission to access.

```csharp
Task<CMObjectFetchResponse<CareProvider>> getResponse = 
	cmAppObjectService.GetObjects<CareProvider> (
		new string[] { });
```

## Fetching a Single Object by ID

Fetching a specific object by its ID is done by calling the `GetObject` method on `CMAppObjectService`. Specify the ID of the object to be retrieved as well as the type of the object. Remember that this type must be derived from `CMObject`.

```csharp
Task<CMObjectFetchResponse<CareProvider>> getResponse = 
	cmAppObjectService.GetObject<CareProvider> ("fe232f191abc");
```

## Fetching Multiple Objects by ID

Fetching multiple objects by their IDs requires that you specify the IDs to retrieve in an array. In this example, we are fetching two objects.

```csharp
Task<CMObjectFetchResponse<CareProvider>> getResponse = 
	cmAppObjectService.GetObjects <CareProvider> (
		new string[] { "fe232f...", "j9jlh..." });
```

**Note:** When fetching multiple IDs, if an ID isn't present within CloudMine the result will still be a successful `HttpStatusCode`. However, the response object's `HasErrors` method will return `true`. IDs that had retrieval issues will be present in the `Errors` dictionary on the response object with a message regarding why each specific object failed to be retrieved.