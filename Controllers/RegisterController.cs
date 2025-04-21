using BackendPractice.AuthModle;
using BackendPractice.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BackendPractice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegisterController : ControllerBase
    {
        private readonly IRegisterRepository registerRepository;

        public RegisterController(IRegisterRepository registerRepository)
        {
            this.registerRepository = registerRepository;
        }

       


     
        [HttpPost]
        [Route("AddUser")]
        public async Task<IActionResult> AddUser([FromBody] RegisterUIModel registerUIModel)
        {
            if (registerUIModel == null)
            {
                return BadRequest(new { err = "Received null user model. Ensure JSON is correctly formatted." });
            }

            try
            {
                var isUserAdded = await registerRepository.AddUser(registerUIModel);

                if (isUserAdded)
                {
                    return Ok(new { msg = "User registered successfully." });  // ✅ Fixed: Returns JSON
                }

                return BadRequest(new { err = "User already exists or another issue occurred." });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in RegisterController AddUser: {ex.Message}");
                return StatusCode(500, new { err = $"Internal Server Error: {ex.Message}" });
            }
        }
        //public async Task<IActionResult> AddUser([FromBody] RegisterUIModel registerUIModel)
        //{
        //    if (registerUIModel == null)
        //    {
        //        return BadRequest("Received null user model. Ensure JSON is correctly formatted.");
        //    }

        //    var isUserAdded = await registerRepository.AddUser(registerUIModel);

        //    if (isUserAdded)
        //    {
        //        return Ok("User registered successfully.");
        //    }

        //    return StatusCode(500, "An error occurred while registering the user.");
        //}

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUser()
        {
            var user = await registerRepository.GetAllUser();
            return Ok(new
            {
                err = 0,
                msg = "Successfully fetched",
                data = user
            });
        }

        [HttpPut("update-status")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUserStatus([FromBody] RegisterDBModel model)
        {
            var result = await registerRepository.UpdateUserStatus(model.Email, model.IsActive);

            if (!result) return NotFound(new { error = "User not found or status update failed." });

            return Ok(new { message = $"User status updated to {(model.IsActive ? "Active" : "Inactive")}" });
        }


        //[HttpPost]
        //[Route("AddUser")]
        //public async Task<IActionResult> AddUser([FromBody] RegisterUIModel registerUIModel)
        //{
        //    if (registerUIModel == null)
        //    {
        //        return BadRequest("User model cannot be null.");
        //    }

        //    var isUserAdded = await registerRepository.AddUser(registerUIModel);

        //    if (isUserAdded)
        //    {
        //        return Ok("User registered successfully.");
        //    }

        //    return StatusCode(500, "An error occurred while registering the user.");
        //}
    }
}