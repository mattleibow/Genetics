using Android.Content;
using Android.Views;
using Genetics.Mappings;

namespace Genetics.EventGenes
{
    public interface IEventGene
    {
        bool Splice(object target, object source, View view, Context context, MethodMapping methodMapping);

        void Sever(object target, object source, View view, Context context, MethodMapping methodMapping);
    }
}