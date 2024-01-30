using System;

namespace LMSProject
{
    class Book
    {
        public Book(string title, string author, string isbn, int copynumber, string id)
        {
            this.Title =  title;
            this.Author = author;
            this.Isbn =  isbn;
            this.CopyNumber = copynumber;
            this.id = id;
        }

        public string id { get; set; }
        public string Title { get; set; }
        public string? Author { get; set; }
        public string? Isbn { get; set; }
        public int? CopyNumber { get; set; }
        public int? borrowerNumber { get; set;}
        public List<Borrower>? borrowerList = new List<Borrower>();

        #region methods
        public void AddBorrowerToBook(Borrower borrower)
        {
            borrowerList?.Add(borrower);
        }
        #endregion
    }
}