CREATE PROCEDURE [dbo].[spSale_SaleReport]
AS
begin
	set nocount on;

	select sale.[SaleDate], sale.[SubTotal], sale.[Tax], sale.[Total],
		   [user].[FirstName], [user].[LastName], [user].[EmailAddress]
	from dbo.Sale sale
	inner join dbo.[User] [user] on sale.CashierId = [user].Id 
end

