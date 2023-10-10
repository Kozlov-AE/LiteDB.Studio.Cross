// See https://aka.ms/new-console-template for more information

using DatabaseGenerator;
using DatabaseGenerator.Models;
using DatabaseGenerator.Models.LiteDbModels;
using LiteDB;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

// Console.WriteLine("Creating database from 1_county_level_confirmed_cases.csv...");
// var countyLevelConfirmedCases = CountyLevelConfirmedCase.LoadMe("1_county_level_confirmed_cases.csv");
// using (var db = new LiteDatabase(@"CoronavirusCaseTracker.db")) {
//     if (db.CollectionExists("CountyLevelConfirmedCase")){
//         var col = db.GetCollection<CountyLevelConfirmedCase>();
//         col.EnsureIndex(x => x.Id, true);
//         col.Insert(countyLevelConfirmedCases);
//     } else {
//         var col = db.GetCollection<CountyLevelConfirmedCase>();
//         col.EnsureIndex(x => x.Id, true);
//         col.Insert(countyLevelConfirmedCases);
//     }
// }
// Console.WriteLine("1_county_level_confirmed_cases.csv loaded!");

Console.WriteLine("Начинаю перенос данных из БД Northwind (SQLite) в новую БД LiteDb!");
var dir = @".\litedbs";
var dbname = "northwind.db";
if(!Directory.Exists(dir))
    Directory.CreateDirectory(dir);
using (var db = new LiteDatabase(Path.Combine(dir, dbname))) {
    using (var ctx = new NorthwindContext()){
        var categories = ctx.Categories;
        var cats = db.GetCollection<CategoryLDb>("Categories");
        var cats1 = categories.Select(x => x.MapToLDb());
        cats.Insert(cats1);

        var customers = ctx.Customers;
        var custs = db.GetCollection<CustomerLDb>("Customers");
        var custs1 = customers.Select(x => x.MapToLDb());
        custs.Insert(custs1);

        var employees = ctx.Employees;
        var emps = db.GetCollection<EmployeeLDb>("Employees");
        var emps1 = employees.Select(x => x.MapToLDb());
        emps.Insert(emps1);

        var orders = ctx.Orders;
        var ords = db.GetCollection<OrderLDb>("Orders");
        var ords1 = orders.Select(x => x.MapToLDb());
        ords.Insert(ords1);

        var orderDetails = ctx.OrderDetails;
        var odets = db.GetCollection<OrderDetailLDb>("OrderDetails");
        var odets1 = orderDetails.Select(x => x.MapToLDb());
        odets.Insert(odets1);

        var products = ctx.Products;
        var prods = db.GetCollection<ProductLDb>("Products");
        var prods1 = products.Select(x => x.MapToLDb());
        prods.Insert(prods1);

        var shippers = ctx.Shippers;
        var shipps = db.GetCollection<ShipperLDb>("Shippers");
        var shipps1 = shippers.Select(x => x.MapToLDb());
        shipps.Insert(shipps1);

        var suppliers = ctx.Suppliers;
        var supps = db.GetCollection<SupplierLDb>("Suppliers");
        var supps1 = suppliers.Select(x => x.MapToLDb());
        supps.Insert(supps1);
    }
    Console.WriteLine("Перенос данных завершен!");
}
