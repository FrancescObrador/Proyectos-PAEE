﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TallerEF.Modelo;

namespace TallerEF
{
    public class TallerEFContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=TallerEF");
            //optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=tallerEF;Username=usuario;Password=pwd1");

            optionsBuilder.UseLazyLoadingProxies();
        }

        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<CuentaCliente> CuentaCliente { get; set; }

    }
}
