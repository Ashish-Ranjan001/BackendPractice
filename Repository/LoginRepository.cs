using BackendPractice.DataAccessLayer;
using BackendPractice.AuthModle;
using BackendPractice.Repository;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text;
namespace BackendPractice.Repository
{
    public class LoginRepository : ILoginRepository
    {
        public readonly AllDbContext allDbContext;

        public LoginRepository(AllDbContext allDbContext)
        {
            this.allDbContext = allDbContext;
        }

        public async Task<RegisterDBModel> Login(string email, string password)
        {
            var sha1 = System.Security.Cryptography.SHA1.Create();
            var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(password));
            var hashedPassword = Convert.ToBase64String(hash);

            var user = await allDbContext.User
                .Where(u => u.Email == email && u.Password == hashedPassword) // ✅ FIXED Query
                .FirstOrDefaultAsync();

            if (user == null) return null; // Invalid credentials

            if (!user.IsActive) return null; // Block deactivated users

            return user;
        }

        //public async Task<RegisterDBModel> Login(string email, string password)
        //{
        //    var sha1 = System.Security.Cryptography.SHA1.Create();
        //    var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(password));
        //    var hashedPassword = Convert.ToBase64String(hash);

        //    var user = await allDbContext.User
        //        .Where(u => u.Email == email && u.Password == hashedPassword) // Query using hashed password
        //        .FirstOrDefaultAsync();

        //    return user; // Return null if not found
        //}

        //public async Task<RegisterDBModel> Login(string email, string password)
        //{
        //    // Validate the email and password against the database
        //    var user = await allDbContext.RegisteredUser
        //        .Where(u => u.Email == email && u.Password == password)
        //        .FirstOrDefaultAsync();
        //    //data.Password = model.Password;
        //    var sha1 = System.Security.Cryptography.SHA1.Create();
        //    var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(password));
        //    password = Convert.ToBase64String(hash);
        //    if (password ==password)
        //    {
        //        return user;
        //    }
        //    return null;
        //    // Return true if user exists, otherwise false

    }
}


        
    


