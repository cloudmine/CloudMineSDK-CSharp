# Push Notifications

## iOS

CloudMine supports recieving push notification from the Apple Push Notification Service. Before sending push notifications, you'll need to register your app with Google, as described [here](#/push_notifications#app-registration). To do so you will need to register the device for push and in `AppDelegate` override `RegisteredForRemoteNotifications`. 

```csharp
public override void RegisteredForRemoteNotifications (
UIApplication application, NSData deviceToken)
{
    // Get current device token
    var DeviceToken = deviceToken.Description;
    if (!string.IsNullOrWhiteSpace(DeviceToken)) {
        DeviceToken = DeviceToken.Trim('<').Trim('>');
    }

    // Get previous device token
    var oldDeviceToken = NSUserDefaults.StandardUserDefaults.StringForKey("PushDeviceToken");

    // Has the token changed?
    if (string.IsNullOrEmpty(oldDeviceToken) || !oldDeviceToken.Equals(DeviceToken))
    {
        //TODO: Unregister the old with CloudMine
    }

    // Save new device token to CloudMine
    IPushNotificationService pushService = 
		new CMPushNotificationService (app, api);
	Task<CMResponse> pushRegisterResponse = 
		pushService.RegisterAndroidDevicePushNotifications (user, DeviceToken);
		
    NSUserDefaults.StandardUserDefaults.SetString(DeviceToken, "PushDeviceToken");
}
```

Given the device token a register call to CloudMine is possible. It is also wise to check that the token equals the old token and if not unregister the old token with CloudMine. While not necessary it definitely makes maintenance and push tracking easier.

## Android
CloudMine supports receiving push notifications from the Google Cloud Messaging service (GCM). Before sending push notifications, you'll need to register your app with Google, as described [here](#/push_notifications#app-registration). Once your app is configured, you'll need to add code to your app that registers the device with Google. Xamarin Android goes over this [here](https://developer.xamarin.com/guides/cross-platform/application_fundamentals/notifications/android/remote_notifications_in_android/). 

Once you have made the call to Google to register the device, you'll get a callback in the `OnHandleIntent()` method of your GCMIntentService. In this method you'll need to send the token to CloudMine. Take special note of the `SendRegistrationToAppServer` method example in the device GCM registration section. This is where CloudMine device registration would hook in.

```csharp
void SendRegistrationToAppServer (string token)
{
	IPushNotificationService pushService = 
		new CMPushNotificationService (app, api);
	Task<CMResponse> pushRegisterResponse = 
		pushService.RegisterAndroidDevicePushNotifications (user, token);
}
```

If the registration was successful, your device is now ready to recieve push notifications.

### Unregistering

If you no longer want this device to receive push notifications, use the `unregisterForGCM(Callback callback)` method on `CMWebService`.

```csharp
IPushNotificationService pushService = 
		new CMPushNotificationService (app, api);
	Task<CMResponse> pushUnRegisterResponse = 
		pushService.UnRegisterAndroidDevicePushNotifications (user);
```

After this, you will no longer recieve push notifications.
