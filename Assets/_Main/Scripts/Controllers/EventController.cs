using System;
using System.Collections.Generic;

namespace _Main.Scripts.Controllers
{
    public static class EventController
    {
        private static readonly Dictionary<string, Delegate> EventDictionary = new();

        public static void Subscribe<T>(string eventName, Action<T> listener)
        {
            if (EventDictionary.TryGetValue(eventName, out var existingDelegate))
            {
                EventDictionary[eventName] = existingDelegate is Action<T> existingAction
                    ? existingAction + listener
                    : throw new InvalidOperationException($"Event {eventName} has a different type!");
            }
            else
            {
                EventDictionary[eventName] = listener;
            }
        }

        public static void Unsubscribe<T>(string eventName, Action<T> listener)
        {
            if (EventDictionary.TryGetValue(eventName, out var existingDelegate) && existingDelegate is Action<T> existingAction)
            {
                existingAction -= listener;

                if (existingAction == null)
                {
                    EventDictionary.Remove(eventName);
                }
                else
                {
                    EventDictionary[eventName] = existingAction;
                }
            }
        }
        public static bool IsListenerSubscribed<T>(string eventName)
        {
            return EventDictionary.TryGetValue(eventName, out var existingDelegate) && existingDelegate is Action<T>;
        }

        public static void Trigger<T>(string eventName, T param)
        {
            if (!IsListenerSubscribed<T>(eventName)) return;

            if (EventDictionary[eventName] is Action<T> action)
            {
                action.Invoke(param);
            }
        }

        public static void Trigger(string eventName)
        {
            if (!IsListenerSubscribed<Action>(eventName)) return;

            if (EventDictionary[eventName] is Action action)
            {
                action.Invoke();
            }
        }
    }

    public static class EventNames
    {
        public const string OnLevelWin = "OnLevelWin";
    }
}