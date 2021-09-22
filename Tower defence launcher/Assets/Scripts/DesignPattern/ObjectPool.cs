using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class ObjectPool<T> where T : IPoolable
{
    private List<T> activePool = new List<T>();
    private List<T> inactivePool = new List<T>();

    public T RequestObject()
    {
        if (inactivePool.Count > 0)
        {
            return ActivateItem(inactivePool[0]);
        }
        return ActivateItem(AddNewItemToPool());
    }

    public T ActivateItem(T _item)
    {
        _item.OnEnableObject();
        _item.active = true;

        if (inactivePool.Contains(_item))
        {
            inactivePool.Remove(_item);
        }
        activePool.Add(_item);
        return _item;
    }

    public void ReturnObjectToPool(T _item)
    {
        if (activePool.Contains(_item))
        {
            activePool.Remove(_item);
        }

        _item.OnDisableObject();
        _item.active = false;
        inactivePool.Add(_item);
    }

    private T AddNewItemToPool()
    {
        T instance = (T)Activator.CreateInstance(typeof(T));
        inactivePool.Add(instance);
        Debug.Log("A new item was added");
        return instance;
    }
}
