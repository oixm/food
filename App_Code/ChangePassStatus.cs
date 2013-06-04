using System.ComponentModel;

/// <summary>
/// 修改密码状态
/// </summary>
public enum ChangePassStatus
{
    [Description("密码修改成功！")]
    Success = 0,
    [Description("原始密码错误.")]
    InvalidOldPass= -1
}