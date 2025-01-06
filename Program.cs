using System.Diagnostics;
using System.Numerics;
using System.Security.Cryptography;
using System.Xml.Serialization;
using System;

namespace s1finalexam
{
    internal class Program
    {
        #region structs

        public struct Client
        {
            public int Id;
            public string FirstName;
            public string LastName;
            public string Password; //------------------------------------------------------------------------------------------Minimum: 6 digits - Maximum: 10 digits.

            //Parametrized constructor.
            public Client(int id, string firstName, string lastName, string password)
            {
                Id = id;
                FirstName = firstName;
                LastName = lastName;
                Password = password;
            }

        }
        public struct Book
        {
            public int Id;
            public string Name;
            public string Author;
            public int Year;
            public double RentalFee;
            public bool IsRented;
            public bool CanBeSold;
            public double UnitPriceForSale;
            public int Quantity;
            //Parametrized contructor.
            public Book(int id, string name, string author, int year, double rentalFee, bool isRented, bool canBeSold, double unitPriceForSale, int quantity)
            {
                Id = id;
                Name = name;
                Author = author;
                Year = year;
                RentalFee = rentalFee;
                IsRented = isRented;
                CanBeSold = canBeSold;
                UnitPriceForSale = unitPriceForSale;
                Quantity = quantity;
            }
        }
        public struct RentedBook
        {
            public Book XBook;
            public Client XClient;
            public bool IsReturned;
            //Parametrized constructor.
            public RentedBook(Book xBook, Client xClient, bool isReturned)
            {
                XBook = xBook;
                XClient = xClient;
                IsReturned = isReturned;
            }
        }
        struct PurchasedBook
        {
            public Book XBook;
            public Client XClient;
            public int QuantityPurchased;
            public double FinalPrice;
            //Parametrized constructor.
            public PurchasedBook(Book xBook, Client xClient, int quantityPurchased, double finalPrice)
            {
                XBook = xBook;
                XClient = xClient;
                QuantityPurchased = quantityPurchased;
                FinalPrice = finalPrice;
            }
        }
        #endregion

        #region constants
        const int MAX_CLIENTS = 20;
        const int MAX_BOOKS = 100;
        const int MIN_ID_NUMBER = 100000;
        #endregion

        #region othervariables
        static int option = 0;
        static int choice = 1;
        static int choiceClient = 1;
        static int iClient = 0;
        static int iBook = 0;
        static int clientId = 100000;
        static int idBook = 100010;
        static Client CLIENTSIGNED;
        #endregion

        #region arrays
        static int nPurchasedBooks = 0;
        static int nRentedBooks = 0;
        static Client[] CLIENTS = new Client[MAX_CLIENTS];
        static Book[] BOOKS = new Book[MAX_BOOKS];
        //  We just make the length of the array wide enough
        static RentedBook[] RENTEDBOOKS = new RentedBook[100000];
        //  same logic as above
        static PurchasedBook[] PURCHASEDBOOKS = new PurchasedBook[100000];
        #endregion

