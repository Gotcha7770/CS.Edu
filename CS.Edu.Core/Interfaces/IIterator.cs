using System;
using System.Collections.Generic;

namespace CS.Edu.Core.Iterators
{
    public struct StateConfiguration<TState, TTrigger>
        where TState : Enum
        where TTrigger : Enum
    {
        public readonly Dictionary<TTrigger, (TState, Func<bool>)> Transitions;

        public TState State { get; }

        public StateConfiguration(TState state)
        {
            Transitions = new Dictionary<TTrigger, (TState, Func<bool>)>();

            State = state;
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

    public class LightStateMachine<TState, TTrigger>
        where TState : Enum
        where TTrigger : Enum
    {
        private readonly Dictionary<TState, StateConfiguration<TState, TTrigger>> _configurations
            = new Dictionary<TState, StateConfiguration<TState, TTrigger>>();

        public TState State { get; private set; }

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
                if (transition.Item2())
                {
                    State = transition.Item1;
                }
            }
        }
    }

    public interface IIterator<out T, TState, TTrigger> : IEnumerator<T>
        where TState : Enum
        where TTrigger : Enum
    {
        LightStateMachine<TState, TTrigger> StateMachine { get; }
    }
}