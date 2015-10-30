# Starting a New Project

If you don't need to extend or debug the actual SDK it is recommended that you install the C# SDK from the Xamarin Component Store or as a Nuget package. However if you've cloned the Library from Github, you can follow these steps.

### Xamarin or Visual Studio

(This assumes you already have cloned the SDK from the CloudMine repository.)

1. Open the solution file. CloudMineSDK -> src -> CloudMineSDK.sln
2. Right click the solution in the solution explorer.
3. Add -> Add New Project -> Select the desired project -> Next

CloudMine's .NET SDK begins with configuring your application object which holds the application ID and the API key needed by the subsequent services to make REST calls to the CloudMine platform. 

4. Configure application ID and API key variables

```java
// Find this in your developer console
private const string appID = "de45fca...";
// Find this in your developer console
private const string apiKey = "856d34a...";
```

5. Instantiate a CMApplication object where needed

```java
// This will initialize your credentials
CMApplication app = new CMApplication (appID, apiKey);
```

6. Configure the REST wrapper which translates service calls to the proper CloudMine format. 

```java
// This will initialize your REST wrappers
IRestWrapper api = new PCLRestWrapper ();
```

7. Leverage any of the services for CloudMine operations and enjoy developing without worrying about the backend!

```java
IAppObjectService appService = new CMAppObjectService (app, api);
IUserService userService = new CMUserService (app, api);
IPushNotificationService pushService = new CMPushNotificationService (app, api);
IAccessListService aclService = new CMAccessListService (app, api);
```

Intellisense and MonoDoc are enabled for documentation of individual services as well as examples further in this documentation.
