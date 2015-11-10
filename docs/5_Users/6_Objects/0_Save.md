# Save User Objects

Individual `CMObject` derivations can be saved by calling [save](/docs/javadocs/com/cloudmine/api/db/BaseLocallySavableCMObject.html#save(Context, com.cloudmine.api.CMSessionToken, , Response.ErrorListener)) directly on the object and passing in a valid sessionToken.

### Single object

```csharp
// assume this user has been created and logged in
PIIMock pii = new PIIMock () {
				FirstName = "Jane",
				LastName = "Doe",
				SocialSecurityNumber = "333-22-4444"
			};

			Task<CMObjectResponse> objResponse = userService.SetUserObject<PIIMock> (pii, user);
```
