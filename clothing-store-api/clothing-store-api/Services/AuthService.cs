using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

using clothing_store_api.Types;
using clothing_store_api.Data;
using clothing_store_api.Util;
using clothing_store_api.Models;
using clothing_store_api.Models.Customers;
using clothing_store_api.Models.Base;
using Microsoft.EntityFrameworkCore;

namespace clothing_store_api.Services
{
    public class AuthService
    {
        SystemContext _context;
        ManagerContext _managerContext;
        IJwtGenerator _jwt;

        public AuthService(
                        SystemContext context,
                        ManagerContext managerContext,
                        IJwtGenerator jwt)
        {
            _context = context;
            _managerContext = managerContext;
            _jwt = jwt;
        }

        public async Task<AuthResult> SignIn(SignInRole data)
        {
            User found = _context.Users.FirstOrDefault(user => user.Email == data.User.Login || user.Phone == data.User.Login);

            if (found == null)
                return await Task.Run(
                    () => new AuthResult
                    { 
                        Message = "User with provided credentials not found.",
                        Status = StatusCodes.Status404NotFound
                    });

            var crypto = new CryptographyProcessor();

            if (!crypto.AreEqual(data.User.Password, found.Password, found.Salt))
            {
                return await Task.Run(
                    () => new AuthResult {
                        Message = "Invalid password.",
                        Status = StatusCodes.Status401Unauthorized
                    });
            }

            var salt = crypto.CreateSalt(32);

            var hashedPassword = crypto.GenerateHash(data.User.Password, salt);

            found.Password = hashedPassword;
            found.Salt = salt;
            found.LastLogin = DateTime.Now;

            await _context.SaveChangesAsync();

            string token = _jwt.CreateToken(new UserRole { User = found.HideSensitive(), Role = data.Role });

            return await Task.Run(() => new AuthResult
            {
                Message = "Sign in success",
                Status = StatusCodes.Status200OK,
                User = found.HideSensitive(),
                Token = token
            });
        }

        

        public async Task<AuthResult> SignUp(SignUpRole data)
        {
            User found = _context.Users.FirstOrDefault(user => user.Email == data.User.Email || user.Phone == data.User.Phone);

            if (found != null)
                return await Task.Run(
                    () => new AuthResult 
                    { 
                        Message = "User with provided credentials already exists.",
                        Status = StatusCodes.Status409Conflict
                    });

            var crypto = new CryptographyProcessor();

            var salt = crypto.CreateSalt(32);

            var hashedPassword = crypto.GenerateHash(data.User.Password, salt);

            User created = new User
            {
                FirstName = data.User.FirstName,
                LastName = data.User.LastName,
                Email = data.User.Email,
                Phone = data.User.Phone,
                Password = hashedPassword,
                Salt = salt,
                RegisteredAt = DateTime.Now
            };

            _context.Users.Add(created);
            await _context.SaveChangesAsync();

            if (data.Role.Value == Role.Customer.Value)
            {
                Customer createdCustomer = new Customer
                {
                    User = created,
                    Balance = (decimal) 0.0,
                    Cart = new List<CartPosition>(),
                    Orders = new List<Order>(),
                };

                _context.Customers.Add(createdCustomer);

                await _context.SaveChangesAsync();

            }
            else if (data.Role.Value == Role.Manager.Value)
            {
                Manager createdManager = new Manager
                {
                    User = created
                };

                _context.Managers.Add(createdManager);

                await _context.SaveChangesAsync();
            }

            string token = _jwt.CreateToken(new UserRole { User = created.HideSensitive(), Role = data.Role });

            return await Task.Run(() => new AuthResult 
            { 
                Message = "Sign up success.", 
                Status = StatusCodes.Status200OK,
                User = created.HideSensitive(), 
                Token = token 
            });
        }

        public UserDTO UpdateUserAccount(int userId, OptionalUserDTO data)
        {
            var user = _context.Users.Find(userId);

            if (user == null)
            {
                throw new Exception($"User with id: {userId} not found");
            }

            if (data.Password != null)
            {
                var crypto = new CryptographyProcessor();

                var salt = crypto.CreateSalt(32);

                var hashedPassword = crypto.GenerateHash(data.Password, salt);

                user.Password = hashedPassword;
                user.Salt = salt;

                _context.SaveChanges();
            }


            user.FirstName = data.FirstName != null ? data.FirstName : user.FirstName;
            user.LastName = data.LastName != null ? data.LastName : user.LastName;
            user.Email = data.Email != null ? data.Email : user.Email;
            user.Phone = data.Phone != null ? data.Phone : user.Phone;

            _context.SaveChanges();

            return user.HideSensitive();
        }

        public void DeleteUserAccount(int userId)
        {
            var user = _context.Users.Find(userId);

            if (user == null)
            {
                throw new Exception($"User with id: {userId} not found");
            }

            // Find customer or manager for this user
            var associated = new
            {
                manager = _context.Managers
                    .Include(m => m.User)
                    .Where(m => m.User.Id == userId)
                    .ToList().FirstOrDefault(),
                customer = _context.Customers
                    .Include(c => c.User)
                    .Include(c => c.ShippingInfo)
                    .Include(c => c.Cart)
                    .Include(c => c.Orders)
                    .Where(c => c.User.Id == userId)
                    .ToList().FirstOrDefault()
            };

            if (associated.manager == null && associated.customer == null)
            {
                throw new Exception($"Associated accounts for user id: {userId} not found");
            }

            // Delete manager record
            if (associated.manager != null)
            {
                _context.Managers.Remove(associated.manager);
                _context.SaveChanges();
            }
            // Delete customer record
            else if (associated.customer != null)
            {
                foreach (var order in associated.customer.Orders)
                {
                    _managerContext.Orders.Remove(order);
                    _managerContext.SaveChanges();
                }
                foreach (var cartPosition in associated.customer.Cart)
                {
                    _managerContext.CartPositions.Remove(cartPosition);
                    _managerContext.SaveChanges();
                }

                if (associated.customer.ShippingInfo != null)
                {
                    _managerContext.ShippingInfos.Remove(associated.customer.ShippingInfo);
                    _managerContext.SaveChanges();
                }

                _context.Customers.Remove(associated.customer);

                _context.SaveChanges();
            }

            _context.Users.Remove(user);
        }

        public Manager GetManager(int userId)
        {
            var found = _context.Users.Find(userId);

            if (found == null)
            {
                throw new Exception($"User with id: {userId} not found");
            }

            var manager = _context.Managers.Include(c => c.User).Where(c => c.User.Id == userId).ToList().FirstOrDefault();

            if (manager == null)
            {
                throw new Exception($"Manager for this user not found");
            }

            return manager;
        }
    }
}
