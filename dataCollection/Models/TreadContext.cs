using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace dataCollection.Models;

public partial class TreadContext : DbContext
{
    public TreadContext()
    {
    }

    public TreadContext(DbContextOptions<TreadContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Rates30> Rates30s { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Tread;Integrated Security=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Rates30>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__rates30__3214EC070F2016E3");

            entity.ToTable("rates30");

            entity.Property(e => e.Date).HasColumnType("datetime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
