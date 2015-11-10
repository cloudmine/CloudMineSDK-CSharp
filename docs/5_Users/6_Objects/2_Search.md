# Search for User Objects

CloudMine's search query language gives you more fine-grained control over which objects you fetch. See the [query reference](#/rest_api#overview) for more detailed information.

### Providing a query

You can do more complex searches, such as loading all of the objects that have a top level field of 'name' with the value 'Ryan' using the `SearchUserObjects` method.

```csharp
Task<CMObjectSearchResponse<HCPMock>> searchResponse = 
	appObjSrvc.SearchObjects<HCPMock> (@"[name=""Ryan""]");
```

For in-depth documentation and examples, see the [query language reference](#/rest_api#overview).

