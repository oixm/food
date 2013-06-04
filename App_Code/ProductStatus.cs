using System.ComponentModel;

/// <summary>
/// 产品状态
/// </summary>
public enum ProductStatus
{
    [Description("在售")]
	OnSale = 1,
    [Description("缺货")]
    OutOfStock = 0,
    [Description("下架")]
    Cancel = -1
}