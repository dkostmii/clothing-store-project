using clothing_store_api.Data;
using clothing_store_api.Models;
using clothing_store_api.Models.Customers;
using clothing_store_api.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace clothing_store_api.Services
{
    public class CustomerService
    {
        private SystemContext _systemContext;
        private ManagerContext _managerContext;
        private ProductsService _productsService;
        private ILogger<CustomerService> _logger;

        public CustomerService(
                            ILogger<CustomerService> logger,
                            SystemContext systemContext,
                            ManagerContext managerContext,
                            ProductsService productsService)
        {
            _systemContext = systemContext;
            _managerContext = managerContext;
            _productsService = productsService;
            _logger = logger;
        }

        public Customer GetCustomer(int userId)
        {
            var found = _systemContext.Users.Find(userId);

            if (found == null)
            {
                throw new Exception($"User with id: {userId} not found");
            }

            var customer = _systemContext.Customers
                    .Include(c => c.User)
                    .Include(c => c.ShippingInfo)
                    .Include(c => c.Cart)
                        .ThenInclude(cart => cart.Product)
                            .Include(c => c.Cart)
                            .ThenInclude(cart => cart.Product.Images)
                            .Include(c => c.Cart)
                            .ThenInclude(cart => cart.Product.Size)
                            .Include(c => c.Cart)
                            .ThenInclude(cart => cart.Product.Type)
                            .Include(c => c.Cart)
                            .ThenInclude(cart => cart.Product.Material)
                                .ThenInclude(m => m.Color)
                            .Include(c => c.Cart)
                            .ThenInclude(cart => cart.Product.Material)
                                .ThenInclude(m => m.Materials)
                                .ThenInclude(material => material.Raw)
                    .Include(c => c.Orders)
                    .ThenInclude(o => o.ShippingInfo)
                    .ToList()
                    .Find(c => c.User.Id == userId);

            if (customer == null)
            {
                throw new Exception($"Customer for this user not found");
            }

            return customer;
        }

        public CartDTO AddToCart(int userId, int productId, int count)
        {
            _logger.LogInformation($"Adding to user's {userId} cart");

            var customer = GetCustomer(userId);

            var product = _productsService.GetProduct(productId);

            if (count > product.Available)
            {
                throw new Exception("Product is unavailable");
            }

            CartPosition position = new CartPosition
            {
                Product = product,
                Quantity = count
            };

            customer.Cart.Add(position);
            _systemContext.SaveChanges();

            return new CartDTO
            {
                Id = position.Id,
                Product = _productsService.GetProductResponse(position.Product),
                Quantity = position.Quantity
            };
        }
        public void RemoveFromCart(int userId, int cartPositionId)
        {
            var customer = GetCustomer(userId);

            customer.Cart.Remove(customer.Cart.ToList().Find(c => c.Id == cartPositionId));
            _systemContext.SaveChanges();
        }

        public void ClearCart(int userId)
        {
            var customer = GetCustomer(userId);

            foreach (var position in customer.Cart)
            {
                _systemContext.CartPositions.Remove(position);
            }
            _systemContext.SaveChanges();
        }

        public Order Order(int userId, int productId, int count)
        {
            AddToCart(userId, productId, count);
            return Order(userId);
        }

        public Order Order(int userId)
        {
            _logger.LogInformation($"Ordering for user: {userId}");

            var customer = GetCustomer(userId);

            CheckShippingInfo(customer);

            var cart = customer.Cart;

            var summary = (decimal) 0;


            foreach (var position in cart)
            {
                if (position.Quantity > position.Product.Available)
                {
                    throw new Exception("Product is unavailable");
                }

                _logger.LogInformation("Product: " + position.Product.Title + " - Product price: " + position.Product.Price + ", product count: " + position.Quantity);

                position.Product.Available -= position.Quantity;

                summary += position.Product.Price * position.Quantity;
            }

            if (customer.Balance < summary)
            {
                throw new Exception("Insuffiecient balance");
            }

            customer.Balance -= summary;

            var order = new Order 
            {
                CustomerId = customer.Id,
                ShippingInfo = customer.ShippingInfo,
                OrderDate = DateTime.Now,
                OrderSummary = summary
            };

            customer.Orders.Add(order);
            _systemContext.SaveChanges();

            foreach (var position in cart)
            {
                _systemContext.CartPositions.Remove(position);
            }

            _systemContext.SaveChanges();

            return order;
        }
        

        public void CheckShippingInfo(Customer customer)
        {
            if (customer.ShippingInfo == null)
            {
                throw new Exception($"Shipping info missing for customer: {customer.Id}");
            }
        }

        public void AddShippingInfo(int userId, ShippingInfoDTO data)
        {
            var customer = GetCustomer(userId);

            customer.ShippingInfo = new ShippingInfo(data);
            _systemContext.SaveChanges();
        }
    }
}
