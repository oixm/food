using System;
using System.Collections.Generic;
using System.Web;
using WebMatrix.Data;

/// <summary>
/// 数据库类
/// </summary>
public class MyDB
{
    /// <summary>
    /// 打开并返回数据库操作类
    /// </summary>
    /// <returns></returns>
    public static Database Open()
    {
        return Database.Open("food");
    }
}
