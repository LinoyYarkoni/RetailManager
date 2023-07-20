using DataManager.Library.Internal.DataAccess;
using DataManager.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataManager.Library.DataAccess
{
    public class SaleData
    {
        public void SaveSale(SaleModel saleInfo, string cashierId)
        {
            List<SaleDetailDBModel> saleDetailDBModel = new List<SaleDetailDBModel>();
            ProductData product = new ProductData();
            var taxRate = ConfigHelper.GetTaxRate() / 100;

            // Initialize SaleDetail DB model from SaleModel (ProductId, Quantity)
            foreach (var item in saleInfo.SaleDetail)
            {
                var currentItem = new SaleDetailDBModel
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                };

                // Get product info by id from the DB
                var productInfo = product.GetProductById(currentItem.ProductId);

                if (productInfo == null)
                {
                    throw new Exception
                        ($"The product id of {currentItem.ProductId} could not be found in the database");
                }

                // Initialize SaleDetail DB model from DB (PurchasePrice) 
                currentItem.PurchasePrice = productInfo.RetailPrice * currentItem.Quantity;

                // Initialize SaleDetail DB model - calculate tax (Tax) 
                if (productInfo.IsTaxable)
                {
                    currentItem.Tax = currentItem.PurchasePrice * taxRate;
                }

                saleDetailDBModel.Add(currentItem);
            }

            // Initialize Sale DB model from SaleDetailDBModel model (CashierId. SubTotal, Tax) 
            SaleDBModel sale = new SaleDBModel
            {
                SubTotal = saleDetailDBModel.Sum(x => x.PurchasePrice),
                Tax = saleDetailDBModel.Sum(x => x.Tax),
                CashierId = cashierId
            };

            // Initialize Sale DB model from SaleDetailDBModel model (Total) 
            sale.Total = sale.SubTotal + sale.Tax;

            /* Transaction */
            using(SqlDataAccess sql = new SqlDataAccess())
            {
                try
                {
                    sql.StartTransaction("RMData");

                    // Save Sale to the DB
                    sql.SaveDataInTransaction("dbo.spSaleInsert", sale);

                    // Get Sale Id
                    var input = new { CashierId = sale.CashierId, SaleDate = sale.SaleDate };

                    sale.Id = sql.LoadDataInTransaction<int, dynamic>("dbo.spSaleLookup", input).FirstOrDefault();

                    // Update the Id in every SaleDetail record
                    foreach (var item in saleDetailDBModel)
                    {
                        item.SaleId = sale.Id;

                        // Save SaleDetail to the DB
                        sql.SaveDataInTransaction<SaleDetailDBModel>("dbo.spSaleDetailInsert", item);
                    }

                    sql.CommitTransaction();
                }
                catch
                {
                    sql.RollbackTransaction();
                    throw;
                }
            }
        }
    }
}
