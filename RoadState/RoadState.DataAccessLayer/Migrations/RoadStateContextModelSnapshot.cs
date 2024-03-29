﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RoadState.DataAccessLayer;

namespace RoadState.DataAccessLayer.Migrations
{
    [DbContext(typeof(RoadStateContext))]
    partial class RoadStateContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("RoadState.Data.BugReport", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AuthorId");

                    b.Property<string>("Description");

                    b.Property<double>("Latitude");

                    b.Property<double>("Longitude");

                    b.Property<DateTime>("PublishDate");

                    b.Property<int>("Rating");

                    b.Property<string>("State");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.ToTable("BugReports");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            AuthorId = "abcd",
                            Description = "first bug report",
                            Latitude = 50.046199999999999,
                            Longitude = 36.315159999999999,
                            PublishDate = new DateTime(2019, 7, 28, 11, 33, 33, 24, DateTimeKind.Local).AddTicks(1007),
                            Rating = 1,
                            State = "Low"
                        });
                });

            modelBuilder.Entity("RoadState.Data.BugReportRate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BugReportId");

                    b.Property<bool>("HasAgreed");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("BugReportId");

                    b.HasIndex("UserId");

                    b.ToTable("BugReportRates");
                });

            modelBuilder.Entity("RoadState.Data.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AuthorId");

                    b.Property<int>("BugReportId");

                    b.Property<DateTime>("PublishDate");

                    b.Property<string>("Text");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("BugReportId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("RoadState.Data.Photo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<byte[]>("Blob");

                    b.Property<int>("BugReportId");

                    b.HasKey("Id");

                    b.HasIndex("BugReportId");

                    b.ToTable("Photos");
                });

            modelBuilder.Entity("RoadState.Data.User", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AvatarUrl");

                    b.Property<string>("Email");

                    b.Property<double>("Latitude");

                    b.Property<double>("Longitude");

                    b.Property<byte[]>("PasswordHash");

                    b.Property<byte[]>("PasswordSalt");

                    b.Property<DateTime>("RegistrationDate");

                    b.Property<string>("UserName");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = "abcd",
                            Email = "123@gmail.com",
                            Latitude = 34.0,
                            Longitude = 55.0,
                            RegistrationDate = new DateTime(2019, 7, 28, 11, 33, 33, 27, DateTimeKind.Local).AddTicks(1234),
                            UserName = "dimasik"
                        });
                });

            modelBuilder.Entity("RoadState.Data.UserMark", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CommentId");

                    b.Property<bool>("HasLiked");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("CommentId");

                    b.HasIndex("UserId");

                    b.ToTable("UserLikes");
                });

            modelBuilder.Entity("RoadState.Data.BugReport", b =>
                {
                    b.HasOne("RoadState.Data.User", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId");
                });

            modelBuilder.Entity("RoadState.Data.BugReportRate", b =>
                {
                    b.HasOne("RoadState.Data.BugReport", "BugReport")
                        .WithMany("BugReportRates")
                        .HasForeignKey("BugReportId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("RoadState.Data.User", "User")
                        .WithMany("BugReportRates")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("RoadState.Data.Comment", b =>
                {
                    b.HasOne("RoadState.Data.User", "Author")
                        .WithMany("Comments")
                        .HasForeignKey("AuthorId");

                    b.HasOne("RoadState.Data.BugReport", "BugReport")
                        .WithMany("Comments")
                        .HasForeignKey("BugReportId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RoadState.Data.Photo", b =>
                {
                    b.HasOne("RoadState.Data.BugReport", "BugReport")
                        .WithMany("Photos")
                        .HasForeignKey("BugReportId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RoadState.Data.UserMark", b =>
                {
                    b.HasOne("RoadState.Data.Comment", "Comment")
                        .WithMany("UserLikes")
                        .HasForeignKey("CommentId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("RoadState.Data.User", "User")
                        .WithMany("UserLikes")
                        .HasForeignKey("UserId");
                });
#pragma warning restore 612, 618
        }
    }
}
