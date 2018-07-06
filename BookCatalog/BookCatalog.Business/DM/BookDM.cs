﻿using AutoMapper;
using BookCatalog.Data.Entity.Author;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookCatalog.Portal.ViewModel.Book;
using BookCatalog.Portal.ViewModel.Author;
using BookCatalog.Data.Entity.Book;
using BookCatalog.Common.Data;
using BookCatalog.Common.Business;
using BookCatalog.Business.DM.DataTables;
using BookCatalog.ViewModel.DataTable;

namespace BookCatalog.Business.DM
{
    public class BookDM : IBookDM
    {
        private readonly IBookRepository repository;
        
        static BookDM()
        {
            Mapper.Initialize(cfg => {
                cfg.CreateMap<BookEM, BookVM>();
                cfg.CreateMap<AuthorEM, AuthorVM>();
            });
        }

        public BookDM(IBookRepository repository)
        {
            this.repository = repository;         
        }

        public BookVM GetBook(int bookId)
        {
            var bookVM = Enumerable.Repeat(Mapper.Map<BookVM>(repository.GetBook(bookId)), 1).ToList();
            
            GetBooksAuthors(bookVM);            

            return bookVM.FirstOrDefault();
        }

        public DataTableResult<BookVM> GetBooksList(DataTableVM dataTableVM)
        {
            string selectQuery = DataTableDM.BuildSelectQuery("Books", dataTableVM),
                countQuery = DataTableDM.BuildCountQuery("Books", dataTableVM);

            var bookVMs = repository
                .GetBooks(selectQuery).Select(x => Mapper.Map<BookVM>(x)).ToList();
            
            GetBooksAuthors(bookVMs);

            var table = new DataTableResult<BookVM>()
            {
                Draw = dataTableVM.Draw,
                RecordsFiltered = bookVMs.Count,
                RecordsTotal = repository.GetCount(countQuery),
                Data = bookVMs
            };

            return table;
        }

        public BookVM Save(BookVM bookVM)
        {
            var bookEM = Mapper.Map<BookEM>(bookVM);

            if (repository.BookIsExist(bookEM))
            {
                bookEM = repository.Update(bookEM);
            }
            else
            {
                bookEM = repository.Save(bookEM);
            }           

            return Mapper.Map<BookVM>(bookEM);
        }

        public void DeleteBook(int bookId)
        {
            repository.DeleteBook(bookId);
        }

        private void GetBooksAuthors(List<BookVM> books)
        {
            var taskList = new List<Task<KeyValuePair<int, IEnumerable<AuthorEM>>>>();
            var booksAuthors = new Dictionary<int, IEnumerable<AuthorEM>>();

            foreach (int id in books.Select(x => x.Id))
            {
                taskList.Add(this.repository.GetBookAuthors(id));
            }

            var taskArray = taskList.ToArray();

            Task.WaitAll(taskArray);

            foreach (Task<KeyValuePair<int, IEnumerable<AuthorEM>>> task in taskArray)
            {
                var pair = task.Result;

                var bookIndex = books.IndexOf(books.First(x => x.Id == pair.Key));

                books[bookIndex].Authors = pair.Value.Select(x => Mapper.Map<AuthorVM>(x)).ToArray();
            }
        }
    }
}
