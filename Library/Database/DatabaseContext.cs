﻿using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace Library
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Database> Databases { get; set; }
        public DbSet<Book> Books { get; set; }
        public string DatabasePath { get; set; }

        public DatabaseContext()
        {
            DatabasePath = "library.db";
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={DatabasePath}");
        }

        public List<Database> GetAllDatabases()
        {
            return Databases.ToList();
        }

        public List<Book> GetBooks(int databaseId)
        {
            return Books.Where(book => book.DatabaseId == databaseId).ToList();
        }
    }
}