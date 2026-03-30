namespace AuraPerfumes.Repositories
{
    public interface ICartRepository
    {
        Task<int> AddItem(int perfumeId, int variantId, int qty, string userId);
        Task<int> RemoveItem(int perfumeId, int variantId);
        Task<ShoppingCart> GetUserCart();
        Task<int> GetCartItemCount(string userId);
        Task<ShoppingCart> GetCart(string userId);
    }
}
