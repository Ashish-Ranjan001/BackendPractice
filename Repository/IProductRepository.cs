using BackendPractice.ProductModel;
namespace BackendPractice.Repository
{
    public interface IProductRepository
    {
        Task<bool> AddProduct(ProductUIModel productUIModel);
       // Task <ProductUIModel> AddProduct(ProductUIModel productUIModel, IFormFile file);

        Task <bool> UpdateProduct(int id,ProductUIModel productUIModel);

        Task <bool> DeleteProduct(int id);

        Task<ProductDBModel?> GetProduct(int id);

        Task<List<ProductDBModel>> GetAllProducts();

    }
}
