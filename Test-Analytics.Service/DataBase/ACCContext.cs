using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Test_Analytics.Model;

namespace Test_Analytics.Service.Database {
    public class ACCContext : DbContext {
        public ACCContext( DbContextOptions<ACCContext> options ) : base( options ) {

        }
        public DbSet<ComponentModel> ComponentModels { get; set; }
        public DbSet<AttributeModel> AttributeModels { get; set; }
        public DbSet<CapabilityModel> CapabilityModels { get; set; }
        public DbSet<ProjectModel> ProjectModels { get; set; }
    }
}
