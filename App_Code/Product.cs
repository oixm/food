using System;
using System.Collections.Generic;
using System.Web;
using WebMatrix.Data;

/// <summary>
/// 产品类
/// </summary>
public class Product
{
    /// <summary>
    /// 返回所有产品
    /// </summary>
    public static IEnumerable<dynamic> GetAllProducts()
    {
        var db = MyDB.Open();
        var sql = @"Select *
                    From Product a
                    Inner Join Category b On a.CategoryID = b.CategoryID
                    Order By a.ProductID Desc";
        return db.Query(sql);
    }

    /// <summary>
    /// 返回最新 N 项产品
    /// </summary>
    public static IEnumerable<dynamic> GetTopProducts(int TopN)
    {
        var db = MyDB.Open();
        return db.Query("Select Top(@0) * From Product Where Status >= 0 Order By ProductID Desc", TopN);
    }
    
    /// <summary>
    /// 返回有产品的类别
    /// </summary>
    public static IEnumerable<dynamic> GetCategoriesByHasProduct()
    {
        var db = MyDB.Open();
        var sql = @"Select * From Category c
                    Where (Select Count(*) From Product Where CategoryID = c.CategoryID And Status >= 0) <> 0";
        return db.Query(sql);
    }
    
    /// <summary>
    /// 返回最新 N 项产品，按类别
    /// </summary>
    public static IEnumerable<dynamic> GetTopProductsByCategoryID(int CategoryID, int TopN)
    {
        var db = MyDB.Open();
        return db.Query("Select Top(@1) * From Product Where CategoryID = @0 And Status >= 0 Order By ProductID Desc", CategoryID, TopN);
    }
    
    /// <summary>
    /// 返回产品，按状态
    /// </summary>
    public static IEnumerable<dynamic> GetProductsByStatus(ProductStatus status)
    {
        var db = MyDB.Open();
        var sql = "Select * From Product Where Status = @0";
        return db.Query(sql, (int)status);
    }

    /// <summary>
    /// 返回产品，按 ID
    /// </summary>
    public static dynamic GetProductByID(int ProductID)
    {
        var db = MyDB.Open();
        var sql = @"Select *
                    From Product a
                    Left Join ProductInfo b On a.ProductID = b.ProductID
                    Inner Join Category c On a.CategoryID = c.CategoryID
                    Where a.ProductID = @0";
        return db.QuerySingle(sql, ProductID);
    }

    /// <summary>
    /// 返回产品数量，按类别
    /// </summary>
    public static int GetCountByCategoryID(int CategoryID)
    {
        var db = MyDB.Open();
        return db.QueryValue("Select Count(*) From Product Where CategoryID = @0 And Status >= 0", CategoryID);
    }
    
    /// <summary>
    /// 返回产品列表，按类别
    /// </summary>
    public static IEnumerable<dynamic> GetProductsByCategoryID(int CategoryID)
    {
        var db = MyDB.Open();
        return db.Query("Select * From Product Where CategoryID = @0 And Status >= 0 Order By ProductID Desc", CategoryID);
    }
    
    /// <summary>
    /// 新增产品
    /// </summary>
    public static decimal Add(string ProductName, int CategoryID, decimal Price, int Inventory, int Hot, int Status)
    {
        var db = MyDB.Open();
        var sql = "Insert Into Product (ProductName, CategoryID, Price, Status, Inventory, Hot) Values (@0, @1, @2, @3, @4, @5)";
        db.Execute(sql, ProductName, CategoryID, Price, Status, Inventory, Hot);
        decimal ProductID = db.GetLastInsertId();
        sql = "Insert Into ProductInfo (ProductID) Values (@0)";
        db.Execute(sql, ProductID);
        return ProductID;
    }
    
    /// <summary>
    /// 更新产品基本信息
    /// </summary>
    public static void Update(int ProductID, string ProductName, int CategoryID, decimal Price, int Inventory, int Hot, int Status)
    {
        var db = MyDB.Open();
        var sql = @"Update Product Set
                    ProductName = @1,
                    CategoryID = @2,
                    Price = @3,
                    Inventory = @4,
                    Hot = @5,
                    Status = @6
                    Where ProductID = @0";
        db.Execute(sql, ProductID, ProductName, CategoryID, Price, Inventory, Hot, Status);
    }
    
    /// <summary>
    /// 更新产品图片
    /// </summary>
    public static void UpdatePhoto(int ProductID, string FileName)
    {
        var db = MyDB.Open();
        var sql = "Update Product Set Photo = @1 Where ProductID = @0";
        db.Execute(sql, ProductID, FileName);
    }
    
    /// <summary>
    /// 更新产品介绍
    /// </summary>
    public static void UpdateDescription(int ProductID, string Description)
    {
        var db = MyDB.Open();
        var sql = "Update ProductInfo Set Description = @1 Where ProductID = @0";
        db.Execute(sql, ProductID, Description);
    }
    
