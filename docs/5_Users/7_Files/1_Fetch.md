# Fetch User Files

{{note 'Files can only be downloaded one at a time.'}}

When files are downloaded, they are stored in memory inside a `CMFileResponse` object as a stream, within the `DataStream` propety.

```csharp
Task<CMFileResponse> fileResponse = 
   		userService.Download ("the-picture-id", user);
// fileResponse.DataStream contains the file's data
```

