using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuraPerfumes.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartRepository(ApplicationDbContext db, IHttpContextAccessor httpContextAccessor
            , UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<int>AddItem(int perfumeId, int qty)
        {
            string userId = GetUserId();
            using var transaction = _db.Database.BeginTransaction();
            try
            {


                if (string.IsNullOrEmpty(userId))
                    throw new Exception("user is not logged-in");
                var cart = await GetCart(userId);
                if (cart is null)
                {
                    cart = new ShoppingCart
                    {
                        UserId = userId
                    };
                    _db.ShoppingCarts.Add(cart);
                }
                _db.SaveChanges();
                var cartItem = _db.CartDetails.FirstOrDefault(a=>a.ShoppingCartId == cart.Id && a.PerfumeId == perfumeId);
                if (cartItem is not null)
                {
                    cartItem.Quantity += qty;

                }
                else
                {
                    cartItem = new CartDetail
                    {
                        PerfumeId = perfumeId,
                        Quantity = qty,
                        ShoppingCartId = cart.Id
                    };
                    _db.CartDetails.Add(cartItem);
                }
                _db.SaveChanges();
                transaction.Commit();
            }
            catch (Exception ex)
            {
            }
            var cartItemCount = await GetCartItemCount(userId);
            return cartItemCount;
        }
        public async Task<int> RemoveItem(int perfumeId)
        {
            string userId = GetUserId();
            //using var transaction = _db.Database.BeginTransaction();
            try
            {

                if (string.IsNullOrEmpty(userId))
                    throw new Exception("user is not logged-in");
                var cart = await GetCart(userId);
                if (cart is null)
                    throw new Exception("Invalid Cart");
                var cartItem = _db.CartDetails.FirstOrDefault(a => a.ShoppingCartId == cart.Id && a.PerfumeId == perfumeId);
                if (cartItem is null)
                    throw new Exception("No items in cart");
                else if (cartItem.Quantity == 1)                
                    _db.CartDetails.Remove(cartItem);              
                else             
                    cartItem.Quantity = cartItem.Quantity - 1;             
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
            }
            var cartItemCount = await GetCartItemCount(userId);
            return cartItemCount;

        }


        public async Task<ShoppingCart> GetUserCart()
        {
            var userId = GetUserId();
            if (userId == null)
                throw new Exception("Invalid userid");
            var shoppingCart = await _db.ShoppingCarts
                .Include(a=>a.CartDetails)
                .ThenInclude(a=>a.Perfume)
                .ThenInclude(a=> a.Gender)
                .Where(a=>a.UserId == userId).FirstOrDefaultAsync();
            return shoppingCart;
        }



        public async Task<ShoppingCart> GetCart(string userId)
        {
            var cart = await _db.ShoppingCarts.FirstOrDefaultAsync(x => x.UserId == userId);
            return cart;
        }

        public async Task<int> GetCartItemCount(string userId = "")
        {
            if (!string.IsNullOrEmpty(userId))
            {
                userId = GetUserId();
            }
            var data = await (from cart in _db.ShoppingCarts
                              join cartDetails in _db.CartDetails
                              on cart.Id equals cartDetails.ShoppingCartId
                              select new { cartDetails.Id }
                              ).ToListAsync();
            return data.Count;
        }

        private string GetUserId()
        {
            var principal = _httpContextAccessor.HttpContext.User;
            var userId = _userManager.GetUserId(principal);
            return userId;
        }



    }
}

