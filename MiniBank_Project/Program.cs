namespace MiniBank_Project
{
    internal class Program
    {
        //constants
        const double MINIMUM_BALANCE = 100.0;
        const string accountsFilePath = "accounts.txt";
        const string reviewsFilePath = "reviews.txt";
        // global list
        static List<int> accountnumbers = new List<int>();
        static List<string> accountnames = new List<string>();
        static List<double> accountbalances = new List<double>();
        static Queue<string> RequestAccountOpeningQueue = new Queue<string>(); // Renamed to avoid conflict with method name
        static Stack<string> reviews = new Stack<string>();// for reviews
        //account number generator 
        static int lastaccountnumber;//so i can add for deposit and substract for the withdraw...

        static void Main()
        {
            SaveAccountsinformationfile();
            ReviewAccountinformationfile();

            // Load account information from the file
            bool processing = true;
            while (processing)
            {
                Console.Clear();
                Console.WriteLine("=========================$$$WELCOME TO CODELINE-$AFE-BANK$$$=============================");
                Console.WriteLine("1. USERMENU ");
                Console.WriteLine("2. ADMINMENU ");
                Console.WriteLine("0. EXIT");
                Console.Write("SELECT OPTION :");
                string? choice = Console.ReadLine();
                switch (choice)
                {
                    case "1": UserMenu(); break;
                    case "2": AdminMenu(); break;
                    case "0": processing = false; break;
                    default: Console.WriteLine("Invalid choice, please try again."); break;
                }
                Console.WriteLine("Thank you for using CODELINE $AFE-BANK services ");
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
                        case "0": enteringmenu = false; break;
                        default: Console.WriteLine("Invalid choice, please try again."); break;
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
                    string? choice = Console.ReadLine();
                    switch (choice)
                    {
                        case "1": ReviewRequest(); break;
                        case "2": ViewAccounts(); break;
                        case "3": ViewReviews(); break;
                        case "4": ProcessRequests(); break;
                        case "0": insertadmin = false; break;
                        default: Console.WriteLine("Invalid choice, please try again."); break;
                    }
                }
            }

            static void RequestAccountOpening()
            {
                Console.WriteLine("Enter your name:");
                string? name = Console.ReadLine();
                Console.WriteLine("National ID ");
                string? nationalID = Console.ReadLine();
                RequestAccountOpeningQueue.Enqueue(name + " === " + nationalID); // Fixed method name and used the renamed queue
            }

            static void Deposit()
            { //we need account number ...
                Console.WriteLine("enter your account number :");
                int accountnumber = Convert.ToInt32(Console.ReadLine());
                if (accountnumbers.Contains(accountnumber))
                {
                    Console.WriteLine("enter the amount you want to deposit :");
                    double depositamount = Convert.ToDouble(Console.ReadLine());
                    // Find the index of the account number in the list
                    int index = accountnumbers.IndexOf(accountnumber);
                    // Add the deposit amount to the account balance at the found index
                    accountbalances[index] += depositamount;
                    // Display a success message with the updated balance
                    Console.WriteLine("Deposit successful. New balance: " + accountbalances[index]);
                }
                else
                {
                    Console.WriteLine("wrong account number inserted ..."); //if false ...
                }

            }
            static void WithDraw() //if i want withdraw my moneyyy

            {
                Console.WriteLine("enter the amount you want to withdraw  :");
                double withdrawamount = Convert.ToDouble(Console.ReadLine());
                int index = accountnumbers.IndexOf(lastaccountnumber);
                if (accountbalances[index] - withdrawamount >= MINIMUM_BALANCE)//so i create a limit for ittt ">="
                {
                    accountbalances[index] -= withdrawamount;
                    Console.WriteLine("withdraw successful. New balance: " + accountbalances[index]);
                }
                else
                {
                    Console.WriteLine(" =( i guess you're poor ...");
                }

            }
            static void Checkbalance()
            {
                Console.WriteLine("enter your account number :");
                int accountnumber = Convert.ToInt32(Console.ReadLine());
                if (accountnumbers.Contains(accountnumber))
                {
                    // Find the index of the account number in the list
                    int index = accountnumbers.IndexOf(accountnumber);
                    // Display the balance at the found index
                    Console.WriteLine("Your balance is: " + accountbalances[index]);
                }
                else
                {
                    Console.WriteLine("wrong account number inserted ..."); //if false ...
                }

            }
            static void SaveAccountsinformationfile()
            {
                // Save account information to a file
                using (StreamWriter writer = new StreamWriter(accountsFilePath))
                {
                    for (int i = 0; i < accountnumbers.Count; i++)
                    {
                        writer.WriteLine($"{accountnumbers[i]},{accountnames[i]},{accountbalances[i]}");
                    }
                }
            }
            static void ReviewAccountinformationfile() //stream reader 
            {
                // Read account information from a file
                if (File.Exists(accountsFilePath))
                {
                    using (StreamReader reader = new StreamReader(accountsFilePath))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            string[] parts = line.Split(',');
                            if (parts.Length == 3)
                            {
                                accountnumbers.Add(int.Parse(parts[0]));
                                accountnames.Add(parts[1]);
                                accountbalances.Add(double.Parse(parts[2]));
                            }
                        }
                    }
                }
            }
            { 

            }
            static void SubmitReview() // i will submit my review wil use stack..
            {
                Console.WriteLine("Enter your review:");
                string? review = Console.ReadLine();
                reviews.Push(review);//push the last in the stack ((out))
                Console.WriteLine("Review submitted successfully.");
            }
            {

            }
            static void ReviewRequest() //will be using queue.. 
            {
                // Check if there are any account opening requests in the queue
                if (RequestAccountOpeningQueue.Count > 0) // Count must be in capital "Count"
                {
                    Console.WriteLine("Reviewing account opening requests:");

                    // Iterate through all requests in the queue and display them
                    foreach (var request in RequestAccountOpeningQueue) // Queue<string> supports enumeration
                    {
                        // Display each request
                        Console.WriteLine(request);
                    }

                    // Dequeue the first request after reviewing
                    string reviewedRequest = RequestAccountOpeningQueue.Dequeue();
                    Console.WriteLine("Reviewed request: " + reviewedRequest);
                }
                else
                {
                    // If no requests are present, display a message
                    Console.WriteLine("No requests to review.");
                }
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