        static void Main(string[] args)
        {
            while (true)
            {
                CreateDefaultBooks();
                MainMenu();


                option = ReadInteger(1, 2);
                switch (option)
                {
                    case 1:
                        EmployeeSignIn();
                        while (choice > 0 && choice < 10)
                        {
                            EmployeeMenu();
                            Console.Write("Please choose an option: ");
                            choice = ReadInteger(1, 10);
                            Console.WriteLine();

                            switch (choice)
                            {
                                case 1:
                                    CreateClient();
                                    break;
                                case 2:
                                    SortAndDisplayClient();
                                    break;
                                case 3:
                                    CreateBook();
                                    break;
                                case 4:
                                    SortAndDisplayBook();
                                    break;
                                case 5:
                                    RentBooks();
                                    break;
                                case 6:
                                    ReturnBook();
                                    break;
                                case 7:
                                    SellBook();
                                    break;
                                case 8:
                                    DisplayAllRentedBooks();
                                    break;
                                case 9:
                                    DisplayAllSoldBooks();
                                    break;
                                case 10:
                                    Console.WriteLine("Returning to main menu.");
                                    break;
                            }
                            Console.ReadLine();
                            Console.Clear();
                        }
                        choice = 1;
                        break;

                    case 2:
                        bool canContinue = ClientSignIn();
                        if (canContinue == false) break;
                        ClientMenu();
                        while (choiceClient > 0 && choiceClient < 3)
                        {
                            Console.Write("Please choose an option: ");
                            choiceClient = ReadInteger(1, 3);
                            switch (choiceClient)
                            {
                                case 1:
                                    ClientRentedBooks();
                                    break;
                                case 2:
                                    ClientPurchasedBooks();
                                    break;
                                case 3:
                                    Console.WriteLine("Returning to main menu.");

                                    return;
                                default:
                                    Console.WriteLine("Invalid option. Please try again.");
                                    break;
                            }
                        }
                        choiceClient = 1;
                        break;
                    case 3: //--------------------------------------------------- >>BONUS!!!<<
                        {
                            Console.WriteLine("1) Find a book by Name or Author to rent it.");

                            option = ReadInteger(1, 4);

                            switch (option)
                            {
                                case 1: //WRONG!!!
                                    {
                                        int id = 0;
                                        string name = " ", author = " ";

                                        Console.WriteLine("Select a option to search a book:\n" +
                                                          "1) Search by Id.\n" +
                                                          "2) Search by Name.\n" +
                                                          "3) Search by Author.");

                                        option = ReadInteger(1, 3);

                                        if (option == 1)
                                        {
                                            Console.WriteLine("Please enter the Book Id: ");
                                            id = ReadInteger();
                                        }
                                        if (option == 2)
                                        {
                                            Console.WriteLine("Please enter the name of the Book: ");
                                            name = ReadString();
                                        }
                                        else
                                        {
                                            Console.WriteLine("Please enter the Author's name: ");
                                            author = ReadString();
                                        }

                                        bool findingABook = Array.Exists(BOOKS, (book) => book.Id == id || book.Name == name || book.Author == author);

                                        if (findingABook)
                                        {
                                            Console.WriteLine($"The book{name}, writen by {author} is available.");
                                        }
                                        else
                                        {
                                            Console.WriteLine("This book is unavailable.");
                                        }

                                    }
                                    break;
                            }
                        }
                        break;
                }
            }

        }

        #region clientsfeatures
        /// <summary>
        /// Client menu
        /// </summary>
        public static void ClientMenu()
        {
            Console.WriteLine("CLIENT MENU: \n" +
                                              "1) Rented books.\n" +
                                              "2) Purchased books.\n" +
                                              "3) Return to Main Menu.");
        }

        /// <summary>
        /// Rented books for signed client
        /// </summary>
        public static void ClientRentedBooks()
        {
            Console.WriteLine("Your rented books:");
            foreach (RentedBook item in RENTEDBOOKS)
            {
                if (item.XClient.Id == CLIENTSIGNED.Id)
                {
                    Console.WriteLine("Title: " + item.XBook.Name + "\tAuthor: " + item.XBook.Author);
                }
            }
            Console.WriteLine("");
        }

        /// <summary>
        /// Purchased books for signed client
        /// </summary>
        public static void ClientPurchasedBooks()
        {
            Console.WriteLine("Your purchased books:");
            foreach (PurchasedBook item in PURCHASEDBOOKS)
            {
                if (item.XClient.Id == CLIENTSIGNED.Id)
                {
                    Console.WriteLine("Title: " + item.XBook.Name + "\tAuthor: " + item.XBook.Author + "\tQuantity: " + item.QuantityPurchased + "\tTotal price:" + item.FinalPrice);
                }
            }
            Console.WriteLine("");
        }

