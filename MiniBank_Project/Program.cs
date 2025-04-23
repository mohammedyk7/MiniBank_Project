namespace MiniBank_Project
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool processing = true;
            while(processing)
            Console.WriteLine("=========================WELCOME TO CODELINE-$AFE-BANK =============================");
            Console.WriteLine("1. USERMENU ");
            Console.WriteLine("2. ADMINMENU ");
            Console.WriteLine("0. EXIT");
            Console.WriteLine("SELECT OPTION :");
            string choice = Console.ReadLine();
            switch (choice) 
            {
                case "1": UserMenu(); break;
                case "2": AdminMenu(); break;
                case "0": processing = false; break;

            }
        }

        static void UserMenu()
        {
            bool enteringmenu = true;
            while (enteringmenu)
            Console.WriteLine("Welcome to Mini Bank");
            Console.WriteLine("1. Create Account");
            Console.WriteLine("2. Deposit Money");
            Console.WriteLine("3. Withdraw Money");
            Console.WriteLine("4. Check Balance");
            Console.WriteLine("5. submit rebiew");
            Console.WriteLine("0. back to mainmenu");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1": RequestAccountOpening(); break;
                case "2": Deposit(); break;
                case "3": WithDraw(); break;
                case "4": Checkbalance(); break;
                case "5": SubmitReview(); break;
                case "0": enteringmenu = false; break;// so that the user can go back to the main menu




            }

        }
        static void AdminMenu()
        {
            Console.WriteLine("Welcome to Mini Bank Admin");
            Console.WriteLine("1. View All Accounts");
            Console.WriteLine("2. Delete Account");
            Console.WriteLine("3. Exit");
        }
        static void RequestAccountOpening() 
        {

        }
        static void Deposit() 
        {

        }
        static void WithDraw() 
        {

        }
        static void Checkbalance() 
        {

        }
        static void SubmitReview() 
        {

        }
        static void ReviewRequest() 
        {

        }
        static void ViewAccounts() 
        {

        }
        static void ViewReviews() 
        {

        }
        static void ProcessRequests() 
        {

        }
    }
}
