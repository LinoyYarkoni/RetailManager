CREATE PROCEDURE [dbo].[spProductGetById]
	@Id int
AS
begin
	set nocount on;

	select Id, ProductName, [Description], RetailPrice, QuantityInStock, IsTaxable
	from dbo.Product
	Where Id = @Id
end
