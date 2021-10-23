using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using clothing_store_api.Data;
using clothing_store_api.Models;
using clothing_store_api.Models.Products;
using clothing_store_api.Types;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace clothing_store_api.Services
{
    public class ProductsService
    {
        private CustomerContext _customerContext;
        private ManagerContext _managerContext;
        private SystemContext _systemContext;

        private ImagesService _imagesService;

        private ILogger<ProductsService> _logger;

        private readonly string host;


        public ProductsService(IConfiguration config, ILogger<ProductsService> logger, ImagesService imagesService, CustomerContext customerContext, ManagerContext managerContext, SystemContext systemContext)
        {
            host = config.GetValue<string>("Host");

            _logger = logger;
            _customerContext = customerContext;
            _managerContext = managerContext;
            _imagesService = imagesService;
            _systemContext = systemContext;
        }

        public ProductResponse GetProductResponse(Product product)
        {
            return new ProductResponse
            {
                Id = product.Id,
                Images = _imagesService.GetImages(product),
                Title = product.Title,
                Description = product.Description,
                Price = product.Price,
                Available = product.Available,
                Material = product.Material,
                Type = product.Type,
                Size = product.Size
            };
        }

        public Product GetProduct(int productId)
        {
            var product = _systemContext.Products
                .Include(p => p.Images)
                .Include(p => p.Material)
                .Include(m => m.Material.Color)
                .Include(m => m.Material.Materials)
                .ThenInclude(m => m.Raw)
                .Include(p => p.Type)
                .Include(p => p.Size)
                .ToList().Find(p => p.Id == productId);

            if (product != null)
            {
                return product;
            }
            throw new Exception("Product with id: {productId} not found");
        }

        public PaginatedResponse<ProductResponse> GetAllProducts(string queryString, int offset, int limit)
        {
            var count = _customerContext.Products.Count();

            var products = _customerContext.Products
                .Skip(offset * limit).Take(limit)
                .Include(p => p.Images)
                .Include(p => p.Material)
                .Include(m => m.Material.Color)
                .Include(m => m.Material.Materials)
                .ThenInclude(m => m.Raw)
                .Include(p => p.Type)
                .Include(p => p.Size)
                .ToList();


            var resData = new PaginatedResponse<ProductResponse>
            {
                Count = count,
                Results = products.Select(product =>
                {
                    return GetProductResponse(product);
                }).ToList()
            };

            if (offset < count / limit)
            {
                resData.Next = host + $"/Products?{ queryString }offset={offset + 1}&limit={limit}";
            }

            if (offset > 0)
            {
                resData.Prev = host + $"/Products?{ queryString }offset={offset - 1}&limit={limit}";
            }

            return resData;
        }


        public ProductResponse CreateProduct(ProductDTO data)
        {
            var material = _managerContext.Materials.Find(data.MaterialId);

            if (material == null)
                throw new Exception($"Material not found. Id: {data.MaterialId}");

            var prodType = _managerContext.Types.Find(data.TypeId);

            if (prodType == null)
                throw new Exception($"Product type not found. Id: {data.TypeId}");

            var size = _managerContext.Sizes.Find(data.SizeId);

            if (size == null)
                throw new Exception($"Product size not found. Id: {data.SizeId}");


            var newProd = new Product
            {
                Title = data.Title,
                Images = new List<ProductImage>(),
                Description = data.Description,
                Price = data.Price,
                Available = data.Available,
                Material = material,
                Type = prodType,
                Size = size
            };


            _managerContext.Products.Add(newProd);

            _managerContext.SaveChanges();

            return GetProductResponse(newProd);
        }

        public void RemoveProduct(int productId)
        {
            var product = GetProduct(productId);

            if (product == null)
            {
                throw new Exception($"Product id: {productId} does not exist");
            }

            var images = product.Images;

            if (images != null)
            {
                if (images.Count() > 0)
                {
                    foreach (var image in images)
                    {
                        _systemContext.ProductImages.Remove(image);
                    }
                }
            }
            _systemContext.SaveChanges();

            var cartPositions = _systemContext.CartPositions.Where(c => c.Product.Id == product.Id);
            if (cartPositions != null)
            {
                foreach (var c in cartPositions)
                {
                    _systemContext.CartPositions.Remove(c);
                }
            }
            _systemContext.SaveChanges();


            _systemContext.Products.Remove(product);

            _systemContext.SaveChanges();
        }
    }
}
