using System.ComponentModel;

/// <summary>
/// 订单状态
/// </summary>
public enum OrderStatus
{
    [Description("已提交")]
	Submit = 1,
    [Description("配送中")]
    Shipping = 2,
    [Description("已完成")]
    Close = 3,
    [Description("已取消")]
    Cancel = -1
}