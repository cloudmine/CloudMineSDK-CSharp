# Upload New User File to Application

In addition to object data, you are also able to store arbitrary files in CloudMine. A stream of the file contents, a file ID, and an optional MIME type are needed to create a CMFile. The MIME type can be specified by supplying the third (optional) parameter to the `Upload` method of type `CMRequestOptions`. If no MIME type is specified, a default of `application/octet-stream` is used.

The content type supplied during upload is what will be presented in the `Content-Type` header during download.

In the example below, we assume that `image.png` is an image that is located in the application's bundle and is readable to the app and that `user` is a valid logged in user.

```csharp
using (FileStream fileStream = File.OpenRead("image.png"))
{
    MemoryStream memStream = new MemoryStream();
    memStream.SetLength(fileStream.Length);
    fileStream.Read(memStream.GetBuffer(), 0, (int)fileStream.Length);
    
    CMApplication app = new CMApplication (appID, apiKey);
    IRestWrapper api = new PCLRestWrapper ();
    userObjSrvc = new CMUserService (app, api);

    CMRequestOptions opts = new CMRequestOptions() {
        ContentType = "image/png"
    };
    Task<CMFileResponse> fileResponse = 
        userObjSrvc.Upload ("the-picture-id", fileStream, user, opts);
    fileResponse.wait ();
}
```

{{warning "If the file ID given already exists on the server, the existing file will be overwritten. **File IDs must be unique across your entire application and all of your users.**"}}
