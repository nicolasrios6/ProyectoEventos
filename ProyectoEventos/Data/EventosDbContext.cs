using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProyectoEventos.Models;

namespace ProyectoEventos.Data
{
    public class EventosDbContext : IdentityDbContext<Usuario>
    {
        public EventosDbContext(DbContextOptions<EventosDbContext> options) : base(options)
        {
        }
        public DbSet<Evento> Eventos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Evento>()
                .HasOne(e => e.Usuario)
                .WithMany(u => u.Eventos)
                .HasForeignKey(e => e.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
