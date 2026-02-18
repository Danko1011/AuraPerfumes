namespace AuraPerfumes.Repositories
{
    public interface ICartRepository
    {
        Task<int> AddItem(int perfumeId, int qty);
            Task<int> RemoveItem(int perfumeId);
        Task<IEnumerable<ShoppingCart>> GetUserCart();
        Task<int> GetCartItemCount(string userId = "");
        Task<ShoppingCart> GetCart(string userId);
    }
}
