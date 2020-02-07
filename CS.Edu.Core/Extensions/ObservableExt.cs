using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Linq;
using System.Reflection;

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

            MemberInfo member = ((MemberExpression)expression.Body).Member;
            string propertyName = member.Name;

            // return Observable.FromEventPattern<PropertyChangedEventArgs>(
            //     x => source.PropertyChanged += x,
            //     x => source.PropertyChanged -= x)

            return Observable.FromEvent<PropertyChangedEventHandler, string>(
                x =>
                {
                    void Handler(object eventSender, PropertyChangedEventArgs e) => x(e.PropertyName);
                    return Handler;
                },
                x => source.PropertyChanged += x,
                x => source.PropertyChanged -= x)
            .Where(x => string.IsNullOrEmpty(x) || x.Equals(propertyName, StringComparison.InvariantCulture))
            .Select(x => (TValue)GetValueFetcherForProperty(member)(source, Array.Empty<object>()));
        }

        public static Func<object, object[], object> GetValueFetcherForProperty(MemberInfo member)
        {
            return member switch
            {
                FieldInfo f => (obj, _) => f.GetValue(obj),
                PropertyInfo p => (obj, _) => p.GetValue(obj),
                _ => null
            };
        }
    }
}