using Application.DTO.Request.Identity;
using Application.DTO.Response;
using Application.DTO.Response.Identity;
using Application.Extension.Identity;
using Application.Interface.Identity; 
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore; 
using System.Security.Claims; 

namespace Infrastructure.Repository
{
    public class Account(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager) : IAccount
    {
        public async Task<ServiceResponse> CreateUserAsync(CreateUserRequestDTO model)
        {
            var user = await FindUserByEmail(model.Email);
            if (user != null)
                return new ServiceResponse(false, "User already exist");

            var newUser = new ApplicationUser()
            {
                UserName = model.Email,
                PasswordHash = model.Password,
                Email = model.Email, 
                Name = model.Name,
            };
            var result = CheckResult(await userManager.CreateAsync(newUser,model.Password));
            if (!result.flag)
                return result;
            else
                return await CreateUserClaims(model);

        }

        public async Task<ServiceResponse> CreateUserClaims(CreateUserRequestDTO model)
        {
            if (!string.IsNullOrEmpty(model.Policy))
                return new ServiceResponse(false, "No Policy specified");
            Claim[] userClaims = [];
            if(model.Policy.Equals(Policy.AdminPolicy ,StringComparison.OrdinalIgnoreCase) ) {
                userClaims = [
                    new Claim(ClaimTypes.Email, model.Email),
                    new Claim(ClaimTypes.Role, "Admin"),
                    new Claim("Create", "true"),
                    new Claim("Update", "true"),
                    new Claim("Read", "true"),
                    new Claim("Delete", "true"),
                    new Claim("ManageUser", "true"),

                ];
            }
            else if (model.Policy.Equals(Policy.ManagerPolicy, StringComparison.OrdinalIgnoreCase))
            {
                userClaims = [
                    new Claim(ClaimTypes.Email, model.Email),
                    new Claim(ClaimTypes.Role, "Manager"),
                    new Claim("Create", "true"),
                    new Claim("Update", "true"),
                    new Claim("Read", "true"),
                    new Claim("Delete", "false"),
                    new Claim("ManageUser", "false"),

                ];
            }
            else if (model.Policy.Equals(Policy.UserPolicy, StringComparison.OrdinalIgnoreCase))
            {
                userClaims = [
                    new Claim(ClaimTypes.Email, model.Email),
                    new Claim(ClaimTypes.Role, "User"),
                    new Claim("Create", "false"),
                    new Claim("Update", "false"),
                    new Claim("Read", "false"),
                    new Claim("Delete", "false"),
                    new Claim("ManageUser", "false"),

                ];
            }

            var result = CheckResult(await userManager.AddClaimsAsync(await FindUserByEmail(model.Email), userClaims));
            if (result.flag) return new ServiceResponse(true, "User created");
            else return result;

        }

        private static ServiceResponse CheckResult(IdentityResult result)
        {
            if (result.Succeeded) return new ServiceResponse(true, null);
            var errors= result.Errors.Select(x=>x.Description);
            return new ServiceResponse(false, String.Join(Environment.NewLine, errors));


        }

        public async Task<IEnumerable<GetUserWithClaimResponseDTO>> GetUsersWithClaimsAsync()
        {
            var userList= new List<GetUserWithClaimResponseDTO>();
            var allUsers = await userManager.Users.ToListAsync();
            if (allUsers.Count == 0) { return userList; }

            foreach(var user in allUsers)
            {
                var currentUser= await userManager.FindByIdAsync(user.Id);
                var currentUserClaims = await userManager.GetClaimsAsync(currentUser);
                if (currentUserClaims.Any())
                {
                    userList.Add(new GetUserWithClaimResponseDTO
                    {
                        UserId = user.Id,
                        Email = currentUserClaims.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value,
                        RoleName = currentUserClaims.FirstOrDefault(x => x.Type == ClaimTypes.Role).Value,
                        Name = currentUserClaims.FirstOrDefault(x => x.Type == "Name").Value,
                        ManageUser = Convert.ToBoolean(currentUserClaims.FirstOrDefault(x => x.Type == "ManageUser").Value),
                        Create = Convert.ToBoolean(currentUserClaims.FirstOrDefault(x => x.Type == "Create").Value),
                        Update = Convert.ToBoolean(currentUserClaims.FirstOrDefault(x => x.Type == "Update").Value),
                        Delete = Convert.ToBoolean(currentUserClaims.FirstOrDefault(x => x.Type == "Delete").Value),
                        Read = Convert.ToBoolean(currentUserClaims.FirstOrDefault(x => x.Type == "Read").Value),
                    }
                        );
                }
            }

            return userList;
        }

        public async Task<ServiceResponse> LoginAsync(LoginUserRequestDTO model)
        {
            var user = await FindUserByEmail(model.Email);
            if (user is null) return new ServiceResponse(false, "User not found");
            var verifyPassword = await signInManager.CheckPasswordSignInAsync(user,model.Password,false);
            if (!verifyPassword.Succeeded) return new ServiceResponse(false, "in correct password");
            var result=await signInManager.PasswordSignInAsync(user,model.Password,false,false);
            if (!result.Succeeded) return new ServiceResponse(false, "Unknown error occured while loggin in");
            else
                return new ServiceResponse(true,null);
        }

        private async Task<ApplicationUser> FindUserByEmail(string email) => await userManager.FindByEmailAsync(email);
        private async Task<ApplicationUser> FindUserById(string id) => await userManager.FindByIdAsync(id);


        public async Task SetUpAsync() => await CreateUserAsync( new CreateUserRequestDTO() { Name="Administrator",Email="admin@admin.com",Password="Admin@123",Policy=Policy.AdminPolicy} );

        public async Task<ServiceResponse> UpdateUserAsync(ChangeUserClaimRequestDTO model)
        {
            var user = await FindUserById(model.UserId);
            if (user is null) return new ServiceResponse(false, "User not found");

             var oldUserClaims= await userManager.GetClaimsAsync(user);

            Claim[] newUserClaims = [
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, model.RoleName),
                new Claim("Create", model.Create.ToString()),
                new Claim("Update", model.Update.ToString()),
                new Claim("Read", model.Read.ToString()),
                new Claim("Delete", model.Delete.ToString()),
                new Claim("ManageUser", model.ManageUser.ToString()),
            ];
            var result = await userManager.RemoveClaimsAsync(user, oldUserClaims);

            var response = CheckResult(result);
            if (!response.flag) return new ServiceResponse(false,response.message);

            var addClaims = await userManager.AddClaimsAsync(user, newUserClaims);

            var outcome = CheckResult(addClaims);
            if (outcome.flag) return new ServiceResponse(false, "User updated");
            else return outcome;


        }
    }
}
