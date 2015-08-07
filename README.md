# Syringe

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

## Download

Syringe can be downloaded as a [Xamarin Component][1] or as a [NuGet][1]:
```batch
Install-Package Syringe
```

## License

    Copyright 2015 .NET Development Addict
    
    Licensed under the Apache License, Version 2.0 (the "License");
    you may not use this file except in compliance with the License.
    You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

    Unless required by applicable law or agreed to in writing, software
    distributed under the License is distributed on an "AS IS" BASIS,
    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
    See the License for the specific language governing permissions and
    limitations under the License.


 [1]: http://www.nuget.org/packages/syringe
