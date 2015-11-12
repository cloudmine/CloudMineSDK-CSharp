# Location Tagging

CloudMine supports attaching location information to objects so they can be searched using typical geospatial queries. To store this data, you must make use of the special `CMLocation` class and set the latitude and longitude within that object. When it is saved to CloudMine, it will automatically be indexed properly for later searching. See our [Geo Querying](#/rest_api#geo-queries) documentation for an example of how to search these fields.

```csharp
CareProvider cp = new CareProvider () {
	ProviderName = "Mercy Hospital",
	ProviderAddress = "1234 Market Street, Suite 100, Philadelphia, PA 19107",
	ProviderEmployeeCount = 431,
	Location = new CMLocation (39.9516960, -75.1610370)
};

Task<CMObjectResponse> objResponse = 
	cmAppObjectService.SetObject <CareProvider> (cp);

objResponse.Wait ();
```

