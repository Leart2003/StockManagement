using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Punim_Diplome.Models;
using Punim_Diplome.Data.Services;
using System.Security.Claims;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace Punim_Diplome.Controllers
{
    public class OrderController : Controller


    {
        private readonly ILogger<OrderController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;


        public OrderController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            
            _context = context;
            _userManager = userManager;

        }
        /// <summary>
        /// Displays the order history for the currently logged-in user.
        /// </summary>
        /// <returns>
        /// The Index view, populated with the list of orders (including product details)
        /// belonging to the current user.
        /// </returns>
        public async Task<IActionResult> Index()

        {

            var userID = _userManager.GetUserId(User);


            var orders = await _context.OrderProducts
                .Include(o => o.Produkt)
                .Where(o => o.UserId == userID)
                .ToListAsync();



            return View(orders);
        }

        /// <summary>
        /// Cancels (deletes) a specific order belonging to the currently logged-in user.
        /// </summary>
        /// <param name="id">The Id of the order to cancel.</param>
        /// <returns>
        /// Redirects back to the Index (order history) page if successful.
        /// Returns a 404 Not Found if the order doesn't exist or doesn't belong to the current user.
        /// </returns>

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelOrder(int id)
        {
            var userID = _userManager.GetUserId(User);

            var orderproduct = await _context.OrderProducts
                .FirstOrDefaultAsync(o => o.Id == id && o.UserId == userID);

            if (orderproduct == null)
            {
                return NotFound();
            }

            _context.OrderProducts.Remove(orderproduct);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }


        /// <summary>
        /// Displays a list of ALL orders in the system, across all users.
        /// Intended for admin use only — shows each order along with its product and the user who placed it.
        /// </summary>
        /// <returns>
        /// The AllOrders view, populated with every order (including related product and user data).
        /// </returns>

        [Authorize(Policy = "AdminEmail")]
        public async Task<IActionResult> AllOrders()
        {
       

            var oders = await _context.OrderProducts
                 .Include(o => o.Produkt)
                 .Include(o => o.User)
                 .ToListAsync();

          
            return View(oders);
        }
        /// <summary>
        /// Displays a form for the current user to place an order for a specific product.
        /// </summary>
        /// <param name="productId">The Id of the product the user wants to order.</param>
        /// <returns>
        /// The OrderForm view, pre-filled with the product info and current user's Id.
        /// </returns>
        [HttpGet]
        public async Task< IActionResult> OrderForm(int productId)
        {
            var userId = _userManager.GetUserId(User);
            var product = await _context.Produktet.FindAsync(productId);
            var model = new OrderProduct
            {
                ProductId = productId,
                UserId = userId,
                Produkt = product

            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> OrderForm(OrderProduct order)
        {
            if (!ModelState.IsValid)
            {
                return View(order);
            }

            order.OrderDate = DateTime.Now;

            _context.OrderProducts.Add(order);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Order"); // Or a confirmation page
        }

        /// <summary>
        /// Updates the status of a specific order (e.g. to "Shipped", "Cancelled", "Delivered").
        /// Intended for admin use, as part of managing all orders.
        /// </summary>
        /// <param name="id">The Id of the order to update.</param>
        /// <param name="status">The new status value to set on the order.</param>
        /// <returns>
        /// Redirects to the AllOrders page after updating.
        /// Returns a 404 Not Found if no order matches the given id.
        /// </returns>
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            var order = await _context.OrderProducts.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }
            
            order.OrderStatus = status;

            _context.OrderProducts.Update(order);

            await _context.SaveChangesAsync();


            return RedirectToAction("AllOrders");


        }



    }

}
