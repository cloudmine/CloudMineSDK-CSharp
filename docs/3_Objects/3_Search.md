# Search for Objects in Application

CloudMine's search query language gives you more fine-grained control over which objects you fetch. See the [query reference](#/rest_api#overview) for more detailed information.

# Building a search query

For in-depth documentation and examples, see the [query language reference](#/rest_api#overview).

### Using a query

Given a query string and the desired CMObject type, the SearchObjects method can be leveraged. Below is a search query where the type is "HCPMock" and the number of employees on that object is equal to 200.

```csharp
Task<CMObjectSearchResponse<HCPMock>> searchResponse = appObjSrvc.SearchObjects<HCPMock> (@"[__class__=""HCPMock"", ProviderEmployeeCount=200]");
```

When a query returns results which don't parse properly to the desired Type parameter, a CMObject will be used. It is advised to investigate the query and the returned objects to determine the issue.
