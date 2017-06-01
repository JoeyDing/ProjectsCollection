using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SkypeIntlPortfolio.Ajax.Pages.InternalTools
{
    public partial class listviewTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            var result = new List<ListGroup<MyProduct>>()
            {
                new ListGroup<MyProduct>()
                {
                    GroupName = "Group 1",
                    Count = 3,
                    Items = new List<MyProduct>()
                            {
                                new MyProduct{
                                    ProductId = "1",
                                    ProductName = "Test1",
                                    ListPrice = 4
                                },
                                new MyProduct{
                                    ProductId = "2",
                                    ProductName = "Test2",
                                    ListPrice = 6
                                },
                                new MyProduct{
                                    ProductId = "3",
                                    ProductName = "Test3",
                                    ListPrice = 8
                                }
                            }
                },
                new ListGroup<MyProduct>()
                {
                    GroupName = "Group 2",
                    Count = 3,
                    Items = new List<MyProduct>()
                            {
                                new MyProduct{
                                    ProductId = "1",
                                    ProductName = "Test4",
                                    ListPrice = 4
                                },
                                new MyProduct{
                                    ProductId = "2",
                                    ProductName = "Test5",
                                    ListPrice = 6
                                },
                                new MyProduct{
                                    ProductId = "3",
                                    ProductName = "Test6",
                                    ListPrice = 8
                                }
                            }
                }
            };

            lstProductsListView.DataSource = result;
            lstProductsListView.DataBind();
        }
    }

    public class ListGroup<T>
    {
        public string GroupName { get; set; }
        public int Count { get; set; }
        public List<T> Items { get; set; }
    }

    public class MyProduct
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal ListPrice { get; set; }
    }
}