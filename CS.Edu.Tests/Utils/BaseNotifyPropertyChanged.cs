using System.Runtime.CompilerServices;
using DynamicData.Binding;

namespace CS.Edu.Tests.Utils;

class BaseNotifyPropertyChanged  : AbstractNotifyPropertyChanged
{
    protected override void SetAndRaise<T>(ref T backingField, T newValue, [CallerMemberName] string propertyName = null)
    {
        if (!Equals(backingField, newValue))
        {
            backingField = newValue;
            OnPropertyChanged(propertyName);
        }
    }
}