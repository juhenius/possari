﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Possari.Infrastructure.Common.Persistence;

#nullable disable

namespace Possari.Infrastructure.Migrations
{
    [DbContext(typeof(PossariDbContext))]
    partial class PossariDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.0");

            modelBuilder.Entity("Possari.Domain.Children.Child", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("TokenBalance")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Version")
                        .IsConcurrencyToken()
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Children");
                });

            modelBuilder.Entity("Possari.Domain.Parents.Parent", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Version")
                        .IsConcurrencyToken()
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Parents");
                });

            modelBuilder.Entity("Possari.Domain.Rewards.Reward", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("TokenCost")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Version")
                        .IsConcurrencyToken()
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Rewards");
                });

            modelBuilder.Entity("Possari.Infrastructure.Outbox.OutboxMessage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Error")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("OccurredOnUtc")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("ProcessedOnUtc")
                        .HasColumnType("TEXT");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("OutboxMessages");
                });

            modelBuilder.Entity("Possari.Domain.Children.Child", b =>
                {
                    b.OwnsMany("Possari.Domain.Children.PendingReward", "PendingRewards", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .HasColumnType("TEXT");

                            b1.Property<Guid>("ChildId")
                                .HasColumnType("TEXT");

                            b1.Property<string>("RewardName")
                                .IsRequired()
                                .HasColumnType("TEXT");

                            b1.Property<int>("Version")
                                .IsConcurrencyToken()
                                .HasColumnType("INTEGER");

                            b1.HasKey("Id");

                            b1.HasIndex("ChildId");

                            b1.ToTable("PendingRewards", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("ChildId");
                        });

                    b.Navigation("PendingRewards");
                });
#pragma warning restore 612, 618
        }
    }
}
