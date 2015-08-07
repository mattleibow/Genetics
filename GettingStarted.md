# Syringe Details

Syringe allows attributes to be used to describe how to inject resources, views and events into an object. Based on the resource ID provided, it can inject the appropriate Android `View` or Android resource. Or, it can automatically attach event handlers to `View` events.

 * Use `[Inject]` on fields and properties instead of `FindViewById` method calls.
 * Use `[Inject]` on fields and properties instead of `GetString` or `GetDrawable` method calls.
 * Use `[InjectEvent]`,  or others like `[InjectClick]`, on methods instead of attaching delegates to events.

```csharp
public class MyActivity : Activity
{
  [Inject(Resource.Id.username)]
  private EditText username;
  [Inject(Resource.Id.password)]
  private EditText password;
  [Inject(Resource.String.loginError)]
  private string loginErrorMessage;

  [InjectClick(Resource.Id.login)]
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
    Needle.Inject(this);
    
    // TODO: use fields...
  }
}
```

## Injecting Views

Instead of writing many `FindViewById` method calls, the fields, or properties, can simply be attributed with the `[Inject]` attribute:
```csharp
[Inject(Resource.Id.username)]
private EditText username;
```
Then, when the fields are to be populated with the actual view, we can call the `Inject` method once:
```csharp
// this can be an Activity, Dialog or View
Needle.Inject(this);
```
The `Inject` method has several overloads that accept an `Activity`, `Dialog` or a `View`. If the object that contains the fields is not the same as the view container, then other overloads can be used:
```csharp
public class MyFragment : Fragment
{
  [Inject(Resource.Id.button)]
  private Button button;
  [Inject(Resource.Id.textView)]
  private TextView textView;


  public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
  {
    // load the view layout
    var view = inflater.Inflate(Resource.Layout.MyFragmentLayout, container, false);
    // inject the fields
    Needle.Inject(this, view);
    
    // TODO: use fields
    
    return view;
  }
}
```

## Injecting Resources

Similar to views, we can also inject resources. By attributing  fields and properties with the resource ID, the `Inject` method will automatically load the resource and convert it to the member type:
```csharp
[Inject(Resource.String.loginError)]
private string loginErrorMessage;
```
Then, when the fields are to be populated with the actual resource value, we can call the `Inject` method once:
```csharp
// this can be an Activity, Dialog or View
Needle.Inject(this);

```
Or, if we don't have a view container, and there are no view injection attributes, we can just pass in a context:
```csharp
// this can be any POCO object
Needle.Inject(this, null, context);
```

## Injecting Events

In addition to injecting views and resources, Syringe can also inject event handlers into the various views:
```csharp
[InjectClick(Resource.Id.login)]
private void LoginClicked(object sender, EventArgs e)
{
}
```
If there is no specific attribute for a specific event, the generic attribute can be used instead:
```csharp
[InjectEvent(Resource.Id.search, "BeforeTextChanged")]
private void SearchTextChanging(object sender, TextChangedEventArgs e)
{
}
```

The event handler method should match that of the event, but does not need to be exactly the same. The parameters need to be assignable (one of the base types or interfaces) from the actual parameter type:
```csharp
[InjectEvent(Resource.Id.search, "BeforeTextChanged")]
private void SearchTextChanging(object sender, EventArgs e)
{
}
```

## Injection for Adapters

One of the uses for Syringe, is the injection of the view holders for adapters. Instead of having to find the views on the item, we store the saved locations in the view's `Tag` property:

```csharp
public class SimpleAdapter : BaseAdapter<string>
{
  public override View GetView(int position, View convertView, ViewGroup parent)
  {
    // get or create a new view holder
    ViewHolder holder;
    if (convertView != null)
    {
      holder = (ViewHolder)convertView.Tag;
    }
    else
    {
      convertView = inflater.Inflate(Resource.Layout.SimpleListItem, parent, false);
      holder = new ViewHolder(convertView);
      convertView.Tag = holder;
    }

    // TODO: use fields...

    // return the final view
    return convertView;
  }

  private class ViewHolder : Java.Lang.Object
  {
    [Inject(Resource.Id.word)] 
    public TextView word;
    [Inject(Resource.Id.length)] 
    public TextView length;
    [Inject(Resource.Id.position)] 
    public TextView position;
    
    public ViewHolder(View view)
    {
      // inject the fields
      Needle.Inject(this, view);
    }
  }
}
```

## Injecting Custom Member Types

If a member is going to be injected from a resource, a custom `INeedle` can be created to correctly set the value of the field. There is a the base `SimpleResourceNeedle` that can be used to set the field value:
```csharp
public class BooleanStringNeedle : SimpleResourceNeedle
{
  public override object GetValue(Resources resources, int resourceId, Type memberType)
  {
    return resources.GetBoolean(resourceId) ? "true" : "false";
  }
}
```
Then, to register it with Syringe, an instance is passed to the `Needle`:
```csharp
// the first parameter is the Android resource type
// and the second is the .NET member type
Needle.RegisterNeedle("bool", typeof(string), new BooleanStringNeedle());
```
For more complex resources, the `INeedle` interface can be used directly, as in the case of a `BitmapNeedle`:
```csharp
public class BitmapNeedle : INeedle
{
  public bool Inject(object target, object source, string resourceType, int resourceId, Context context, MemberMapping memberMapping)
  {
    Bitmap bitmap = null;
    try
    {
      bitmap = BitmapFactory.DecodeResource(context.Resources, resourceId);
      memberMapping.SetterMethod(target, bitmap);
      return true;
    }
    catch (Exception exception)
    {
      if (bitmap != null)
      {
        bitmap.Recycle();
        bitmap.Dispose();
      }
      Needle.HandleError(
        exception,
        "Unable to inject resource '{0}' with id '{1}' to member '{2}'.",
        context.Resources.GetResourceName(resourceId),
        resourceId,
        memberMapping.Member.Name);
    }
    return false;
  }

  public void Withdraw(object target, object source, string resourceType, int resourceId, Context context, MemberMapping memberMapping)
  {
    if (memberMapping.Attribute.DisposeOnWithdraw)
    {
      var bitmap = memberMapping.GetterMethod(target) as Bitmap;
      bitmap.Recycle();
      bitmap.Dispose();
    }
    memberMapping.SetterMethod(target, null);
  }
}
```
