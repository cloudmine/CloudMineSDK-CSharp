# List All Users

Each user has a public profile associated with their account. These can be retrieved using the `ListUsers` method.

```csharp
Task<CMResponse> userProfiles = 
	userService.ListUsers<CMUserProfile> ();
```
