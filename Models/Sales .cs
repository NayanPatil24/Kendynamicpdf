using iText.Commons.Actions.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace Kendynamicpdf.Models
{
    public class Sales
    {
        public int Sales_Id { get; set; }
        public string Corp_Id_No { get; set; }
        public string GST_No { get; set; }
        public string PAN_No { get; set; }
        public string Buyer_Details { get; set; }
        public string Consignee_Details { get; set; }
        public DateTime Date { get; set; }
        public string SoNo { get; set; }
        public string PoNo { get; set; }
        public DateTime Order_Conf_Date { get; set; }
        public string Order_Ref{ get; set; }
        public string Transporter { get; set; }
        public string Buyer_GST_No { get; set; }
        public string Buyer_State { get; set; }
        public string Consignee_GST_No { get; set; }
        public string Cosignee_State { get; set; }
        public string Agent_Name { get; set; }
        public string Amount_In_Words { get; set; }
        public decimal Total {  get; set; }
        public string Payment_Terms { get; set; }
        public string Delivery_Terms{ get; set; }
        public decimal SGST { get; set; }
        public decimal CGST { get; set; }
        public decimal IGST { get; set; }
        public string Round_Off { get; set; }
        public decimal Grand_Total { get; set; }



        // Define your DbContext class
        public class MyDbContext : DbContext
        {

            public DbSet<Sales> SalesOrders { get; set; }

            // Other DbSet properties for your entities
        }

    }
}
