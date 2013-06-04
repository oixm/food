using System.ComponentModel;

/// <summary>
/// 登录状态
/// </summary>
public enum LoginStatus
{
    [Description("登录成功！")]
    Success = 0,
    [Description("没有这个用户.")]
    InvalidLoginName = -1,
    [Description("密码错误！")]
    InvalidLoginPass = -2
}