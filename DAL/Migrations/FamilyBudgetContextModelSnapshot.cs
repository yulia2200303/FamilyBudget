using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using DAL.DbContext;

namespace DAL.Migrations
{
    [DbContext(typeof(FamilyBudgetContext))]
    partial class FamilyBudgetContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0-rc1-16348");

            modelBuilder.Entity("DAL.Model.Asset", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 128);

                    b.Property<int>("Type");

                    b.Property<int>("UserId");

                    b.HasKey("Id");
                });

            modelBuilder.Entity("DAL.Model.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 128);

                    b.Property<int?>("ParentId");

                    b.HasKey("Id");
                });

            modelBuilder.Entity("DAL.Model.Currency", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 16);

                    b.Property<double>("Converter");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 128);

                    b.HasKey("Id");
                });

            modelBuilder.Entity("DAL.Model.Transaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AssetId");

                    b.Property<string>("Comment")
                        .HasAnnotation("MaxLength", 1024);

                    b.Property<double>("Cost");

                    b.Property<int>("CurrencyId");

                    b.Property<DateTime>("Date");

                    b.Property<int>("ProductId");

                    b.Property<int>("Type");

                    b.HasKey("Id");
                });

            modelBuilder.Entity("DAL.Model.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Hash")
                        .HasAnnotation("MaxLength", 1024);

                    b.Property<bool>("IsPasswordSet");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 64);

                    b.Property<string>("Salt")
                        .HasAnnotation("MaxLength", 1024);

                    b.HasKey("Id");
                });

            modelBuilder.Entity("DAL.Model.Asset", b =>
                {
                    b.HasOne("DAL.Model.User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("DAL.Model.Category", b =>
                {
                    b.HasOne("DAL.Model.Category")
                        .WithMany()
                        .HasForeignKey("ParentId");
                });

            modelBuilder.Entity("DAL.Model.Transaction", b =>
                {
                    b.HasOne("DAL.Model.Asset")
                        .WithMany()
                        .HasForeignKey("AssetId");

                    b.HasOne("DAL.Model.Currency")
                        .WithMany()
                        .HasForeignKey("CurrencyId");

                    b.HasOne("DAL.Model.Category")
                        .WithMany()
                        .HasForeignKey("ProductId");
                });
        }
    }
}