        /// <summary>
        /// Client sign in
        /// </summary>
        private static bool ClientSignIn()
        {
            Console.WriteLine("Sign in as client.");
            if (AreClientsNotExist()) return false;
            int id = 0;
            string password = " ";
            Console.Write("Enter Id:");
            id = ReadInteger();
            Console.Write("Enter Password:");
            password = ReadString();

            bool correctCredentials = Array.Exists(CLIENTS, (client) => client.Id == id && client.Password == password);

            while (!correctCredentials)
            {
                Console.WriteLine("invalid Id or password. Please try again.");
                Console.Write("Id:");
                id = ReadInteger();
                Console.Write("password:");
                password = ReadString();
                correctCredentials = Array.Exists(CLIENTS, (client) => client.Id == id && client.Password == password);
            }
            CLIENTSIGNED = FindAClient(iClient); //save client id global variable 
            Console.Write("You are signed in.");
            Console.WriteLine("Welcome " + CLIENTSIGNED.FirstName + " " + CLIENTSIGNED.LastName + ". It is " + DateTime.Now + "\n\n");
            return true;
        }

        #endregion

        #region employeefeatures
        /// <summary>
        /// Employee sign in
        /// </summary>
        public static void EmployeeSignIn()
        {
            int id = 0;
            string password = " ";
            Console.WriteLine("Sign in as employee.");
            Console.Write("Id: ");
            id = ReadInteger();//validsate
            Console.Write("Password: ");
            password = ReadString();
            while (id != 111111 || password != "tester")
            {
                Console.WriteLine("Id or password invalid please try again.");
                Console.Write("Id: ");
                id = ReadInteger();
                Console.Write("Password: ");
                password = ReadString();
            }
            Console.WriteLine("You are signed in.");
            Console.WriteLine("Welcome 111111. It is " + DateTime.Now + "\n\n");
        }

        /// <summary>
        /// Menu for employee 
        /// </summary>
        public static void EmployeeMenu()
        {
            Console.Write("Menu\n1.Create a client\n2.Display all the clients sorted by ID\n3.Create a book\n4.Display all the books sorted by ID\n5.Rent a book\n6.Return a book\n7.Sell a book\n8.Display all the books rented\n9.Display all the books sold\n10.Sign Out: to return to the main menu\n");
        }

        /// <summary>
        /// Display all the books rented 
        /// </summary>
        public static void DisplayAllRentedBooks()
        {
            Console.WriteLine("All rented books: ");
            for (int i = 0; i < nRentedBooks; i++)
            {
                Console.WriteLine("Book " + RENTEDBOOKS[i].XBook.Name + " rented by " + RENTEDBOOKS[i].XClient.FirstName + " " + RENTEDBOOKS[i].XClient.LastName + ". Is returned? " + RENTEDBOOKS[i].IsReturned);
            }
            Console.WriteLine("");
        }

        /// <summary>
        /// Display all the books purchased 
        /// </summary>
        public static void DisplayAllSoldBooks()
        {
            Console.WriteLine("All sold books: ");
            for (int i = 0; i < nPurchasedBooks; i++)
            {
                Console.WriteLine("Book " + PURCHASEDBOOKS[i].XBook.Name + " purchased by " + PURCHASEDBOOKS[i].XClient.FirstName + " " + PURCHASEDBOOKS[i].XClient.LastName);
            }
            Console.WriteLine("");
        }

        /// <summary>
        /// Add a new client
        /// </summary>
        public static void CreateClient()
        {
            Console.WriteLine("You are about to add a client.");
            if (iClient > MAX_CLIENTS)
            {
                Console.WriteLine("You can't add clients anymore.");
                return;
            }
            Console.WriteLine("Insert information for the client");
            int id = clientId;
            Console.Write("Please enter first name: ");
            string firstName = ReadString();
            Console.Write("Please enter last name: ");
            string lastName = ReadString();
            Console.Write("Please enter password: ");
            string password = ReadString(6, 10);
            Client client = new Client(id, firstName, lastName, password);

            CLIENTS[iClient] = client;
            iClient++;
            clientId++;
            Console.WriteLine("Client added.\n");
        }

