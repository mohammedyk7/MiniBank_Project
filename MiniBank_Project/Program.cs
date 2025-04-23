namespace MiniBank_Project
{
    internal class Program
    {
        static void Main(string[] args)
        {
            
        }

        static void UserMenu()
        {
            Console.WriteLine("Welcome to Mini Bank");
            Console.WriteLine("1. Create Account");
            Console.WriteLine("2. Deposit Money");
            Console.WriteLine("3. Withdraw Money");
            Console.WriteLine("4. Check Balance");
            Console.WriteLine("5. Exit");
        }
        static void AdminMenu()
        {
            Console.WriteLine("Welcome to Mini Bank Admin");
            Console.WriteLine("1. View All Accounts");
            Console.WriteLine("2. Delete Account");
            Console.WriteLine("3. Exit");
        }
    }
}
