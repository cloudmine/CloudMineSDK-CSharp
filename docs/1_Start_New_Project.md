# Starting a New Project

If you don't need to extend or debug the SDK itself, we recommend that you install the C# SDK from the Xamarin Component Store or as a Nuget package. Otherwise, you can follow these steps.

## Xamarin or Visual Studio

(This assumes you already a local clone of the SDK from the CloudMine repository.)

1. Open the solution file. *CloudMineSDK -> src -> CloudMineSDK.sln*
1. Right click the solution in the solution explorer. And select *Add -> Add New Project -> Select the desired project -> Next*

    Using CloudMine's C# SDK begins with configuring your application object. The application object holds the App ID and the API Key needed by the subsequent services to make REST calls to the CloudMine platform.

1. Configure application ID and API key variables

    ```csharp
// Find this in your developer console
private const string appID = "de45fca...";
// Find this in your developer console
private const string apiKey = "856d34a...";
```

1. Instantiate a `CMApplication` object where needed

    ```csharp
// This will initialize your credentials
CMApplication app = new CMApplication (appID, apiKey);
```

1. Configure the REST wrapper which translates service calls to the proper CloudMine format. 

    ```csharp
// This will initialize your REST wrappers
IRestWrapper api = new PCLRestWrapper ();
```

1. Leverage the services such as those below to access any of the CloudMine microservices.

    ```csharp
IAppObjectService appService = new CMAppObjectService (app, api);
IUserService userService = new CMUserService (app, api);
IPushNotificationService pushService = new CMPushNotificationService (app, api);
IAccessListService aclService = new CMAccessListService (app, api);
```

IntelliSense and MonoDoc are enabled for documentation of individual services. Keep reading this documentation to find examples later.