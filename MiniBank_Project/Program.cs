namespace MiniBank_Project
{
    internal class Program
    {
        static void Main()
        {
            bool processing = true;
            while (processing)
            {
                Console.Clear();
                Console.WriteLine("=========================$$$WELCOME TO CODELINE-$AFE-BANK$$$=============================");
                Console.WriteLine("1. USERMENU ");
                Console.WriteLine("2. ADMINMENU ");
                Console.WriteLine("0. EXIT");
                Console.Write("SELECT OPTION :");
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1": UserMenu(); break;
                    case "2": AdminMenu(); break; 
                    case "0": processing = false; break; // so that the user can exit the program
                    default: Console.WriteLine("Invalid choice, please try again."); break; // i have to avoid the infinite loop run:)
                }
                Console.WriteLine("Thank you for using our service. Goodbye!");
            }

            static void UserMenu()
            {
                bool enteringmenu = true;
                while (enteringmenu)
                {
                    Console.Clear();
                    Console.WriteLine("Welcome to Mini Bank");
                    Console.WriteLine("1. Create Account");
                    Console.WriteLine("2. Deposit Money");
                    Console.WriteLine("3. Withdraw Money");
                    Console.WriteLine("4. Check Balance");
                    Console.WriteLine("5. submit rebiew");
                    Console.WriteLine("0. back to mainmenu");
                    Console.Write("SELECT OPTION :");
                    string choice = Console.ReadLine();
                    switch (choice)
                    {
                        case "1": RequestAccountOpening(); break;
                        case "2": Deposit(); break;
                        case "3": WithDraw(); break;
                        case "4": Checkbalance(); break;
                        case "5": SubmitReview(); break;
                        case "0": enteringmenu = false; break; // so that the user can go back to the main menu
                        default: Console.WriteLine("Invalid choice, please try again."); break; // i have to avoid the infinite loop
                    }
                    Console.WriteLine("welcome to $afe bank");
                }
            }

            static void AdminMenu() 
            {
                bool insertadmin = true;
                while (insertadmin)
                {
                    Console.WriteLine("Welcome to Mini Bank Admin");
                    Console.WriteLine("1. review requests");
                    Console.WriteLine("2. view accounts ");
                    Console.WriteLine("3. view reviews ");
                    Console.WriteLine("4. process requests");
                    Console.WriteLine("0. exit");
                    Console.Write("SELECT OPTION :");
                    string choice = Console.ReadLine();
                    switch (choice)
                    {
                        case "1": ReviewRequest(); break;
                        case "2": ViewAccounts(); break;
                        case "3": ViewReviews(); break;
                        case "4": ProcessRequests(); break;
                        case "0": insertadmin = false; break; // so that the user can go back to the main menu
                        default: Console.WriteLine("Invalid choice, please try again."); break; // i have to avoid the infinite loop run
                    }
                }
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
}