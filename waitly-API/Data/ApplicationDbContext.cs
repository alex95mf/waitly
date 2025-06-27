using waitly_API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace waitly_API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<UsuarioEmpresa> UsuarioEmpresas { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<UsuarioRol> UsuarioRoles { get; set; }
        public DbSet<Permiso> Permisos { get; set; }
        public DbSet<RolPermiso> RolPermisos { get; set; }
        public DbSet<Pantalla> Pantallas { get; set; }
        public DbSet<PermisoPantalla> PermisosPantallas { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Asiento> Asientos { get; set; }
        public DbSet<GrupoAsiento> GruposAsientos { get; set; }
        public DbSet<Catalogo> Catalogos { get; set; }
        public DbSet<Carta> Cartas { get; set; }
        public DbSet<ItemCarta> ItemsCarta { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure UsuarioEmpresa relationship
            modelBuilder.Entity<UsuarioEmpresa>()
                .HasKey(ue => ue.Id);

            modelBuilder.Entity<UsuarioEmpresa>()
                .HasOne(ue => ue.Usuario)
                .WithMany(u => u.UsuarioEmpresas)
                .HasForeignKey(ue => ue.IdUsuario);

            modelBuilder.Entity<UsuarioEmpresa>()
                .HasOne(ue => ue.Empresa)
                .WithMany(e => e.UsuarioEmpresas)
                .HasForeignKey(ue => ue.IdEmpresa);

            // Configure UsuarioRol relationship
            modelBuilder.Entity<UsuarioRol>()
                .HasKey(ur => ur.Id);

            modelBuilder.Entity<UsuarioRol>()
                .HasOne(ur => ur.Usuario)
                .WithMany(u => u.UsuarioRoles)
                .HasForeignKey(ur => ur.IdUsuario);

            modelBuilder.Entity<UsuarioRol>()
                .HasOne(ur => ur.Rol)
                .WithMany(r => r.UsuarioRoles)
                .HasForeignKey(ur => ur.IdRol);

            // Configure Rol - Empresa relationship
            modelBuilder.Entity<Rol>()
                .HasOne(r => r.Empresa)
                .WithMany(e => e.Roles)
                .HasForeignKey(r => r.IdEmpresa);

            // Configure RolPermiso relationship
            modelBuilder.Entity<RolPermiso>()
                .HasKey(rp => rp.Id);

            modelBuilder.Entity<RolPermiso>()
                .HasOne(rp => rp.Rol)
                .WithMany(r => r.RolPermisos)
                .HasForeignKey(rp => rp.IdRol);

            modelBuilder.Entity<RolPermiso>()
                .HasOne(rp => rp.Permiso)
                .WithMany(p => p.RolPermisos)
                .HasForeignKey(rp => rp.IdPermiso);

            // Configure PermisosPantalla relationship
            modelBuilder.Entity<PermisoPantalla>()
                .HasKey(pp => pp.Id);

            modelBuilder.Entity<PermisoPantalla>()
                .HasOne(pp => pp.Permiso)
                .WithMany(p => p.PermisosPantallas)
                .HasForeignKey(pp => pp.IdPermiso);

            modelBuilder.Entity<PermisoPantalla>()
                .HasOne(pp => pp.Pantalla)
                .WithMany(p => p.PermisosPantallas)
                .HasForeignKey(pp => pp.IdPantalla);

            // Configure Permiso - Catalogo relationship
            modelBuilder.Entity<Permiso>()
                .HasOne(p => p.Tipo)
                .WithMany()
                .HasForeignKey(p => p.IdTipo);

            // Configure Menu - Empresa relationship
            modelBuilder.Entity<Menu>()
                .HasOne(m => m.Empresa)
                .WithMany(e => e.Menus)
                .HasForeignKey(m => m.IdEmpresa);

            // Configure Menu - Permiso relationship
            modelBuilder.Entity<Menu>()
                .HasOne(m => m.Permiso)
                .WithMany(p => p.Menus)
                .HasForeignKey(m => m.IdPermiso);

            // Configure Menu parent-child relationship
            modelBuilder.Entity<Menu>()
                .HasOne(m => m.MenuPadre)
                .WithMany(m => m.SubMenus)
                .HasForeignKey(m => m.IdPadre)
                .IsRequired(false);

            // Configure Menu - Catalogo relationship
            modelBuilder.Entity<Menu>()
                .HasOne(m => m.Tipo)
                .WithMany()
                .HasForeignKey(m => m.IdTipo);

            // Configure Pantalla - Menu relationship
            modelBuilder.Entity<Pantalla>()
                .HasOne(p => p.Menu)
                .WithMany(m => m.Pantallas)
                .HasForeignKey(p => p.IdMenu);

            // Configure Asiento - GrupoAsiento relationship
            modelBuilder.Entity<Asiento>()
                .HasOne(a => a.GrupoAsiento)
                .WithMany(g => g.Asientos)
                .HasForeignKey(a => a.IdGrupoAsiento);

            // Configure Asiento - Catalogo relationship for Tipo
            modelBuilder.Entity<Asiento>()
                .HasOne(a => a.Tipo)
                .WithMany()
                .HasForeignKey(a => a.IdTipo);

            // Configure Asiento - Catalogo relationship for Estado
            modelBuilder.Entity<Asiento>()
                .HasOne(a => a.Estado)
                .WithMany()
                .HasForeignKey(a => a.IdEstado);

            // Configure Catalogo parent-child relationship
            modelBuilder.Entity<Catalogo>()
                .HasOne(c => c.Padre)
                .WithMany(c => c.Hijos)
                .HasForeignKey(c => c.IdPadre)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure Carta - Empresa relationship
            modelBuilder.Entity<Carta>()
             .HasOne(c => c.Empresa)
             .WithMany(e => e.Cartas)
             .HasForeignKey(c => c.IdEmpresa);

            // Configure Carta - ItemCarta relationship
            modelBuilder.Entity<ItemCarta>()
              .HasOne(ic => ic.Carta)
              .WithMany(c => c.ItemsCarta)
              .HasForeignKey(ic => ic.IdCarta);

            // Configure ItemCarta - Categoria relationship
            modelBuilder.Entity<ItemCarta>()
                .HasOne(ic => ic.Categoria)
                .WithMany()
                .HasForeignKey(ic => ic.IdCategoria);
        }
    }
}