using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;




public class UEventHandler
{

    #region UEvent
    public abstract class UEventBase
    {
        public abstract void Unsubscribe(object action);

    }
    public class UEvent : UEventBase
    {
        private event Action eventData;
        private Dictionary<int, Action> registedEvents = new Dictionary<int, Action>();
        public void TryInvoke() => eventData?.Invoke();
        public uint Subscribe(UEventHandler source, Action action, bool singleCall = false)
        {
            CheckInstatiated(ref source);
            eventData += action;

            if (singleCall)
                eventData += () => Unsubscribe(action);

            var id = source.RegisterEvent(this, action);

            UnsubscribeAction unsubAction = null;
            unsubAction = (e) =>
           {
               if (e != source) return;

               source.OnGlobalUnsub -= unsubAction;
               Unsubscribe(action);
           };
            source.OnGlobalUnsub += unsubAction;

            return id;
        }
        public override void Unsubscribe(object action) => Unsubscribe((Action)action);
        public void Unsubscribe(Action action) => eventData -= action;


    }



    public class UEvent<T> : UEventBase
    {
        private event Action<T> eventData;
        public void TryInvoke(T arg1) => eventData?.Invoke(arg1);
        public uint Subscribe(UEventHandler source, Action<T> action, bool singleCall = false)
        {
            CheckInstatiated(ref source);
            eventData += action;

            if (singleCall)
                eventData += (x) => Unsubscribe(action);

            var id = source.RegisterEvent(this, action);

            UnsubscribeAction unsubAction = null;
            unsubAction = (e) =>
            {
                if (e != source) return;
                source.OnGlobalUnsub -= unsubAction;
                Unsubscribe(action);
            };
            source.OnGlobalUnsub += unsubAction;
            return id;
        }
        public override void Unsubscribe(object action) => Unsubscribe((Action<T>)action);
        public void Unsubscribe(Action<T> action) => eventData -= action;
    }

    public class UEvent<T1, T2> : UEventBase
    {
        private event Action<T1, T2> eventData;
        public void TryInvoke(T1 arg1, T2 arg2) => eventData?.Invoke(arg1, arg2);
        public uint Subscribe(UEventHandler source, Action<T1, T2> action, bool singleCall = false)
        {
            CheckInstatiated(ref source);
            eventData += action;

            if (singleCall)
                eventData += (x, y) => Unsubscribe(action);

            var id = source.RegisterEvent(this, action);

            UnsubscribeAction unsubAction = null;
            unsubAction = (e) =>
            {
                if (e != source) return;
                source.OnGlobalUnsub -= unsubAction;
                Unsubscribe(action);
            };
            source.OnGlobalUnsub += unsubAction;
            return id;
        }
        public override void Unsubscribe(object action) => Unsubscribe((Action<T1, T2>)action);
        public void Unsubscribe(Action<T1, T2> action) => eventData -= action;

    }

    public class UEvent<T1, T2, T3> : UEventBase
    {
        private event Action<T1, T2, T3> eventData;
        public void TryInvoke(T1 arg1, T2 arg2, T3 arg3) => eventData?.Invoke(arg1, arg2, arg3);
        public uint Subscribe(UEventHandler source, Action<T1, T2, T3> action, bool singleCall = false)
        {
            CheckInstatiated(ref source);
            eventData += action;

            if (singleCall)
                eventData += (x, y, z) => Unsubscribe(action);

            var id = source.RegisterEvent(this, action);


            UnsubscribeAction unsubAction = null;
            unsubAction = (e) =>
            {
                if (e != source) return;
                source.OnGlobalUnsub -= unsubAction;
                Unsubscribe(action);
            };
            source.OnGlobalUnsub += unsubAction;
            return id;
        }
        public override void Unsubscribe(object action) => Unsubscribe((Action<T1, T2, T3>)action);
        public void Unsubscribe(Action<T1, T2, T3> action) => eventData -= action;
    }

    private static void CheckInstatiated(ref UEventHandler uEventHandler)
    {
        if (uEventHandler != null) return;
        Debug.LogError($"This uEventHandler ({uEventHandler}) is not instantiated\nConsider instatiating it with: eventHandler= new UEventHandler();\nWill instantiate for debug purposes");
        uEventHandler = new UEventHandler();
    }

    #endregion UEvent

    public delegate void UnsubscribeAction(UEventHandler eventHandler);

    private Dictionary<uint, (UEventBase, object)> eventsRegisted = new Dictionary<uint, (UEventBase, object)>();
    private static uint idCounter = 0;

    //private Dictionary<object, UEventBase> eventsData = new Dictionary<object, UEventBase>();
    private event UnsubscribeAction OnGlobalUnsub;
    public void UnsubcribeAll()
    {
        OnGlobalUnsub?.Invoke(this);
    }

    protected uint RegisterEvent(UEventBase uEvent, object action)
    {
        var id = GenerateId();
        eventsRegisted.Add(id, (uEvent, action));
        return id;
    }

    public void UnsubcribeAllOfEvent(UEventBase uEvent)
    {
        var allDataOfEvent = eventsRegisted.Where(x => x.Value.Item1 == uEvent).ToList();
        foreach (var eventEntry in allDataOfEvent)
        {
            uEvent.Unsubscribe(eventEntry.Value.Item2);
            RemoveEntry(eventEntry.Key);
        }
    }

    public void UnsubcribeById(uint id)
    {
        if (!eventsRegisted.ContainsKey(id)) return;

        var eventEntry = eventsRegisted[id];
        eventEntry.Item1.Unsubscribe(eventEntry.Item2);

        RemoveEntry(id);
    }

    private static uint GenerateId()
    {
        idCounter++;
        return idCounter;
    }
    private void RemoveEntry(uint id)
    {
        var eventEntry = eventsRegisted[id];
        RemoveEntry(eventEntry.Item2);
    }

    private void RemoveEntry(object action)
    {
        var entry = eventsRegisted.FirstOrDefault(x => x.Value.Item2 == action);
        eventsRegisted.Remove(entry.Key);
    }
}
