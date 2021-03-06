﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Demo_LINQ_ClassOfProducts
{

    //
    // demo adapted from MSDN demo
    // https://code.msdn.microsoft.com/SQL-Ordering-Operators-050af19e/sourcecode?fileId=23914&pathId=1978010539
    //
    class Program
    {
        public static IEnumerable<object> UnitPrice { get; private set; }

        static void Main(string[] args)
        {
            //
            // write all data files to Data folder
            //
            //GenerateDataFiles.InitializeXmlFile();

            List<Product> productList = GenerateDataFiles.InitializeProductList();

            OrderByCatagory(productList); // -- Example, John

            OrderByCatagoryAnoymous(productList); // -- Example, John
            
            OrderByCatagoryExpensive(productList); // -- Morgan

            OrderByLowOnStock(productList); // -- Morgan - student choice

            OrderByTotalValueAnonymous(productList); // -- Wyatt
            
            OrderByNameAnonymous(productList); // -- Wyatt
            
            OrderByStockAnonymous(productList); // -- Wyatt, custom
            
            OrderByStartingWithL(productList); // -- Justina

            OrderByUnits(productList); // -- Justina
            
            OrderByPrice(productList); // -- Justina
            
            //
            // Write the following methods
            //

            // OrderByUnits(): List the names and units of all products with less than 10 units in stock. Order by units. -- Justina

            // OrderByPrice(): List all products with a unit price less than $10. Order by price. -- Justina

            // FindExpensive(): List the most expensive Seafood. Consider there may be more than one. -- Morgan

            // OrderByTotalValue(): List all condiments with total value in stock (UnitPrice * UnitsInStock). Sort by total value. -- Wyatt

            // OrderByName(): List all products with names that start with "S" and calculate the average of the units in stock. -- Wyatt

            // Query: Student Choice - Minimum of one per team member

        }


        /// <summary>
        /// read all products from an XML file and return as a list of Product
        /// in descending order by price
        /// </summary>
        /// <returns>List of Product</returns>
        private static List<Product> ReadAllProductsFromXml()
        {
            string dataPath = @"Data\Products.xml";
            List<Product> products;

            try
            {
                StreamReader streamReader = new StreamReader(dataPath);
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Product>), new XmlRootAttribute("Products"));

                using (streamReader)
                {
                    products = (List<Product>)xmlSerializer.Deserialize(streamReader);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return products;
        }

        /// <summary>
        /// Order by category
        /// </summary>
        /// <param name="products"></param>
        private static void OrderByCatagory(List<Product> products)
        {
            string TAB = "   ";

            Console.Clear();
            Console.WriteLine(TAB + "List all beverages and sort by the unit price.");
            Console.WriteLine();

            //
            // query syntax
            //
            var sortedProducts =
                from product in products
                where product.Category == "Beverages"
                orderby product.UnitPrice descending
                select product;

            //
            // lambda syntax
            //
            //var sortedProducts = products.Where(p => p.Category == "Beverages").OrderByDescending(p => p.UnitPrice);

            Console.WriteLine(TAB + "Category".PadRight(15) + "Product Name".PadRight(25) + "Unit Price".PadLeft(10));
            Console.WriteLine(TAB + "--------".PadRight(15) + "------------".PadRight(25) + "----------".PadLeft(10));

            foreach (Product product in sortedProducts)
            {
                Console.WriteLine(TAB + product.Category.PadRight(15) + product.ProductName.PadRight(25) + product.UnitPrice.ToString("C2").PadLeft(10));
            }

            Console.WriteLine();
            Console.WriteLine(TAB + "Press any key to continue.");
            Console.ReadKey();
        }

        private static void OrderByCatagoryAnoymous(List<Product> products)
        {
            string TAB = "   ";

            Console.Clear();
            Console.WriteLine(TAB + "List all beverages that cost more the $15 and sort by the unit price.");
            Console.WriteLine();

            //
            // query syntax
            //
            var sortedProducts =
                from product in products
                where product.Category == "Beverages" &&
                    product.UnitPrice > 15
                orderby product.UnitPrice descending
                select new
                {
                    Name = product.ProductName,
                    Price = product.UnitPrice
                };

            //
            // lambda syntax
            //
            //var sortedProducts = products.Where(p => p.Category == "Beverages" && p.UnitPrice > 15).OrderByDescending(p => p.UnitPrice).Select(p => new
            //{
            //    Name = p.ProductName,
            //    Price = p.UnitPrice
            //});

            decimal average = products.Average(p => p.UnitPrice);

            Console.WriteLine(TAB + "Product Name".PadRight(20) + "Product Price".PadLeft(15));
            Console.WriteLine(TAB + "------------".PadRight(20) + "-------------".PadLeft(15));

            foreach (var product in sortedProducts)
            {
                Console.WriteLine(TAB + product.Name.PadRight(20) + product.Price.ToString("C2").PadLeft(15));
            }

            Console.WriteLine();
            Console.WriteLine(TAB + "Average Price:".PadRight(20) + average.ToString("C2").PadLeft(15));

            Console.WriteLine();
            Console.WriteLine(TAB + "Press any key to continue.");
            Console.ReadKey();
        }

        /// <summary>
        /// List the most expensive Seafood. Consider there may be more than one. -- Morgan
        /// </summary>
        /// <param name="products"></param>
        private static void OrderByCatagoryExpensive(List<Product> products)
        {
            string TAB = "   ";

            Console.Clear();
            Console.WriteLine(TAB + "List of the most expensive Seafoods.");
            Console.WriteLine();

            //
            // query syntax
            //
            var mostExpensiveProducts = (
                from product in products
                where product.Category == "Seafood"
                orderby product.UnitPrice descending

                select new
                {
                    Name = product.ProductName,
                    Price = product.UnitPrice
                }).Take(2);

            Console.WriteLine(TAB + "Product Name".PadRight(20) + "Product Price".PadLeft(15));
            Console.WriteLine(TAB + "------------".PadRight(20) + "-------------".PadLeft(15));

            foreach (var product in mostExpensiveProducts)
            {
                Console.WriteLine(TAB + product.Name.PadRight(20) + product.Price.ToString("C2").PadLeft(15));
            }

            Console.WriteLine();
            Console.WriteLine(TAB + "Press any key to continue.");
            Console.ReadKey();
        }

        /// <summary>
        /// Lists products low in stock (under X units) -- Morgan, Student Choice
        /// </summary>
        private static void OrderByLowOnStock(List<Product> products)
        {
            string TAB = "   ";

            Console.Clear();
            Console.WriteLine(TAB + "List of Products Under 5 Units of Stock.");
            Console.WriteLine();

            //
            // query syntax
            //
            var lowOnStock = (
                from product in products
                where product.UnitsInStock < 5
                orderby product.UnitsInStock ascending

                select new
                {
                    Category = product.Category,
                    Name = product.ProductName,
                    Unit = product.UnitsInStock
                }
                );

            Console.WriteLine(TAB + "Category".PadRight(20) + "Product Name".PadRight(30) + "# In Stock".PadLeft(10));
            Console.WriteLine(TAB + "--------".PadRight(20) + "------------".PadRight(30) + "----------".PadLeft(10));

            foreach (var product in lowOnStock)
            {
                Console.WriteLine(TAB + product.Category.PadRight(20) + product.Name.PadRight(30) + product.Unit.ToString().PadLeft(10));
            }

            Console.WriteLine();
            Console.WriteLine(TAB + "Press any key to continue.");
            Console.ReadKey();
        }


        // OrderByUnits(): List the names and units of all products with less than 10 units in stock. Order by units. -- Justina

        private static void OrderByUnits(List<Product> products)
        {
            string TAB = "   ";

            Console.Clear();
            Console.WriteLine(TAB + "List all the names and units of all products with less than 10 units in stock. Order by units.");
            Console.WriteLine();

            //
            // query syntax
            //
            var sortedProducts = (
                from product in products
                where product.UnitsInStock < 10
                orderby product.UnitsInStock descending
                select new
                {
                    Name = product.ProductName,
                    Unit = product.UnitsInStock,
                    Price = product.UnitPrice
                }
                );

            Console.WriteLine(TAB + "Units".PadRight(15) + "Product Name".PadRight(25) + "Unit Price".PadLeft(10));
            Console.WriteLine(TAB + "--------".PadRight(15) + "------------".PadRight(25) + "----------".PadLeft(10));

            foreach (var product in sortedProducts)
            {
                Console.WriteLine(TAB + product.Unit.ToString().PadRight(15) + product.Name.ToString().PadRight(25) + product.Price.ToString().PadLeft(10));
            }

            Console.WriteLine();
            Console.WriteLine(TAB + "Press any key to continue.");
            Console.ReadKey();
        }

        private static void OrderByUnitsAnoymous(List<Product> products)
        {
            string TAB = "   ";

            Console.Clear();
            Console.WriteLine(TAB + "List all the names and units of all products with less than 10 units in stock. Order by units.");
            Console.WriteLine();

            //
            // query syntax
            //
            var sortedProducts =
                from product in products
                where product.Units == "Units" &&
                    product.UnitsInStock > 10
                orderby product.UnitsInStock descending
                select new
                {
                    Name = product.ProductName,
                    Price = product.UnitsInStock
                };
        }

        // OrderByPrice(): List all products with a unit price less than $10. Order by price. -- Justina

        private static void OrderByPrice(List<Product> products)
        {
            string TAB = "   ";
            Console.Clear();
            Console.WriteLine(TAB + "List all products under $10 in unit price.");
            Console.WriteLine();

            //
            // query syntax
            //
            var productsUnder10 = (
                      from product in products
                      where product.UnitPrice < 10
                      orderby product.UnitPrice descending
                      select new
                      {
                          Name = product.ProductName,
                          Price = product.UnitPrice
                      });

            
            Console.WriteLine(TAB + "Product Name".PadRight(35) + "Unit Price".PadLeft(20));
            Console.WriteLine(TAB + "------------".PadRight(35) + "----------".PadLeft(20));

            foreach (var product in productsUnder10)
            {
                Console.WriteLine(TAB + product.Name.PadRight(35) + product.Price.ToString("C2").PadLeft(20));
            }

        
            Console.WriteLine();
            Console.WriteLine(TAB + "Press any key to continue.");
            Console.ReadKey();
        }

        private static void OrderByPriceAnoymous(List<Product> products)
        {
            string TAB = "   ";

            Console.Clear();
            Console.WriteLine(TAB + "List all beverages that cost more the $15 and sort by the unit price.");
            Console.WriteLine();

            //
            // query syntax
            //
            var sortedProducts =
                from product in products
                where product.Price == "Price" &&
                    product.UnitPrice > 10
                orderby product.UnitPrice descending
                select new
                {
                    Name = product.ProductName,
                    Price = product.UnitPrice
                };
        }

        // OrderCanBringOnBus(): List all products with a unit CanBringOnBus less than $10. Order by CanBringOnBus. -- Justina

        private static void OrderByCanBringOnBus(List<Product> products)
        {
            string TAB = "   ";

            Console.Clear();
            Console.WriteLine(TAB + "List all beverages and sort by the unit CanBringOnBus.");
            Console.WriteLine();

            //
            // query syntax
            //
            var sortedProducts =
                from product in products
                where product.CanBringOnBus == "Beverages"
                orderby product.CanBringOnBus descending
                select product;

            //
            // lambda syntax
            //
            //var sortedProducts = products.Where(p => p.CanBringOnBus == "Beverages").OrderByDescending(p => p.UnitCanBringOnBus);

            Console.WriteLine(TAB + "CanBringOnBus".PadRight(15) + "Product Name".PadRight(25) + "Unit CanBringOnBus".PadLeft(10));
            Console.WriteLine(TAB + "-------------".PadRight(15) + "------------".PadRight(25) + "------------------".PadLeft(10));

            foreach (Product product in sortedProducts)
            {
                Console.WriteLine(TAB + product.CanBringOnBus.PadRight(15) + product.ProductName.PadRight(25) + product.CanBringOnBus.ToString().PadLeft(10));
            }

            Console.WriteLine();
            Console.WriteLine(TAB + "Press any key to continue.");
            Console.ReadKey();
        }
        
        /// <summary>
        /// List all condiments with total value in stock (UnitPrice * UnitsInStock). Sort by total value. -- Wyatt
        /// </summary>
        /// <param name="products"></param>
        private static void OrderByTotalValueAnonymous(List<Product> products)
        {
            string TAB = "    ";
            
            Console.Clear();
            Console.WriteLine(TAB + "List of total order by stock.");
            Console.WriteLine();

            var totalValue = (
                from product in products
                orderby product.UnitPrice descending
                select new
                {
                    Name = product.ProductName,
                    Price = product.UnitPrice
                });

            Console.WriteLine(TAB + "Product Name".PadRight(40) + "Product Price".PadLeft(30));
            Console.WriteLine(TAB + "------------".PadRight(40) + "-------------".PadLeft(30));

            foreach (var product in totalValue)
            {
                Console.WriteLine(TAB + product.Name.PadRight(40) + product.Price.ToString("C2").PadLeft(30));
            }

            Console.WriteLine();
            Console.WriteLine(TAB + "Press any key to continue.");
            Console.ReadKey();
            
        }
       
        /// <summary>
        /// List the items that start with the letter S and find the price average of said items
        /// </summary>
        /// <param name="products"></param>
        private static void OrderByNameAnonymous(List<Product> products)
        {
            string TAB = "    ";
            
            Console.Clear();
            Console.WriteLine(TAB + "List of items that start with the letter S and finding the price average.");
            Console.WriteLine();

            var totalValue = (
                from product in products
                where product.ProductName.StartsWith("S")
                orderby product.UnitPrice descending
                select new
                {
                    Name = product.ProductName,
                    Price = product.UnitPrice
                });

            decimal average = products.Average(p => p.UnitPrice);

            Console.WriteLine(TAB + "Product Name".PadRight(40) + "Product Average Price".PadLeft(30));
            Console.WriteLine(TAB + "------------".PadRight(40) + "---------------------".PadLeft(30));

            foreach (var product in totalValue)
            {
                Console.WriteLine(TAB + product.Name.PadRight(40) + product.Price.ToString("C2").PadLeft(30));
            }

            Console.WriteLine();
            Console.WriteLine(TAB + "Average Stock Value:".PadRight(40) + average.ToString("C2").PadLeft(30));

            Console.WriteLine();
            Console.WriteLine(TAB + "Press any key to continue.");
            Console.ReadKey();            
        }

        /// <summary>
        /// Justina's Choice - List products starting with L
        /// </summary>
        /// <param name="products"></param>
        private static void OrderByStartingWithL(List<Product> products)
        {
            string TAB = "    ";

            Console.Clear();
            Console.WriteLine(TAB + "List of items that start with the letter L and their price.");
            Console.WriteLine();

            var productsWithL = (
                from product in products
                where product.ProductName.StartsWith("L")
                orderby product.UnitPrice descending
                select new
                {
                    Name = product.ProductName,
                    Price = product.UnitPrice
                });

            Console.WriteLine(TAB + "Product Name".PadRight(40) + "Product Average Price".PadLeft(30));
            Console.WriteLine(TAB + "------------".PadRight(40) + "---------------------".PadLeft(30));

            foreach (var product in productsWithL)
            {
                Console.WriteLine(TAB + product.Name.PadRight(40) + product.Price.ToString("C2").PadLeft(30));
            }

            Console.WriteLine();
            Console.WriteLine(TAB + "Press any key to continue.");
            Console.ReadKey();
        }

        /// <summary>
        /// Grab all the things and list them by units in stock -- Wyatt
        /// </summary>
        /// <param name="products"></param>
        private static void OrderByStockAnonymous(List<Product> products)
        {
            string TAB = "    ";
            
            Console.Clear();
            Console.WriteLine(TAB + "List everything in product list and sort by units in stock");
            Console.WriteLine();            
            
            var stock =
                from product in products
                orderby product.UnitsInStock descending
                select new
                {
                    Name = product.ProductName,
                    Stock = product.UnitsInStock
                };

            Console.WriteLine(TAB + "Product Name".PadRight(40) + "Product Stock".PadLeft(30));
            Console.WriteLine(TAB + "------------".PadRight(40) + "-------------".PadLeft(30));

            foreach (var product in stock)
            {
                Console.WriteLine(TAB + product.Name.PadRight(40) + product.Stock.ToString().PadLeft(30));
            }

            Console.WriteLine();
            Console.WriteLine(TAB + "Press any key to continue.");
            Console.ReadKey();
        }
    }
}
