namespace LMSProject
{
    static class SearchManager
    {
        //public static List<Book> nullList = new List<Book>();
        public static int SearchBook(string? bookName = "", string id = "", SearchLevel searchLevel = SearchLevel.Name, List<Book>? books = null)
        {
            int bookIndex = -1;
            int lengthOfBooksList = books.Count;
            string nameWihoutSpace = RemoveSpaces(bookName);

            for (int i = 0; i < lengthOfBooksList; i++)
            {
                var obj = books[i];
                string titleWithouSpace = RemoveSpaces(obj.Title);

                switch (searchLevel)
                {
                    case SearchLevel.Name:
                        if (titleWithouSpace == nameWihoutSpace)
                        {
                            bookIndex = i;
                            return bookIndex;
                        }
                        break;
                    case SearchLevel.Id:
                        if (id == obj.id)
                        {
                            bookIndex = i;
                            return bookIndex;
                        }
                        break;
                    case SearchLevel.BothNameAndId:
                        if (titleWithouSpace == nameWihoutSpace || nameWihoutSpace == obj.id)
                        {
                            bookIndex = i;
                            return bookIndex;
                        }
                        break;

                }
            }
            return bookIndex;
        }

        public static Borrower? SearchBorrower(List<Borrower> borrowerList, User user)
        {
            foreach (Borrower borrowerObj in borrowerList)
            {
                if (borrowerObj.user.name == user.name)
                {
                    return borrowerObj;
                }
            }
            return null;
        }

        public static Book SearchBookInBorrower(string bookID, Dictionary<Book, DateTime> booksDict)
        {
            foreach (KeyValuePair<Book, DateTime> kvp in booksDict)
            {
                if (kvp.Key.id == bookID)
                {
                    return kvp.Key;
                }
            }
            return null;
        }

        public static string RemoveSpaces(string input)
        {
            return input.Replace(" ", string.Empty);
        }

        public enum SearchLevel
        {
            Name,
            Id,
            Author,
            BothNameAndId,
        }
    }
}