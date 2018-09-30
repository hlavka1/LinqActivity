using System;
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
        static void Main(string[] args)
        {
            //
            // write all data files to Data folder
            //
            GenerateDataFiles.InitializeXmlFile();

            List<Product> productList = ReadAllProductsFromXml();

            OrderByCatagory(productList); // -- Example, John

            OrderByCatagoryAnoymous(productList); // -- Example, John
            
            OrderByCatagoryExpensive(productList); // -- Morgan
            
            OrderByTotalValueAnonymous(productList); // -- Wyatt
            
            OrderByNameAnonymous(productList); // -- Wyatt
            
            OrderByCanBringOnBusAnoymous(productList); // -- Justina

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

            foreach (var product in mostExpensiveProducts)
            {
                Console.WriteLine(TAB + product.Name.PadRight(20) + product.Price.ToString("C2").PadLeft(15));
            }

            Console.WriteLine();
            Console.WriteLine(TAB + "Average Price:".PadRight(20) + average.ToString("C2").PadLeft(15));

            Console.WriteLine();
            Console.WriteLine(TAB + "Press any key to continue.");
            Console.ReadKey();
        }

        // OrderByUnits(): List the names and units of all products with less than 10 units in stock. Order by units. -- Justina

        private static void OrderByUnits(List<Product> products)
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
                where product.Units == "Beverages"
                orderby product.UnitPrice descending
                select product;

            //
            // lambda syntax
            //
            //var sortedProducts = products.Where(p => p.Units == "Beverages").OrderByDescending(p => p.UnitPrice);

            Console.WriteLine(TAB + "Units".PadRight(15) + "Product Name".PadRight(25) + "Unit Price".PadLeft(10));
            Console.WriteLine(TAB + "--------".PadRight(15) + "------------".PadRight(25) + "----------".PadLeft(10));

            foreach (Product product in sortedProducts)
            {
                Console.WriteLine(TAB + product.Units.PadRight(15) + product.ProductName.PadRight(25) + product.UnitPrice.ToString("C2").PadLeft(10));
            }

            Console.WriteLine();
            Console.WriteLine(TAB + "Press any key to continue.");
            Console.ReadKey();
        }

        private static void OrderByUnitsAnoymous(List<Product> products)
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
                where product.Units == "Beverages" &&
                    product.UnitPrice > 15
                orderby product.UnitPrice descending
                select new
                {
                    Name = product.ProductName,
                    Price = product.UnitPrice
                };
        }

        // OrderByPrice(): List all products with a unit price less than $10. Order by price. -- Justina

        private static void OrderByPrice(List<Product> products)
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
                where product.Price == "Beverages"
                orderby product.UnitPrice descending
                select product;

            //
            // lambda syntax
            //
            //var sortedProducts = products.Where(p => p.Price == "Beverages").OrderByDescending(p => p.UnitPrice);

            Console.WriteLine(TAB + "Price".PadRight(15) + "Product Name".PadRight(25) + "Unit Price".PadLeft(10));
            Console.WriteLine(TAB + "--------".PadRight(15) + "------------".PadRight(25) + "----------".PadLeft(10));

            foreach (Product product in sortedProducts)
            {
                Console.WriteLine(TAB + product.Price.PadRight(15) + product.ProductName.PadRight(25) + product.UnitPrice.ToString("C2").PadLeft(10));
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
                where product.Price == "Beverages" &&
                    product.UnitPrice > 15
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
        
        private static void OrderByCanBringOnBusAnoymous(List<Product> products)
        {
            string TAB = "   ";

            Console.Clear();
            Console.WriteLine(TAB + "List all beverages that cost more the $15 and sort by the unit CanBringOnBus.");
            Console.WriteLine();

            //
            // query syntax
            //
            var sortedProducts =
                from product in products
                where product.CanBringOnBus == "Beverages" &&
                      product.UnitCanBringOnBus == true
                orderby product.UnitCanBringOnBus descending
                select new
                {
                    Name = product.ProductName,
                    CanBringOnBus = product.UnitCanBringOnBus
                      
                };
        }
    }
}
