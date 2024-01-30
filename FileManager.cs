using System;
using System.Collections.Generic;
using System.IO;

namespace LMSProject
{
    static class FileManager
    {
        public static void WriteABookToFile(string filePath, string? name, string? author, string? isbn, int? copyNumber, string id)
        {
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                string line = $"{id}, {name}, {author}, {isbn}, {copyNumber}";
                writer.WriteLine(line);
            }
        }

        public static void WriteABorrowToFile(string filePath, string name, Dictionary<Book, DateTime> booksDict, int lastBorrowerID, int lastBorrowID)
        {
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                string borrowerId = lastBorrowerID.ToString();
                string borrowId = lastBorrowID.ToString();
                int dictLength = booksDict.Count;
                int iterationCounter = 0;
                Console.WriteLine("dict len : " + dictLength);

                //yalnızca son iterasyonda yani sözlüğe son eklenen bilgiyi dosyaya yazdırıyoruz
                foreach (KeyValuePair<Book, DateTime> kvp in booksDict)
                {
                    Console.WriteLine("iterationCounter : " + iterationCounter);
                    if (iterationCounter == dictLength - 1)
                    {
                        string line = $"{borrowId}, {borrowerId}, {name}, {kvp.Key.Title}, {kvp.Value}";
                        writer.WriteLine(line);
                    }
                    iterationCounter++;
                }
            }
        }

        public static void UpdateBookTxtFile(string filePath, List<Book> books)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (var book in books)
                {
                    string line = $"{book.id}, {book.Title}, {book.Author}, {book.Isbn}, {book.CopyNumber}";
                    writer.WriteLine(line);
                }
            }
        }

        public static void UpdateBorrowerTxtFile(string filePath, List<Borrower> borrowerList)
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
    }
}