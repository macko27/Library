﻿using Microsoft.EntityFrameworkCore;

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
            return [.. Databases];
        }

        public List<Book> GetBooks(int databaseId)
        {
            return [.. Books.Where(book => book.DatabaseId == databaseId)];
        }

        public List<Book> GetAllBooks()
        { 
            return [.. Books.OrderBy(book => book.Title)];
        }

        public List<Book> GetBooksNameContains(string text)
        {
            return [.. Books.Where(book => book.Title.Contains(text) || book.Author.Contains(text))];
        }

        public List<Book> GetBooksNameContainsWithId(string text, int databaseId)
        {
            return [.. Books.Where(book => book.DatabaseId == databaseId && (book.Title.Contains(text) || book.Author.Contains(text)))];
        }
    }
}
