using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMD.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CMD.Data.Context
{
    /// <summary>
    /// Represents the database context for the Doctor domain.
    /// Provides access to the doctor-related entities and manages the mapping between entities and database tables.
    /// </summary>
    public class DoctorDbContext : DbContext
    {
        public DoctorDbContext()
        {
            
        }
        public DoctorDbContext(DbContextOptions<DoctorDbContext> options): base(options)
        {
            
        }

        // map entities to table
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<DoctorAddress> DoctorAddresses { get; set; }
        public DbSet<DoctorSchedule> DoctorSchedules { get; set; }
    }
}
