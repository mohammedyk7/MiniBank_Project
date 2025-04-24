namespace MiniBank_Project
{
    internal class Program
    {
        // Constants
        private const double MINIMUM_BALANCE = 100.0;
        private const string accountsFilePath = "accounts.txt";

        // Global Variables
        static int lastaccountnumber = 0;

        // Lists
        private static List<int> accountnumbers = new List<int>();
        private static List<string> accountnames = new List<string>();
        private static List<double> accountbalances = new List<double>();

        // Queues and Stacks
        private static Queue<string> RequestAccountOpeningQueue = new Queue<string>();
        private static Stack<string> reviews = new Stack<string>();

        static void Main()
        {
            try
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
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }

        static void UserMenu()
        {
            try
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
                    Console.WriteLine("5. Submit Review");
                    Console.WriteLine("0. Back to Main Menu");
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
                    Console.WriteLine("Welcome to $afe Bank");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred in the User Menu: {ex.Message}");
            }
        }

        static void AdminMenu()
        {
            try
            {
                bool insertadmin = true;
                while (insertadmin)
                {
                    Console.WriteLine("Welcome to Mini Bank Admin");
                    Console.WriteLine("1. Review Requests");
                    Console.WriteLine("2. View Accounts");
                    Console.WriteLine("3. View Reviews");
                    Console.WriteLine("4. Process Requests");
                    Console.WriteLine("0. Exit");
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
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred in the Admin Menu: {ex.Message}");
            }
        }

        static void RequestAccountOpening()
        {
            try
            {
                Console.WriteLine("Enter your name:");
                string? name = Console.ReadLine();
                Console.WriteLine("National ID:");
                string? nationalID = Console.ReadLine();
                RequestAccountOpeningQueue.Enqueue(name + " === " + nationalID);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while requesting account opening: {ex.Message}");
            }
        }

        static void Deposit()
        {
            try
            {
                Console.WriteLine("Enter your account number:");
                int accountnumber = Convert.ToInt32(Console.ReadLine());
                if (accountnumbers.Contains(accountnumber))
                {
                    Console.WriteLine("Enter the amount you want to deposit:");
                    double depositamount = Convert.ToDouble(Console.ReadLine());
                    int index = accountnumbers.IndexOf(accountnumber);
                    accountbalances[index] += depositamount;
                    Console.WriteLine("Deposit successful. New balance: " + accountbalances[index]);
                }
                else
                {
                    Console.WriteLine("Wrong account number inserted.");
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input format. Please enter numeric values where required.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during deposit: {ex.Message}");
            }
        }

        static void WithDraw()
        {
            try
            {
                Console.WriteLine("Enter the amount you want to withdraw:");
                double withdrawamount = Convert.ToDouble(Console.ReadLine());
                int index = accountnumbers.IndexOf(lastaccountnumber);
                if (index == -1)
                {
                    Console.WriteLine("Account not found.");
                    return;
                }
                if (accountbalances[index] - withdrawamount >= MINIMUM_BALANCE)
                {
                    accountbalances[index] -= withdrawamount;
                    Console.WriteLine("Withdraw successful. New balance: " + accountbalances[index]);
                }
                else
                {
                    Console.WriteLine("Insufficient balance.");
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input format. Please enter numeric values where required.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during withdrawal: {ex.Message}");
            }
        }

        static void Checkbalance()
        {
            try
            {
                Console.WriteLine("Enter your account number:");
                int accountnumber = Convert.ToInt32(Console.ReadLine());
                if (accountnumbers.Contains(accountnumber))
                {
                    int index = accountnumbers.IndexOf(accountnumber);
                    Console.WriteLine("Your balance is: " + accountbalances[index]);
                }
                else
                {
                    Console.WriteLine("Wrong account number inserted.");
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input format. Please enter numeric values where required.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while checking balance: {ex.Message}");
            }
        }

        static void SaveAccountsinformationfile()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(accountsFilePath))
                {
                    for (int i = 0; i < accountnumbers.Count; i++)
                    {
                        writer.WriteLine($"{accountnumbers[i]},{accountnames[i]},{accountbalances[i]}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while saving account information: {ex.Message}");
            }
        }

        static void ReviewAccountinformationfile()
        {
            try
            {
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
            catch (FormatException)
            {
                Console.WriteLine("Error parsing account information. Please check the file format.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while reviewing account information: {ex.Message}");
            }
        }

        static void SubmitReview()
        {
            try
            {
                Console.WriteLine("Enter your review:");
                string? review = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(review))
                {
                    Console.WriteLine("Review cannot be empty.");
                    return;
                }
                reviews.Push(review);
                Console.WriteLine("Review submitted successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while submitting the review: {ex.Message}");
            }
        }

        static void ReviewRequest()
        {
            try
            {
                if (RequestAccountOpeningQueue.Count > 0)
                {
                    Console.WriteLine("Reviewing account opening requests:");
                    foreach (var request in RequestAccountOpeningQueue)
                    {
                        Console.WriteLine(request);
                    }
                    string reviewedRequest = RequestAccountOpeningQueue.Dequeue();
                    Console.WriteLine("Reviewed request: " + reviewedRequest);
                }
                else
                {
                    Console.WriteLine("No requests to review.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while reviewing requests: {ex.Message}");
            }
        }

        static void ViewAccounts()
        {
            try
            {
                Console.WriteLine("==========ALL ACCOUNTS==========");
                for (int i = 0; i < accountnumbers.Count; i++)
                {
                    Console.WriteLine($"Account Number: {accountnumbers[i]}, Name: {accountnames[i]}, Balance: {accountbalances[i]}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while viewing accounts: {ex.Message}");
            }
        }

        static void ViewReviews()
        {
            try
            {
                Console.WriteLine("==========ALL REVIEWS==========");
                foreach (var review in reviews)
                {
                    Console.WriteLine(review);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while viewing reviews: {ex.Message}");
            }
        }

        static void ProcessRequests()
        {
            try
            {
                if (RequestAccountOpeningQueue.Count > 0)
                {
                    string request = RequestAccountOpeningQueue.Dequeue();
                    Console.WriteLine("Processing request: " + request);
                }
                else
                {
                    Console.WriteLine("No requests to process.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while processing requests: {ex.Message}");
            }
        }
    }
}