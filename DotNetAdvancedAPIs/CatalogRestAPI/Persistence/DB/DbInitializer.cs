using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.DB
{
    public class DbInitializer
    {
        public static void Initialize(CatalogDB dbContext)
        {
            if(!dbContext.Categories.Any())
            {
                dbContext.Categories.Add(new Category()
                {
                    Name = "Category1"
                });
                dbContext.Categories.Add(new Category()
                {
                    Name = "Category2"
                });                
                dbContext.SaveChanges();
            }

            if (!dbContext.Products.Any())
            {

                List<Product> products = new List<Product>()
                { new Product() {Name = "Prod 1",CategoryId = 1 },
                  new Product() { Name = "Prod 2", CategoryId = 1 } ,
                  new Product() { Name = "Prod 3", CategoryId = 1 } ,
                  new Product() { Name = "Prod 4", CategoryId = 1 } ,
                  new Product() { Name = "Prod 5", CategoryId = 1 },
                  new Product() { Name = "Prod 6", CategoryId = 1 }
                };

                dbContext.Products.AddRange(products);
                dbContext.SaveChanges();
            }
        }


    }
}
