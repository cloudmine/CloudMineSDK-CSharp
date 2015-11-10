# Fetch Files

Unlike objects, files can only be loaded one at a time.

Files are retireved to a `CMFileResponse` object which contains the returned stream based on the key passed into the `Download` method on the `CMUserService` along with the user object with a valid session.

```csharp
Task<CMFileResponse> fileResponse = 
   		userService.Download ("iej20...", user);
```

