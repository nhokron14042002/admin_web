
using Dapper;
using Microsoft.Data.SqlClient;
using SV20T1080033.DataLayers;
using SV20T1080033.DataLayers.SQLServer;
using SV20T1080033.DomainModels;
using System.Data;


namespace SV20T1080033.DataLayers.SQLServer
{
    public class ProductDAL : _BaseDAL, IProductDAL
    {
       
            public ProductDAL(string connectionString) : base(connectionString)
            {
            }

            public int Add(Product data)
            {
                int id = 0;
			using (var connection = OpenConnection())
			{
				var sql = @"if exists(select * from Products where ProductName = @ProductName)
                                select -1
                            else
                                begin
                                    insert into Products(ProductName,ProductDescription,SupplierID,CategoryID,Unit,Price,Photo,IsSelling)
                                    values(@ProductName,@ProductDescription,@supplierId,@categoryId,@Unit,@Price,@Photo,@IsSelling);
                                    select @@identity;
                                end";
				var parameters = new
				{
					productName = data.ProductName ?? "",
					productDescription = data.ProductDescription ?? "",
					supplierId = data.SupplierId,
					categoryId = data.CategoryId,
					unit = data.Unit ?? "",
					price = data.Price,
					photo = data.Photo ?? "",
					IsSelling = data.IsSelling
				};
				id = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: CommandType.Text);
				connection.Close();
			}
			return id;
            }

