using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// 用户操作类
/// </summary>
public class Users
{
    /// <summary>
    /// 注册新用户
    /// </summary>
    public static RegisterStatus Register(string LoginName, string LoginPass, string UserName, string Email, string Mobile)
    {
        var db = MyDB.Open();
        if (GetUserByLoginName(LoginName) != null)
        {
            // 帐号重复
            return RegisterStatus.DuplicatedLoginName;
        }
        else
        {
            if (GetUserByEmail(Email) != null)
            {
                // Email 重复
                return RegisterStatus.DuplicatedEmail;
            }
            else
            {
                if (GetUserByMobile(Mobile) != null)
                {
                    // 手机号重复
                    return RegisterStatus.DuplicatedMobile;
                }
                else
                {
                    // 注册成功
                    var sql = @"Insert Into Users (LoginName, LoginPass, UserName, Email, Mobile, RegTime)
                        Values (@0, @1, @2, @3, @4, GetDate())";
                    db.Execute(sql, LoginName, LoginPass, UserName, Email, Mobile);
                    return RegisterStatus.Success;                    
                }
            }
        }
    }
    
    /// <summary>
    /// 返回所有用户
    /// </summary>
    public static dynamic GetAllUsers()
    {
        var db = MyDB.Open();
        var sql = @"Select a.*, b.Birthday, b.Sex, b.Height, b.Weight, b.JobID, c.JobName, b.AreaID, d.AreaName, b.Intro
                    From Users a
                    Left Join UserInfo b On a.UserID = b.UserID
                    Left Join Job c On b.JobID = c.JobID
                    Left Join Area d On b.AreaID = d.AreaID
                    Order By a.UserID Desc";
        return db.Query(sql);
    }

    /// <summary>
    /// 返回用户基本信息，按 LoginName
    /// </summary>
    public static dynamic GetUserByLoginName(string LoginName)
    {
        var db = MyDB.Open();
        return db.QuerySingle("Select * From Users Where LoginName = @0", LoginName);
    }

    /// <summary>
    /// 返回用户基本信息，按 Email
    /// </summary>
    public static dynamic GetUserByEmail(string Email)
    {
        var db = MyDB.Open();
        return db.QuerySingle("Select * From Users Where Email = @0", Email);
    }
    
    /// <summary>
    /// 返回用户基本信息，按 Mobile
    /// </summary>
    public static dynamic GetUserByMobile(string Mobile)
    {
        var db = MyDB.Open();
        return db.QuerySingle("Select * From Users Where Mobile = @0", Mobile);
    }

    /// <summary>
    /// 更新最后登录时间为当前时间
    /// </summary>
    public static void UpdateLastLoginTime(int UserID)
    {
        var db = MyDB.Open();
        db.Execute("Update Users Set LastLoginTime = GetDate() Where UserID = @0", UserID);
    }

    /// <summary>
    /// 修改密码
    /// </summary>
    public static ChangePassStatus ChangePass(int UserID, string OldPass, string NewPass)
    {
        var db = MyDB.Open();
        var count = db.QueryValue("Select Count(*) From Users Where UserID = @0 And LoginPass = @1", UserID, OldPass);
        if (count == 0)
        {
            return ChangePassStatus.InvalidOldPass;
        }
        else
        {
            db.Execute("Update Users Set LoginPass = @0 Where UserID = @1", NewPass, UserID);
            return ChangePassStatus.Success;
        }
    }
        
    /// <summary>
    /// 返回用户详细，按 UserID
    /// </summary>
    public static dynamic GetUserByID(int UserID)
    {
        var db = MyDB.Open();
        var sql = @"Select *
                    From Users a
                    Left Join UserInfo b On a.UserID = b.UserID
                    Left Join Job c On b.JobID = c.JobID
                    Left Join Area d On b.AreaID = d.AreaID
                    Where a.UserID = @0";
        return db.QuerySingle(sql, UserID);
    }

    /// <summary>
    /// 返回用户兴趣爱好
    /// </summary>
    public static IEnumerable<dynamic> GetUserHobby(int UserID)
    {
        var db = MyDB.Open();
        var sql = @"Select *
                    From UserHobby a
                    Inner Join Users b On a.UserID = b.UserID
                    Inner Join Hobby c On a.HobbyID = c.HobbyID
                    Where a.UserID = @0";
        return db.Query(sql, UserID);
    }
    
    /// <summary>
    /// 删除用户
    /// </summary>
    public static void Delete(int UserID)
    {
        var db = MyDB.Open();
        db.Execute("Delete From UserInfo Where UserID = @0", UserID);
        db.Execute("Delete From Users Where UserID = @0", UserID);
    }
}