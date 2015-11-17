# Push Notifications

## iOS

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