        /// <summary>
        /// Add a new book
        /// </summary>
        public static void CreateBook()
        {
            Console.WriteLine("You are about to add a book.");
            if (iBook > MAX_BOOKS)
            {
                Console.WriteLine("You can't add books anymore.");
                return;
            }
            // create a book 

            int bookId = idBook;
            Console.Write("Book Name: ");
            string bookName = ReadString();
            Console.Write("Author: ");
            string bookAuthor = ReadString();
            Console.Write("Year of publication: ");
            int bookYear = ReadInteger();
            Console.Write("Rental fees: ");
            double bookRentalFees = ReadDouble();
            bool bookIsRented = false;
            bool bookCanBeSold = true;
            Console.Write("Unit price: ");
            double bookUnitPrice = ReadDouble();
            Console.Write("Quantity: ");
            int bookQuantity = ReadInteger();

            Book book = new Book(bookId, bookName, bookAuthor, bookYear, bookRentalFees, bookIsRented, bookCanBeSold, bookUnitPrice, bookQuantity);
            BOOKS[iBook] = book;
            iBook++;
            idBook++;
            Console.WriteLine("Book added.\n");
        }
        /// <summary>
        /// Sort and display the books 
        /// </summary>
        public static void SortAndDisplayBook()
        {

            int n = iBook;
            // We make a copy of all the books. We wan to sort the copy not the original books array.
            Book[] sortedBooks = (Book[])BOOKS.Clone();
            for (int i = 0; i <= n - 2; i++)
            {
                for (int j = 0; j <= n - 2; j++)
                {
                    if (sortedBooks[j].Id > sortedBooks[j + 1].Id)
                    {
                        Book temp = sortedBooks[j + 1];
                        sortedBooks[j + 1] = sortedBooks[j];
                        sortedBooks[j] = temp;
                    }

                }
            }
            Console.WriteLine("All books sorted by Id:\n");
            DisplayBooks(sortedBooks);
            Console.WriteLine("");
        }

        /// <summary>
        /// Sort and display the clients
        /// </summary>
        public static void SortAndDisplayClient()
        {
            int n = iClient;
            // T: We make a copy of all the books. We wan to sort the copy not the original books array.
            Client[] sorted = (Client[])CLIENTS.Clone();
            for (int i = 0; i <= n - 2; i++)
            {
                for (int j = 0; j <= n - 2; j++)
                {
                    if (sorted[j].Id > sorted[j + 1].Id)
                    {
                        Client temp = sorted[j + 1];
                        sorted[j + 1] = sorted[j];
                        sorted[j] = temp;
                    }

                }
            }
            Console.WriteLine("All clients sorted by Id:");
            DisplayClients(sorted);
            Console.WriteLine("");
        }

        /// <summary>
        /// Display list of clients
        /// </summary>
        public static void DisplayClients(Client[] _clients)
        {
            for (int i = 0; i < iClient; i++)
            {
                Console.WriteLine(_clients[i].Id + "\t" + _clients[i].FirstName + "\t" + _clients[i].LastName);
            }
        }

        /// <summary>
        /// Display list of books
        /// </summary>
        public static void DisplayBooks(Book[] _books)
        {
            Console.WriteLine("Id\tTitle\tAuthor\tYear\tRentalFee\tUnitPrice\tQuantity");
            Console.WriteLine("ano ny tay");
            for (int i = 0; i < iBook; i++)
            {
                Console.WriteLine(_books[i].Id + "\t" + _books[i].Name + "\t" + _books[i].Author + "\t" + _books[i].Year + "\t" + _books[i].RentalFee + "\t\t" + _books[i].UnitPriceForSale + "\t\t" + _books[i].Quantity);
            }
        }

        /// <summary>
        /// Selling books menu
        /// </summary>
        public static void SellBook()
        {
            Console.WriteLine("You are about to sell books.");
            if (AreBooksNotExist()) return;
            if (AreClientsNotExist()) return;
            int idClient;
            int idBook;
            int quantity;

            Console.Write("Enter id client: ");
            idClient = ReadInteger(MIN_ID_NUMBER, MIN_ID_NUMBER + iClient - 1);
            Console.Write("Enter id book: ");
            idBook = ReadInteger(MIN_ID_NUMBER, MIN_ID_NUMBER + iBook - 1);
            Console.Write("Enter quantity: ");
            quantity = ReadInteger();
            SellBook(idBook, quantity, idClient);
        }

