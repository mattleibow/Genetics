using Android.Content;
using Android.Views;
using Syringe.Mappings;

namespace Syringe.EventNeedles
{
    public interface IEventNeedle
    {
        bool Inject(object target, object source, View view, Context context, MethodMapping methodMapping);

        void Withdraw(object target, object source, View view, Context context, MethodMapping methodMapping);
    }
}