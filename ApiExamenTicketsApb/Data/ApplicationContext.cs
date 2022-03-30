using ApiExamenTicketsApb.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiExamenTicketsApb.Data {
    public class ApplicationContext: DbContext {
        public ApplicationContext(DbContextOptions<ApplicationContext> options): base(options) { }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
    }
}
