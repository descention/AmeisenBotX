using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmeisenBotX.Plugins.Questing.Database.Repository
{
    internal class RemoteContext : WorldContext
    {
        public RemoteContext(DbContextOptions options) : base(options)
        {
        }
    }
}
