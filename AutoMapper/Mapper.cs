using AutoMapper;

namespace BackendPractice.AutoMapper
{
    public class Mapper: Profile
    {
        public Mapper()
        {
            CreateMap<ProductModel.ProductUIModel, ProductModel.ProductDBModel>().ReverseMap();
            CreateMap<AuthModle.RegisterUIModel, AuthModle.RegisterDBModel>().ReverseMap();
        }
    }
}
