CREATE PROCEDURE [dbo].[spProductGetAll]
AS
begin
	set nocount on;

	select Id, ProductName, [Description], RetailPrice, QuantityInStock
	from dbo.Product
	order by ProductName 
end