using System;
using System.Collections.Generic;
using System.Linq;

namespace CS.Edu.Core
{
    public class Arg
    {
        public Info Content { get; set; } //это типизировать
    }

    public class ArgExt : Arg
    {
        public IEnumerable<Info> Dependencies { get; set; }
    }

    public class Result { }

    public class Info { }

    public class Info<T> : Info { }

    public interface IUtilWrapper<T>
    {
        T Compute(params object[] typeInputValue);
    }

    public interface IProcessConditions { }

    public interface IInfoProvider
    {
        IUtilWrapper<T> LoadWrapper<T>(Arg arg);

        IProcessConditions LoadConditions(Arg arg);

        Info GetInfo(Arg arg);
    }

    public class ConditionsResource<T>
    {
        public IProcessConditions Conditions { get; internal set; }
        public IUtilWrapper<object> UtilWrapper { get; internal set; }
    }

    public abstract class CalculatedCurveProviderBase<T> : IInfoProvider
    {        
        public virtual Info GetInfo(Arg arg) //расширение тут
        {
            return arg.Content;
        }

        public IProcessConditions LoadConditions(Arg arg)
        {
            var conditionsResource = GetConditionsResource(arg);

            return conditionsResource.Conditions;
        }

        public IUtilWrapper<TWrapper> LoadWrapper<TWrapper>(Arg arg)
        {
            var conditionsResource = GetConditionsResource(arg);

            var util = conditionsResource.UtilWrapper as IUtilWrapper<TWrapper>;

            return util;
        }

        private ConditionsResource<T> GetConditionsResource(Arg arg)
        {
            var conditionsResource = new ConditionsResource<T>();
            CalculateElement(arg, conditionsResource);

            return conditionsResource;
        }

        protected abstract void CalculateElement(Arg arg, ConditionsResource<T> conditionsResource);
    }

    public class CalculatedCurveProvider : CalculatedCurveProviderBase<Result>
    {
        public override Info GetInfo(Arg arg)
        {
            var argExt = arg as ArgExt;
            var content = arg.Content as Info<double>;
            var dep = argExt.Dependencies.FirstOrDefault(x => x is Info<DateTime>);

            return new Info<double>(); //вот этот тип сохранить бы
        }
        protected override void CalculateElement(Arg arg, ConditionsResource<Result> conditionsResource)
        {
            var argExt = arg as ArgExt;
            var content = arg.Content as Info<double>;
            var dep = argExt.Dependencies.FirstOrDefault(x => x is Info<DateTime>);
        }
    }
}
