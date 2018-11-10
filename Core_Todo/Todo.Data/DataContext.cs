using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Todo.Data.Maps;
using Todo.Entities;

namespace Todo.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            //this.Configuration.LazyLoadingEnabled = false;
            //this.Configuration.ProxyCreationEnabled = false;
        }
        public DataContext()
        {

        }
        public DbSet<UserTask> UserTask { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //base.OnConfiguring(optionsBuilder);
            var connection = @"Server=.;Database=ToDo;Trusted_Connection=True;ConnectRetryCount=0";
            optionsBuilder.UseSqlServer(connection);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyAllConfigurations();
            base.OnModelCreating(modelBuilder);
            new UserTaskMap(modelBuilder.Entity<UserTask>());
        }
    }
}
