# Fetch Files

{{note 'Files can only be downloaded one at a time.'}}

When files are downloaded, they are stored in memory inside a `CMFileResponse` object as a stream, within the `DataStream` propety.

```csharp
Task<CMFileResponse> fileResponse = 
   		cmAppObjectService.Download ("the-picture-id");
fileResponse.Wait ();
// fileResponse.DataStream contains the file's data
```

