using System;
using System.Collections.Generic;

namespace CS.Edu.Core.StateMachine;

public readonly struct StateConfiguration<TState, TTrigger>
    where TState : Enum
    where TTrigger : Enum
{
    public readonly Dictionary<TTrigger, (TState Target, Func<bool> Guard)> Transitions;

    public StateConfiguration(TState state)
    {
        Transitions = new Dictionary<TTrigger, (TState, Func<bool>)>();
    }

    public StateConfiguration<TState, TTrigger> CanFire(TTrigger trigger, TState state, Func<bool> func)
    {
        Transitions[trigger] = (state, func);

        return this;
    }

    public StateConfiguration<TState, TTrigger> CanFire(TTrigger trigger, TState state)
    {
        return CanFire(trigger, state, () => true);
    }
}