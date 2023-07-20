﻿using DataManager.Library.Internal.DataAccess;
using DataManager.Library.Models;
using System.Collections.Generic;
using System.Linq;

namespace DataManager.Library.DataAccess
{
    public class ProductData
    {
        public List<ProductModel> GetProducts()
        {
            SqlDataAccess sql = new SqlDataAccess();

            var output = sql.LoadData<ProductModel, dynamic>("dbo.spProduct_GetAll", new { }, "RMData");

            return output;
        }

        public ProductModel GetProductById(int productId)
        {
            SqlDataAccess sql = new SqlDataAccess();

            var output = 
                sql
                .LoadData<ProductModel, dynamic>("dbo.spProduct_GetById", new {id = productId}, "RMData")
                .FirstOrDefault();

            return output;
        }
    }
}
