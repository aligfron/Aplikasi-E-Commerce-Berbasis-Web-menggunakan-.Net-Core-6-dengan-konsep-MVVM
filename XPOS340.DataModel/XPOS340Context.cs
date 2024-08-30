using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPOS340.DataModel
{
    public class XPOS340Context : DbContext
    {
        public XPOS340Context() { }

        public XPOS340Context(DbContextOptions<XPOS340Context> Options) : base(Options) { 
        
        
        }

        public virtual DbSet<DMTblCoba> TblCobas { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DMTblCoba>(entity =>
            {
                entity.Property(e => e.createDate).HasDefaultValueSql("(getdate())");
            });
        }
    }
}
