using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;
using static UnityEngine.Rendering.HDROutputUtils;


public class Upgrade : MonoBehaviour
{
    public enum Operation { Add, Set }
    public Operation operation = Operation.Add;
    public upgrade upgrade;
    public void updateUI()
    {
        GetComponent<Image>().sprite = upgrade.icon;
    }
    public void applyUpgrade()
    {
        var type = upgrade.componentToUpgrade.GetType();
        var field = type.GetField(upgrade.variableName);
        var prop = type.GetProperty(upgrade.variableName);
        if (field != null)
        {
            float current = Convert.ToSingle(field.GetValue(upgrade.componentToUpgrade));
            float result = operation == Operation.Add ? current + upgrade.valueChange : upgrade.valueChange;
            field.SetValue(upgrade.componentToUpgrade, Convert.ChangeType(result, field.FieldType));
        }
        else if (prop != null && prop.CanWrite)
        {
            float current = Convert.ToSingle(prop.GetValue(upgrade.componentToUpgrade));
            float result = operation == Operation.Add ? current + upgrade.valueChange : upgrade.valueChange;
            prop.SetValue(upgrade.componentToUpgrade, Convert.ChangeType(result, prop.PropertyType));
        }
    }
}
