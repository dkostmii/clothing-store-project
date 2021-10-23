using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

using clothing_store_api.Models;
using clothing_store_api.Models.Base;
using clothing_store_api.Services;

using clothing_store_api.Types;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;

namespace clothing_store_api.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ILogger<Program> logger, InitContext context)
        {
            context.Database.EnsureCreated();
            logger.LogInformation("Seeding base tables");
            Seed(logger, context);
            logger.LogInformation("Successfully seeded base tables");
        }

        public static async Task AdvancedInit(
                                    IConfiguration config,
                                    InitContext context,
                                    AuthService auth,
                                    ProductsService productsService,
                                    ImagesService imagesService,
                                    ILogger<Program> logger)
        {
            context.Database.EnsureCreated();
            if (!context.Managers.Any())
            {
                await SeedManagers(auth, logger);
            }
            else
            {
                logger.LogInformation("Managers table already seeded");
            }
            if (!context.Customers.Any())
            {
                await SeedCustomers(auth, logger);
            }
            else
            {
                logger.LogInformation("Customers table already seeded");
            }

            var imagePath = config.GetValue<string>("ImagePath");
            if (!context.Products.Any())
            {
                SeedProducts(imagePath, context, productsService, imagesService, logger);
            }
            else
            {
                logger.LogInformation("Products table already seeded");
            }
            logger.LogInformation("Successfully seeded Managers, Customers, Products tables");
        }

        private static void SeedProducts(string imagePath, InitContext context, ProductsService productsService, ImagesService imagesService, ILogger<Program> logger)
        {
            try
            {
                List<string> fileNames = new List<string>
                {
                    $"{imagePath}\\image01.jpg",
                    $"{imagePath}\\image02.jpg",
                    $"{imagePath}\\image03.jfif",
                    $"{imagePath}\\image04.jfif"
                };
                for (int i = 0; i < 45; i++)
                {
                    var product = productsService.CreateProduct(new ProductDTO
                    {
                        Title = "Jeansy",
                        Description = "Klasyczne Jeansy",
                        Price = (decimal)90.0,
                        Available = 10,
                        MaterialId = 1,
                        TypeId = 1,
                        SizeId = 1
                    });
                    foreach (var fileName in fileNames)
                    {
                        using (FileStream file = File.Open(fileName, FileMode.Open))
                        {
                            imagesService.UploadImage(product.Id, file, "image/jpeg");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error seeding Products: " + ex.Message);
            }
            logger.LogInformation("Products table successfully seeded");
        }

        private static async Task SeedCustomers(AuthService auth, ILogger<Program> logger)
        {
            List<AuthResult> results = new List<AuthResult>
            {
                await auth.SignUp(
                new SignUpRole
                    {
                        Role = Role.Customer,
                        User = new SignUpDTO
                        {
                            FirstName = "Grzegorz",
                            LastName = "Brzęczyszczykiewicz",
                            Email = "grzegorzbrzeczyszczykiewicz@poczta.pl",
                            Phone = "+48888888881",
                            Password = "Qwerty1234"
                        }
                    }),
                await auth.SignUp(
                    new SignUpRole
                    {
                        Role = Role.Customer,
                        User = new SignUpDTO
                        {
                            FirstName = "Grzegorz",
                            LastName = "Brzęczyszczykiewicz",
                            Email = "grzegorzbrzeczyszczykiewicz1@poczta.pl",
                            Phone = "+48888888882",
                            Password = "Qwerty1234"
                        }
                    }),
                await auth.SignUp(
                    new SignUpRole
                    {
                        Role = Role.Customer,
                        User = new SignUpDTO
                        {
                            FirstName = "Grzegorz",
                            LastName = "Brzęczyszczykiewicz",
                            Email = "grzegorzbrzeczyszczykiewicz2@poczta.pl",
                            Phone = "+48888888883",
                            Password = "Qwerty1234"
                        }
                    }),
                await auth.SignUp(
                    new SignUpRole
                    {
                        Role = Role.Customer,
                        User = new SignUpDTO
                        {
                            FirstName = "Grzegorz",
                            LastName = "Brzęczyszczykiewicz",   
                            Email = "grzegorzbrzeczyszczykiewicz3@poczta.pl",
                            Phone = "+48888888884",
                            Password = "Qwerty1234"
                        }
                    })
            };
            
            foreach (var result in results)
            {
                if (result.Status != StatusCodes.Status200OK)
                {
                    logger.LogError("Error seeding customers. Object: " + JsonConvert.SerializeObject(result));
                }
            }
            logger.LogInformation("Customers table successfully seeded");
        }

        private static async Task SeedManagers(AuthService auth, ILogger<Program> logger)
        {
            List<AuthResult> results = new List<AuthResult>
            {
                await auth.SignUp(
                new SignUpRole
                {
                    Role = Role.Manager,
                    User = new SignUpDTO
                    {
                        FirstName = "Grzegorz",
                        LastName = "Brzęczyszczykiewicz",
                        Email = "grzegorzmanager@poczta.pl",
                        Phone = "+48888888818",
                        Password = "Qwerty1234"
                    }
                }),
                await auth.SignUp(
                    new SignUpRole
                    {
                        Role = Role.Manager,
                        User = new SignUpDTO
                        {
                            FirstName = "Grzegorz",
                            LastName = "Brzęczyszczykiewicz",
                            Email = "grzegorzmanager1@poczta.pl",
                            Phone = "+48888888819",
                            Password = "Qwerty1234"
                        }
                    })
            };

            foreach (var result in results)
            {
                if (result.Status != StatusCodes.Status200OK)
                {
                    logger.LogError("Error seeding managers. Object: " + JsonConvert.SerializeObject(result));
                }
            }

            logger.LogInformation("Managers table successfully seeded");
        }

        private static void Seed(ILogger<Program> logger, InitContext context)
        {
            SeedColor(logger, context);
            SeedSize(logger, context);
            SeedType(logger, context);
            SeedRaw(logger, context);
            SeedMaterial(logger, context);
        }

        private static void Unseed(ILogger<Program> logger, InitContext context)
        {
            context.Colors.RemoveRange(context.Colors);
            context.SaveChanges();
            context.Sizes.RemoveRange(context.Sizes);
            context.SaveChanges();
            context.Types.RemoveRange(context.Types);
            context.SaveChanges();
            context.Raws.RemoveRange(context.Raws);
            context.SaveChanges();
            context.Materials.RemoveRange(context.Materials);
            context.SaveChanges();
        }



        private static void SeedColor(ILogger<Program> logger, InitContext context)
        {
            if (context.Colors.Any())
            {
                logger.LogInformation("Colors table already seeded");
                return;
            }

            var colors = new Color[]
            {
                new Color { Name = "White", HexCode = "#FFFFFF" },
                new Color { Name = "Black", HexCode = "#000000" },
                new Color { Name = "Khaki", HexCode = "#c3b091" },
                new Color { Name = "Lawngreen", HexCode = "#7CFC00" },
                new Color { Name = "Shafirowy", HexCode = "#082567" }
            };

            foreach (var color in colors)
            {
                context.Colors.Add(color);
            }

            context.SaveChanges();
            logger.LogInformation("Colors table successfully seeded");
        }

        private static void SeedSize(ILogger<Program> logger, InitContext context)
        {
            if (context.Sizes.Any())
            {
                logger.LogInformation("Sizes table already seeded");
                return;
            }

            var sizes = new Size[]
            {
                new Size { Name = "XS" },
                new Size { Name = "S" },
                new Size { Name = "M" },
                new Size { Name = "L" },
                new Size { Name = "XL" },
                new Size { Name = "XXL" }
            };

            foreach (var size in sizes)
            {
                context.Sizes.Add(size);
            }
            context.SaveChanges();
        }

        private static void SeedType(ILogger<Program> logger, InitContext context)
        {
            if (context.Types.Any())
            {
                logger.LogInformation("Types table already seeded");
                return;
            }

            var types = new Models.Base.Type[]
            {
                new Models.Base.Type { Name = "Outerwear" },
                new Models.Base.Type { Name = "Accessories" },
                new Models.Base.Type { Name = "Top" },
                new Models.Base.Type { Name = "Bottom" },
                new Models.Base.Type { Name = "Shoes" }
            };

            foreach (var type in types)
            {
                context.Types.Add(type);
            }

            context.SaveChanges();
            logger.LogInformation("Types table successfully seeded");
        }

        private static void SeedRaw(ILogger<Program> logger, InitContext context)
        {
            if (context.Raws.Any())
            {
                logger.LogInformation("Raws table already seeded");
                return;
            }

            var raws = new Raw[]
            {
                new Raw { Name = "Cotton" },
                new Raw { Name = "Flax" },
                new Raw { Name = "Cashmere" },
                new Raw { Name = "Wool" },
                new Raw { Name = "Silk" },
                new Raw { Name = "Polyamide" },
                new Raw { Name = "Polyester" },
                new Raw { Name = "Acrylic" },
                new Raw { Name = "Microfibres" },
                new Raw { Name = "Modal" },
                new Raw { Name = "Viscose" }

            };

            foreach (var raw in raws)
            {
                context.Raws.Add(raw);
            }

            context.SaveChanges();
            logger.LogInformation("Raws table successfully seeded");
        }

        private static void SeedMaterial(ILogger<Program> logger, InitContext context)
        {
            if (context.Materials.Any())
            {
                logger.LogInformation("Materials table already seeded");
                return;
            }

            var materials = new Material[]
            {
                new Material
                {
                    Title = "Jeans",
                    Description = "Jeans",
                    Color = context.Colors.Find(4),
                    Materials = new MaterialRaw[]
                    {
                        new MaterialRaw
                        {
                            Raw = context.Raws.Where(
                                raw => raw.Name == "Polyamide"
                                ).FirstOrDefault(),
                            Amount = (decimal) 20.0
                        }
                    }
                }
            };

            foreach (var material in materials)
            {
                context.Materials.Add(material);
            }

            context.SaveChanges();
            logger.LogInformation("Materials table successfully seeded");

            return;
        }
    }
}
