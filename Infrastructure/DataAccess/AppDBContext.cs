using Application.Extension.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore; 

namespace Infrastructure.DataAccess
{
    public class AppDBContext(DbContextOptions<AppDBContext> options):IdentityDbContext<ApplicationUser>(options)
    {
    }
}
