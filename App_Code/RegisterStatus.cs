using System.ComponentModel;

/// <summary>
/// 注册状态
/// </summary>
public enum RegisterStatus
{
    [Description("注册成功！")]
    Success = 0,
    [Description("用户名已经存在！")]
    DuplicatedLoginName = -1,
    [Description("Email 已经存在！")]
    DuplicatedEmail = -2,
    [Description("手机号已经存在！")]
    DuplicatedMobile = -3
}