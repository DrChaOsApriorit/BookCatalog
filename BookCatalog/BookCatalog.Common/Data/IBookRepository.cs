﻿using BookCatalog.Data.Entity.Author;
using BookCatalog.Data.Entity.Book;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookCatalog.Common.Data
{
    public interface IBookRepository
    {
        BookEM GetBook(int bookId);

        long GetCount(string query);

        List<BookEM> GetBooks(string query);

        Task<KeyValuePair<int, IEnumerable<AuthorEM>>> GetBookAuthors(int bookId);

        bool BookIsExist(BookEM bookEM);

        BookEM Save(BookEM bookEM);

        BookEM Update(BookEM bookEM);

        void DeleteBook(int bookId);
    }
}
