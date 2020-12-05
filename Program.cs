using System;
using NLog.Web;
using System.IO;
using System.Linq;
using NorthwindConsole.Model;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace NorthwindConsole
{
    class Program
    {
         // create static instance of Logger
        private static NLog.Logger logger = NLogBuilder.ConfigureNLog(Directory.GetCurrentDirectory() + "\\nlog.config").GetCurrentClassLogger();
        static void Main(string[] args)
        {
            logger.Info("Program started");

            try
            {
                string choice;
                do
                {
                    Console.WriteLine("1) Display Categories");
                    Console.WriteLine("2) Add Category");
                    Console.WriteLine("3) Display Category and related products");
                    Console.WriteLine("4) Display all Categories and their related products");
                    Console.WriteLine("5) Add Product");
                    Console.WriteLine("6) Edit Product");
                    Console.WriteLine("7) Display Product");
                    Console.WriteLine("8) Display Product records");
                  
                    Console.WriteLine("\"q\" to quit");
                    choice = Console.ReadLine();
                    Console.Clear();

                     logger.Info($"Option {choice} selected");
                    if (choice == "1")
                    {
                        var db = new NorthwindConsole_32_JMKContext();
                        var query = db.Categories.OrderBy(p => p.CategoryName);

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"{query.Count()} records returned");
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        foreach (var item in query)
                        {
                            Console.WriteLine($"{item.CategoryName} - {item.Description}");
                        }
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                     else if (choice == "2")
                    {
                        Category category = new Category();
                        Console.WriteLine("Enter Category Name:");
                        category.CategoryName = Console.ReadLine();
                        Console.WriteLine("Enter the Category Description:");
                        category.Description = Console.ReadLine();
                        
                        ValidationContext context = new ValidationContext(category, null, null);
                        List<ValidationResult> results = new List<ValidationResult>();

                        var isValid = Validator.TryValidateObject(category, context, results, true);
                        if (isValid)
                        {
                            var db = new NorthwindConsole_32_JMKContext();
                            // check for unique name
                            if (db.Categories.Any(c => c.CategoryName == category.CategoryName))
                            {
                                // generate validation error
                                isValid = false;
                                results.Add(new ValidationResult("Bro this name already exists", new string[] { "CategoryName" }));
                            }
                            else
                            {
                                logger.Info("Validation passed");
                                // TODO: save category to db
                            }
                        }
                        if (!isValid)
                        {
                            foreach (var result in results)
                            {
                                logger.Error($"{result.MemberNames.First()} : {result.ErrorMessage}");
                            }
                        }
                    }
                    else if (choice == "3")
                    {
                        var db = new NorthwindConsole_32_JMKContext();
                        var query = db.Categories.OrderBy(p => p.CategoryId);

                        Console.WriteLine("Select the category whose products you want to display:");
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        foreach (var item in query)
                        {
                            Console.WriteLine($"{item.CategoryId}) {item.CategoryName}");
                        }
                        Console.ForegroundColor = ConsoleColor.White;
                        int id = int.Parse(Console.ReadLine());
                        Console.Clear();
                        logger.Info($"CategoryId {id} selected");
                        Category category = db.Categories.Include("Products").FirstOrDefault(c => c.CategoryId == id);
                        Console.WriteLine($"{category.CategoryName} - {category.Description}");
                        foreach (Product p in category.Products)
                        {
                            Console.WriteLine(p.ProductName);
                        }
                    }
                     else if (choice == "4")
                    {
                        var db = new NorthwindConsole_32_JMKContext();
                        var query = db.Categories.Include("Products").OrderBy(p => p.CategoryId);
                        foreach (var item in query)
                        {
                            Console.WriteLine($"{item.CategoryName}");
                            foreach (Product p in item.Products)
                            {
                                Console.WriteLine($"\t{p.ProductName}");
                            }
                        }
                    }
                     else if (choice == "5")
                    {
                        Product product = new Product();
                        Console.WriteLine("Enter Product Name:");
                        product.ProductName = Console.ReadLine();
                        Console.WriteLine("Enter the Supplier ID:");
                        product.SupplierId = Console.Read();
                        Console.WriteLine("Enter the Category ID:");
                        product.CategoryId = Console.Read();
                        Console.WriteLine("Enter the Quantity Per Unit:");
                        product.QuantityPerUnit = Console.ReadLine();
                        Console.WriteLine("Enter the Unit Price:");
                        product.UnitPrice = Console.Read();
                        Console.WriteLine("Enter the Units In Stock:");
                        product.UnitsInStock = Console.Read();
                        Console.WriteLine("Enter the Units On Order:");
                        product.UnitsOnOrder = Console.Read();
                        Console.WriteLine("Enter the Reorder Level:");
                        product.ReorderLevel = Console.Read();

                        Console.WriteLine("The product was added");


                    


                    } else if (choice == "6")
                    {
                         Console.WriteLine("Choose the product to edit:");
                        var db = new NorthwindConsole_32_JMKContext();
                        var product = db.Products.OrderBy(p => p.ProductName);
                        foreach (var item in product)
                        {
                            Console.WriteLine($"{item.ProductName}");
                        }
                          string namedProduct = Console.ReadLine();
                          Console.WriteLine("What is the new name of the product?");
                          string newNamedProduct = Console.ReadLine();


                        
                    } else if (choice == "7")
                    {
                        var db = new NorthwindConsole_32_JMKContext();
                        var query = db.Products.OrderBy(p => p.ProductName);
                        Console.WriteLine($"{query.Count()} records returned");

                         foreach (var item in query)
                        {
                            Console.WriteLine($"{item.ProductName} SupplierID: {item.SupplierId} CategoryId: {item.CategoryId} QuantityPerUnit: {item.QuantityPerUnit} UnitPrice: {item.UnitPrice} UnitsInStock: {item.UnitsInStock} UnitsOnOrder: {item.UnitsOnOrder} ReorderLevel: {item.ReorderLevel} Discontinued: {item.Discontinued} ");
                        }

                        
                    } else if (choice == "8")
                    {
                         var db = new NorthwindConsole_32_JMKContext();
                        var query = db.Products.OrderBy(p => p.ProductName);
                        Console.WriteLine($"{query.Count()} records returned");
                        Console.WriteLine("Do you want to view (1) Discontinued products or (2) Active products?");
                        string choose = Console.ReadLine();

                        if (choose == "1"){
                            foreach (var item in query)
                        {
                            if (item.Discontinued == true){
                            Console.WriteLine($"{item.ProductName}");
                            }
                        }
                        }
                        if (choose == "2"){
                             foreach (var item in query)
                        {
                            if (item.Discontinued == false){
                            Console.WriteLine($"{item.ProductName}");
                            }
                        }

                        }
                        
                    }
                
                    Console.WriteLine();

                } while (choice.ToLower() != "q");
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }

            logger.Info("Program ended");
        }
    }
}
