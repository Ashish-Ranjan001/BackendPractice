using AutoMapper;
using BackendPractice.ProductModel;
using BackendPractice.DataAccessLayer;
using System;
using Microsoft.EntityFrameworkCore;

namespace BackendPractice.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly AllDbContext allDbContext;
        private readonly IMapper mapper;

        public ProductRepository(AllDbContext allDbContext, IMapper mapper)
        {
            this.allDbContext = allDbContext;
            this.mapper = mapper;
        }

        public async Task<bool> AddProduct(ProductUIModel productUIModel)
        {
            var productToAdd = mapper.Map<ProductDBModel>(productUIModel);
            var existingProduct = allDbContext.Product.FirstOrDefault(p => p.PName == productUIModel.PName);
            if (existingProduct == null)
            {

                if (productUIModel.Photo != null)
                {
                    string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(productUIModel.Photo.FileName);

                    string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "uploads");

                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        productUIModel.Photo.CopyTo(fileStream);
                    }

                    productToAdd.PhotoPath = "uploads/" + uniqueFileName;
                }

              await allDbContext.Product.AddAsync(productToAdd);
              await  allDbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }
        //    // Fix for CS1061: Ensure the correct DbSet property is used
        //    var existingProduct = await allDbContext.AddedProducts
        //        .FirstOrDefaultAsync(p => p.PName == productUIModel.PName);

        //    if (existingProduct == null)
        //    {
        //        var productToAdd = mapper.Map<ProductDBModel>(productUIModel);

        //        if (productUIModel.Photo != null)
        //        {
        //            string uniqueFileName = productUIModel.Photo.FileName;
        //            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "uploads");

        //            if (!Directory.Exists(uploadsFolder))
        //            {
        //                Directory.CreateDirectory(uploadsFolder);
        //            }

        //            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

        //            using (var fileStream = new FileStream(filePath, FileMode.Create))
        //            {
        //                await productUIModel.Photo.CopyToAsync(fileStream);
        //            }

        //            productToAdd.PhotoPath = "uploads/" + uniqueFileName;
        //        }

        //        await allDbContext.AddedProducts.AddAsync(productToAdd);
        //        await allDbContext.SaveChangesAsync();

        //        return true;
        //    }

        //    return false;
        //}

        public async Task<bool> UpdateProduct(int id, ProductUIModel productUIModel)
        {
            var existingProduct = await allDbContext.Product.FirstOrDefaultAsync(p => p.Id == id);

            if (existingProduct != null)
            {
                existingProduct.Category = productUIModel.Category;
                existingProduct.PName = productUIModel.PName;
                existingProduct.Price = productUIModel.Price;
                existingProduct.Quantity = productUIModel.Quantity;
                existingProduct.Features = productUIModel.Features;

                if (productUIModel.Photo != null)
                {
                    string uniqueFileName = productUIModel.Photo.FileName;
                    string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "uploads");

                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await productUIModel.Photo.CopyToAsync(fileStream);
                    }

                    existingProduct.PhotoPath = "uploads/" + uniqueFileName;
                }

                allDbContext.Product.Update(existingProduct);
                await allDbContext.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<bool> DeleteProduct(int id)
        {
            var existingProduct = await allDbContext.Product.FirstOrDefaultAsync(p => p.Id == id);

            if (existingProduct != null)
            {
                allDbContext.Product.Remove(existingProduct);
                await allDbContext.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<ProductDBModel?> GetProduct(int id) // Add nullable return type
        {
            var existingProduct = await allDbContext.Product.FirstOrDefaultAsync(p => p.Id == id);

            if (existingProduct != null)
            {
                //return mapper.Map<ProductUIModel>(existingProduct);
                return existingProduct;
            }

            return new ProductDBModel(); // Ensure all code paths return a value
        }

        public async Task<List<ProductDBModel>> GetAllProducts()
        {
            var products = await allDbContext.Product.ToListAsync();
            return products;
        }

      
    }
    
}
