# Location Tagging

CloudMine supports attaching location information to objects. The base CMObject class has a Location member which is ignored while null. Any object which derives from CMObject can set the Location field and the value will be handled and indexed at the server.

```csharp
CareProvider cp = new CareProvider () {
	ProviderName = "CloudMine Data Hospital 3",
	ProviderAddress = "1217 Sansom Street | Suite 600, Philadelphia, PA 19107",
	ProviderEmployeeCount = 25,
	Location = new CMLocation (40.5, -60)
};

Task<CMObjectResponse> objResponse = 
	cmAppObjectService.SetObject <CareProvider> (cp);
objResponse.Wait ();
```

