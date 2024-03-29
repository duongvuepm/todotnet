﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TodoApp.Models;

#nullable disable

namespace TodoApp.Migrations
{
    [DbContext(typeof(TodoContext))]
    [Migration("20240321090751_AddStateTable")]
    partial class AddStateTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("TodoApp.Models.State", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    b.Property<bool>("IsDefault")
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("is_default");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(100)")
                        .HasColumnName("name");

                    b.Property<long?>("PreviousStateId")
                        .HasColumnType("bigint")
                        .HasColumnName("previous_state_id");

                    b.Property<long?>("StateId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("StateId");

                    b.ToTable("state");
                });

            modelBuilder.Entity("TodoApp.Models.TodoItem", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<DateTime>("CreatedTimestamp")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("CreatedTimestamp");

                    b.Property<bool>("IsComplete")
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("IsComplete");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(100)")
                        .HasColumnName("Name");

                    b.Property<long>("StateId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("StateId");

                    b.ToTable("TodoItems");
                });

            modelBuilder.Entity("TodoApp.Models.State", b =>
                {
                    b.HasOne("TodoApp.Models.State", null)
                        .WithMany("Transitions")
                        .HasForeignKey("StateId");
                });

            modelBuilder.Entity("TodoApp.Models.TodoItem", b =>
                {
                    b.HasOne("TodoApp.Models.State", "State")
                        .WithMany("TodoItems")
                        .HasForeignKey("StateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("State");
                });

            modelBuilder.Entity("TodoApp.Models.State", b =>
                {
                    b.Navigation("TodoItems");

                    b.Navigation("Transitions");
                });
#pragma warning restore 612, 618
        }
    }
}
