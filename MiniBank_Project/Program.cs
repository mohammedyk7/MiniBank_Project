﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography; //for hashing passwords
using System.Text; //for StringBuilder
using System.Linq;


namespace MiniProjectExplanation
{
    class Program
    {
        // Constants
        const double MinimumBalance = 100.0;
        const string AccountsFilePath = "accounts.txt"; // File to store account information
        const string ReviewsFilePath = "reviews.txt"; // File to store reviews and complaints
        const string AdminID = "admin"; // Admin ID for login
        const string AdminPassword = "1234"; // Admin password for login


        // Global lists (parallel)
        static List<int> accountNumbers = new List<int>();
        static List<string> accountNames = new List<string>();
        static List<double> balances = new List<double>();
        //static List<Queue<string>> transactions = new List<Queue<string>>();
        // Queues and Stacks
        //static Queue<(string name, string nationalID)> createAccountRequests = new Queue<(string, string)>();
        static Queue<string> createAccountRequests = new Queue<string>(); // format: "Name|NationalID" //for pending requestsss
        static Stack<string> reviewsStack = new Stack<string>(); // Stack to store reviews and complaints
        static List<string> passwordHashes = new List<string>();// List to store hashed passwords
        static List<string> nationalIDs = new List<string>();// List to store National IDs
        static string HashPassword(string password)
        //to improve security no one can see the raw password
        // takes a user's raw password as input and returns a hashed string
        {
            using (SHA256 sha256 = SHA256.Create()) //to convert password to hash
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));  //256 bits character 
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                    builder.Append(b.ToString("x2")); //turns something like 11101010 into "ea"
                return builder.ToString();
            }
        }
        static string ReadPassword() //Reads password with hidden input (****).
        {
            StringBuilder password = new StringBuilder();
            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password.Length--;
                    Console.Write("\b \b");
                }
                else if (!char.IsControl(key.KeyChar))
                {
                    password.Append(key.KeyChar);
                    Console.Write("*");
                }
            } while (key.Key != ConsoleKey.Enter);
            Console.WriteLine();
            return password.ToString();
        }
        static List<List<string>> transactions = new List<List<string>>(); // Each string: "yyyy-MM-dd,Type,Amount"
        static List<string> phoneNumbers = new List<string>(); // List to store phone numbers (if needed)
        static List<string> addresses = new List<string>(); // list to store addresses (if needed)
        static List<bool> hasActiveLoan = new List<bool>();
        static List<double> loanAmounts = new List<double>();
        static List<double> loanInterestRates = new List<double>();
        static List<int> feedbackRatings = new List<int>();
        static Queue<(int accountIndex, double amount, double interestRate)> loanRequests = new Queue<(int, double, double)>();
        static Queue<(int accountIndex, DateTime appointmentDate, string purpose)> appointments = new Queue<(int, DateTime, string)>();
        static List<bool> hasAppointment = new List<bool>(); // One per user
        static List<int> failedLoginAttempts = new List<int>();
        static List<bool> isLocked = new List<bool>();







        // Account number generator
        static int lastAccountNumber;

        static void Main()
        {
            LoadAccountsInformationFromFile();
            LoadReviews();

            bool running = true;

            while (running)
            {
                Console.Clear();
                Console.WriteLine("\n====== SafeBank System Main Menu ======");
                Console.WriteLine("1. User Menu");
                Console.WriteLine("2. Admin Menu");
                Console.WriteLine("0. Exit");
                Console.Write("Select option: ");
                string mainChoice = Console.ReadLine();

                switch (mainChoice)
                {
                    case "1": UserMenu(); break;
                    case "2":
                        if (AdminLogin())
                            AdminMenu();
                        break;

                    case "0": 
                        SaveAccountsInformationToFile();
                        SaveReviews();

                        Console.Write("Do you want to create a backup before exit? (Y/N): ");
                        string backupChoice = Console.ReadLine().Trim().ToUpper();
                        if (backupChoice == "Y")
                            BackupAccountsInformationToFile();

                        running = false;
                        break;
                    default: Console.WriteLine("Invalid choice."); break;
                }
            }

            Console.WriteLine("Thank you for using SafeBank!");
            Console.ReadKey();
        }

        // ========== USER MENU ==========
        static void UserMenu()
        {
            bool inUserMenu = true;

            while (inUserMenu)
            {
                Console.Clear();
                Console.WriteLine("\n------ User Menu ------");
                Console.WriteLine("1. Request Account Creation");
                Console.WriteLine("2. Deposit");
                Console.WriteLine("3. Withdraw");
                Console.WriteLine("4. View Balance");
                Console.WriteLine("5. Submit Review/Complaint");
                Console.WriteLine("6. Generate Monthly Statement"); // New feature
                Console.WriteLine("7. Update My Contact Info");
                Console.WriteLine("8. Request a Loan");
                Console.WriteLine("9. View Transaction History"); // new option
                Console.WriteLine("10. View Transaction History");
                Console.WriteLine("11. Book Appointment");
                Console.WriteLine("12. LINQ Tools (Search/Sort)");





                Console.WriteLine("0. Return to Main Menu");
                Console.Write("Select option: ");
                string userChoice = Console.ReadLine();

                switch (userChoice)
                {
                    case "1": RequestAccountCreation(); break;
                    case "2": Deposit(); break;
                    case "3": Withdraw(); break;
                    case "4": ViewBalance(); break;
                    case "5": SubmitReview(); break;
                    case "6": GenerateMonthlyStatement(); break;
                    case "7": UpdateContactInfo(); break;
                    case "8": RequestLoan(); break;
                    case "9": ViewTransactions(); break;
                    case "10": ViewTransactionHistory(); break;
                    case "11": BookAppointment(); break;
                    case "12": LINQTools(); break;







                    case "0": inUserMenu = false; break;
                    default: Console.WriteLine("Invalid choice."); break;
                }
                Console.ReadKey();
            }
        }

        // ========== ADMIN MENU ==========
        static void AdminMenu() 
        {
            bool inAdminMenu = true;

            while (inAdminMenu)
            {
                Console.Clear();
                Console.WriteLine("\n------ Admin Menu ------");
                Console.WriteLine("1. Process Next Account Request");
                Console.WriteLine("2. View Submitted Reviews");
                Console.WriteLine("3. View All Accounts");
                Console.WriteLine("4. View Pending Account Requests");
                Console.WriteLine("5. Process Loan Requests");
                Console.WriteLine("6. View Average User Feedback");
                Console.WriteLine("7. View Appointments");
                Console.WriteLine("8. Unlock Locked Account");


                Console.WriteLine("0. Return to Main Menu");
                Console.Write("Select option: ");
                string adminChoice = Console.ReadLine();

                switch (adminChoice)
                {
                    case "1": ProcessNextAccountRequest(); break;
                    case "2": ViewReviews(); break;
                    case "3": ViewAllAccounts(); break;
                    case "4": ViewPendingRequests(); break;
                    case "5": ProcessLoanRequest(); break; // new feature
                    case "6": ShowAverageFeedback(); break;
                    case "7": ViewAppointments(); break;
                    case "8": UnlockAccount(); break;




                    case "0": inAdminMenu = false; break;
                    default: Console.WriteLine("Invalid choice."); break;
                }
                Console.ReadKey();
            }
        }
        static bool AdminLogin() //admin login function
        {
            Console.Write("Enter Admin ID: "); 
            string id = Console.ReadLine();
            Console.Write("Enter Admin Password: ");
            string password = ReadPassword(); //read password with hidden input

            if (id == AdminID && password == AdminPassword)
            {
                return true;
            }
            else
            {
                Console.WriteLine("Access denied. Invalid credentials.");
                return false;
            }
        }
        static void LINQTools()
        {
            //firsr we get the data source 
            Console.Clear();
            Console.WriteLine("\n--- LINQ Tools ---");
            Console.WriteLine("1. Sort accounts by balance (descending)");
            Console.WriteLine("2. Search accounts by name");
            Console.WriteLine("3. Show accounts with balance > X");
            Console.Write("Choose option: ");
            string choice = Console.ReadLine();

            if (choice == "1")
            {
                //query creation 
                var sorted = accountNumbers 
                    .Select((acc, i) => new { acc, name = accountNames[i], balance = balances[i] }) //acc=account number 101,102,, , i=current element 0,1,2
                    .OrderByDescending(x => x.balance); //if we want ascending order we can use OrderBy instead of OrderByDescending

                foreach (var x in sorted) //x is an anonymous type with acc, name, and balance
                    Console.WriteLine($"Acc#: {x.acc}, Name: {x.name}, Balance: {x.balance:F2}");
            }
            else if (choice == "2")
            {
                Console.Write("Enter name to search: ");
                string searchName = Console.ReadLine(); //declare a variable to hold the search name

                var matchingAccounts = accountNumbers //mathcing accounts by name
                    .Select((acc, i) => new { acc, name = accountNames[i], balance = balances[i] })
                    .Where(x => x.name.Contains(searchName, StringComparison.OrdinalIgnoreCase));

                if (!matchingAccounts.Any())
                {
                    Console.WriteLine(" No accounts found matching the given name.");
                }
                else
                {
                    foreach (var x in matchingAccounts)
                        Console.WriteLine($"Acc#: {x.acc}, Name: {x.name}, Balance: {x.balance:F2}");
                }
            }
            else if (choice == "3")
            {
                Console.Write("Enter minimum balance: ");
                if (double.TryParse(Console.ReadLine(), out double min))
                {
                    var richAccounts = balances
                        .Select((b, i) => new { acc = accountNumbers[i], name = accountNames[i], balance = b })
                        .Where(x => x.balance > min)
                        .ToList(); //  Convert to list so we can check the count

                    if (richAccounts.Count == 0)
                    {
                        Console.WriteLine(" No accounts found with balance above the given amount.");
                    }
                    else
                    {
                        foreach (var x in richAccounts)
                            Console.WriteLine($"Acc#: {x.acc}, Name: {x.name}, Balance: {x.balance:F2}"); // account number, name, and balance
                    }
                }
                else
                {
                    Console.WriteLine("Invalid amount.");
                }
            }
            else
            {
                Console.WriteLine("Invalid choice.");
            }
        }

        static void UnlockAccount()
        {
            Console.Write("Enter account number to unlock: ");
            if (int.TryParse(Console.ReadLine(), out int accNum)) // use int.TryParse for safe conversion 
            {
                int index = accountNumbers.IndexOf(accNum); // find index of account number
                if (index == -1)
                {
                    Console.WriteLine("Account not found.");
                    return;
                }

                isLocked[index] = false; // unlock the account
                failedLoginAttempts[index] = 0; // reset failed attempts
                Console.WriteLine($" Account {accNum} has been unlocked.");
            }
            else
            {
                Console.WriteLine("Invalid input.");
            }
        }


        static void SubmitFeedback()
        {
            Console.Write("Rate our service (1 to 5): "); // prompt user for feedback rating
            if (int.TryParse(Console.ReadLine(), out int rating) && rating >= 1 && rating <= 5) // check if input is a valid integer between 1 and 5
            {
                feedbackRatings.Add(rating); // add rating to the list
                Console.WriteLine(" Thank you for your feedback!");
            }
            else
            {
                Console.WriteLine(" Invalid rating. Must be between 1 and 5."); 
            }
        }

        static void ShowAverageFeedback()
        {
            if (feedbackRatings.Count == 0) // 
            {
                Console.WriteLine("No feedback ratings yet.");
                return;
            }

            double average = feedbackRatings.Average();
            Console.WriteLine($" Average User Rating: {average:F2} / 5");
        }

        static void BackupAccountsInformationToFile()
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HHmm");
            string backupFileName = $"Backup_{timestamp}.txt";

            try
            {
                using (StreamWriter writer = new StreamWriter(backupFileName))
                {
                    for (int i = 0; i < accountNumbers.Count; i++)
                    {
                        string safeName = accountNames[i].Replace(',', ' ');
                        string safePhone = phoneNumbers[i].Replace(',', ' ');
                        string safeAddress = addresses[i].Replace(',', ' ');
                        string tx = string.Join(";", transactions[i]);

                        string dataLine = $"{accountNumbers[i]},{safeName},{balances[i]},{passwordHashes[i]},{nationalIDs[i]},{safePhone},{safeAddress},{tx},{isLocked[i]},{failedLoginAttempts[i]}";

                    }
                }

                Console.WriteLine($"✅ Backup saved as: {backupFileName}");
            }
            catch
            {
                Console.WriteLine("❌ Error creating backup.");
            }
        }

        static void ViewTransactionHistory()
        {
            int index = Login();
            if (index == -1) return;

            Console.WriteLine($"\nTransaction History for Account {accountNumbers[index]} - {accountNames[index]}");
            Console.WriteLine("Date        | Type            | Amount     | Balance After");

            double runningBalance = 0;

            foreach (var tx in transactions[index])
            {
                var parts = tx.Split(',');
                if (parts.Length >= 3)
                {
                    string date = parts[0];
                    string type = parts[1];
                    if (double.TryParse(parts[2], out double amount))
                    {
                        if (type.ToLower().Contains("withdraw"))
                            runningBalance -= amount;
                        else
                            runningBalance += amount;

                        Console.WriteLine($"{date,-11} | {type,-15} | {amount,10:F2} | {runningBalance,10:F2}");
                    }
                }
            }

            if (transactions[index].Count == 0)
                Console.WriteLine("No transactions recorded.");
        }

        static void BookAppointment()
        {
            int index = Login();
            if (index == -1) return;

            
            if (hasAppointment[index])
            {
                Console.WriteLine(" You already have an active appointment.");
                return;
            }

            Console.Write("Enter appointment date (yyyy-mm-dd): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime date))
            {
                Console.WriteLine("Invalid date format.");
                return;
            }

            Console.Write("Purpose (Loan, Consultation, etc.): ");
            string purpose = Console.ReadLine();

            appointments.Enqueue((index, date, purpose));
            hasAppointment[index] = true;

            Console.WriteLine(" Appointment booked successfully.");
        }

        static void ViewAppointments()
        {
            if (appointments.Count == 0)
            {
                Console.WriteLine("No appointments scheduled.");
                return;
            }

            Console.WriteLine("\n--- Scheduled Appointments ---");
            foreach (var (index, date, purpose) in appointments)
            {
                Console.WriteLine($"{accountNames[index]} ({accountNumbers[index]}) - {purpose} on {date:yyyy-MM-dd}");
            }
        }








        static void RequestAccountCreation()
        {
            Console.Write("Enter your full name: ");
            string name = Console.ReadLine();

            Console.Write("Enter your National ID: ");
            string nationalID = Console.ReadLine();

            string request = name + "|" + nationalID;

            //Queue<string> queue = new Queue<string>();
            //queue.Enqueue(request);

            //createAccountRequests.Enqueue((name, nationalID));
            createAccountRequests.Enqueue(request);

            Console.WriteLine("Your account request has been submitted.");
        }

 
        static void ProcessNextAccountRequest()
        {
            if (createAccountRequests.Count == 0)
            {
                Console.WriteLine("No pending account requests.");
                return;
            }

            string request = createAccountRequests.Dequeue();
            string[] parts = request.Split('|');
            string name = parts[0];
            string nationalID = parts[1];

            int newAccountNumber = lastAccountNumber + 1;

            Console.Write("Set a password for the new account: ");
            string password = ReadPassword();
            string hash = HashPassword(password);

            Console.Write("Enter your phone number: ");
            string phone = Console.ReadLine();

            Console.Write("Enter your address: ");
            string address = Console.ReadLine();

            accountNumbers.Add(newAccountNumber);
            accountNames.Add(name.Trim()); // Remove any leading/trailing spaces
            balances.Add(0.0);
            passwordHashes.Add(hash);
            nationalIDs.Add(nationalID);
            transactions.Add(new List<string>());
            phoneNumbers.Add(phone);
            addresses.Add(address);
            hasAppointment.Add(false);
            hasActiveLoan.Add(false);
            loanAmounts.Add(0);             //  FIXED: Initialize loan amount
            loanInterestRates.Add(0);       //  FIXED: Initialize loan interest rate
            failedLoginAttempts.Add(0);
            isLocked.Add(false);

            lastAccountNumber = newAccountNumber;

            Console.WriteLine($" Account created for {name} with Account Number: {newAccountNumber}");
        }


      


   

        static int Login()
        {
            Console.Write("Enter your National ID: ");
            string inputID = Console.ReadLine();

            int index = nationalIDs.IndexOf(inputID);
            if (index == -1)
            {
                Console.WriteLine("Account not found.");
                return -1;
            }

            //  Fix: Ensure lists are long enough before accessing index
            while (isLocked.Count <= index) isLocked.Add(false);
            while (failedLoginAttempts.Count <= index) failedLoginAttempts.Add(0);
            while (passwordHashes.Count <= index) passwordHashes.Add("");

            if (isLocked[index])
            {
                Console.WriteLine(" Account is locked. Contact admin.");
                return -1;
            }

            Console.Write("Enter your password: ");
            string password = ReadPassword();
            string hash = HashPassword(password);

            if (passwordHashes[index] == hash)
            {
                failedLoginAttempts[index] = 0; // reset on success
                return index;
            }
            else
            {
                failedLoginAttempts[index]++;
                Console.WriteLine(" Incorrect password.");

                if (failedLoginAttempts[index] >= 3)
                {
                    isLocked[index] = true;
                    Console.WriteLine(" Too many failed attempts. Account locked.");
                }

                return -1;
            }
        }





        static void Deposit()
        {
            int index = Login();
            if (index == -1) return;

            Console.WriteLine("Select currency: ");
            Console.WriteLine("1. OMR (default)");
            Console.WriteLine("2. USD");
            Console.WriteLine("3. EUR");
            Console.Write("Choice: ");
            string currencyChoice = Console.ReadLine();

            double exchangeRate = 1.0;
            string currency = "OMR";

            switch (currencyChoice)
            {
                case "2":
                    exchangeRate = 3.8;
                    currency = "USD";
                    break;
                case "3":
                    exchangeRate = 4.1;
                    currency = "EUR";
                    break;
            }

            try
            {
                Console.Write($"Enter deposit amount ({currency}): ");
                double foreignAmount = Convert.ToDouble(Console.ReadLine());

                if (foreignAmount <= 0)
                {
                    Console.WriteLine("Amount must be positive.");
                    return;
                }

                double convertedAmount = foreignAmount * exchangeRate;
                balances[index] += convertedAmount;

                string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                string record = $"[{timestamp}] Deposit - {foreignAmount:F2} {currency} => {convertedAmount:F2} OMR | New Balance: {balances[index]:F2}";

                transactions[index].Add(record);

                Console.WriteLine($" Deposit successful. {foreignAmount:F2} {currency} = {convertedAmount:F2} OMR.");
            }
            catch
            {
                Console.WriteLine("Invalid amount.");
            }
        }



        static void Withdraw()
        {
            int index = Login();
            if (index == -1) return;

            try
            {
                Console.Write("Enter withdrawal amount: ");
                double amount = Convert.ToDouble(Console.ReadLine());

                if (amount <= 0)
                {
                    Console.WriteLine("Amount must be positive.");
                    return;
                }

                if (balances[index] - amount >= MinimumBalance)
                {
                    balances[index] -= amount;
                    string record = $"{DateTime.Now:yyyy-MM-dd},Withdraw,{amount}";
                    transactions[index].Add(record);
                    Console.WriteLine("Withdrawal successful.");
                }
                else
                {
                    Console.WriteLine("Insufficient balance after minimum limit.");
                }
            }
            catch
            {
                Console.WriteLine("Invalid amount.");
            }
            SubmitFeedback();

        }

        static void ViewBalance()
        {
            int index = Login();
            if (index == -1) return;

            Console.WriteLine($"Account Number: {accountNumbers[index]}");
            Console.WriteLine($"Holder Name: {accountNames[index]}");
            Console.WriteLine($"Current Balance: {balances[index]}");
        }
        
        static void SaveAccountsInformationToFile()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(AccountsFilePath))
                {
                    for (int i = 0; i < accountNumbers.Count; i++)
                    {
                        string safeName = accountNames[i].Replace(',', ' ');
                        string safePhone = phoneNumbers[i].Replace(',', ' ');
                        string safeAddress = addresses[i].Replace(',', ' ');

                        string tx = string.Join(";", transactions[i]);

                        string dataLine = $"{accountNumbers[i]},{safeName},{balances[i]},{passwordHashes[i]},{nationalIDs[i]},{safePhone},{safeAddress},{tx},{hasAppointment[i]},{hasActiveLoan[i]},{loanAmounts[i]},{loanInterestRates[i]},{isLocked[i]},{failedLoginAttempts[i]}";
                        writer.WriteLine(dataLine);

                    }
                }
                Console.WriteLine("Accounts saved successfully.");
            }
            catch
            {
                Console.WriteLine("Error saving file.");
            }
        }



  
  

        static void LoadAccountsInformationFromFile()
        {
            try
            {
                if (!File.Exists(AccountsFilePath))
                {
                    Console.WriteLine("No saved data found.");
                    return;
                }

                // Clear all lists before loading
                accountNumbers.Clear();
                accountNames.Clear();
                balances.Clear();
                passwordHashes.Clear();
                nationalIDs.Clear();
                transactions.Clear();
                phoneNumbers.Clear();
                addresses.Clear();
                hasAppointment.Clear();
                hasActiveLoan.Clear();
                loanAmounts.Clear();
                loanInterestRates.Clear();
                isLocked.Clear();
                failedLoginAttempts.Clear();

                using (StreamReader reader = new StreamReader(AccountsFilePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] parts = line.Split(',');

                        int accNum = Convert.ToInt32(parts[0]);
                        accountNumbers.Add(accNum);
                        accountNames.Add(parts[1]);
                        balances.Add(Convert.ToDouble(parts[2]));
                        passwordHashes.Add(parts[3]);
                        nationalIDs.Add(parts[4]);
                        phoneNumbers.Add(parts[5]);
                        addresses.Add(parts[6]);

                        // Load transactions (semicolon-separated)
                        List<string> txList = new List<string>();
                        if (!string.IsNullOrWhiteSpace(parts[7]))
                            txList.AddRange(parts[7].Split(';'));
                        transactions.Add(txList);

                        // Load extra fields with safety checks
                        hasAppointment.Add(parts.Length > 8 ? bool.Parse(parts[8]) : false);
                        hasActiveLoan.Add(parts.Length > 9 ? bool.Parse(parts[9]) : false);
                        loanAmounts.Add(parts.Length > 10 ? double.Parse(parts[10]) : 0);
                        loanInterestRates.Add(parts.Length > 11 ? double.Parse(parts[11]) : 0);
                        isLocked.Add(parts.Length > 12 ? bool.Parse(parts[12]) : false);
                        failedLoginAttempts.Add(parts.Length > 13 ? int.Parse(parts[13]) : 0);

                        // Update lastAccountNumber
                        if (accNum > lastAccountNumber)
                            lastAccountNumber = accNum;
                    }
                }

                Console.WriteLine("Accounts loaded successfully.");
            }
            catch
            {
                Console.WriteLine("❌ Error loading account file.");
            }
        }





        // ===== Monthly Statement Generation =====
        static void GenerateMonthlyStatement()
        {
            int index = Login(); //fix: ensure index is valid
            if (index == -1) return;

            Console.Write("Enter year (e.g., 2025): ");
            if (!int.TryParse(Console.ReadLine(), out int year)) return;
            Console.Write("Enter month (1-12): ");
            if (!int.TryParse(Console.ReadLine(), out int month)) return;

            var txs = transactions[index] //ensure transactions are valid
                .Select(tx => tx.Split(','))
                .Where(parts => DateTime.TryParse(parts[0], out DateTime dt) && dt.Year == year && dt.Month == month)
                .ToList();

            if (txs.Count == 0)
            {
                Console.WriteLine("No transactions for this period.");
                return;
            }

            string filename = $"Statement_Acc{accountNumbers[index]}_{year:D4}-{month:D2}.txt";
            using (StreamWriter writer = new StreamWriter(filename))
            {
                writer.WriteLine($"Statement for Account: {accountNumbers[index]} ({accountNames[index]})");
                writer.WriteLine($"Period: {year}-{month:D2}");
                writer.WriteLine("Date        | Type     | Amount"); //design the header
                writer.WriteLine("-------------------------------");
                foreach (var tx in txs)
                    writer.WriteLine($"{tx[0],-11} | {tx[1],-8} | {tx[2]}");
            }
            Console.WriteLine($"Statement saved to {filename}");
        }

        static void UpdateContactInfo()
        {
            int index = Login();
            if (index == -1) return;

            Console.WriteLine($"\nCurrent phone: {phoneNumbers[index]}");
            Console.Write("Enter new phone number (leave blank to keep current): ");
            string phone = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(phone))
                phoneNumbers[index] = phone;

            Console.WriteLine($"Current address: {addresses[index]}");
            Console.Write("Enter new address (leave blank to keep current): ");
            string address = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(address))
                addresses[index] = address;

            Console.WriteLine(" Your contact info has been updated.");
        }

       
        static void RequestLoan()
        {
            int index = Login();
            if (index == -1) return;

            // Fix: ensure lists are safe to access
            while (hasActiveLoan.Count <= index) hasActiveLoan.Add(false);
            while (balances.Count <= index) balances.Add(0);
            while (loanAmounts.Count <= index) loanAmounts.Add(0);
            while (loanInterestRates.Count <= index) loanInterestRates.Add(0);

            if (hasActiveLoan[index]) 
            {
                Console.WriteLine(" You already have an active loan.");
                return;
            }

            if (balances[index] < 5000)
            {
                Console.WriteLine(" You must have at least 5000 balance to request a loan.");
                return;
            }

            Console.Write("Enter loan amount: ");
            if (!double.TryParse(Console.ReadLine(), out double amount) || amount <= 0)
            {
                Console.WriteLine("Invalid amount.");
                return;
            }

            Console.Write("Enter interest rate (e.g., 5 for 5%): ");
            if (!double.TryParse(Console.ReadLine(), out double rate) || rate < 0)
            {
                Console.WriteLine("Invalid rate.");
                return;
            }

            loanRequests.Enqueue((index, amount, rate));
            Console.WriteLine(" Loan request submitted. Awaiting admin approval."); //
        }



        static void ViewAllAccounts()
        {
            Console.WriteLine("\n--- All Accounts ---");
            for (int i = 0; i < accountNumbers.Count; i++)
            {
                Console.WriteLine($"{accountNumbers[i]} - {accountNames[i]} - Balance: {balances[i]}");
            }
        }

        static void ViewPendingRequests()
        {
            Console.WriteLine("\n--- Pending Account Requests ---");
            if (createAccountRequests.Count == 0)
            {
                Console.WriteLine("No pending requests.");
                return;
            }

            foreach (string request in createAccountRequests)
            {
                string[] parts = request.Split('|');
                Console.WriteLine($"Name: {parts[0]}, National ID: {parts[1]}");
            }
        }

        static int GetAccountIndex()
        {
            Console.Write("Enter account number: ");
            try
            {
                int accNum = Convert.ToInt32(Console.ReadLine());
                int index = accountNumbers.IndexOf(accNum);

                if (index == -1)
                {
                    Console.WriteLine("Account not found.");
                    return -1;
                }

                return index;
            }
            catch
            {
                Console.WriteLine("Invalid input.");
                return -1;
            }
        }

        // ===== Reviews & Complaints =====

        static void SubmitReview()
        {
            Console.Write("Enter your review or complaint: ");
            string review = Console.ReadLine();
            reviewsStack.Push(review);
            Console.WriteLine("Thank you! Your feedback has been recorded.");
        }

        static void ViewReviews()
        {
            if (reviewsStack.Count == 0)
            {
                Console.WriteLine("No reviews or complaints submitted yet.");
                return;
            }

            Console.WriteLine("Recent Reviews/Complaints (most recent first):");
            foreach (string r in reviewsStack)
            {
                Console.WriteLine("- " + r);
            }
        }

        static void SaveReviews()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(ReviewsFilePath))
                {
                    foreach (var review in reviewsStack)
                    {
                        writer.WriteLine(review);
                    }
                }
            }
            catch
            {
                Console.WriteLine("Error saving reviews.");
            }
        }

        static void ProcessLoanRequest()
        {
            if (loanRequests.Count == 0)
            {
                Console.WriteLine("No pending loan requests.");
                return;
            }

            var (index, amount, rate) = loanRequests.Dequeue();

            Console.WriteLine($"Loan Request:");
            Console.WriteLine($"Account: {accountNumbers[index]} - {accountNames[index]}");
            Console.WriteLine($"Requested Amount: {amount}");
            Console.WriteLine($"Interest Rate: {rate}%");

            Console.Write("Approve loan? (Y/N): ");
            string choice = Console.ReadLine().Trim().ToUpper();

            if (choice == "Y")
            {
                balances[index] += amount;
                hasActiveLoan[index] = true;
                loanAmounts[index] = amount;
                loanInterestRates[index] = rate;

                string record = $"{DateTime.Now:yyyy-MM-dd},Loan Approved,{amount}";
                transactions[index].Add(record);

                Console.WriteLine(" Loan approved and amount credited.");
            }
            else
            {
                Console.WriteLine(" Loan request rejected.");
            }
        }




        static void LoadReviews()
        {
            try
            {
                if (!File.Exists(ReviewsFilePath)) return;

                using (StreamReader reader = new StreamReader(ReviewsFilePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        reviewsStack.Push(line);
                    }
                }
            }
            catch
            {
                Console.WriteLine("Error loading reviews.");
            }
        }
        static void ViewTransactions()
        {
            int index = Login();
            if (index == -1) return;

            Console.WriteLine("\n--- View Transactions ---");
            Console.WriteLine("1. Show last N transactions");
            Console.WriteLine("2. Show transactions after specific date");
            Console.Write("Choose option: ");
            string option = Console.ReadLine();

            if (option == "1")
            {
                Console.Write("Enter N (number of transactions): ");
                if (int.TryParse(Console.ReadLine(), out int n) && n > 0)
                {
                    var recent = transactions[index].TakeLast(n);
                    foreach (var tx in recent)
                        Console.WriteLine(tx);
                }
                else
                {
                    Console.WriteLine("Invalid number.");
                }
            }
            else if (option == "2")
            {
                Console.Write("Enter date (yyyy-MM-dd): ");
                string input = Console.ReadLine();
                if (DateTime.TryParse(input, out DateTime date))
                {
                    var filtered = transactions[index]
                        .Where(tx => DateTime.TryParse(tx.Split(',')[0], out DateTime txDate) && txDate > date);
                    foreach (var tx in filtered)
                        Console.WriteLine(tx);
                }
                else
                {
                    Console.WriteLine("Invalid date.");
                }
            }
            else
            {
                Console.WriteLine("Invalid option.");
            }
        }


    }
}