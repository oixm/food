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

    /// <summary>
    /// 更新幻灯片
    /// </summary>
    public static void Update(int ID, string Title, string Link, int Sort)
    {
        var db = MyDB.Open();
        var sql = @"Update Slide Set
                    Title = @1,
                    Link = @2,
                    Sort = @3
                    Where ID = @0";
        db.Execute(sql, ID, Title, Link, Sort);
    }
    
    /// <summary>
    /// 删除幻灯片
    /// </summary>
    public static void Delete(int ID)
    {
        var db = MyDB.Open();
        db.Execute("Delete From Slide Where ID = @0", ID);
    }
}
