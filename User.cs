using System;

namespace LMSProject
{
    class User
    {
/*         public User(string id, string name)
        {
            this.id = id;
            this.name = name;
        } */

        public string? id { get; set; }
        public string? name { get; set; }

        public void Login(string id)
        {
            Console.WriteLine("Please Enter The Name To Login");
            string? name = Console.ReadLine();
            this.name = name;
            this.id = id;
        }
    }
}