        /// <summary>
        /// Selling a book to a particular client
        /// </summary>
        /// <param name="idBook"></param>
        /// <param name="quantity"></param>
        /// <param name="idClient"></param>
        public static void SellBook(int idBook, int quantity, int idClient)
        {

            Book foundBook = FindABook(idBook);
            Client foundClient = FindAClient(idClient);
            if (foundBook.CanBeSold == true)
            {
                int remainBooks = foundBook.Quantity - quantity;
                if (remainBooks >= 0)
                {
                    PurchasedBook purchased = new PurchasedBook(foundBook, foundClient, quantity, quantity * foundBook.UnitPriceForSale);
                    //  Put the new pruchased book inside purchasedBooks
                    PURCHASEDBOOKS[nPurchasedBooks] = purchased;
                    //  Number of purchased books increase by one
                    nPurchasedBooks++;
                    UpdateBookQuantity(idBook, remainBooks);
                    Console.WriteLine("Selling book (id=" + foundBook.Id + "): " + foundBook.Name + " to " + foundClient.FirstName + " " + foundClient.LastName + " is sucessful.");
                    Console.WriteLine("Book sold.\n");
                }
                else
                {
                    Console.WriteLine("Not enough quantity of this book.\n");
                }
            }
        }

        /// <summary>
        /// validate that integer is within allowed
        /// </summary>
        /// <param name="allowed"></param>
        /// <returns></returns>
        public static int ReadIntegerRented()
        {
            int integer = ReadInteger();
            while (IsNumberInArray(RENTEDBOOKS, nRentedBooks, integer) == false)
            {
                Console.Write("Please choose among the rented books: ");
                integer = ReadInteger();
            }
            return integer;
        }


        /// <summary>
        /// Check if value is in allowed list
        /// </summary>
        /// <param name="allowed"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNumberInArray(RentedBook[] allowed, int len, int idbook)
        {
            for (int i = 0; i < len; i++)
            {
                if (allowed[i].XBook.Id == idbook) return true;
            }

            return false;

        }

        /// <summary>
        /// Returning a book menu
        /// </summary>
        public static void ReturnBook()
        {
            Console.WriteLine("You are about to return a book.");
            if (AreBooksNotExist()) return;

            if (nRentedBooks == 0)
            {
                Console.WriteLine("No books have been rented.");
                return;
            }

            Console.WriteLine("Choose the book to return among the rented books:");
            for (int i = 0; i < nRentedBooks; i++)
            {
                if (RENTEDBOOKS[i].IsReturned == false)
                {
                    Console.WriteLine("Book: " + RENTEDBOOKS[i].XBook.Id + "\t" + RENTEDBOOKS[i].XBook.Name + "\tClient:" + RENTEDBOOKS[i].XClient.Id + "\t" + RENTEDBOOKS[i].XClient.FirstName + " " + RENTEDBOOKS[i].XClient.LastName);
                }
            }
            int idBook;
            int numberDays;
            double totalFee;
            int confirm;

            Console.Write("Enter book Id: ");
            idBook = ReadIntegerRented();
            Book foundBook = FindABook(idBook);

            Console.Write("Enter number of days book was rented: ");
            numberDays = Convert.ToInt32(Console.ReadLine());
            totalFee = numberDays * foundBook.RentalFee;
            Console.WriteLine("Total fee applied is: " + totalFee + " CAD");
            Console.WriteLine("Do you want to confirm the return? 0 = true or false = 1");
            confirm = ReadInteger(0, 1);
            if (confirm == 0)
            {
                UpdateBookRent(idBook, false);
                UpdateRentedBook(idBook);
                Console.WriteLine("Book is returned.\n");
            }
            else
            {
                Console.WriteLine("Book not returned.\n");
            }
        }

        public static void UpdateRentedBook(int idBook)
        {
            for (int i = 0; i < nRentedBooks; i++)
            {
                if (RENTEDBOOKS[i].XBook.Id == idBook)
                {
                    RENTEDBOOKS[i].IsReturned = true;
                }
            }

        }

        /// <summary>
        /// Updating if book is rented or not
        /// </summary>
        /// <param name="idBook"></param>
        /// <param name="isRent"></param>
        public static void UpdateBookRent(int idBook, bool isRent)
        {
            for (int i = 0; i < iBook; i++)
            {
                if (BOOKS[i].Id == idBook)
                {
                    BOOKS[i].IsRented = isRent;
                }
            }
        }

