using AutoMapper;
using BackendPractice.AuthModle;
using BackendPractice.DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendPractice.Repository
{
    public class RegisterRepository : IRegisterRepository
    {
        private readonly AllDbContext allDbContext; // Dependency injection for DbContext
        private readonly IMapper mapper; // Dependency injection for AutoMapper

        public RegisterRepository(AllDbContext allDbContext, IMapper mapper)
        {
            this.allDbContext = allDbContext;
            this.mapper = mapper;
        }
        public async Task<bool> AddUser(RegisterUIModel user)
        {
            try
            {
                var existingUser = await allDbContext.User.FirstOrDefaultAsync(x => x.Email == user.Email);

                if (existingUser == null)
                {
                    var sha1 = System.Security.Cryptography.SHA1.Create();
                    var res = mapper.Map<RegisterDBModel>(user);
                    var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(res.Password));
                    res.Password = Convert.ToBase64String(hash);

                    await allDbContext.User.AddAsync(res);
                    await allDbContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AddUser: {ex.Message}");
                return false; // Prevent throwing but indicate failure
            }
        }
        public async Task<List<RegisterDBModel>>GetAllUser( )
        {
            var user = await allDbContext.User.ToListAsync();
            return user;
        }


        public async Task<bool> UpdateUserStatus(string email, bool isActive)
        {
            var user = await allDbContext.User.FirstOrDefaultAsync(x => x.Email == email); // ✅ FIXED

            if (user == null) return false;

            user.IsActive = isActive;
            user.UpdatedAt = DateTime.UtcNow;
            await allDbContext.SaveChangesAsync();
            return true;
        }

    }
}




























































































































//using AutoMapper;
//using BackendPractice.AuthModle;
//using BackendPractice.DataAccessLayer;
//using Microsoft.EntityFrameworkCore;

//namespace BackendPractice.Repository
//{
//    public class RegisterRepository : IRegisterRepository
//    {
//        private readonly AllDbContext allDbContext; // Fixed field declaration to match constructor injection
//        private readonly IMapper mapper;

//        public RegisterRepository(AllDbContext allDbContext, IMapper mapper)
//        {
//            this.allDbContext = allDbContext; // Correctly assigning injected dependency
//            this.mapper = mapper;
//        }

//        public async Task<bool> AddUser(RegisterUIModel user)
//        {
//            var result = mapper.Map<RegisterDBModel>(user);
//            var ex = allDbContext.RegisteredUser.FirstOrDefaultAsync(x => x.Email == user.Email);
//            if(ex == null)
//            {
//               await allDbContext.RegisteredUser.AddAsync(result);
//               await allDbContext.SaveChangesAsync();
//                return true;
//            }
//            else
//            {
//                return false;
//            }
//            //await allDbContext.RegisteredUser.AddAsync(result); // Fixed usage of 'allDbContext' instead of 'AllDbContext'
//            //await allDbContext.SaveChangesAsync(); // Fixed usage of 'allDbContext' instead of 'AllDbContext'
//            //return true;
//        }
//    }
//}
