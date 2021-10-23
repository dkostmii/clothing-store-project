using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using clothing_store_api.Data;
using clothing_store_api.Models;
using clothing_store_api.Models.Products;
using clothing_store_api.Types;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace clothing_store_api.Services
{
    public class ImagesService
    {
        private readonly string host;

        private CustomerContext _customerContext;
        private ManagerContext _managerContext;

        ILogger<ImagesService> _logger;

        public ImagesService(
                    ILogger<ImagesService> logger,
                    CustomerContext customerContext,
                    ManagerContext managerContext,
                    IConfiguration config)
        {
            _logger = logger;
            _customerContext = customerContext;
            _managerContext = managerContext;

            host = config.GetValue<string>("Host");
        }

        private Image GetImageDTO(ProductImage image)
        {
            return new Image
            {
                Id = image.Id,
                ImageType = image.ImageMimeType,
                Url = host + $"/Products/Images/{image.Id}"
            };
        }

        // Get images for product
        public ICollection<Image> GetImages(int productId)
        {
            var product = _customerContext.Products.Include(p => p.Images).ToList()
                .Find(p => p.Id == productId);

            if (product == null)
            {
                throw new Exception($"Product with id: {productId} not found");
            }

            return product.Images.Select(image => GetImageDTO(image)).ToList();
        }

        public ICollection<Image> GetImages(Product product)
        {
            return product.Images.Select(productImage =>
            {
                return GetImageDTO(productImage);
            }).ToList();
        }

        // Fetch image
        public ProductImage FetchImage(int id)
        {
            var image = _customerContext.ProductImages.Find(id);

            if (image != null)
            {
                return image;
            }

            throw new Exception($"Image with id: {id} not found");
        }

        public void UploadImage(int productId, IFormFile file)
        {
            var product = _managerContext.Products.Include(p => p.Images).ToList().Find(p => p.Id == productId);

            if (product == null)
            {
                throw new Exception($"Product with id: {productId} not found");
            }

            using (MemoryStream ms = new MemoryStream())
            {
                file.CopyTo(ms);
                var bytes = ms.ToArray();

                var image = new ProductImage
                {
                    ImageData = bytes,
                    ImageMimeType = file.ContentType
                };

                if (product.Images == null)
                    product.Images = new List<ProductImage>() { image };
                else
                    product.Images.Add(image);

                _managerContext.SaveChanges();
            }
        }

        public void UploadImage(int productId, FileStream file, string imageMimeType)
        {
            var product = _managerContext.Products.Include(p => p.Images).ToList().Find(p => p.Id == productId);

            if (product == null)
            {
                throw new Exception($"Product with id: {productId} not found");
            }

            using (MemoryStream ms = new MemoryStream())
            {
                file.CopyTo(ms);
                var bytes = ms.ToArray();

                var image = new ProductImage
                {
                    ImageData = bytes,
                    ImageMimeType = imageMimeType
                };

                if (product.Images == null)
                    product.Images = new List<ProductImage>() { image };
                else
                    product.Images.Add(image);

                _managerContext.SaveChanges();
            }
        }
    }
}
