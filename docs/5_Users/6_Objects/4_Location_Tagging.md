# Location Tagging

CloudMine supports attaching location information to objects so they can be searched using typical geospatial queries. To store this data, you must make use of the special `CMLocation` class and set the latitude and longitude within that object. When it is saved to CloudMine, it will automatically be indexed properly for later searching. See our [Geo Querying](#/rest_api#geo-queries) documentation for an example of how to search these fields.

```csharp
PatientRecord pr = new PatientRecord () {
	FirstName = "Jane",
	LastName = "Doe",
	SocialSecurityNumber = "333-22-4444"
};

// assume `user` has been created and logged in successfully already
Task<CMObjectResponse> objResponse = 
	userObjSrvc.SetUserObject <CareProvider> (user, cp);

objResponse.Wait ();
```