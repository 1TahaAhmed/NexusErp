namespace NexusErp.Application.Common.Permissions
{
    public static class Permissions
    {
        public static class Users
        {
            public const string View = "Permissions.Users.View";
            public const string Create = "Permissions.Users.Create";
            public const string Edit = "Permissions.Users.Edit";
            public const string Delete = "Permissions.Users.Delete";
        }

        public static class Products
        {
            public const string View = "Permissions.Products.View";
            public const string AddProduct = "Permissions.Products.AddProduct";
            public const string EditProduct = "Permissions.Products.EditProduct";
            public const string DeleteProduct = "Permissions.Products.DeleteProduct";
        }

        public static class Categories
        {
            public const string View = "Permissions.Categories.View";
            public const string CreateCategory = "Permissions.Categories.CreateCategory";
            public const string EditCategory = "Permissions.Categories.EditCategory";
            public const string DeleteCategory = "Permissions.Categories.DeleteCategory";
        }

        public static class Sales
        {
            public const string View = "Permissions.Sales.View";
            public const string CreateInvoice = "Permissions.Sales.CreateInvoice";
            public const string CancelInvoice = "Permissions.Sales.CancelInvoice";
        }

        public static class PurchaseOrders
        {
            public const string View = "Permissions.PurchaseOrders.View";
            public const string ReceiveGoods = "Permissions.PurchaseOrders.ReceiveGoods";
            public const string CreatePurchaseOrder = "Permissions.PurchaseOrders.CreatePurchaseOrder";
        }

        public static class SalesReturns
        {
            public const string View = "Permissions.SalesReturns.View";
            public const string CreateSalesReturns = "Permissions.SalesReturns.CreateSalesReturns";
        }
    }
}