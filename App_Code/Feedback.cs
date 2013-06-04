using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// 意见反馈
/// </summary>
public class Feedback
{
    /// <summary>
    /// 返回所有意见
    /// </summary>
    public static IEnumerable<dynamic> GetAllFeedbacks()
    {
        var db = MyDB.Open();
        var sql = @"Select *
                    From Feedback a
                    Inner Join Users b On a.UserID = b.UserID
                    Order By a.ID Desc";
        return db.Query(sql);
    }

    /// <summary>
    /// 新增意见
    /// </summary>
    public static void Add(int UserID, string Suggestion)
    {
        var db = MyDB.Open();
        var sql = "Insert Into Feedback (UserID, Suggestion, PostTime) Values (@0, @1, GetDate())";
        db.Execute(sql, UserID, Suggestion);
    }
    
    /// <summary>
    /// 删除意见
    /// </summary>
    public static void Delete(int ID)
    {
        var db = MyDB.Open();
        var sql = "Delete From Feedback Where ID = @0";
        db.Execute(sql, ID);
    }
}
