# Geolocation Tag User Objects

### Specifying a location field

CloudMine supports attaching location information to objects. The base `CMObject` class has a Location member which is ignored while null. Any object which derives from CMObject can set the Location field and the value will be handled and indexed at the server.

CloudMine supports attaching location information to objects. The base `CMObject` class has a Location member which is ignored while null. Any object which derives from CMObject can set the Location field and the value will be handled and indexed at the server.

```csharp
HCPMock hcp = new HCPMock () {
	ProviderName = "CloudMine Data Hospital 3",
	ProviderAddress = "1217 Sansom Street | Suite 600, Philadelphia, PA 19107",
	ProviderEmployeeCount = 25,
	Location = new CMLocation (40.5, -60)
};

Task<CMObjectResponse> objResponse = 
	cmAppObjectService.SetObject <HCPMock> (hcp);
objResponse.Wait ();
```

Of course, you don't need to rely on the single location optional field on `CMObject`. You can also add a location field to sub objects or an array of locations. There is a base object called `CMLocation` which can be leveraged to add geo members as needed.