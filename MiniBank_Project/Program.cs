﻿using System;
using System.Collections.Generic;
using System.IO;

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



        /// <summary>
        /// //////////////////////////////////////////////////////////////
        /// </summary>
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

            //var (name, nationalID) = createAccountRequests.Dequeue();
            string request = createAccountRequests.Dequeue();
            string[] parts = request.Split('|');
            string name = parts[0];
            string nationalID = parts[1];

            int newAccountNumber = lastAccountNumber + 1;

            accountNumbers.Add(newAccountNumber);
            accountNames.Add($"{name} ");
            balances.Add(0.0);

            lastAccountNumber = newAccountNumber;

            Console.WriteLine($"Account created for {name} with Account Number: {newAccountNumber}");
        }

        static void Deposit()
        {
            int index = GetAccountIndex();
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
                Console.WriteLine("Deposit successful.");
            }
            catch
            {
                Console.WriteLine("Invalid amount.");
            }
        }

        static void Withdraw()
        {
            int index = GetAccountIndex();
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
            int index = GetAccountIndex();
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
                        string dataLine = $"{accountNumbers[i]},{accountNames[i]},{balances[i]}";
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
                //transactions.Clear();

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
    }
}