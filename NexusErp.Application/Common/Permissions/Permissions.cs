using System;
using System.Collections.Generic;
using System.Text;

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

        public static class Inventory
        {
            public const string View = "Permissions.Inventory.View";
            public const string AddProduct = "Permissions.Inventory.AddProduct";
            public const string EditProduct = "Permissions.Inventory.EditProduct";
            public const string StockAdjustment = "Permissions.Inventory.StockAdjustment";
        }

        public static class Sales
        {
            public const string View = "Permissions.Sales.View";
            public const string CreateInvoice = "Permissions.Sales.CreateInvoice";
            public const string CancelInvoice = "Permissions.Sales.CancelInvoice";
        }
    }
}
