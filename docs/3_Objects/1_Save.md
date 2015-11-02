# Save Objects

Individual "CMObject"s can be saved by calling "SetObject" variants on the "CMAppObjectService" which implements the "IAppObjectService" Interface definition. CMAppObjectService makes it easy to work with your preferred dependency injection methods to manage the application and api keys being worked with.

CMAppObjectService set methods can be invoked by passing in a CMObject type or by passing in an object and a key value. It is advised to leverage CMObject for things like auto ID and time stamping. 

### Single object with Type Explicit

```csharp
HCPMock hcp = new HCPMock () {
	ProviderName = "CloudMine Data Hospital 3",
	ProviderAddress = "1217 Sansom Street | Suite 600, Philadelphia, PA 19107",
	ProviderEmployeeCount = 25
};

CMApplication app = new CMApplication (appID, apiKey);
IRestWrapper api = new PCLRestWrapper ();
appObjSrvc = new CMAppObjectService (app, api);

Task<CMObjectResponse> objResponse = 
	appObjSrvc.SetObject<HCPMock> (hcp);
objResponse.Wait ();
```

### Single object with key & dyanmic object

```csharp
dynamic hcp = new ExpandoObject ();
hcp.__class__ = "HCPDynamic";
hcp.ProviderName = "CloudMine Data Hospital 2";
hcp.ProviderAddress = "1217 Sansom Street | Suite 600, Philadelphia, PA 19107";
hcp.ProviderEmployeeCount = 25;

var id = Guid.NewGuid ().ToString ();

CMApplication app = new CMApplication (appID, apiKey);
IRestWrapper api = new PCLRestWrapper ();
appObjSrvc = new CMAppObjectService (app, api);

Task<CMObjectResponse> objResponse = 
	appObjSrvc.SetObject (hcp, null, id);
objResponse.Wait ();
```
