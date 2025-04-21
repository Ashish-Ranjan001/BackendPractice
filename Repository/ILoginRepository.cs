using BackendPractice.AuthModle;

namespace BackendPractice.Repository
{
    public interface ILoginRepository
    {
        Task<RegisterDBModel> Login(string email, string password);
    }
}
