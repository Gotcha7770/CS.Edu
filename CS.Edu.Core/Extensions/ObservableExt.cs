using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Linq;

namespace CS.Edu.Core.Extensions
{
    public static class ObservableExt
    {
        public static IObservable<TValue> CreateFromProperty<TSender, TValue>(TSender source,
                                                                              Expression<Func<TSender, TValue>> expression)
            where TSender : INotifyPropertyChanged
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            Func<TSender, TValue> getter = expression.Compile();
            string propertyName = ((MemberExpression)expression.Body).Member.Name;

            return Observable.FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
                x => source.PropertyChanged += x,
                x => source.PropertyChanged -= x)
            .Where(x => propertyName.Equals(x.EventArgs.PropertyName, StringComparison.InvariantCulture))
            .Select(x => getter(source));
        }
    }
}