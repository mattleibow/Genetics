
Genetics allows attributes to be used to describe how to inject resources, views and events into an object. Based on the resource ID provided, it can inject the appropriate Android `View` or Android resource. Or, it can automatically attach event handlers to `View` events.

 * Use `[Splice]` on fields and properties instead of `FindViewById` method calls.
 * Use `[Splice]` on fields and properties instead of `GetString` or `GetDrawable` method calls.
 * Use `[SpliceEvent]`,  or others like `[SpliceClick]`, on methods instead of attaching delegates to events.

```csharp
public class MyActivity : Activity
{
  [Splice(Resource.Id.username)]
  private EditText username;
  [Splice(Resource.Id.password)]
  private EditText password;
  [Splice(Resource.String.loginError)]
  private string loginErrorMessage;

  [SpliceClick(Resource.Id.login)]
  private void LoginClicked(object sender, EventArgs e)
  {
    // TODO: perform login...
  }

  protected override void OnCreate(Bundle savedInstanceState)
  {
    base.OnCreate(savedInstanceState);
    
    // load the view layout
    SetContentView(Resource.Layout.MyActivityLayout);
    // inject the views
    Geneticist.Splice(this);
    
    // TODO: use fields...
  }
  
  protected override void OnDestroy()
  {
    // dispose of events and resources
    Geneticist.Sever(this);

    base.OnDestroy();
  }
}
```
