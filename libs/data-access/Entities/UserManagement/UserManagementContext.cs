using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Api.Services.DataAccess.Entities.UserManagement;

public partial class UserManagementContext : DbContext
{
    //protected readonly IConfiguration _configuration = null;
    public UserManagementContext()
    {
    }
    //public UserManagementContext(IConfiguration configuration) => _configuration = configuration;

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //    var env = Environment.GetEnvironmentVariable("CONTEXT_ENVIRONMENT");

    //    if (!string.IsNullOrEmpty(env) && env.Contains("Development", StringComparison.OrdinalIgnoreCase))
    //    {
    //        // Development-specific configuration can be set here
    //        var connectionString = _configuration.GetConnectionString("Data Source=DESKTOP-DU3PCRV;Initial Catalog=UserManagement;User ID=sa;Password=asd123;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
    //        optionsBuilder.UseSqlServer(connectionString);
    //    }
    //    else if (!string.IsNullOrEmpty(env) && env.Equals("Production", StringComparison.OrdinalIgnoreCase))
    //    {
    //        // Production-specific configuration can be set here
    //        optionsBuilder.UseSqlServer(_configuration["UserManagementDB"]);
    //    }
    //}
    public UserManagementContext(DbContextOptions<UserManagementContext> options)
        : base(options)
    {
    }
    public virtual DbSet<TblApplication> TblApplications { get; set; }

    public virtual DbSet<TblClient> TblClients { get; set; }

    public virtual DbSet<TblEmailSetting> TblEmailSettings { get; set; }

    public virtual DbSet<TblMenu> TblMenus { get; set; }

    public virtual DbSet<TblMenuPermission> TblMenuPermissions { get; set; }

    public virtual DbSet<TblRefreshToken> TblRefreshTokens { get; set; }

    public virtual DbSet<TblRole> TblRoles { get; set; }

    public virtual DbSet<TblUser> TblUsers { get; set; }

    public virtual DbSet<TblUserDocument> TblUserDocuments { get; set; }

    public virtual DbSet<TblUserRole> TblUserRoles { get; set; }
   
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblApplication>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Application");

            entity.ToTable("tbl_Application");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Address1).HasMaxLength(50);
            entity.Property(e => e.Address2).HasMaxLength(50);
            entity.Property(e => e.Address3).HasMaxLength(50);
            entity.Property(e => e.Address4).HasMaxLength(50);
            entity.Property(e => e.City).HasMaxLength(50);
            entity.Property(e => e.Country).HasMaxLength(50);
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Email1).HasMaxLength(50);
            entity.Property(e => e.Email2).HasMaxLength(50);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.MiddleName).HasMaxLength(50);
            entity.Property(e => e.Mobile1).HasMaxLength(50);
            entity.Property(e => e.Mobile2).HasMaxLength(50);
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Phone1).HasMaxLength(50);
            entity.Property(e => e.Phone2).HasMaxLength(50);
            entity.Property(e => e.PinCode).HasMaxLength(50);
            entity.Property(e => e.State).HasMaxLength(50);

            entity.HasOne(d => d.Client).WithMany(p => p.TblApplications)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Application_Client_Id");
        });

        modelBuilder.Entity<TblClient>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Client");

            entity.ToTable("tbl_Client");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Address1).HasMaxLength(50);
            entity.Property(e => e.Address2).HasMaxLength(50);
            entity.Property(e => e.Address3).HasMaxLength(50);
            entity.Property(e => e.Address4).HasMaxLength(50);
            entity.Property(e => e.City).HasMaxLength(50);
            entity.Property(e => e.Country).HasMaxLength(50);
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Email1).HasMaxLength(50);
            entity.Property(e => e.Email2).HasMaxLength(50);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.MiddleName).HasMaxLength(50);
            entity.Property(e => e.Mobile1).HasMaxLength(50);
            entity.Property(e => e.Mobile2).HasMaxLength(50);
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Phone1).HasMaxLength(50);
            entity.Property(e => e.Phone2).HasMaxLength(50);
            entity.Property(e => e.PinCode).HasMaxLength(50);
            entity.Property(e => e.State).HasMaxLength(50);
        });

        modelBuilder.Entity<TblEmailSetting>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_EmailSettings");

            entity.ToTable("tbl_EmailSettings");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.EmailId).HasMaxLength(50);
            entity.Property(e => e.EnableSsl).HasColumnName("EnableSSL");
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.SmtpAddress).HasMaxLength(50);

            entity.HasOne(d => d.Application).WithMany(p => p.TblEmailSettings)
                .HasForeignKey(d => d.ApplicationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EmailSettings_Application_Id");
        });

        modelBuilder.Entity<TblMenu>(entity =>
        {
            entity.ToTable("tbl_Menu");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.LinkName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Application).WithMany(p => p.TblMenus)
                .HasForeignKey(d => d.ApplicationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_Menu_tbl_Application_Id");
        });

        modelBuilder.Entity<TblMenuPermission>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("tbl_MenuPermission");

            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.MenuId)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TblRefreshToken>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("tbl_RefreshToken");

            entity.Property(e => e.TokenId)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Application).WithMany()
                .HasForeignKey(d => d.ApplicationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_RefreshToken_tbl_Application_Id");
        });

        modelBuilder.Entity<TblRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Roles");

            entity.ToTable("tbl_Roles");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.NormalizedName).HasMaxLength(256);

            entity.HasOne(d => d.Application).WithMany(p => p.TblRoles)
                .HasForeignKey(d => d.ApplicationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Roles_Application_Id");
        });

        modelBuilder.Entity<TblUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_User");

            entity.ToTable("tbl_User");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.DateOfBirth).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.Gender).HasMaxLength(50);
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
            entity.Property(e => e.UserName).HasMaxLength(256);

            entity.HasOne(d => d.Application).WithMany(p => p.TblUsers)
                .HasForeignKey(d => d.ApplicationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_Application_Id");
        });

        modelBuilder.Entity<TblUserDocument>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_UserDocuments");

            entity.ToTable("tbl_UserDocuments");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.FileExtension).HasMaxLength(255);
            entity.Property(e => e.Filename).HasMaxLength(255);
            entity.Property(e => e.ServerFileName).HasMaxLength(255);
            entity.Property(e => e.ServerPath).HasMaxLength(255);

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.TblUserDocument)
                .HasForeignKey<TblUserDocument>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserDocuments_Application_Id");

            entity.HasOne(d => d.Id1).WithOne(p => p.TblUserDocument)
                .HasForeignKey<TblUserDocument>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserDocuments_User_Id");
        });

        modelBuilder.Entity<TblUserRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_UserRoles");

            entity.ToTable("tbl_UserRoles");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Application).WithMany(p => p.TblUserRoles)
                .HasForeignKey(d => d.ApplicationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserRoles_Application_Id");

            entity.HasOne(d => d.Role).WithMany(p => p.TblUserRoles)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserRoles_Roles_Id");

            entity.HasOne(d => d.User).WithMany(p => p.TblUserRoles)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserRoles_User_Id");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
