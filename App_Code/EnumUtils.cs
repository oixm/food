using System;
using System.Collections.Generic;
using System.Reflection;
using System.ComponentModel;

/// <summary>
/// 枚举描述操作类
/// </summary>
public class EnumUtils
{
    /// <summary>
    /// 返回枚举类型指定值的描述
    /// </summary>
    /// <param name="enumType">枚举类型，例如：typeof(OrderStatus)</param>
    /// <param name="value">值，例如：2</param>
    /// <returns>枚举值描述，例如：配送中</returns>
	public static string GetDescription(Type enumType, object value)
    {
        FieldInfo fi = enumType.GetField(Enum.GetName(enumType, value));
        object[] objs = fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
        DescriptionAttribute da = (DescriptionAttribute)objs[0];
        return da.Description;
    }

    /// <summary>
    /// 返回枚举类型指定值的描述
    /// </summary>
    /// <param name="enumItem">枚举值，例如：OrderStatus.Shipping</param>
    /// <returns>枚举值描述，例如：配送中</returns>
    public static string GetDescription(object enumItem)
    {
        Type enumType = enumItem.GetType();
        return GetDescription(enumType, enumItem);
    }
}