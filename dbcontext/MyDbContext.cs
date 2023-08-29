using Microsoft.EntityFrameworkCore;

namespace mywebapp.dbcontext
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options): base(options)
        {
            
        }
    }
}
