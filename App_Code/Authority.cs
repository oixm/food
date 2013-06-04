using System;
using System.Collections.Generic;
using System.Web;
using System.Web.SessionState;

/// <summary>
/// 身份验证类
/// </summary>
public class Authority
{
    /// <summary>
    /// 返回是否已登录
    /// </summary>
    public static bool IsLogin
    {
        get
        {
            HttpSessionState Session = HttpContext.Current.Session;
            return (Session["User"] == null) ? false : true;
        }
    }

    /// <summary>
    /// 登录校验
    /// </summary>
    public static void CheckLogin()
    {
        CheckLogin(false);
    }

    /// <summary>
    /// 登录校验
    /// </summary>
    /// <param name="isBackAfterLogin">登录成功后跳转回当前页面</param>
    public static void CheckLogin(bool isBackAfterLogin)
    {
        if (!IsLogin)
        {
            string url;
            if (isBackAfterLogin)
            {
                url = "~/Login?BackURL=" + HttpContext.Current.Server.UrlEncode(HttpContext.Current.Request.Url.ToString());
            }
            else
            {
                url = "~/Login";
            }
            HttpContext.Current.Response.Redirect(url);
            HttpContext.Current.Response.End();
        }
    }

    /// <summary>
    /// 返回当前登录用户
    /// </summary>
    public static  UserInfo CurrentUser
    {
        get
        {
            HttpSessionState Session = HttpContext.Current.Session;
            return (UserInfo)Session["User"];
        }
    }

    /// <summary>
    /// 登录
    /// </summary>
    public static LoginStatus Login(string LoginName, string LoginPass)
    {
        var u = Users.GetUserByLoginName(LoginName);
        if (u == null)
        {
            return LoginStatus.InvalidLoginName;    // 用户不存在
        }
        else
        {
            if (u.LoginPass != LoginPass)
            {
                return LoginStatus.InvalidLoginPass;    // 密码错误
            }
            else
            {
                // 更新最后登录时间
                Users.UpdateLastLoginTime(u.UserID);

                // 保存至 Session
                var su = new UserInfo();
                su.UserID = u.UserID;
                su.LoginName = u.LoginName;
                su.UserName = u.UserName;
                su.Email = u.Email;
                su.Mobile = u.Mobile;
                HttpSessionState Session = HttpContext.Current.Session;
                Session["User"] = su;

                return LoginStatus.Success;     // 登录成功
            }
        }
    }

    /// <summary>
    /// 注销
    /// </summary>
    public static void Logout()
    {
        HttpSessionState Session = HttpContext.Current.Session;
        Session["User"] = null;
    }
}
