using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.EJ2.Base;
using Syncfusion.EJ2.Grids;
using Syncfusion.EJ2.GridExport;

namespace ReactGrid.Controllers
{
    public class HomeController : Controller
    {
        public static List<OrdersDetails> orddata = new List<OrdersDetails>();
        public IActionResult Index()
        {
            if (orddata.Count == 0)
                BindDataSource();
            ViewBag.dataSource = orddata;
            return View();
        }
        public IActionResult UrlDatasource([FromBody] DataManagerRequest dm)
        {
            if (orddata.Count == 0)
                BindDataSource();
            IEnumerable<OrdersDetails> DataSource = orddata;
            DataOperations operation = new DataOperations();
            int count = DataSource.Cast<OrdersDetails>().Count();
            return dm.RequiresCounts ? Json(new { result = DataSource, count = count }) : Json(DataSource);
        }
        public ActionResult ExcelExport([FromForm] string gridModel)
        {
            GridExcelExport exp = new GridExcelExport();
            Grid gridProperty = ConvertGridObject(gridModel);
            return exp.ExcelExport<OrdersDetails>(gridProperty, orddata);
        }
        public ActionResult PdfExport([FromForm] string gridModel)
        {
            GridPdfExport exp = new GridPdfExport();
            Grid gridProperty = ConvertGridObject(gridModel);
            return exp.PdfExport<OrdersDetails>(gridProperty, orddata);
        }

        private Grid ConvertGridObject(string gridProperty)
        {
            Grid GridModel = (Grid)Newtonsoft.Json.JsonConvert.DeserializeObject(gridProperty, typeof(Grid));
            GridColumnModel cols = (GridColumnModel)Newtonsoft.Json.JsonConvert.DeserializeObject(gridProperty, typeof(GridColumnModel));
            GridModel.Columns = cols.columns;
            return GridModel;
        }

        public class GridColumnModel
        {
            public List<GridColumn> columns { get; set; }
        }
        private void BindDataSource()
        {
            int code = 10000;
            for (int i = 1; i < 10; i++)
            {
                orddata.Add(new OrdersDetails(code + 2, new EmployeeData("name1", "name2"), "ANATR", i + 0, 3.3 * i, true, new DateTime(1995, 7, 2, 2, 3, 5), "Madrid", "Queen Cozinha", "Brazil", new DateTime(1996, 9, 11), "Avda. Azteca 123"));
                orddata.Add(new OrdersDetails(code + 3, new EmployeeData("name3", "name4"), "ANTON", i + 1, 4.3 * i, true, new DateTime(2012, 12, 25, 2, 3, 5), "Cholchester", "Frankenversand", "Germany", new DateTime(1996, 10, 7), "Carrera 52 con Ave. Bolívar #65-98 Llano Largo"));
                orddata.Add(new OrdersDetails(code + 4, new EmployeeData("name5", "name6"), "BLONP", i + 2, 5.3 * i, false, new DateTime(2002, 12, 25, 2, 3, 5), "Marseille", "Ernst Handel", "Austria", new DateTime(1996, 12, 30), "Magazinweg 7"));
                orddata.Add(new OrdersDetails(code + 5, new EmployeeData("name7", "name8"), "BOLID", i + 3, 6.3 * i, true, new DateTime(1953, 02, 18, 05, 2, 4), "Tsawassen", "Hanari Carnes", "Switzerland", new DateTime(1997, 12, 3), "1029 - 12th Ave. S."));
                code += 5;
            }
        }
        public class OrdersDetails
        {
            public OrdersDetails()
            {

            }
            public OrdersDetails(int OrderID, EmployeeData Employee, string CustomerId, int EmployeeId, double Freight, bool Verified, DateTime OrderDate, string ShipCity, string ShipName, string ShipCountry, DateTime ShippedDate, string ShipAddress)
            {
                this.OrderID = OrderID;
                this.Employee = Employee;
                this.CustomerID = CustomerId;
                this.EmployeeID = EmployeeId;
                this.Freight = Freight;
                this.ShipCity = ShipCity;
                this.Verified = Verified;
                this.OrderDate = OrderDate;
                this.ShipName = ShipName;
                this.ShipCountry = ShipCountry;
                this.ShippedDate = ShippedDate;
                this.ShipAddress = ShipAddress;
            }

            public int? OrderID { get; set; }

            public EmployeeData Employee { get; set; }
            public string CustomerID { get; set; }
            public int? EmployeeID { get; set; }
            public double? Freight { get; set; }
            public string ShipCity { get; set; }
            public bool Verified { get; set; }
            public DateTime OrderDate { get; set; }

            public string ShipName { get; set; }

            public string ShipCountry { get; set; }

            public DateTime ShippedDate { get; set; }
            public string ShipAddress { get; set; }
        }

        public class EmployeeData
        {
            public EmployeeData()
            {

            }
            public EmployeeData(string FirstName, string LastName)
            {
                this.FirstName = FirstName;
                this.LastName = LastName;

            }
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }
    }
    
}
