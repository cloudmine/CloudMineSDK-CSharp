# Search for Objects

CloudMine's search query language gives you more fine-grained control over which objects you fetch. See the [query reference](#/rest_api#overview) for more detailed information.

Given a query string and the desired object type, the `SearchObjects` method can be used to run a search query against your CloudMine object store. Below is a search query where the type is `CareProvider` and the number of employees on that object is equal to 200.

```csharp
Task<CMObjectSearchResponse<CareProvider>> searchResponse = 
    appObjSrvc.SearchObjects<CareProvider> (@"[__class__=""CareProvider"", ProviderEmployeeCount=200]");
```

When a query returns results which don't parse properly to the desired Type parameter, a raw `CMObject` will be used. This is typically not desired behavior, so be sure to check if the objects coming back are all of the correct type.
