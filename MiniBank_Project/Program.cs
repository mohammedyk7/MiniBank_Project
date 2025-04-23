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
                case "0":

            }
        }

        static void UserMenu()
        {
            Console.WriteLine("Welcome to Mini Bank");
            Console.WriteLine("1. Create Account");
            Console.WriteLine("2. Deposit Money");
            Console.WriteLine("3. Withdraw Money");
            Console.WriteLine("4. Check Balance");
            Console.WriteLine("5. Exit");
            string choice = Console.ReadLine();
            switch (choice)
            {
                
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
