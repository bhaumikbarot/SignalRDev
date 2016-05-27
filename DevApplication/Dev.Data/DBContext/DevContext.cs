using Dev.BO.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dev.Data.DBContext
{
    public class DevContext : DbContext
    {
        static DevContext()
        {
            Database.SetInitializer<DevContext>(null);
        }

        public DevContext()
            : base("Name=DevContext")
        {
        }

        public DbSet<DevTest> DevTest { get; set; }
    }
}
