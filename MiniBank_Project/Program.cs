namespace MiniBank_Project
{
    internal class Program
    {
        // Constants
        const double MINIMUM_BALANCE = 100.0; // Minimum balance required in an account
        const string accountsFilePath = "accounts.txt"; // File path to store account information

        // Global Variables
        static int lastaccountnumber = 0; // Tracks the last assigned account number

        // Lists to store account details
        static List<int> accountnumbers = new List<int>(); // Stores account numbers
        static List<string> accountnames = new List<string>(); // Stores account holder names
        static List<double> accountbalances = new List<double>(); // Stores account balances
        static List<string> transactionHistory = new List<string>(); // Stores transaction history


        // Queue for account opening requests and stack for reviews
        static Queue<string> RequestAccountOpeningQueue = new Queue<string>(); // Stores account opening requests
        static Stack<string> reviews = new Stack<string>(); // Stores user reviews

        static void Main()
        {
            try
            {
                // Load account information from the file
                ReviewAccountinformationfile();
                SaveAccountsinformationfile();

                bool processing = true;
                while (processing)
                {
                    // Display the main menu
                    Console.Clear();
                    Console.WriteLine("=========================$$$WELCOME TO CODELINE-$AFE-BANK$$$=============================");
                    Console.WriteLine("1. USERMENU ");
                    Console.WriteLine("2. ADMINMENU ");
                    Console.WriteLine("0. EXIT");
                    Console.Write("SELECT OPTION :");
                    Console.WriteLine("\n$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$");
                    string? choice = Console.ReadLine();

                    // Handle user choice
                    switch (choice)
                    {
                        case "1": UserMenu(); break; // Navigate to User Menu
                        case "2": AdminMenu(); break; // Navigate to Admin Menu
                        case "0": processing = false; break; // Exit the application
                        default: Console.WriteLine("Invalid choice, please try again."); break;
                    }
                    Console.WriteLine("Thank you for using CODELINE $AFE-BANK services ");
                }
            }
            catch (Exception ex)
            {
                // Handle unexpected errors
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }

        // Displays the User Menu and handles user actions
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
                    Console.WriteLine("6. View Transaction History");
                    Console.WriteLine("0. Back to Main Menu");
                    Console.Write("SELECT OPTION :");
                    string choice = Console.ReadLine();

                    // Handle user choice
                    switch (choice)
                    {
                        case "1": RequestAccountOpening(); break; // Request to open a new account
                        case "2": Deposit(); break; // Deposit money into an account
                        case "3": WithDraw(); break; // Withdraw money from an account
                        case "4": Checkbalance(); break; // Check account balance
                        case "5": SubmitReview(); break; // Submit a review
                        case "6": transactionHistory(); break; // View transaction history
                        case "0": enteringmenu = false; break; // Return to main menu
                        default: Console.WriteLine("Invalid choice, please try again."); break;
                    }
                    Console.WriteLine("Welcome to $afe Bank");
                }
            }
            catch (Exception ex)
            {
                // Handle errors in the User Menu
                Console.WriteLine($"An error occurred in the User Menu: {ex.Message}");
            }
        }

        // Displays the Admin Menu and handles admin actions
        static void AdminMenu()
        {
            try
            {
                bool insertadmin = true;//so that it keeps running....
                while (insertadmin)//while the program is running ...
                {
                    Console.WriteLine("Welcome to Mini Bank Admin");
                    Console.WriteLine("1. Review Requests");
                    Console.WriteLine("2. View Accounts");
                    Console.WriteLine("3. View Reviews");
                    Console.WriteLine("4. Process Requests");
                    Console.WriteLine("0. Exit");
                    Console.Write("SELECT OPTION :");
                    string? choice = Console.ReadLine();

                    // Handle admin choice
                    switch (choice)
                    {
                        case "1": ReviewRequest(); break; // Review account opening requests
                        case "2": ViewAccounts(); break; // View all accounts
                        case "3": ViewReviews(); break; // View all reviews
                        case "4": ProcessRequests(); break; // Process account opening requests
                        case "0": insertadmin = false; break; // Exit Admin Menu
                        default: Console.WriteLine("Invalid choice, please try again."); break;
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle errors in the Admin Menu
                Console.WriteLine($"An error occurred in the Admin Menu: {ex.Message}");
            }
        }

        // Handles account opening requests
        static void RequestAccountOpening()
        {
            try
            {
                Console.WriteLine("Enter your name:");
                string? name = Console.ReadLine();
                Console.WriteLine("National ID:");
                string? nationalID = Console.ReadLine();

                // Add the request to the queue
                RequestAccountOpeningQueue.Enqueue(name + " = = = " + nationalID);
            }
            catch (Exception ex)
            {
                // Handle errors during account opening request
                Console.WriteLine($"An error occurred while requesting account opening: {ex.Message}");
            }
        }

        // Handles depositing money into an account
        static void Deposit()
        {
            try
            {
                Console.WriteLine("Enter your account number:");
                int accountnumber = Convert.ToInt32(Console.ReadLine());

                // Check if the account exists
                if (accountnumbers.Contains(accountnumber))// if the account numbers exist in the index....
                {
                    Console.WriteLine("Enter the amount you want to deposit:");
                    double depositamount = Convert.ToDouble(Console.ReadLine());

                    // Update the account balance
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
                // Handle invalid input format
                Console.WriteLine("Invalid input format. Please enter numeric values where required.");
            }
            catch (Exception ex)
            {
                // Handle other errors during deposit
                Console.WriteLine($"An error occurred during deposit: {ex.Message}");
            }
        }

        // Handles withdrawing money from an account
        static void WithDraw()//substracting cash 
        {
            try
            {
                Console.WriteLine("================================Enter---Your---Account---Number:==============================");
                int accountnumber = Convert.ToInt32(Console.ReadLine());

                // Check if the account exists
                if (accountnumbers.Contains(accountnumber))
                {
                    Console.WriteLine("Enter the amount you want to withdraw:");
                    double withdrawamount = Convert.ToDouble(Console.ReadLine());

                    int index = accountnumbers.IndexOf(accountnumber);//[accountnumber:1234]

                    if (accountbalances[index] >= withdrawamount)
                    {
                        accountbalances[index] -= withdrawamount;
                        Console.WriteLine("Withdraw successful. New balance: " + accountbalances[index]);
                    }
                    else
                    {
                        Console.WriteLine("Insufficient balance.");
                    }
                }
                else
                {
                    Console.WriteLine("Wrong account number inserted.");
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input. Please enter numbers only.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something went wrong: " + ex.Message);
            }
        }

        // Handles checking the balance of an account
        static void Checkbalance()
        {
            try
            {
                Console.WriteLine("Enter your account number:");
                int accountnumber = Convert.ToInt32(Console.ReadLine());

                // Check if the account exists
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
                // Handle invalid input format
                Console.WriteLine("Invalid input format. Please enter numeric values where required.");
            }
            catch (Exception ex)
            {
                // Handle other errors during balance check
                Console.WriteLine($"An error occurred while checking balance: {ex.Message}");
            }
        }
        static void transactionHistory() 
        {
            //i need account numbers,names,balances, and transaction history
            // i need deposits
            static void ViewTransactionHistory()
            {
                try
                {
                    if (transactionHistory.Count == 0)
                    {
                        Console.WriteLine("No transactions have been made yet.");
                    }
                    else
                    {
                        Console.WriteLine("Transaction History:");
                        foreach (string transaction in transactionHistory)
                        {
                            Console.WriteLine(transaction);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error while viewing transaction history: " + ex.Message);
                }
            }


        }

        // Saves account information to a file
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
                // Handle errors during file saving
                Console.WriteLine($"An error occurred while saving account information: {ex.Message}");
            }
        }
        

        // Loads account information from a file
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
                // Handle invalid file format
                Console.WriteLine("Error parsing account information. Please check the file format.");
            }
            catch (Exception ex)
            {
                // Handle other errors during file reading
                Console.WriteLine($"An error occurred while reviewing account information: {ex.Message}");
            }
        }

        // Allows users to submit reviews
        static void SubmitReview()
        {
            try
            {
                Console.WriteLine("Enter your review:");
                string? review = Console.ReadLine();

                // Validate and add the review to the stack
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
                // Handle errors during review submission
                Console.WriteLine($"An error occurred while submitting the review: {ex.Message}");
            }
        }

        // Reviews account opening requests
        static void ReviewRequest()//check if the account is not empty 
        {
            try
            {
                if (RequestAccountOpeningQueue.Count > 0)//here im checking the queue that its not empty ..
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
                // Handle errors during request review
                Console.WriteLine($"An error occurred while reviewing requests: {ex.Message}");
            }
        }

        // Displays all accounts
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
                // Handle errors during account viewing
                Console.WriteLine($"An error occurred while viewing accounts: {ex.Message}");
            }
        }

        // Displays all reviews
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
                // Handle errors during review viewing
                Console.WriteLine($"An error occurred while viewing reviews: {ex.Message}");
            }
        }

        // Processes account opening requests
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
                // Handle errors during request processing
                Console.WriteLine($"An error occurred while processing requests: {ex.Message}");
            }
        }
    }
}