    /// <summary>
    /// 更新产品特征
    /// </summary>
    public static void UpdateStory(int ProductID, string Story)
    {
        var db = MyDB.Open();
        var sql = "Update ProductInfo Set Story = @1 Where ProductID = @0";
        db.Execute(sql, ProductID, Story);
    }
    
    /// <summary>
    /// 更新制作过程
    /// </summary>
    public static void UpdateProcess(int ProductID, string Process)
    {
        var db = MyDB.Open();
        var sql = "Update ProductInfo Set Process = @1 Where ProductID = @0";
        db.Execute(sql, ProductID, Process);
    }
    
    /// <summary>
    /// 删除产品
    /// </summary>
    public static void Delete(int ProductID)
    {
        var db = MyDB.Open();
        db.Execute("Delete From ProductMaterial Where ProductID = @0", ProductID);
        db.Execute("Delete From ProductInfo Where ProductID = @0", ProductID);
        db.Execute("Delete From Product Where ProductID = @0", ProductID);
    }
    
    /// <summary>
    /// 返回所有类型
    /// </summary>
    public static IEnumerable<dynamic> GetAllCategory()
    {
        var db = MyDB.Open();
        return db.Query("Select * From Category");
    }
    
    /// <summary>
    /// 返回类型，按 ID
    /// </summary>
    public static dynamic GetCategoryByID(int CategoryID)
    {
        var db = MyDB.Open();
        return db.QuerySingle("Select * From Category Where CategoryID = @0", CategoryID);
    }

    /// <summary>
    /// 新增类型
    /// </summary>
    public static decimal AddCategory(string CategoryName)
    {
        var db = MyDB.Open();
        var sql = "Insert Into Category (CategoryName) Values (@0)";
        db.Execute(sql, CategoryName);
        return db.GetLastInsertId();
    }
    
    /// <summary>
    /// 更新类型
    /// </summary>
    public static void UpdateCategory(int CategoryID, string CategoryName)
    {
        var db = MyDB.Open();
        var sql = "Update Category Set CategoryName = @1 Where CategoryID = @0";
        db.Execute(sql, CategoryID, CategoryName);
    }
    
    /// <summary>
    /// 删除类型
    /// </summary>
    public static void DeleteCategory(int CategoryID)
    {
        var db = MyDB.Open();
        var sql = "Delete From Category Where CategoryID = @0";
        db.Execute(sql, CategoryID);
    }

    /// <summary>
    /// 返回产品制作材料
    /// </summary>
    public static IEnumerable<dynamic> GetProductMaterial(int ProductID)
    {
        var db = MyDB.Open();
        var sql = @"Select a.*, b.MaterialName, b.UnitID, c.UnitName
                    From ProductMaterial a
                    Inner Join Material b On a.MaterialID = b.MaterialID
                    Inner Join Unit c On b.UnitID = c.UnitID
                    Where a.ProductID = @0";
        return db.Query(sql, ProductID);
    }
    
    /// <summary>
    /// 新增产品制作材料
    /// </summary>
    public static void AddProductMaterial(int ProductID, int MaterialID, int Quantity)
    {
        var db = MyDB.Open();
        var sql = "Insert Into ProductMaterial (ProductID, MaterialID, Quantity) Values (@0, @1, @2)";
        db.Execute(sql, ProductID, MaterialID, Quantity);
    }
    
    /// <summary>
    /// 删除产品制作材料
    /// </summary>
    public static void DeleteProductMaterial(int ProductID, int MaterialID)
    {
        var db = MyDB.Open();
        var sql = "Delete From ProductMaterial Where ProductID = @0 And MaterialID = @1";
        db.Execute(sql, ProductID, MaterialID);
    }
    
    /// <summary>
    /// 返回所有制作材料
    /// </summary>
    public static IEnumerable<dynamic> GetAllMaterials()
    {
        var db = MyDB.Open();
        var sql = @"Select a.*, b.UnitName
                    From Material a
                    Inner Join Unit b On a.UnitID = b.UnitID
                    Order By a.MaterialID";
        return db.Query(sql);
    }

    /// <summary>
    /// 返回制作材料，按 ID
    /// </summary>
    public static dynamic GetMaterialByID(int MaterialID)
    {
        var db = MyDB.Open();
        return db.QuerySingle("Select * From Material Where MaterialID = @0", MaterialID);
    }
    
    /// <summary>
    /// 新增制作材料
    /// </summary>
    public static decimal AddMaterial(string MaterialName, int UnitID)
    {
        var db = MyDB.Open();
        var sql = "Insert Into Material (MaterialName, UnitID) Values (@0, @1)";
        db.Execute(sql, MaterialName, UnitID);
        return db.GetLastInsertId();
    }
    
