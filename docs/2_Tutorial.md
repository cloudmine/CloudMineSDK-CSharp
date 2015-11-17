# Tutorial Xamarin

(This assumes you already have cloned the SDK from the CloudMine repository.)

1. Rick click on packages in the project folder feom within the solution explorer
2. Click 'Add Packages'
3. Search for CloudMine in the search helper and click 'Add Package'. Also add the Json.Net package for defining your serializable classes.

CloudMine's .NET SDK begins with configuring your application object which holds the application ID and the API key needed by the subsequent services to make REST calls to the CloudMine platform. 

4. Configure your application ID and API key variables. When going to production you will most likely want to obfuscate these by storing in an encrypted client database or at the very least not making them static for lifecycle of the application. Not doing so makes an application vulnerable to advanced memory forensics to obtain credentials. Regardless it is never recommended to store a master key on the application. It it recommended that you further limit the data API keys have access to through custom filters in the CloudMine dashboard.

```csharp
// Find this in your developer console
private string appID = "de45fca...";
// Find this in your developer console
private string apiKey = "856d34a...";
```

5. Instantiate a CMApplication object where needed

```csharp
// This will initialize your credentials
CMApplication app = new CMApplication (appID, apiKey);
```

6. Configure the REST wrapper which translates service calls to the proper CloudMine format. 

```csharp
// This will initialize your REST wrappers
IRestWrapper api = new PCLRestWrapper ();
```

7. Leverage any of the services for CloudMine operations and enjoy developing without worrying about the backend!

```csharp
IAppObjectService appService = new CMAppObjectService (app, api);
IUserService userService = new CMUserService (app, api);
IPushNotificationService pushService = new CMPushNotificationService (app, api);
IAccessListService aclService = new CMAccessListService (app, api);
```

Check out some of the sample applications which leverage some of the CloudMine SDK services.

[Github](https://github.com/cloudmine/cloudmine-csharp/tree/master/samples)