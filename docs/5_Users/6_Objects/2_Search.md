# Search for User Objects

CloudMine's search query language gives you more fine-grained control over which objects you fetch. See the [query reference](#/rest_api#overview) for more detailed information.

Given a query string and the desired object type, the `SearchUserObjects` method can be used to run a search query against the CloudMine object store for the given user. Below is a search query where the type is `PatientRecord ` and the first name is "Jane".

```csharp
Task<CMObjectSearchResponse<PatientRecord>> searchResponse = 
    userObjSrvc.SearchUserObjects<PatientRecord> (@"[__class__=""PatientRecord"", FirstName=""Jane""]");
```

When a query returns results which don't parse properly to the desired Type parameter, a raw `CMObject` will be used. This is typically not desired behavior, so be sure to check if the objects coming back are all of the correct type.

