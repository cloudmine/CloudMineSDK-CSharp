# cloudmine-xamarin

We have C# Library available for you to download to make native development on .Net/Mono/Xamarin environments even easier. Developing with the C# SDK is designed to be a plug and play Portable Class Library (PCL 4.5) meaning .NET Framework 4.5 or later.

### Current tested/validated platforms
* .NET/Mono Framework 4.5 or later
* Windows Phone 8 or later
* Windows Phone Silverlight 8
* Windows Store apps (Windows 8)
* Xamarin Android
* Xamarin iOS Classic
* Xamarin iOS Unified
* Xamarin Mac Unified

The Library is open-source and under the MIT license.

### Github

We're actively developing and invite you to fork and send pull requests on GitHub.

* [cloudmine/cloudmine-xamarin](https://github.com/cloudmine/cloudmine-xamarin)

###Xamarin/Nuget

You can add the C# library as a Nuget package or from the Xamarin Component store.

Before you can begin using the C# PCL Library, you must first [create an application](/dashboard/app/create) in the CloudMine dashboard.

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

