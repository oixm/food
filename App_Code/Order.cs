using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// 订单类
/// </summary>
public class Order
{
    /// <summary>
    /// 保存订单
    /// </summary>
    /// <returns>返回订单 ID</returns>
    public static decimal Save(int UserID, string Address, string Tel, DateTime RequestTime, string Remark, List<Cart.Item> CartItemList, decimal TotalPrice)
    {
        var db = MyDB.Open();

        // 插入 Order
        var sql = @"Insert Into Orders (UserID, OrderTime, TotalPrice, Address, RequestTime, Tel, Status, Remark)
                    Values (@0, GetDate(), @1, @2, @3, @4, 1, @5)";
        db.Execute(sql, 
                   UserID,
                   TotalPrice,
                   Address,
                   RequestTime,
                   Tel,
                   Remark);
        decimal OrderID = db.GetLastInsertId();

        // Temp: 订单编号
        db.Execute("Update Orders Set OrderNo = @0 Where OrderID = @1", OrderID, OrderID);

        // 插入 Order Detail
        foreach (var item in CartItemList)
        {
            sql = @"Insert Into OrderDetail (OrderID, DetailNo, ProductID, Quantity)
                    Values (@0, @1, @2, @3)";
            db.Execute(sql,
                       OrderID,
                       item.No,
                       item.ID,
                       item.Quantity);
        }

        // 返回订单 ID
        return OrderID;
    }

    /// <summary>
    /// 取消订单
    /// </summary>
    public static void Cancel(int OrderID)
    {
        var db = MyDB.Open();
        db.Execute("Update Orders Set Status = -1 Where OrderID = @0", OrderID);
    }
    
    /// <summary>
    /// 删除订单
    /// </summary>
    public static void Delete(int OrderID)
    {
        var db = MyDB.Open();
        db.Execute("Delete From OrderDetail Where OrderID = @0", OrderID);
        db.Execute("Delete From Orders Where OrderID = @0", OrderID);
    }

    /// <summary>
    /// 配送订单
    /// </summary>
    public static void Shipping(int OrderID)
    {
        var db = MyDB.Open();
        db.Execute("Update Orders Set Status = 2 Where OrderID = @0", OrderID);
    }

    /// <summary>
    /// 完成订单
    /// </summary>
    public static void Close(int OrderID)
    {
        var db = MyDB.Open();
        db.Execute("Update Orders Set Status = 3 Where OrderID = @0", OrderID);
    }

    /// <summary>
    /// 返回用户订单列表
    /// </summary>
    public static IEnumerable<dynamic> GetOrdersByUserID(int UserID)
    {
        var db = MyDB.Open();
        return db.Query("Select * From Orders Where UserID = @0 Order By OrderID Desc", UserID);
    }

    /// <summary>
    /// 是否订单所有者
    /// </summary>
    public static bool IsOwner(int UserID, int OrderID)
    {
        var db = MyDB.Open();
        int count = db.QueryValue("Select Count(*) From Orders Where UserID = @0 And OrderID = @1", UserID, OrderID);
        return (count == 0) ? false : true;
    }

    /// <summary>
    /// 返回订单，按 ID
    /// </summary>
    public static dynamic GetOrderByID(int OrderID)
    {
        var db = MyDB.Open();
        var sql = @"Select *
                    From Orders a
                    Inner Join Users b On a.UserID = b.UserID
                    Where OrderID = @0";
        return db.QuerySingle(sql, OrderID);
    }

    /// <summary>
    /// 返回订单明细
    /// </summary>
    public static IEnumerable<dynamic> GetOrderDetails(int OrderID)
    {
        var db = MyDB.Open();
        var sql = @"Select *
                    From Orders a
                    Inner Join OrderDetail b On a.OrderID = b.OrderID
                    Inner Join Product c On b.ProductID = c.ProductID
                    Where a.OrderID = @0";
        return db.Query(sql, OrderID);
    }

    /// <summary>
    /// 返回所有订单
    /// </summary>
    public static IEnumerable<dynamic> GetAllOrders()
    {
        var db = MyDB.Open();
        var sql = @"Select *
                    From Orders a
                    Inner Join Users b On a.UserID = b.UserID
                    Order By a.OrderID Desc";
        return db.Query(sql);
    }

    /// <summary>
    /// 返回订单，按状态
    /// </summary>
    public static IEnumerable<dynamic> GetOrdersByStatus(OrderStatus status)
    {
        var db = MyDB.Open();
        var sql = @"Select *
                    From Orders a
                    Inner Join Users b On a.UserID = b.UserID
                    Where a.Status = @0
                    Order By a.OrderID Desc";
        return db.Query(sql, (int)status);
    }

}
