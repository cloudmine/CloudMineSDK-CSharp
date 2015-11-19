# CUSTOM SERVER CODE

## Code Snippet Usage

Custom code execution allows you to write custom code snippets that run on our servers to perform processing and post-processing operations that are inappropriate to run on a mobile device or to offload certain business logic to the server. Your code runs in a sandboxed server-side environment that has access to the CloudMine [JavaScript API](http://github.com/cloudmine/cloudmine-js) and a simple HTTP client. All snippets are killed after 30 seconds of execution.

JavaScript and Java server-side applications are available. For more information on implementing this, see the [Java application documentation](#/android_and_java) or the [Javascript SDK documentation](#/javascript).

Server-side snippets are invoked in the C# SDK by creating an instance of `CMRequest` and passing the configured snippet parameters as options on almost all service methods. It's also possible to directly call the `Run` operation with the name of the snippet and any parameters.

```csharp
CMRequestOptions opts = new CMRequestOptions () {
	Snippet = "ProcessData",
	SnippetResultOnly = false,
	Parameters = new System.Collections.Generic.Dictionary<string, string> () {
		{ "name" = "Ryan" },
		{ "occupation" = "developer" }
	}
};
Task<CMResponse> objResponse = 
	appObjSrvc.SetObject<CMObject> (obj, opts);
```

```csharp
Task<CMObjectResponse> objResponse = appObjSrvc.Run (
	"ProcessData", 
	new Dictionary<string, string> () {
		{ "name" = "Ryan" },
		{ "occupation" = "developer" }
});
```

## Code Snippet Options

`CMRequestOptions` has several options to control code snippet execution.

### CMRequest options

CMRequest class can be used with any call the app and user services provide with the C# SDK. When configuring the run of a snippet the parameters dictionary on CMRequest can be instantiated with value a snippet requires as input.

* `boolean resultOnly`
  * Only include the results of the snippet call in the response.

* `boolean async`
  * Don't wait for the snippet execution to complete before returning.

* `Dictionary<string, string> Parameters`
  * These will be passed into the snippet as parameters. Se the documentation on server side snippets to see how to leverage `params` for computation in a server-side snippet.
