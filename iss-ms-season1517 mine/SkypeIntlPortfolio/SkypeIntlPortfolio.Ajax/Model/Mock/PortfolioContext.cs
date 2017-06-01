using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkypeIntlPortfolio.Ajax.Model.Mock
{
    public class PortfolioContext : IDisposable
    {
        public List<Product> Products
        {
            get
            {
                return new List<Product>
                {
                    new Product
                    {
                        ProductName = "Lync Rich Client",
                        ProductKey = 1,
                        ProductEols =  new List<Eol>
                            {
                                new Eol
                                {
                                    IICC = "fr-fr",
                                    Language_Name = "French",
                                    ProductKey =1
                                }
                            }
                    },

                     new Product
                    {
                        ProductName = "Skype Threshold",
                        ProductKey = 2,
                        ProductEols =  new List<Eol>
                            {
                                new Eol
                                {
                                    IICC = "de-de",
                                    Language_Name = "German",
                                    ProductKey =2
                                }
                            }
                    }
                };
            }
        }

        public List<EolExtended> EolsExtended
        {
            get
            {
                return new List<EolExtended>
                {
                    new EolExtended
                    {
                        IICC = "fr-fr",
                        Language_Name = "French"
                    },

                    new EolExtended
                    {
                        IICC = "it-it",
                        Language_Name = "Italian"
                    },

                    new EolExtended
                    {
                        IICC = "en-us",
                        Language_Name = "English"
                    },

                    new EolExtended
                    {
                        IICC = "de-de",
                        Language_Name = "German"
                    }
                };
            }
        }

        public void Dispose()
        {
        }
    }

    public class Product
    {
        public int ProductKey { get; set; }

        public string ProductName { get; set; }

        public List<Eol> ProductEols { get; set; }
    }

    public class Eol
    {
        //foreign key
        public int ProductKey { get; set; }

        //
        public string IICC { get; set; }

        public string Language_Name { get; set; }
    }

    public class EolExtended
    {
        //foreign key
        public string IICC { get; set; }

        public string Language_Name { get; set; }
    }
}