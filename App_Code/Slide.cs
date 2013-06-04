using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// 幻灯片
/// </summary>
public class Slide
{
    /// <summary>
    /// 返回所有幻灯片
    /// </summary>
    public static IEnumerable<dynamic> GetSlides()
    {
        var db = MyDB.Open();
        return db.Query("Select * From Slide Order By Sort");
    }
}
