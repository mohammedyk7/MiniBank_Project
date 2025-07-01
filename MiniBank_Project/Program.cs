﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace MiniProjectExplanation
{
    class Program
    {
        // Constants
        const double MinimumBalance = 100.0;
        const string AccountsFilePath = "accounts.txt";
        const string ReviewsFilePath = "reviews.txt";

        // Global lists (parallel)
        static List<int> accountNumbers = new List<int>();
        static List<string> accountNames = new List<string>();
        static List<double> balances = new List<double>();
        //static List<Queue<string>> transactions = new List<Queue<string>>();
        // Queues and Stacks
        //static Queue<(string name, string nationalID)> createAccountRequests = new Queue<(string, string)>();
        static Queue<string> createAccountRequests = new Queue<string>(); // format: "Name|NationalID"
        static Stack<string> reviewsStack = new Stack<string>();
        static List<string> passwordHashes = new List<string>();
        static List<string> nationalIDs = new List<string>();
        static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                    builder.Append(b.ToString("x2"));
                return builder.ToString();
            }
        }
        static string ReadPassword()
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
                    case "2": AdminMenu(); break;
                    case "0":
                        SaveAccountsInformationToFile();
                        SaveReviews();
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
                Console.WriteLine("0. Return to Main Menu");
                Console.Write("Select option: ");
                string adminChoice = Console.ReadLine();

                switch (adminChoice)
                {
                    case "1": ProcessNextAccountRequest(); break;
                    case "2": ViewReviews(); break;
                    case "3": ViewAllAccounts(); break;
                    case "4": ViewPendingRequests(); break;
                    case "0": inAdminMenu = false; break;
                    default: Console.WriteLine("Invalid choice."); break;
                }
                Console.ReadKey();
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

            accountNumbers.Add(newAccountNumber);
            accountNames.Add(name.Trim()); // Remove any leading spaces
            balances.Add(0.0);
            passwordHashes.Add(hash);
            nationalIDs.Add(nationalID);
            transactions.Add(new List<string>());

            lastAccountNumber = newAccountNumber;

            Console.WriteLine($"Account created for {name} with Account Number: {newAccountNumber}");
        }


        static void Deposit()
        {
            int index = Login();
            if (index == -1) return;

            try
            {
                Console.Write("Enter deposit amount: ");
                double amount = Convert.ToDouble(Console.ReadLine());

                if (amount <= 0)
                {
                    Console.WriteLine("Amount must be positive.");
                    return;
                }

                balances[index] += amount;
                string record = $"{DateTime.Now:yyyy-MM-dd},Deposit,{amount}";
                transactions[index].Add(record);
                Console.WriteLine("Deposit successful.");
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
                      
                        string tx = string.Join(";", transactions[i]); //join transaction 
                        // Save all fields, separated by comma
                        string dataLine = $"{accountNumbers[i]},{accountNames[i]},{balances[i]},{passwordHashes[i]},{nationalIDs[i]}";
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

                accountNumbers.Clear();
                accountNames.Clear();
                balances.Clear();
                passwordHashes.Clear();
                nationalIDs.Clear();
                transactions.Clear();

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
                        var txList = new List<string>();
                        if (parts.Length > 5 && !string.IsNullOrEmpty(parts[5]))
                            txList.AddRange(parts[5].Split(';'));


                        if (accNum > lastAccountNumber)
                            lastAccountNumber = accNum;
                    }
                }

                Console.WriteLine("Accounts loaded successfully.");
            }
            catch
            {
                Console.WriteLine("Error loading file.");
            }
        }
        // ===== Monthly Statement Generation =====
        static void GenerateMonthlyStatement()
        {
            int index = Login();
            if (index == -1) return;

            Console.Write("Enter year (e.g., 2025): ");
            if (!int.TryParse(Console.ReadLine(), out int year)) return;
            Console.Write("Enter month (1-12): ");
            if (!int.TryParse(Console.ReadLine(), out int month)) return;

            var txs = transactions[index]
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
                writer.WriteLine("Date        | Type     | Amount");
                writer.WriteLine("-------------------------------");
                foreach (var tx in txs)
                    writer.WriteLine($"{tx[0],-11} | {tx[1],-8} | {tx[2]}");
            }
            Console.WriteLine($"Statement saved to {filename}");
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

        // ===== Reviews & Complaints (Stack) =====

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

            Console.Write("Enter your password: ");
            string password = ReadPassword();
            string hash = HashPassword(password);

            if (passwordHashes[index] == hash)
            {
                return index;
            }
            else
            {
                Console.WriteLine("Incorrect password.");
                return -1;
            }
        }
    }
}