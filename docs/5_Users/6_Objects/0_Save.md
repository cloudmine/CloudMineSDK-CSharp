# Save User Objects

Instances of `CMObject` can be saved by calling the `SetObject()` variants on the `CMAppObjectService` which implements the `IAppObjectService` interface. `CMAppObjectService` makes it easy to work with your preferred dependency injection methods to manage the application and API keys being worked with.

`CMUserService`'s set methods can be invoked by passing in a `CMObject` instance or by passing in an object and a key value. We recommend you use `CMObject` when writing production code so you can take advantage of automatic ID generation and client-side timestamping.

In the examples below, once `objResponse.Wait()` has returned this indicates that the communication with CloudMine has completed.

## Single Object with Explicit Type

```csharp
PatientRecord pr = new PatientRecord () {
	FirstName = "Jane",
	LastName = "Doe",
	SocialSecurityNumber = "333-22-4444"
};

CMApplication app = new CMApplication (appID, apiKey);
IRestWrapper api = new PCLRestWrapper ();
userObjSrvc = new CMUserService (app, api);

// assume `user` has been created and logged in successfully already
Task<CMUserResponse> objResponse = userObjSrvc.SetUserObject<CareProvider> (cp, user);
objResponse.Wait ();
```

## Single Object with Dynamic Type

```csharp
dynamic pr = new ExpandoObject ();
pr.__class__ = "PatientRecord";
pr.FirstName = "Jane";
pr.LastName = "Doe";
pr.SocialSecurityNumber = "333-22-4444";

var id = Guid.NewGuid ().ToString ();

CMApplication app = new CMApplication (appID, apiKey);
IRestWrapper api = new PCLRestWrapper ();
userObjSrvc = new CMUserService (app, api);

// assume `user` has been created and logged in successfully already
Task<CMUserResponse> objResponse = userObjSrvc.SetUserObject<CareProvider> (cp, user);
objResponse.Wait ();
```