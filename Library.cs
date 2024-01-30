using System;
using System.Collections.Generic;
using System.IO;

namespace LMSProject
{
    class Library
    {
        private User user;
        List<Book> books = new List<Book>();
        List<string> bookNames = new List<string>();
        List<Borrower> borrowerList = new List<Borrower>();
        private int lastBookID;
        private int lastBorrowerID;
        private int lastBorrowID;

        #region UI Methods
        public void LibraryMainMenu()
        {
            while (true)
            {
                Console.WriteLine("********** LIBRARY MANAGEMENT SYSTEM **********");
                Console.WriteLine("1 - Display All Books\n2 - Add A New Book\n3 - Display Borrowed Books\n4 - Search A Book\n5 - Borrow A Book\n6 - Return A Book\n7 - Exit The Program");
                var key = Console.ReadLine();

                switch (key)
                {
                    case "1":
                        DisplayBooksMenu();
                        break;
                    case "2":
                        DisplayAddBookMenu();
                        break;
                    case "3":
                        DisplayBorrowedBooks();
                        break;
                    case "4":
                        Console.WriteLine("Please give a book name you search.");
                        string? bookName = Console.ReadLine();
                        int bookIndex = SearchManager.SearchBook(bookName, books: books);
                        //Console.WriteLine("index :" + bookIndex);
                        if (bookIndex == -1)
                        {
                            Console.WriteLine("Can not found the book you searched");
                        }
                        else
                        {
                            var obj = books[bookIndex];
                            Console.WriteLine("********");
                            Console.WriteLine("ID: " + obj.id + "\n" + "The book you searched: " + obj.Title + "\n" + "Author: " + obj.Author + "\n" + "ISBN: " + obj.Isbn + "\n" + "Copy Number: " + obj.CopyNumber);
                            Console.WriteLine("********");
                        }
                        break;
                    case "5":
                        BorrowBookMenu();
                        break;
                    case "6":
                        ReturnBookMenu();
                        break;
                    case "7":
                        System.Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Pressed a invalid key. Please press the key exist on the screen.");
                        break;
                }
            }
        }

        public void DisplayBooksMenu()
        {
            while (true)
            {
                DisplayBooks();
                Console.WriteLine("1 - Back To Main Menu\n");
                var key = Console.ReadLine();
                if (key == "1")
                    break;
                else
                {
                    Console.WriteLine("Pressed a invalid key. Please press the key exist on the screen.");
                }
            }
        }

        public void DisplayAddBookMenu()
        {
            while (true)
            {
                Console.WriteLine("********* Add A Book *********");
                Console.WriteLine("1 - Press 1 to Add A Book\n2 - Back To Main Menu");
                var key = Console.ReadLine();

                if (key == "1")
                {
                    Console.Write("Name: ");
                    string? name = Console.ReadLine();

                    Console.Write("Author: ");
                    string? author = Console.ReadLine();

                    Console.Write("Isbn: ");
                    string? isbn = Console.ReadLine();

                    if (CheckTheBookExistInLibrary(name))
                    {
                        AddExistingBook(name);
                        Console.WriteLine("existing book triggered");
                    }
                    else
                    {
                        CreateNewBook(name, author, isbn, lastBookID.ToString());
                        Console.WriteLine("new book triggered");
                    }
                }
                else if (key == "2")
                    break;
                else
                {
                    Console.WriteLine("Pressed a invalid key. Please press the key exist on the screen.");
                }
            }
        }

        private void BorrowBookMenu()
        {
            DisplayBooks();
            Console.Write("Choose a book from list and enter the book ID or Name: ");
            string? idOrName = Console.ReadLine();

            BorrowBook(idOrName);
        }

        private void ReturnBookMenu()
        {
            DisplayBorrowedBooks();
            Console.WriteLine("Choose book id that want to return: ");
            string? bookID = Console.ReadLine();
            ReturnBorrowedBook(bookID);
        }
        #endregion

