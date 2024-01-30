using System;

namespace LMSProject
{
    class Borrower
    {
        public Borrower(string id, User user)
        {
            //NameOfBorrower = name;
            this.user = user;
            this.id = id;
        }
        public string id { get; set; }
        public string? NameOfBorrower { get; set; }
        public User? user { get; set; }
        //public List<Book>? books  = new List<Book>(); 
        public Dictionary<Book, DateTime > booksDict =  new Dictionary<Book, DateTime>();
        
        #region methods
        public void AddBookToBooksDict(Book book, DateTime dateTime)
        {
            booksDict.Add(book, dateTime);
            //books?.Add(book);
        }
        #endregion
    }
}