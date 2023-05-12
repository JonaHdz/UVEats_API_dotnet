using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace API_PROYECTO.Models;

public partial class UveatsContext : DbContext
{
    public UveatsContext()
    {
    }

    public UveatsContext(DbContextOptions<UveatsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Categoria> Categorias { get; set; }

    public virtual DbSet<Conversacionespedido> Conversacionespedidos { get; set; }

    public virtual DbSet<Pedido> Pedidos { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<Productospedido> Productospedidos { get; set; }

    public virtual DbSet<Resena> Resenas { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=127.0.0.1;database=uveats;user=root;pwd=minecraftPE1976", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.33-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb3_general_ci")
            .HasCharSet("utf8mb3");

        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.IdCategoria).HasName("PRIMARY");

            entity.ToTable("categorias");

            entity.Property(e => e.IdCategoria).HasColumnName("idCategoria");
            entity.Property(e => e.Categoria1)
                .HasMaxLength(45)
                .HasColumnName("categoria");
        });

        modelBuilder.Entity<Conversacionespedido>(entity =>
        {
            entity.HasKey(e => e.IdConversacionesPedido).HasName("PRIMARY");

            entity.ToTable("conversacionespedido");

            entity.HasIndex(e => e.IdPedido, "idPedido_idx");

            entity.Property(e => e.IdConversacionesPedido)
                .ValueGeneratedNever()
                .HasColumnName("idConversacionesPedido");
            entity.Property(e => e.Conversacion).HasColumnName("conversacion");
            entity.Property(e => e.IdPedido).HasColumnName("idPedido");

            entity.HasOne(d => d.IdPedidoNavigation).WithMany(p => p.Conversacionespedidos)
                .HasForeignKey(d => d.IdPedido)
                .HasConstraintName("idPedido");
        });

        modelBuilder.Entity<Pedido>(entity =>
        {
            entity.HasKey(e => e.IdPedido).HasName("PRIMARY");

            entity.ToTable("pedidos");

            entity.HasIndex(e => e.IdUsuario, "idUsuario_idx");

            entity.Property(e => e.IdPedido).HasColumnName("idPedido");
            entity.Property(e => e.EstadoPedido).HasMaxLength(45);
            entity.Property(e => e.FechaPedido).HasColumnName("fechaPedido");
            entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");
            entity.Property(e => e.Total)
                .HasPrecision(6, 2)
                .HasColumnName("total");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Pedidos)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("idUsuarioPedido");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.IdProducto).HasName("PRIMARY");

            entity.ToTable("productos");

            entity.HasIndex(e => e.IdCategoria, "idCategoria_idx");

            entity.Property(e => e.IdProducto).HasColumnName("idProducto");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(500)
                .HasColumnName("descripcion");
            entity.Property(e => e.EstadoProducto)
                .HasMaxLength(45)
                .HasColumnName("estadoProducto");
            entity.Property(e => e.FotoProducto).HasColumnName("fotoProducto");
            entity.Property(e => e.IdCategoria).HasColumnName("idCategoria");
            entity.Property(e => e.Nombre)
                .HasMaxLength(45)
                .HasColumnName("nombre");
            entity.Property(e => e.Precio)
                .HasPrecision(6, 2)
                .HasColumnName("precio");

            entity.HasOne(d => d.IdCategoriaNavigation).WithMany(p => p.Productos)
                .HasForeignKey(d => d.IdCategoria)
                .HasConstraintName("idCategoria");
        });

        modelBuilder.Entity<Productospedido>(entity =>
        {
            entity.HasKey(e => e.IdProductoPedido).HasName("PRIMARY");

            entity.ToTable("productospedido");

            entity.HasIndex(e => e.IdPedido, "idPedido_idx");

            entity.HasIndex(e => e.IdProducto, "idProducto_idx");

            entity.Property(e => e.IdProductoPedido).HasColumnName("idProductoPedido");
            entity.Property(e => e.EstadoProducto)
                .HasMaxLength(45)
                .HasColumnName("estadoProducto");
            entity.Property(e => e.IdPedido).HasColumnName("idPedido");
            entity.Property(e => e.IdProducto).HasColumnName("idProducto");
            entity.Property(e => e.Subtotal).HasPrecision(6, 2);

            entity.HasOne(d => d.IdPedidoNavigation).WithMany(p => p.Productospedidos)
                .HasForeignKey(d => d.IdPedido)
                .HasConstraintName("idPedidoProducto");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.Productospedidos)
                .HasForeignKey(d => d.IdProducto)
                .HasConstraintName("idProductoPedido");
        });

        modelBuilder.Entity<Resena>(entity =>
        {
            entity.HasKey(e => e.IdResena).HasName("PRIMARY");

            entity.ToTable("resenas");

            entity.HasIndex(e => e.IdProducto, "idProducto_idx");

            entity.HasIndex(e => e.IdUsuario, "idUsuario_idx");

            entity.Property(e => e.IdResena).HasColumnName("idResena");
            entity.Property(e => e.IdProducto).HasColumnName("idProducto");
            entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");
            entity.Property(e => e.Resena1)
                .HasMaxLength(500)
                .HasColumnName("resena");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.Resenas)
                .HasForeignKey(d => d.IdProducto)
                .HasConstraintName("idProducto");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Resenas)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("idUsuario");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PRIMARY");

            entity.ToTable("usuarios");

            entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");
            entity.Property(e => e.Apellido)
                .HasMaxLength(45)
                .HasColumnName("apellido");
            entity.Property(e => e.Contrasena)
                .HasMaxLength(45)
                .HasColumnName("contrasena");
            entity.Property(e => e.Correo)
                .HasMaxLength(45)
                .HasColumnName("correo");
            entity.Property(e => e.Firstname)
                .HasMaxLength(45)
                .HasColumnName("firstname");
            entity.Property(e => e.Foto).HasColumnName("foto");
            entity.Property(e => e.Telefono)
                .HasMaxLength(10)
                .HasColumnName("telefono");
            entity.Property(e => e.Tipo)
                .HasMaxLength(45)
                .HasColumnName("tipo");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