            public long AddAttribute(ProductAttribute data)
            {
                int id = 0;
                using (var connection = OpenConnection())
                {
                    var sql = @"insert into ProductAttributes(ProductID, AttributeName, AttributeValue, DisplayOrder)
                            values(@productID,@attributeName,@attributeValue,@displayOrder);
                            select @@identity;
                               ";
                    var parameters = new
                    {
                        productID = data.ProductId,
                        attributeName = data.AttributeName ?? "",
                        attributeValue = data.AttributeValue ?? "",
                        displayOrder = data.DisplayOrder
                    };
                    id = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: CommandType.Text);
                    connection.Close();
                }
                return id;
            }

            public long AddPhoto(ProductPhoto data)
            {
                int id = 0;
                using (var connection = OpenConnection())
                {
                    var sql = @"insert into ProductPhotos(ProductID, Photo, Description, DisplayOrder, IsHidden)
                            values(@productID,@photo,@description,@displayOrder,@isHidden);
                            select @@identity;
                               ";
                    var parameters = new
                    {
                        productID = data.ProductId,
                        photo = data.Photo ?? "",
                        description = data.Description ?? "",
                        displayOrder = data.DisplayOrder,
                        isHidden = data.IsHidden
                    };
                    id = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: CommandType.Text);
                    connection.Close();
                }
                return id;
            }

            public int Count(string searchValue = "", int categoryID = 0, int supplierID = 0, decimal minPrice = 0, decimal maxPrice = 0)
            {
                int count = 0;
                if (!string.IsNullOrEmpty(searchValue))
                    searchValue = "%" + searchValue + "%";

                using (var connection = OpenConnection())
                {
                    var sql = @"select count(*) from Products
                    where (@searchValue = N'' or ProductName like @searchValue)
                    and (@supplierID = 0 or SupplierID = @supplierID)
                    and (@categoryID = 0 or CategoryID = @categoryID)
                    and ((@minPrice = 0 and @maxPrice = 0) or (ProductName between @minPrice and @maxPrice))";

                    var parameters = new
                    {
                        searchValue = searchValue,
                        categoryID = categoryID,
                        supplierID = supplierID,
                        minPrice = minPrice,
                        maxPrice = maxPrice
                    };

                    count = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: CommandType.Text);
                    connection.Close();
                }

                return count;
            }


            public bool Delete(int productID)
            {
                bool result = false;
                using (var connection = OpenConnection())
                {
                    var sql = "delete from Products where ProductID = @productID and not exists(select * from OrderDetails where ProductID = @productID)";
                    var parameters = new { productID = productID };
                    result = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0;
                    connection.Close();
                }
                return result;
            }

            public bool DeleteAttribute(long attributeID)
            {
                bool result = false;
                using (var connection = OpenConnection())
                {
                    var sql = "delete from ProductAttributes where AttributeID = @attributeID ";
                    var parameters = new { attributeID = attributeID };
                    result = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0;
                    connection.Close();
                }
                return result;
            }

            public bool DeletePhoto(long photoID)
            {
                bool result = false;
                using (var connection = OpenConnection())
                {
                    var sql = "delete from ProductPhotos where PhotoID = @photoID ";
                    var parameters = new { photoID = photoID };
                    result = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0;
                    connection.Close();
                }
                return result;
            }

            public Product? Get(int productID)
            {
                Product? data = null;
                using (var connection = OpenConnection())
                {
                    var sql = "select * from Products where ProductID = @productID";
                    var parameters = new { productID = productID };
                    data = connection.QueryFirstOrDefault<Product>(sql: sql, param: parameters, commandType: CommandType.Text);
                    connection.Close();
                }
                return data;
            }

            public ProductAttribute? GetAttribute(long attributeID)
            {
                ProductAttribute? data = null;
                using (var connection = OpenConnection())
                {
                    var sql = "select * from ProductAttributes where AttributeID = @attributeID";
                    var parameters = new { attributeID = attributeID };
                    data = connection.QueryFirstOrDefault<ProductAttribute>(sql: sql, param: parameters, commandType: CommandType.Text);
                    connection.Close();
                }
                return data;
            }

            public ProductPhoto? GetPhoto(long photoID)
            {
                ProductPhoto? data = null;
                using (var connection = OpenConnection())
                {
                    var sql = "select * from ProductPhotos where PhotoID = @photoID";
                    var parameters = new { photoID = photoID };
                    data = connection.QueryFirstOrDefault<ProductPhoto>(sql: sql, param: parameters, commandType: CommandType.Text);
                    connection.Close();
                }
                return data;
            }

            public bool InUsed(int productID)
            {
                bool result = false;
                using (var connection = OpenConnection())
                {
                    var sql = @"if exists(select * from OrderDetails where ProductID = @productID)
                                select 1
                            else 
                                select 0";
                    var parameters = new { productID = productID };
                    result = connection.ExecuteScalar<bool>(sql: sql, param: parameters, commandType: CommandType.Text);
                    connection.Close();
                }
                return result;
            }

            public IList<Product> List(int page = 1, int pageSize = 0, string searchValue = "", int categoryID = 0, int supplierID = 0, decimal minPrice = 0, decimal maxPrice = 0)
            {
                List<Product> data;
                if (!string.IsNullOrEmpty(searchValue))
                    searchValue = "%" + searchValue + "%";

                using (var connection = OpenConnection())
                {
                    var sql = @"with cte as
                (
                    select *,
                    ROW_NUMBER() over (order by ProductName) as RowNumber
                    from Products
                    where (@searchValue = N'' or ProductName like @searchValue)
                    and (@supplierID = 0 or SupplierID = @supplierID)
                    and (@categoryID = 0 or CategoryID = @categoryID)
                    and ((@minPrice = 0 and @maxPrice = 0) or (ProductName between @minPrice and @maxPrice))
                )

                select * from cte
                where (@pageSize = 0)
                    or (RowNumber between (@page - 1) * @pageSize + 1 and @page * @pageSize)
                    order by RowNumber;";

                    var parameters = new
                    {
                        page = page,
                        pageSize = pageSize,
                        searchValue = searchValue,
                        categoryID = categoryID,
                        supplierID = supplierID,
                        minPrice = minPrice,
                        maxPrice = maxPrice
                    };

                    data = connection.Query<Product>(sql: sql, param: parameters, commandType: CommandType.Text).ToList();
                    connection.Close();
                }
                return data;
            }


            public IList<ProductAttribute> ListAttributes(int productID)
            {
                List<ProductAttribute> data;
                using (var connection = OpenConnection())
                {
                    var sql = @"
            with cte as
            (
                select 
                    PA.AttributeID, 
                    PA.ProductID, 
                    PA.AttributeName, 
                    PA.AttributeValue, 
                    PA.DisplayOrder 
                from ProductAttributes as PA
                join Products as P on PA.ProductID = P.ProductID
                Where PA.ProductID = @productID
            )

            select * from cte"; // Chọn dữ liệu từ CTE cần thiết
                    var parameters = new
                    {
                        productID = productID
                    };
                    data = connection.Query<ProductAttribute>(sql: sql, param: parameters, commandType: CommandType.Text).ToList();

                    connection.Close();
                }
                return data;
            }

            public IList<ProductPhoto> ListPhotos(int productID)
            {
                List<ProductPhoto> data;
                using (var connection = OpenConnection())
                {
                    var sql = @"
            with cte as
            (
                select 
                    PP.PhotoID, 
                    PP.ProductID, 
                    PP.Photo, 
                    PP.Description, 
                    PP.DisplayOrder, 
                    PP.IsHidden
                from ProductPhotos as PP
                join Products as P on PP.ProductID = P.ProductID
                Where PP.ProductID = @productID
            )

            select * from cte"; // Chọn dữ liệu từ CTE cần thiết
                    var parameters = new
                    {
                        productID = productID
                    };
                    data = connection.Query<ProductPhoto>(sql: sql, param: parameters, commandType: CommandType.Text).ToList();

                    connection.Close();
                }
                return data;
            }

            public bool Update(Product data)
            {
                bool result = false;
                using (var connection = OpenConnection())
                {
                    var sql = @"if not exists(select * from Products where ProductID <> @productID and ProductName = @productName)
                                begin
                                    update Products 
                                    set ProductName = @productName,
                                        ProductDescription = @productDescription,
                                        SupplierID = @supplierID,
                                        CategoryID = @categoryID,
                                        Unit = @unit,
                                        Price = @price,
                                        Photo = @photo,
                                        IsSelling = @isSelling
                                    where ProductID = @productID
                                end";
                    var parameters = new
                    {
                        productID = data.ProductId,
                        productName = data.ProductName ?? "",
                        productDescription = data.ProductDescription ?? "",
                        supplierID = data.SupplierId,
                        categoryID = data.CategoryId,
                        unit = data.Unit ?? "",
                        price = data.Price,
                        photo = data.Photo ?? "",
                        isSelling = data.IsSelling
                    };
                    result = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0;
                }
                return result;
            }

            public bool UpdateAttribute(ProductAttribute data)
            {
                bool result = false;
                using (var connection = OpenConnection())
                {
                    var sql = @"if not exists(select * from ProductAttributes where AttributeID <> @attributeID)
                                begin
                                    update ProductAttributes 
                                    set ProductID = @productID,
                                        AttributeName = @attributeName,
                                        AttributeValue = @attributeValue,
                                        DisplayOrder = @displayOrder
                                    where AttributeID = @attributeID
                                end";
                    var parameters = new
                    {
                        productID = data.ProductId,
                        attributeName = data.AttributeName ?? "",
                        attributeValue = data.AttributeValue ?? "",
                        displayOrder = data.DisplayOrder
                    };
                    result = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0;
                }
                return result;
            }

            public bool UpdatePhoto(ProductPhoto data)
            {
                bool result = false;
                using (var connection = OpenConnection())
                {
                    var sql = @"if not exists(select * from ProductPhotos where PhotoID <> @photoID)
                                begin
                                    update ProductPhotos 
                                    set ProductID = @productID,
                                        Photo = @photo,
                                        Description = @description,
                                        DisplayOrder = @displayOrder,
                                        IsHidden = @isHidden
                                    where PhotoID = @photoID
                                end";
                    var parameters = new
                    {
                        productID = data.ProductId,
                        photo = data.Photo ?? "",
                        description = data.Description ?? "",
                        displayOrder = data.DisplayOrder,
                        isHidden = data.IsHidden
                    };
                    result = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0;
                }
                return result;
            }
        }
    
}
