using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// 购物车类
/// </summary>
public class Cart
{
    /// <summary>
    /// 加入购物车
    /// </summary>
    public static void Add(int ProductID, int Quantity)
    {
        HttpCookie cookie = GetCookie();
        if (cookie == null) cookie = new HttpCookie("Cart");
        if (cookie.Values[ProductID.ToString()] == null)
        {
            cookie.Values[ProductID.ToString()] = Quantity.ToString();
        }
        else
        {
            int qty = int.Parse(cookie.Values[ProductID.ToString()]);
            cookie.Values[ProductID.ToString()] = (qty + Quantity).ToString();
        }
        SaveCookie(cookie);
    }

    /// <summary>
    /// 从购物车删除
    /// </summary>
    public static void Delete(int ProductID)
    {
        HttpCookie cookie = GetCookie();
        if (cookie != null)
        {
            if (cookie.Values[ProductID.ToString()] != null)
            {
                cookie.Values.Remove(ProductID.ToString());
                if (cookie.Value == "")
                {
                    Clear();
                }
                else
                {
                    SaveCookie(cookie);
                }
            }
        }
    }

    /// <summary>
    /// 返回购物车清单
    /// </summary>
    public static List<Item> GetItemList()
    {
        HttpCookie cookie = GetCookie();
        List<Item> list = new List<Item>();
        if (cookie != null)
        {
            for (int i = 0; i < cookie.Values.Count; i++)
            {
                Item item = new Item();
                item.No = i + 1;
                item.ID = int.Parse(cookie.Values.GetKey(i));
                item.Quantity = int.Parse(cookie.Values.Get(i));
                var p = Product.GetProductByID(item.ID);
                item.Name = p.ProductName;
                item.Price = p.Price;
                list.Add(item);
            }
        }
        return list;
    }

    /// <summary>
    /// 返回购物车清单数量
    /// </summary>
    public static int GetItemCount()
    {
        HttpCookie cookie = GetCookie();
        if (cookie == null)
        {
            return 0;
        }
        else
        {
            return cookie.Values.Count;
        }
    }

    /// <summary>
    /// 返回订单总价
    /// </summary>
    public static decimal GetTotalPrice()
    {
        decimal total = 0;
        foreach (var item in GetItemList())
        {
            total += item.Price * item.Quantity;
        }
        return total;
    }

    /// <summary>
    /// 清空购物车
    /// </summary>
    public static void Clear()
    {
        HttpCookie cookie = GetCookie();
        cookie.Expires = DateTime.Now.AddDays(-1);
        HttpContext.Current.Response.Cookies.Set(cookie);
    }

    // 获取购物车 Cookie
    private static HttpCookie GetCookie()
    {
        return HttpContext.Current.Request.Cookies["Cart"];
    }

    // 保存购物车 Cookie
    private static void SaveCookie(HttpCookie cookie)
    {
        cookie.Expires = DateTime.Now.AddDays(30);
        HttpContext.Current.Response.Cookies.Set(cookie);
    }

    /// <summary>
    /// 购物清单项
    /// </summary>
    public class Item
    {
        public int No;
        public int ID;
        public string Name;
        public decimal Price;
        public int Quantity;
    }
}
