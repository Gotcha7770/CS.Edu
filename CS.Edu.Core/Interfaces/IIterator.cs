using System;
using System.Collections.Generic;
using CS.Edu.Core.StateMachine;

namespace CS.Edu.Core.Interfaces
{
    public interface IIterator<out T, TState, TTrigger> : IEnumerator<T>
        where TState : Enum
        where TTrigger : Enum
    {
        LightStateMachine<TState, TTrigger> StateMachine { get; }
    }
}