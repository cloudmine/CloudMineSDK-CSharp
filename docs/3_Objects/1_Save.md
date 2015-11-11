# Save Objects

Instances of `CMObject` can be saved by calling the `SetObject()` variants on the `CMAppObjectService` which implements the `IAppObjectService` interface. `CMAppObjectService` makes it easy to work with your preferred dependency injection methods to manage the application and API keys being worked with.

`CMAppObjectService`'s set methods can be invoked by passing in a `CMObject` instance or by passing in an object and a key value. We recommend you use `CMObject` when writing production code so you can take advantage of automatic ID generation and client-side timestamping.

In the examples below, once `objResponse.Wait()` has returned this indicates that the communication with CloudMine has completed.

## Single Object with Explicit Type

```csharp
CareProvider cp = new CareProvider () {
	ProviderName = "Mercy Hospital",
	ProviderAddress = "1234 Market Street, Suite 100, Philadelphia, PA 19107",
	ProviderEmployeeCount = 431
};

CMApplication app = new CMApplication (appID, apiKey);
IRestWrapper api = new PCLRestWrapper ();
appObjSrvc = new CMAppObjectService (app, api);

Task<CMObjectResponse> objResponse = appObjSrvc.SetObject<CareProvider> (cp);
objResponse.Wait ();
```

## Single Object with Dynamic Type

```csharp
dynamic cp = new ExpandoObject ();
cp.__class__ = "CareProvider";
cp.ProviderName = "Mercy Hospital";
cp.ProviderAddress = "1234 Market Street, Suite 100, Philadelphia, PA 19107";
cp.ProviderEmployeeCount = 431;

var id = Guid.NewGuid ().ToString ();

CMApplication app = new CMApplication (appID, apiKey);
IRestWrapper api = new PCLRestWrapper ();
appObjSrvc = new CMAppObjectService (app, api);

Task<CMObjectResponse> objResponse = appObjSrvc.SetObject (cp, null, id);
objResponse.Wait ();
```
