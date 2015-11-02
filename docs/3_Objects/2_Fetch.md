# Fetch Objects

CMAppObjectService offers a few ways to retrieve objects by id. GetObject and GetObjects are the methods that will be leveraged to retrieve these objects. Setting up the CMAppObjectService is done as follows:

```csharp
CMApplication app = new CMApplication (appID, apiKey);
IRestWrapper api = new PCLRestWrapper ();
cmAppObjectService = new CMAppObjectService (app, api);
```

### Fetching all objects

Fetching all objects stored for an application is easy. Just call on the the GetObject methods with an empty set for the keys parameter.

```csharp
Task<CMObjectFetchResponse<HCPMock>> getResponse = 
	cmAppObjectService.GetObjects<HCPMock> (
		new string[] { });
```

### Fetching an individual key

Fetching a specific key is done by calling the GetObject method on CMAppObjectService. Specify the key to be found as well as the type of the object which should derive from CMObject.

```csharp
Task<CMObjectFetchResponse<HCPMock>> getResponse = 
	cmAppObjectService.GetObject<HCPMock> ("fe232f...");
```

### Fetching multiple keys

Fetching multiple keys is the same as fetching all keys except a list or array of IDs is specified

```csharp
Task<CMObjectFetchResponse<HCPMock>> getResponse = 
	cmAppObjectService.GetObjects <HCPMock> (
		new string[] { "fe232f...", j9jlh..." });
```

Note: When fetching multiple IDs, if an ID isn't present at CloudMine the result will still be a successful HttpStatusCode. However, the response object HasErrors method should return true. IDs which had retrieval issues will be present in the Errors dictionary on the response with a message regarding why the call specifically failed. 