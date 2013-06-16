using System;
using System.Collections.Generic;
using System.Web;
using System.Web.SessionState;

/// <summary>
/// 管理员身份验证类
/// </summary>
public class Admin
{
    /// <summary>
    /// 返回是否已登录
    /// </summary>
    public static bool IsLogin
    {
        get
        {
            HttpSessionState Session = HttpContext.Current.Session;
            return (Session["Admin"] == null) ? false : true;
        }
    }

    /// <summary>
    /// 登录校验
    /// </summary>
    public static void CheckLogin()
    {
        if (!IsLogin)
        {
            HttpContext.Current.Response.Redirect("~/Admin/Login");
            HttpContext.Current.Response.End();
        }
    }
    
    /// <summary>
    /// 登录
    /// </summary>
    public static LoginStatus Login(string LoginPass)
    {
        string AdminPass = System.Configuration.ConfigurationManager.AppSettings["AdminPass"];
        if (LoginPass != AdminPass)
        {
            return LoginStatus.InvalidLoginPass;    // 密码错误
        }
        else
        {
            HttpSessionState Session = HttpContext.Current.Session;
            Session["Admin"] = "Admin";
            return LoginStatus.Success;     // 登录成功
        }
    }

    /// <summary>
    /// 注销
    /// </summary>
    public static void Logout()
    {
        HttpSessionState Session = HttpContext.Current.Session;
        Session["Admin"] = null;
    }
}