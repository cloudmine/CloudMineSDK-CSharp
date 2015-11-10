# Sorting and Paging

## Sorting

By default, loaded objects are returned in an undefined (and often inconsistent) order. If you care about the order of returned results, you can specify a field to sort by specifying the sort field in the request options passed into the service method.

```csharp
// create a new instance of CMRequestOptions, sort by name, and run the request
var opts = new CMRequestOptions () {
	SortResults = "__updated__"
};
```

## Paging

Paging is used to control the number of results returned from each request. Use the `LimitResults` and `SkipResults` members to specify paging options. Make sure to specify the count member on the first request to get the total count of matched objects on the server.

```csharp
// create a new instance of CMRequestOptions, sort by name, and run the request
var opts = new CMRequestOptions () {
	SortResults = "__updated__",
	CountResults = true,
	SkipResults = 0,
	LimitResults = 20
};
```
Note: running count on every query in a large dataset could greatly impact the runtime of the query. It is advised to limit the use of count to the first or at timed intervals.