        /// <summary>
        /// Find a client by ID
        /// </summary>
        /// <param name="idClient"></param>
        public static Client FindAClient(int idClient)
        {

            int index_found = 0;
            for (int i = 0; i < iClient; i++)
            {
                if (CLIENTS[i].Id == idClient)
                {
                    index_found = i;
                    break;
                }
            }
            return CLIENTS[index_found];
        }

        /// <summary>
        /// find a book by ID
        /// </summary>
        /// <param name="id_book"></param>
        public static Book FindABook(int id_book)
        {

            int index_found = 0;
            for (int i = 0; i < iBook; i++)
            {
                if (BOOKS[i].Id == id_book)
                {
                    index_found = i;
                    break;
                }
            }
            return BOOKS[index_found];
        }
        /// <summary>
        /// Check if at least one book exists
        /// </summary>
        /// <returns>True if there are no books, false if there is at least one book</returns>
        public static bool AreBooksNotExist()
        {
            bool noBooks = false;
            if (iBook == 0)
            {
                Console.WriteLine("Please enter at least one book before proceeding.");
                noBooks = true;
            }
            return noBooks;
        }

        /// <summary>
        /// Check if at least one client exists
        /// </summary>
        public static bool AreClientsNotExist()
        {
            bool noClients = false;
            if (iClient == 0)
            {
                Console.WriteLine("Please enter at least one client before proceeding.");
                noClients = true;
            }
            return noClients;
        }
        /// <summary>
        /// Renting books menu
        /// </summary>
        public static void RentBooks()
        {
            Console.WriteLine("You are about to rent books.");
            if (AreBooksNotExist())
            {
                return;
            }
            if (AreClientsNotExist())
            {
                return;
            }


            int idclient;
            int numberBooks;
            Console.Write("Enter Id client: ");
            idclient = ReadInteger(MIN_ID_NUMBER, MIN_ID_NUMBER + iClient - 1);

            Console.Write("Choose number of books to rent: ");
            numberBooks = ReadInteger(1, iBook);

            int[] idBooksWanted = new int[numberBooks];
            int idBook;
            int i = 0;
            while (i < idBooksWanted.Length)
            {
                Console.Write("#" + i + " - Enter Id book: ");
                idBook = ReadInteger(MIN_ID_NUMBER, MIN_ID_NUMBER + iBook - 1);
                if (IsIdAlreadyPresent(idBooksWanted, idBook))
                {
                    Console.WriteLine("Please choose another Id. You already added this to the list.");
                }
                else
                {
                    idBooksWanted[i] = idBook;
                    i++;
                }
            }
            RentBooks(idBooksWanted, idclient);
        }