    /// <summary>
    /// 更新制作材料
    /// </summary>
    public static void UpdateMaterial(int MaterialID, string MaterialName, int UnitID)
    {
        var db = MyDB.Open();
        var sql = "Update Material Set MaterialName = @1, UnitID = @2 Where MaterialID = @0";
        db.Execute(sql, MaterialID, MaterialName, UnitID);
    }
    
    /// <summary>
    /// 删除制作材料
    /// </summary>
    public static void DeleteMaterial(int MaterialID)
    {
        var db = MyDB.Open();
        db.Execute("Delete From MaterialNutrition Where MaterialID = @0", MaterialID);
        db.Execute("Delete From Material Where MaterialID = @0", MaterialID);
    }

    /// <summary>
    /// 返回材料营养成分
    /// </summary>
    public static IEnumerable<dynamic> GetMaterialNutrition(int MaterialID)
    {
        var db = MyDB.Open();
        var sql = @"Select * 
                    From MaterialNutrition a
                    Inner Join Material b On a.MaterialID = b.MaterialID
                    Inner Join Nutrition c On a.NutritionID = c.NutritionID
                    Inner Join Unit d On c.UnitID = d.UnitID
                    Where a.MaterialID = @0";
        return db.Query(sql, MaterialID);
    }
    
    /// <summary>
    /// 新增材料营养成分
    /// </summary>
    public static void AddMaterialNutrition(int MaterialID, int NutritionID, decimal Quantity)
    {
        var db = MyDB.Open();
        var sql = "Insert Into MaterialNutrition (MaterialID, NutritionID, Quantity) Values (@0, @1, @2)";
        db.Execute(sql, MaterialID, NutritionID, Quantity);
    }
    
    /// <summary>
    /// 删除材料营养成分
    /// </summary>
    public static void DeleteMaterialNutrition(int MaterialID, int NutritionID)
    {
        var db = MyDB.Open();
        var sql = "Delete From MaterialNutrition Where MaterialID = @0 And NutritionID = @1";
        db.Execute(sql, MaterialID, NutritionID);
    }
    
    /// <summary>
    /// 返回所有营养成分
    /// </summary>
    public static IEnumerable<dynamic> GetAllNutrition()
    {
        var db = MyDB.Open();
        var sql = @"Select a.*, b.UnitName
                    From Nutrition a
                    Inner Join Unit b On a.UnitID = b.UnitID
                    Order By a.NutritionID";
        return db.Query(sql);
    }
    
    /// <summary>
    /// 新增营养成分
    /// </summary>
    public static decimal AddNutrition(string NutritionName, int UnitID)
    {
        var db = MyDB.Open();
        var sql = "Insert Into Nutrition (NutritionName, UnitID) Values (@0, @1)";
        db.Execute(sql, NutritionName, UnitID);
        return db.GetLastInsertId();
    }
    
    /// <summary>
    /// 更新营养成分
    /// </summary>
    public static void UpdateNutrition(int NutritionID, string NutritionName, int UnitID)
    {
        var db = MyDB.Open();
        var sql = "Update Nutrition Set NutritionName = @1, UnitID = @2 Where NutritionID = @0";
        db.Execute(sql, NutritionID, NutritionName, UnitID);
    }
    
    /// <summary>
    /// 删除营养成分
    /// </summary>
    public static void DeleteNutrition(int NutritionID)
    {
        var db = MyDB.Open();
        var sql = "Delete From Nutrition Where NutritionID = @0";
        db.Execute(sql, NutritionID);
    }

    /// <summary>
    /// 返回计量单位，按类型
    /// </summary>
    public static IEnumerable<dynamic> GetUnitsByType(int UnitType)
    {
        var db = MyDB.Open();
        return db.Query("Select * From Unit Where UnitType = @0", UnitType);
    }
    
    /// <summary>
    /// 新增计量单位
    /// </summary>
    public static decimal AddUnit(string UnitName, int UnitType)
    {
        var db = MyDB.Open();
        int UnitID = db.QueryValue("Select Max(UnitID) From Unit Where UnitType = @0", UnitType) + 1;
        var sql = "Insert Into Unit (UnitID, UnitName, UnitType) Values (@0, @1, @2)";
        db.Execute(sql, UnitID, UnitName, UnitType);
        return UnitID;
    }
    
    /// <summary>
    /// 更新计量单位
    /// </summary>
    public static void UpdateUnit(int UnitID, string UnitName)
    {
        var db = MyDB.Open();
        var sql = "Update Unit Set UnitName = @1 Where UnitID = @0";
        db.Execute(sql, UnitID, UnitName);
    }
    
    /// <summary>
    /// 删除计量单位
    /// </summary>
    public static void DeleteUnit(int UnitID)
    {
        var db = MyDB.Open();
        var sql = "Delete From Unit Where UnitID = @0";
        db.Execute(sql, UnitID);
    }
}
