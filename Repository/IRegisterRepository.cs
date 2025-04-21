using BackendPractice.AuthModle;

namespace BackendPractice.Repository
{
    public interface IRegisterRepository
    {
        Task <bool> AddUser(RegisterUIModel registerUIModel);
        Task<List<RegisterDBModel>> GetAllUser();
        Task<bool> UpdateUserStatus(string email, bool isActive);
    }
}
