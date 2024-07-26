using ETICARET.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETICARET.DataAccess.Concrete.EfCore
{
    public class SeedDatabase
    {
        public static void Seed()
        {
            var context = new DataContext();


            if (context.Database.GetPendingMigrations().Count() == 0)
            {
                if (context.Categories.Count() == 0)
                {
                    context.AddRange(Categories);
                }

                if (context.Products.Count() == 0)
                {
                    context.AddRange(Products);
                    context.AddRange(ProductCategories);
                }

                context.SaveChanges();
            }
        }

        private static Category[] Categories =
        {
            new Category(){ Name = "Telefon"},
            new Category(){ Name = "Bilgisayar"},
            new Category(){ Name = "Elektronik"},
            new Category(){ Name = "Ev Gereçleri"}
        };

        private static Product[] Products =
        {
            new Product(){ Name = "Samsung Note 8" , Price = 15000, Images = { new Image() {ImageUrl = "samsung.jpg" },  new Image() {ImageUrl = "samsung2.jpg" }, new Image() {ImageUrl = "samsung3.jpg" }, new Image() {ImageUrl = "samsung4.jpg" } },Description ="<p>Güzel telefon</p>" },
            new Product(){ Name = "Samsung Note 7" , Price = 6000, Images = { new Image() {ImageUrl = "samsung5.jpg" },  new Image() {ImageUrl = "samsung6.jpg" }, new Image() {ImageUrl = "samsung7.jpg" }, new Image() {ImageUrl = "samsung8.jpg" } },Description ="<p>Samsung Note 7 Farkı ile Tanışın</p>" },
            new Product(){ Name = "Samsung Note 8" , Price = 7000, Images = { new Image() {ImageUrl = "samsung9.jpg" },  new Image() {ImageUrl = "samsung10.jpg" }, new Image() {ImageUrl = "samsung1.jpg" }, new Image() {ImageUrl = "samsung4.jpg" } },Description ="<p>Samsung Note 8 ile Anı ölümsüzleştirin</p>" },
            new Product(){ Name = "Samsung Note 9" , Price = 9000, Images = { new Image() {ImageUrl = "samsung3.jpg" },  new Image() {ImageUrl = "samsung4.jpg" }, new Image() {ImageUrl = "samsung5.jpg" }, new Image() {ImageUrl = "samsung7.jpg" } },Description ="<p>Samsung Note 9 u çıkmış almalıyım</p>" },
            new Product(){ Name = "Samsung Note 10" , Price = 13000, Images = { new Image() {ImageUrl = "samsung7.jpg" },  new Image() {ImageUrl = "samsung8.jpg" }, new Image() {ImageUrl = "samsung9.jpg" }, new Image() {ImageUrl = "samsung3.jpg" } },Description ="<p>Samsung Note 10 mu la o</p>" },
            new Product(){ Name = "iPhone 14 128 GB" , Price = 50000, Images = { new Image() {ImageUrl = "iphone14.jpg" },  new Image() {ImageUrl = "iphone15.jpg" }, new Image() {ImageUrl = "iphone14mini.jpg" }, new Image() {ImageUrl = "iphone12.jpg" } },Description ="<p>elma</p>" },
            new Product(){ Name = "İphone 13 128 GB" , Price = 36000, Images = { new Image() {ImageUrl = "iphone5.jpg" },  new Image() {ImageUrl = "iphone6.jpg" }, new Image() {ImageUrl = "iphone15.jpg" }, new Image() {ImageUrl = "iphone13.jpg" } },Description ="<p>3 kameralı elma</p>" },
            new Product(){ Name = "İphone 12 128 GB" , Price = 36000, Images = { new Image() {ImageUrl = "iphone5.jpg" },  new Image() {ImageUrl = "iphone6.jpg" }, new Image() {ImageUrl = "iphone15.jpg" }, new Image() {ImageUrl = "iphone13.jpg" } },Description ="<p>hocanın telefonundan</p>" },
            new Product(){ Name = "İphone 8 128 GB" , Price = 15000, Images = { new Image() {ImageUrl = "iphone11.jpg" },  new Image() {ImageUrl = "iphone5.jpg" }, new Image() {ImageUrl = "iphone15.jpg" }, new Image() {ImageUrl = "iphone.jpg" } },Description ="<p>Emektar Elma</p>" },
            new Product(){ Name = "İphone 15 128 GB" , Price = 90000, Images = { new Image() {ImageUrl = "iphone13.jpg" },  new Image() {ImageUrl = "iphone15.jpg" }, new Image() {ImageUrl = "iphone14mini.jpg" }, new Image() {ImageUrl = "iphone.jpg" } },Description ="<p>Amasya Elması</p>" },
            new Product(){ Name = "samsung monitör" , Price = 50000, Images = { new Image() {ImageUrl = "samsungmonitor3.jpg" },  new Image() {ImageUrl = "samsungmonitor1.jpg" }, new Image() {ImageUrl = "samsungmonitor2.jpg" }, new Image() {ImageUrl = "samsungmonitor4.jpg" } },Description ="<p>Evrim teorisi</p>" },
            new Product(){ Name = "Ipad" , Price = 7000, Images = { new Image() {ImageUrl = "ipad3.jpg" },  new Image() {ImageUrl = "ipad6.jpg" }, new Image() {ImageUrl = "ipad5.jpg" }, new Image() {ImageUrl = "ipad3.jpg" } },Description ="<p>Tablet</p>" },
            new Product(){ Name = "Ipad3" , Price = 7000, Images = { new Image() {ImageUrl = "ipad3.jpg" },  new Image() {ImageUrl = "ipad6.jpg" }, new Image() {ImageUrl = "ipad5.jpg" }, new Image() {ImageUrl = "ipad3.jpg" } },Description ="<p>Bunda pubg ne oynanır</p>" },
            new Product(){ Name = "Tablet Casper" , Price = 3000, Images = { new Image() {ImageUrl = "tablet1.jpg" },  new Image() {ImageUrl = "tablet2.jpg" }, new Image() {ImageUrl = "tablet.jpg" }, new Image() {ImageUrl = "tablet2.jpg" } },Description ="<p>8 gigotyt ram var</p>" },
            new Product(){ Name = "Tablet" , Price = 4000, Images = { new Image() {ImageUrl = "tablet1.jpg" },  new Image() {ImageUrl = "tablet2.jpg" }, new Image() {ImageUrl = "tablet.jpg" }, new Image() {ImageUrl = "tablet2.jpg" } },Description ="<p>tablet</p>" },
            new Product(){ Name = "Monster notebook" , Price = 22000, Images = { new Image() {ImageUrl = "monster2.jpg" },  new Image() {ImageUrl = "monster.jpg" }, new Image() {ImageUrl = "monster2.jpg" }, new Image() {ImageUrl = "monster.jpg" } },Description ="<p>Güzel laptop</p>" },
            new Product(){ Name = "Excalibur notebook" , Price = 40000, Images = { new Image() {ImageUrl = "excalibur.jpg" },  new Image() {ImageUrl = "macpro.jpg" }, new Image() {ImageUrl = "monster.jpg" }, new Image() {ImageUrl = "slimeasus.jpg" } },Description ="<p>Güzel laptop</p>" },
            new Product(){ Name = "Lenovo IdeaPad" , Price = 8000, Images = { new Image() {ImageUrl = "slimeasus.jpg" },  new Image() {ImageUrl = "asus5.jpg" }, new Image() {ImageUrl = "asus7.jpg" }, new Image() {ImageUrl = "asus2.jpg" } },Description ="<p>Asus alınır ya</p>" },
            new Product(){ Name = "HP 2W6K4EA Pavilion" , Price = 12000, Images = { new Image() {ImageUrl = "asus7.jpg" },  new Image() {ImageUrl = "slimeasus.jpg" }, new Image() {ImageUrl = "asus2.jpg" }, new Image() {ImageUrl = "asus5.jpg" } },Description ="<p>Al bu kirazdan kalmaz birazdan</p>" },
            new Product(){ Name = "Asus X515JF Core i7" , Price = 17000, Images = { new Image() {ImageUrl = "asuslptp.jpg" },  new Image() {ImageUrl = "slimeasus.jpg" }, new Image() {ImageUrl = "asuslptp.jpg" }, new Image() {ImageUrl = "monster.jpg" } },Description ="<p>Asus X515JF Core i7</p>" },
            new Product(){ Name = "Dyson Süpürge" , Price = 35000, Images = { new Image() {ImageUrl = "dayson2.jpg" },  new Image() {ImageUrl = "dayson3.jpg" }, new Image() {ImageUrl = "dyson3.jpg" }, new Image() {ImageUrl = "dyson.jpg" } },Description ="<p>havanızı değiştirin</p>" },
            new Product(){ Name = "Ütü" , Price = 3000, Images = { new Image() {ImageUrl = "utu.jpg" },  new Image() {ImageUrl = "utu.jpg" }, new Image() {ImageUrl = "utu.jpg" }, new Image() {ImageUrl = "utu.jpg" } },Description ="<p>Güzel Laptop</p>" },
            new Product(){ Name = "Philips airfryer" , Price = 15000, Images = { new Image() {ImageUrl = "air.jpg" },  new Image() {ImageUrl = "air.jpg" }, new Image() {ImageUrl = "air.jpg" }, new Image() {ImageUrl = "air.jpg" } },Description ="<p>Yeni gelin Çeyizi</p>" },
            new Product(){ Name = "Karaca " , Price = 5000, Images = { new Image() {ImageUrl = "karaca.jpg" },  new Image() {ImageUrl = "karaca2.jpg" }, new Image() {ImageUrl = "karaca.jpg" }, new Image() {ImageUrl = "karaca2.jpg" } },Description ="<p>Karaca</p>" },
            new Product(){ Name = "Rowta Saç Düzleştirici" , Price =1000, Images = { new Image() {ImageUrl = "sacduz2.jpg" },  new Image() {ImageUrl = "sackurut2.jpg" }, new Image() {ImageUrl = "sackurut2.jpg" }, new Image() {ImageUrl = "sacduz2.jpg" } },Description ="<p>Kaliteli</p>" },
            new Product(){ Name = "Kendwood KATLE " , Price = 3000, Images = { new Image() {ImageUrl = "katle.jpg" },  new Image() {ImageUrl = "katle.jpg" }, new Image() {ImageUrl = "katle.jpg" }, new Image() {ImageUrl = "katle.jpg" } },Description ="<p>Kaliteli Katle</p>" },
            new Product(){ Name = "Tencere " , Price = 3000, Images = { new Image() {ImageUrl = "TencereSeti1.jpg" },  new Image() {ImageUrl = "TencereSeti2.jpg" }, new Image() {ImageUrl = "TencereSeti3.jpg" }, new Image() {ImageUrl = "TencereSeti4.jpg" } },Description ="<p>Tencere Seti</p>" },
            new Product(){ Name = "Logitech " , Price = 3500, Images = { new Image() {ImageUrl = "Logitech3.jpg" },  new Image() {ImageUrl = "Logitech2.jpg" }, new Image() {ImageUrl = "Logitech1.jpg" }, new Image() {ImageUrl = "Logitech.jpg" } },Description ="<p>Hocanın Mousu</p>" },
            new Product(){ Name = "Dell Monitör " , Price = 22000, Images = { new Image() {ImageUrl = "Dell.jpg" },  new Image() {ImageUrl = "Dell1.jpg" }, new Image() {ImageUrl = "Dell2.jpg" }, new Image() {ImageUrl = "Dell3.jpg" } },Description ="<p>Efsane Monitör</p>" }

        };

        private static ProductCategory[] ProductCategories =
        {
            new ProductCategory(){ Product = Products[0],Category=Categories[0]},
            new ProductCategory(){ Product = Products[1],Category=Categories[0]},
            new ProductCategory(){ Product = Products[2],Category=Categories[0]},
            new ProductCategory(){ Product = Products[3],Category=Categories[0]},
            new ProductCategory(){ Product = Products[4],Category=Categories[0]},
            new ProductCategory(){ Product = Products[5],Category=Categories[0]},
            new ProductCategory(){ Product = Products[6],Category=Categories[0]},
            new ProductCategory(){ Product = Products[7],Category=Categories[1]},
            new ProductCategory(){ Product = Products[8],Category=Categories[1]},
            new ProductCategory(){ Product = Products[9],Category=Categories[1]},
            new ProductCategory(){ Product = Products[10],Category=Categories[0]},
            new ProductCategory(){ Product = Products[11],Category=Categories[2]},
            new ProductCategory(){ Product = Products[12],Category=Categories[2]},
            new ProductCategory(){ Product = Products[13],Category=Categories[2]},
            new ProductCategory(){ Product = Products[14],Category=Categories[2]},
            new ProductCategory(){ Product = Products[15],Category=Categories[1]},
            new ProductCategory(){ Product = Products[16],Category=Categories[1]},
            new ProductCategory(){ Product = Products[17],Category=Categories[1]},
            new ProductCategory(){ Product = Products[18],Category=Categories[1]},
            new ProductCategory(){ Product = Products[19],Category=Categories[3]},
            new ProductCategory(){ Product = Products[20],Category=Categories[3]},
            new ProductCategory(){ Product = Products[21],Category=Categories[3]},
            new ProductCategory(){ Product = Products[22],Category=Categories[3]},
            new ProductCategory(){ Product = Products[23],Category=Categories[3]},
            new ProductCategory(){ Product = Products[24],Category=Categories[3]},
            new ProductCategory(){ Product = Products[25],Category=Categories[3]},
            new ProductCategory(){ Product = Products[26],Category=Categories[3]},
            new ProductCategory(){ Product = Products[27],Category=Categories[3]}
        };
    }
}
