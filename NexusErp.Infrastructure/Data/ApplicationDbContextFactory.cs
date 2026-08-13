using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace NexusErp.Infrastructure.Data;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

        optionsBuilder.UseSqlServer("Data Source=DESKTOP-CJ469SU\\SQLEXPRESS;Initial Catalog=MyInventoryDB;Integrated Security=True;Trust Server Certificate=True;");

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}