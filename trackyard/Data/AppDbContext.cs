using Microsoft.EntityFrameworkCore;
using Sprint1CSharp.Models;

namespace Sprint1CSharp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Cliente> Clientes => Set<Cliente>();
        public DbSet<Veiculo> Veiculos => Set<Veiculo>();
        public DbSet<Patio>   Patios   => Set<Patio>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // =======================
            // CLIENTES
            // =======================
            modelBuilder.Entity<Cliente>(e =>
            {
                e.ToTable("CLIENTES");

                e.HasKey(x => x.Id)
                 .HasName("PK_CLIENTES");

                e.Property(x => x.Id)
                 .HasColumnName("ID")
                 .HasColumnType("NUMBER(19)")
                 .UseIdentityColumn();

                e.Property(x => x.Nome)
                 .HasColumnName("NOME")
                 .HasColumnType("VARCHAR2(120)")
                 .IsRequired();

                e.Property(x => x.CPF)
                 .HasColumnName("CPF")
                 .HasColumnType("VARCHAR2(14)")
                 .IsRequired();

                e.Property(x => x.Email)
                 .HasColumnName("EMAIL")
                 .HasColumnType("VARCHAR2(120)");

                e.Property(x => x.Endereco)
                 .HasColumnName("ENDERECO")
                 .HasColumnType("VARCHAR2(200)");

                e.HasIndex(x => x.CPF)
                 .IsUnique()
                 .HasDatabaseName("UQ_CLIENTES_CPF");

                e.HasMany(x => x.Veiculos)
                 .WithOne(v => v.Cliente!)
                 .HasForeignKey(v => v.ClienteId)
                 .OnDelete(DeleteBehavior.Cascade)
                 .HasConstraintName("FK_VEICULOS_CLIENTE");
            });

            // =======================
            // VEICULOS
            // =======================
            modelBuilder.Entity<Veiculo>(e =>
            {
                e.ToTable("VEICULOS");

                e.HasKey(x => x.Id)
                 .HasName("PK_VEICULOS");

                e.Property(x => x.Id)
                 .HasColumnName("ID")
                 .HasColumnType("NUMBER(19)")
                 .UseIdentityColumn(); 

                e.Property(x => x.Modelo)
                 .HasColumnName("MODELO")
                 .HasColumnType("VARCHAR2(80)")
                 .IsRequired();

                e.Property(x => x.Placa)
                 .HasColumnName("PLACA")
                 .HasColumnType("VARCHAR2(10)")
                 .IsRequired();

                e.Property(x => x.Cor)
                 .HasColumnName("COR")
                 .HasColumnType("VARCHAR2(40)");

                e.Property(x => x.Ano)
                 .HasColumnName("ANO")
                 .HasColumnType("VARCHAR2(4)");

                e.Property(x => x.ClienteId)
                 .HasColumnName("CLIENTE_ID")
                 .HasColumnType("NUMBER(19)");

                e.HasIndex(x => x.Placa)
                 .IsUnique()
                 .HasDatabaseName("UQ_VEICULOS_PLACA");

                e.HasIndex(x => x.ClienteId)
                 .HasDatabaseName("IX_VEICULOS_CLIENTE_ID");
            });

            // =======================
            // PATIOS
            // =======================
            modelBuilder.Entity<Patio>(e =>
            {
                e.ToTable("PATIOS");

                e.HasKey(x => x.Id)
                 .HasName("PK_PATIOS");

                e.Property(x => x.Id)
                 .HasColumnName("ID")
                 .HasColumnType("NUMBER(19)")
                 .UseIdentityColumn(); // Oracle 12c+

                e.Property(x => x.Nome)
                 .HasColumnName("NOME")
                 .HasColumnType("VARCHAR2(120)")
                 .IsRequired();

                e.Property(x => x.Endereco)
                 .HasColumnName("ENDERECO")
                 .HasColumnType("VARCHAR2(200)");
            });
        }
    }
}
