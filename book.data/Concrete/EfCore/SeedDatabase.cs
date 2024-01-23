using System.Linq;
using Microsoft.EntityFrameworkCore;
using book.entity;

namespace book.data.Concrete.EfCore
{
    public static class SeedDatabase
    {
        public static void Seed()
        {
            var context = new ShopContext();

            if (context.Database.GetPendingMigrations().Count() == 0)
            {
                if (context.Categories.Count() == 0)
                {
                    context.Categories.AddRange(Categories);
                }

                if (context.Products.Count() == 0)
                {
                    context.Products.AddRange(Products);
                    context.AddRange(ProductCategories);
                }
            }
            context.SaveChanges();
        }

        private static Category[] Categories = {
            new Category(){Name="Drama",Url="drama"},
            new Category(){Name="Derslik",Url="derslik"},
           
            new Category(){Name="Beddi Eser",Url="beddi-eser"}
        };

        private static Product[] Products = {
            new Product(){Name="Yeddi Gozel",Url="yeddi-gozel",Price=20,ImageUrl="1.jpg",Description="Nizami Gencevi", IsHome=true, IsApproved=true},
            new Product(){Name="Leyli ve Mecnun",Url="leyli-ve-mecnun",Price=30,ImageUrl="2.jpg",Description="Nizami Gencevi", IsApproved=false},
            new Product(){Name="Rübayət",Url="rubayet",Price=24,ImageUrl="3.jpg",Description="Ömər Xeyyam", IsApproved=true},
            new Product(){Name="Arifmetika",Url="arifmetika",Price=12,ImageUrl="4.jpg",Description="Derslik",IsHome=true, IsApproved=false},
            new Product(){Name="Başlanğic kurs müəllimler üçün",Url="başlangic-kurs-muellimler-uçun",Price=6,ImageUrl="5.jpg",Description="Derslik",IsHome=true, IsApproved=true},
            new Product(){Name="Saxby Smart",Url="saxby-smart",Price=12,ImageUrl="6.jpeg",Description="Simon Chiserie", IsApproved=false},
            new Product(){Name="Tatyana Polyakova",Url="arifmetika",Price=12,ImageUrl="7.jpg",Description="Tatyana Polyakova",IsHome=true, IsApproved=false},
            new Product(){Name="Parisin xeyallari",Url="arifmetika",Price=12,ImageUrl="8.jpeg",Description="Natalya Andreeviya", IsApproved=false},
            new Product(){Name="Coyote Weekend",Url="coyote-weekend",Price=12,ImageUrl="9.jpg",Description="Coyote Weekend",IsHome=true, IsApproved=false},
            new Product(){Name="Parisin xeyallari",Url="arifmetika",Price=12,ImageUrl="10.jpg",Description="Violeta stim", IsApproved=false},
            new Product(){Name="Cəlil Məmmedquluzadə",Url="celil-memedquluzade",Price=12,ImageUrl="11.jpg",Description="Cəlil Məmmedquluzadə",IsHome=true, IsApproved=false},
        };

        private static ProductCategory[] ProductCategories ={
            new ProductCategory(){Product=Products[0],Category=Categories[0]},
            new ProductCategory(){Product=Products[0],Category=Categories[2]},
            new ProductCategory(){Product=Products[1],Category=Categories[0]},
            new ProductCategory(){Product=Products[1],Category=Categories[2]},
            new ProductCategory(){Product=Products[2],Category=Categories[0]},
            new ProductCategory(){Product=Products[2],Category=Categories[2]},
            new ProductCategory(){Product=Products[3],Category=Categories[0]},
            new ProductCategory(){Product=Products[3],Category=Categories[2]},
        };
    }
}