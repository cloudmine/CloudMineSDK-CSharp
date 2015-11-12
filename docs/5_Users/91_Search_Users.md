# Search User Profiles

User profiles can be queried using the [query language](#/rest_api#overview) over profiles.

This will find all users where the user's age is greater than 18. This requires you to have created a user with a profile that has an age field.

```csharp
userService.SearchUsers(CMUser user, @"[age > 18]");
```

