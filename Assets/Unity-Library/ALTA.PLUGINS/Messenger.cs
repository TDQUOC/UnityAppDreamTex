using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface INotify<T> : INotify
{
    void OnListen(T message);
}

public interface INotify
{
}


public class Messenger
{
    private static Messenger instance;
    private Dictionary<Type, List<INotify>> ListSubcribe;
    public static Messenger Default
    {
        get
        {
            if (instance == null)
            {
                instance = new Messenger();
                instance.ListSubcribe = new Dictionary<Type, List<INotify>>();
            }
            return instance;
        }
    }
    /// <summary>
    /// register for receive messages type of T 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="c"></param>
    public void Subcribe<T>(INotify<T> c)
    {
        if (!ListSubcribe.ContainsKey(typeof(T)))
        {
            var l = new List<INotify>();
            l.Add(c);
            ListSubcribe.Add(typeof(T), l);
        }
        else
        {
            ListSubcribe[typeof(T)].Add(c);
        }
    }

    /// <summary>
    /// push the notification for all listener
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="target"></param>
    public void Publish<T>(T target)
    {
        if (ListSubcribe.ContainsKey(typeof(T)))
            foreach (INotify inoty in ListSubcribe[typeof(T)])
            {
                if (inoty is INotify<T>)
                {
                    INotify<T> tmp = inoty as INotify<T>;
                    tmp.OnListen(target);
                }
            }
    }

    /// <summary>
    /// unregister this message type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="c"></param>
    public void UnSubcribe<T>(INotify<T> c)
    {
        Type type = typeof(T);
        if (ListSubcribe.ContainsKey(type))
        {
            foreach (INotify inoty in ListSubcribe[type])
            {
                if (inoty.GetHashCode() == c.GetHashCode())
                {
                    ListSubcribe[type].Remove(inoty);
                    break;
                }
            }
            if (ListSubcribe[type].Count == 0)
                ListSubcribe.Remove(type);
        }
    }
}

