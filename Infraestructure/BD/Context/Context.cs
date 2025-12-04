using Domain.Entities.API;
using Domain.Entities.CSV;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.BD.Context
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> bdContext) : base(bdContext)
        { 
        
        }

        public DbSet<ESurveys> surveys_part1 { get; set; }
        public DbSet<ESocialComments> social_comments { get; set; }
    }
}
