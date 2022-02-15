using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System;
using Object = UnityEngine.Object;

public class Variable<T> : ScriptableObject
{
    public bool IsArray = false;
    [DisableIf("IsArray")]
    public T Value;
    [EnableIf("IsArray")]
    public List<T> Values;

    public event Action onValueChanged;

    public void ChangeValue(T newValue)
    {
        Value = newValue;
        onValueChanged?.Invoke();
    }

    public void ChangeValue(List<T> newValues)
    {
        Values = newValues;
        onValueChanged?.Invoke();
    }
}

[CreateAssetMenu(menuName = "Variables/Asset object")]
public class AssetObject : Variable<Object>
{
}