        public static bool IsIdAlreadyPresent(int[] idBooksWanted, int idBook)
        {
            for (int i = 0; i < idBooksWanted.Length; i++)
            {
                if (idBooksWanted[i] == idBook)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Rent a book 
        /// </summary>
        /// <param name="idBooksWanted"></param>
        /// <param name="idClient"></param>
        public static void RentBooks(int[] idBooksWanted, int idClient)
        {
            Client foundClient = FindAClient(idClient);
            for (int iW = 0; iW < idBooksWanted.Length; iW++)
            {
                Book foundBook = FindABook(idBooksWanted[iW]);
                if (CanBookBeRented(idBooksWanted[iW]) && foundBook.Quantity > 0)
                {
                    RentedBook rented = new RentedBook(foundBook, foundClient, false);
                    // T: We put the new rented books into the rentedbooks array
                    RENTEDBOOKS[nRentedBooks] = rented;
                    // T: The number of rented books increase by one
                    nRentedBooks++;
                    UpdateBookQuantity(idBooksWanted[iW], foundBook.Quantity--);
                    UpdateBookRent(idBooksWanted[iW], true);
                }
            }
            Console.WriteLine("Books rented.\n");
        }

        /// <summary>
        /// Updating book quantity
        /// </summary>
        /// <param name="idBook"></param>
        /// <param name="quantity"></param>
        public static void UpdateBookQuantity(int idBook, int quantity)
        {
            for (int i = 0; i < iBook; i++)
            {
                if (BOOKS[i].Id == idBook)
                {
                    BOOKS[i].Quantity = quantity;
                    if (quantity <= 0)
                    {
                        BOOKS[i].CanBeSold = false;
                    }
                }
            }
        }

        /// <summary>
        /// Check if book can be rented
        /// </summary>
        /// <param name="idBook"></param>
        /// <returns></returns>
        public static bool CanBookBeRented(int idBook)
        {
            for (int i = 0; i < nRentedBooks; i++)
            {
                if (BOOKS[i].Id == idBook)
                {
                    if (BOOKS[i].IsRented)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
        #endregion

        #region others
        public static void CreateDefaultBooks()
        {
            BOOKS[0] = new Book(100000, "B0", "A0", 2010, 10, false, true, 48, 3);
            BOOKS[1] = new Book(100001, "B1", "A1", 2011, 4, false, true, 98, 7);
            BOOKS[2] = new Book(100002, "B2", "A2", 2012, 8, false, false, 36, 8);
            BOOKS[3] = new Book(100003, "B3", "A3", 2013, 5, false, true, 29, 9);
            BOOKS[4] = new Book(100004, "B4", "A4", 2014, 2, false, false, 29, 2);
            BOOKS[5] = new Book(100005, "B5", "A5", 2015, 6, false, true, 12, 1);
            BOOKS[6] = new Book(100006, "B6", "A6", 2016, 3, false, true, 29, 1);
            BOOKS[7] = new Book(100007, "B7", "A7", 2017, 15, false, true, 12, 6);
            BOOKS[8] = new Book(100009, "B9", "A9", 2019, 9, false, true, 29, 6);
            BOOKS[9] = new Book(100008, "B8", "A8", 2018, 25, false, true, 65, 6);

            iBook = 10;
        }
        /// <summary>
        /// validate string
        /// </summary>
        /// <returns>string value </returns>
        public static string ReadString()
        {
            string readString = Console.ReadLine();
            while (string.IsNullOrEmpty(readString))
            {
                Console.WriteLine("Enter a valid string");
                readString = Console.ReadLine();
            }
            return readString;
        }

        /// <summary>
        /// validate if it s a double 
        /// </summary>
        /// <returns>double value </returns>
        public static double ReadDouble()
        {
            double readDouble = 0;
            while (!double.TryParse(Console.ReadLine(), out readDouble))
            {
                Console.Write("Please enter a valid double: ");
            }
            return readDouble;
        }


        /// <summary>
        /// validate string with length
        /// </summary>
        /// <param name="minLen"></param>
        /// <param name="maxLen"></param>
        public static string ReadString(int minLen, int maxLen)
        {
            string readString = Console.ReadLine();
            while (string.IsNullOrEmpty(readString) || readString.Length < minLen || readString.Length > maxLen)
            {
                Console.Write("Enter a valid string between " + minLen + " and " + maxLen + ": ");
                readString = Console.ReadLine();
            }
            return readString;
        }

        /// <summary>
        /// Display the Main menu ( client user or employee)
        /// </summary>
        public static void MainMenu()
        {
            Console.Write("\nWelcome\nMain Menu\n1-Employee\n2-Client\n\nChoose your option: ");
        }

        /// <summary>
        /// Validation of integer
        /// </summary>
        /// <returns></returns>
        public static int ReadInteger()
        {
            int readInteger = 0;
            while (!Int32.TryParse(Console.ReadLine(), out readInteger))
            {
                Console.Write("Please enter a valid number: ");
            }
            return readInteger;
        }

        /// <summary>
        /// validate that integer is within limit
        /// </summary>
        /// <param lowest limit="min"></param>
        /// <param highest limit="max"></param>
        /// <returns></returns>
        public static int ReadInteger(int min, int max)
        {
            int integer = ReadInteger();
            while (integer > max || integer < min)
            {
                Console.Write("Please enter a valid number between " + min + " and " + max + ": ");
                integer = ReadInteger();
            }
            return integer;


        }

        #endregion
    }


}
// int rent = 0;
//  rentsales = book.price
/*rent = rent + rent sales*/