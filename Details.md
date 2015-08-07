# Syringe Details

Syringe allows attributes to be used to describe how to inject resources, views and events into an object. Based on the resource ID provided, it can inject the appropriate Android `View` or Android resource. Or, it can automatically attach event handlers to `View` events.

 * Use `[Inject]` on fields and properties instead of `FindViewById` method calls.
 * Use `[Inject]` on fields and properties instead of `GetString` or `GetDrawable` method calls.
 * Use `[InjectEvent]`,  or others like `[InjectClick]`, on methods instead of attaching delegates to events.

```csharp
Using upublic class MyActivity : Activity
{
  [Inject(Resource.Id.username)]
  private EditText username;
  [Inject(Resource.Id.password)]
  private EditText password;
  [Inject(Resource.String.loginError)]
  private string loginErrorMessage;

  [InjectClick(Resource.Id.login)]
  private void LoginClicked()
  {
    // TODO: perform login...
  }

  protected override void OnCreate(Bundle savedInstanceState)
  {
    base.OnCreate(savedInstanceState);
    
    // load the view layout
    SetContentView(Resource.Layout.MyActivityLayout);
    // inject the views
    Needle.Inject(this);
    
    // TODO: use fields...
  }
}
```
