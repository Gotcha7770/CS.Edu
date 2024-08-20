using System;
using System.Collections.Generic;

namespace CS.Edu.Core.StateMachine;

public class LightStateMachine<TState, TTrigger>
    where TState : Enum
    where TTrigger : Enum
{
    private readonly Dictionary<TState, StateConfiguration<TState, TTrigger>> _configurations = new();

    public TState State { get; protected set; }

    public StateConfiguration<TState, TTrigger> WhenActive(TState state)
    {
        _configurations[state] = new StateConfiguration<TState, TTrigger>(state);

        return _configurations[state];
    }

    public void Fire(TTrigger trigger, params object[] args)
    {
        if(_configurations.TryGetValue(State, out var configuration))
            TryApplay(configuration, trigger, args);
    }

    private void TryApplay(StateConfiguration<TState,TTrigger> configuration, TTrigger trigger, object[] args)
    {
        if (configuration.Transitions.TryGetValue(trigger, out var transition))
        {
            if (transition.Guard())
            {
                State = transition.Target;
            }
        }
    }
}