        #region Mechanic methods
        public void SetUser(User user)
        {
            this.user = user;
        }
        public void PopulateBooksList()
        {
            string filePath = "books.txt";

            int iterationIndex = 0;
            foreach (var line in File.ReadLines(filePath))
            {
                iterationIndex++;
                string[] parts = line.Split(',');

                string id = parts[0].Trim();
                string title = parts[1].Trim();
                PopulateBookNamesList(title);
                //Console.WriteLine("title :" + title);
                string author = parts[2].Trim();
                //Console.WriteLine("author :" + author);
                string isbn = parts[3].Trim();
                int copyNumber = int.Parse(parts[4].Trim());

                Book book = new Book(title, author, isbn, copyNumber, id);
                books.Add(book);
            }
            lastBookID = iterationIndex + 1;
        }

        private void PopulateBookNamesList(string name)
        {
            string nameWihoutSpace = RemoveSpaces(name);
            bookNames.Add(nameWihoutSpace);
        }

        public void PopulateBorrowerList()
        {
            //lastBorrowerID++;
            string filePath = "borrower.txt";
            string nameBuffer = "";
            Borrower lastBorrower = new Borrower("0", user);
            int iterationIndex = 0;
            foreach (var line in File.ReadLines(filePath))
            {
                iterationIndex++;
                string[] parts = line.Split(',');

                string borrowId = parts[0].Trim();
                string borrowerId = parts[1].Trim();
                string name = parts[2].Trim();
                //Console.WriteLine("title :" + title);
                string bookName = parts[3].Trim();
                //Console.WriteLine("author :" + author);
                DateTime lastDeliveryDate = DateTime.Parse(parts[4].Trim());

                if (nameBuffer == name)
                {
                    int bookIndex = SearchManager.SearchBook(bookName, books: books);
                    Book book = books[bookIndex];
                    //lastBorrower.id = id;
                    //lastBorrower.NameOfBorrower = name;
                    lastBorrower.AddBookToBooksDict(book, lastDeliveryDate);
                }
                else
                {
                    nameBuffer = name;
                    int bookIndex = SearchManager.SearchBook(bookName, books: books);
                    Book book = books[bookIndex];

                    User localUser = new User();
                    localUser.name = nameBuffer;

                    lastBorrower = new Borrower(borrowerId, localUser);

                    lastBorrower.id = borrowerId;
                    lastBorrower.NameOfBorrower = name;
                    lastBorrower.AddBookToBooksDict(book, lastDeliveryDate);
                    borrowerList.Add(lastBorrower);
                    lastBorrowerID += 1;
                }
            }
            lastBorrowID = iterationIndex;

        }

        private void DisplayBooks()
        {
            int lengthOfBooksList = books.Count;
            Console.WriteLine("************************");
            for (int i = 0; i < lengthOfBooksList; i++)
            {
                Console.WriteLine("*********");
                var obj = books[i];
                Console.WriteLine("ID: " + obj.id + "\n" + "Name: " + obj.Title + "\n" + "Author: " + obj.Author + "\n" + "Copy Numbers: " + obj.CopyNumber + "\n" + "Isbn: " + obj.Isbn + "\n");
                Console.WriteLine("*********");
            }
            Console.WriteLine("************************");
        }

        private bool CheckTheBookExistInLibrary(string name)
        {
            string nameWihoutSpace = RemoveSpaces(name);
            //Console.WriteLine("CheckTheBookExistInLibrary : " + nameWihoutSpace);
            if (bookNames.Contains(nameWihoutSpace))
            {
                //Console.WriteLine("contains");
                return true;
            }
            else
            {
                //Console.WriteLine("not contains");
                return false;
            }
        }

        private void AddExistingBook(string name)
        {
            int bookIndex = SearchManager.SearchBook(name, books: books);

            if (bookIndex == -1)
            {
                Console.WriteLine("Can not found the book you searched");
            }
            else
            {
                books[bookIndex].CopyNumber++;
                FileManager.UpdateBookTxtFile("books.txt", books);
                Console.WriteLine("The Book that named " + books[bookIndex].Title + " was added to library succesfuly as copy.");
            }
        }

        private void CreateNewBook(string? name, string? author, string? isbn, string id)
        {
            Book book = new Book(name ?? "unknown name", author ?? "unknown author", isbn ?? "unknown isbn", 1, id);
            books.Add(book);
            FileManager.WriteABookToFile("books.txt", name, author, isbn, 1, id);
            PopulateBookNamesList(name);
            Console.WriteLine("The Book that named " + name + " was added to library succesfuly.");
            lastBookID++;
        }

