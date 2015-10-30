## Starting a New Project

### Xamarin or Visual Studio

(This assumes you already have cloned the SDK from the CloudMine repository.)

1. Rick click on packages in the project folder feom within the solution explorer
2. Click 'Add Packages'
3. Search for CloudMine in the search helper and click 'Add Package'

CloudMine's .NET SDK begins with configuring your application object which holds the application ID and the API key needed by the subsequent services to make REST calls to the CloudMine platform. 

4. Configure application ID and API key variables

```csharp
// Find this in your developer console
private const string appID = "de45fca...";
// Find this in your developer console
private const string apiKey = "856d34a...";
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

Intellisense and MonoDoc are enabled for documentation of individual services as well as examples further in this documentation.


## Additional Resources

### Source Code
* Github:

### Documentation
* Tutorials:
* Full API documentation:
* CloudMine Dashboard:

### Contact
* Sales:
* Engineering support: 
* Tel: 
* Twitter: @CloudMine @ry_donahue
