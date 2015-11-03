# Fetch Files

Unlike objects, files can only be loaded one at a time.

Files are retireved to a `CMFileResponse` object which contains the returned stream based on the key passed into the Download method on the app or user service.

```csharp
Task<CMFileResponse> fileResponse = 
   		appObjSrvc.Download ("iej20...");
fileResponse.wait ();
```