        private void UpdateBorrowerTxtFile(string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                int id = 1;
                foreach (var borrower in borrowerList)
                {
                    foreach (var kvp in borrower.booksDict)
                    {
                        string line = $"{id.ToString()}, {borrower.id}, {borrower.user.name}, {kvp.Key.Title}, {kvp.Value}";
                        writer.WriteLine(line);
                        id++;
                    }
                }
            }
        }
        private void BorrowBook(string? idOrName)
        {
            lastBorrowID++;
            int bookIndex = SearchManager.SearchBook(idOrName, searchLevel: SearchManager.SearchLevel.BothNameAndId, books: books);

            if (bookIndex != -1)
            {
                Console.WriteLine("book index from borrow: " + bookIndex);
                Book book = books[bookIndex];
                book.CopyNumber--;
                FileManager.UpdateBookTxtFile("books.txt", books);
                DateTime dateTime = DateTime.Now.AddDays(15);

                Borrower existingBorrowerObject = SearchManager.SearchBorrower(borrowerList, user);
                if (existingBorrowerObject != null)
                {
                    existingBorrowerObject.AddBookToBooksDict(book, dateTime);
                    FileManager.WriteABorrowToFile("Borrower.txt", user.name, existingBorrowerObject.booksDict, lastBorrowerID, lastBorrowID);
                    Console.WriteLine("--existing borrower has borrowed the book--");
                }
                else
                {
                    lastBorrowerID++;
                    Borrower borrower = new Borrower(lastBorrowerID.ToString(), user);
                    borrower.AddBookToBooksDict(book, dateTime);
                    borrowerList.Add(borrower);
                    FileManager.WriteABorrowToFile("Borrower.txt", user.name, borrower.booksDict, lastBorrowerID, lastBorrowID);
                    Console.WriteLine("--New borrower has borrowed the book--");
                }

                //Console.WriteLine("date : " + dateTime);
                Console.WriteLine("--Borrewed the book succesfully--");
            }
            else
            {
                Console.WriteLine("--No book has the id you entered--");
            }
        }
        private void DisplayBorrowedBooks()
        {
            bool foundUser = false;
            Console.WriteLine("                   ******* Borrowed Books *******");
            foreach (Borrower borrowerObj in borrowerList)
            {
                if (borrowerObj.user?.name == user.name)
                {
                    foreach (KeyValuePair<Book, DateTime> kvpInsideBorrower in borrowerObj.booksDict)
                    {
                        TimeSpan timeOfset = kvpInsideBorrower.Value - DateTime.Now;
                        int remainingDay = timeOfset.Days;
                        Console.WriteLine("Book ID: " + kvpInsideBorrower.Key.id + " - " + "Borrower ID: " + borrowerObj.id + " - " + "Book: " + kvpInsideBorrower.Key.Title + " - " + "Deadline: " + kvpInsideBorrower.Value + " - " + "Remaining Day: " + remainingDay);
                    }
                    foundUser = true;
                }
            }

            if (!foundUser)
            {
                Console.WriteLine("--No book registered to you was found--");
            }
            Console.WriteLine("                           **************");
        }

        private void ReturnBorrowedBook(string bookId)
        {
            Borrower borrowerObj = SearchManager.SearchBorrower(borrowerList, user);

            if (borrowerObj != null)
            {
                Book book = SearchManager.SearchBookInBorrower(bookId, borrowerObj.booksDict);
                if (book != null)
                {
                    book.CopyNumber++;
                    borrowerObj.booksDict.Remove(book);
                    FileManager.UpdateBookTxtFile("books.txt", books);
                    FileManager.UpdateBorrowerTxtFile("Borrower.txt", borrowerList);
                    Console.WriteLine("--The book has returned succesfully--");
                }
                else
                {
                    Console.WriteLine("--Not found borrowed book with the id--");
                }

            }
        }
        private string RemoveSpaces(string input)
        {
            return input.Replace(" ", string.Empty);
        }
        #endregion
    }
}