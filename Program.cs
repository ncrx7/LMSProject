using System;

namespace LMSProject
{
    class Program
    {
        static Library? libraryInstance;
        static User? userInstance;
        static void Main(string[] args)
        {
            MainProgram();
        }

        static void MainProgram()
        {
            InitializeInstance();

            libraryInstance?.PopulateBooksList();
            libraryInstance?.PopulateBorrowerList();

            userInstance?.Login("1");
            
            libraryInstance?.SetUser(userInstance);
            
            libraryInstance?.LibraryMainMenu();
        }
        static void InitializeInstance()
        {
            libraryInstance = new Library();
            userInstance = new User();
            //Console.WriteLine("library instance was created");
        }
    }
}