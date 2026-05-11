using System.Transactions;

namespace NovaPayBanking_System
{
 

    internal class Program
    {
        static Bank bank = new Bank("banksystem");
        public static void DisplayMenu()
        {
            Console.WriteLine("========== NovaPay Banking System ==========");
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("1. Open Savings Account");
            Console.WriteLine("2. Open Current Account");
            Console.WriteLine("3. Open Fixed Deposit Account");

            Console.WriteLine("4. Deposit");
            Console.WriteLine("5. Withdraw");
            Console.WriteLine("6. Print Account Statement");

            Console.WriteLine("7. Apply Interest (Savings Only)");
            Console.WriteLine("8. Bank Summary");
            Console.WriteLine("9.  Exit");

        }
        static void Main(string[] args)
        {
            bool exit = false;

            while (!exit)
            {
                DisplayMenu();

                Console.WriteLine("choose option:");
                int choice;

                try
                {
                    choice = int.Parse((Console.ReadLine() ?? "").Trim());
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                    continue;
                }

                switch (choice)
                {
                    case 1:

                        {
                            Console.Write("Enter owner name: ");
                            string ownerName = Console.ReadLine();

                            SavingsAccount account = new SavingsAccount(ownerName, 0.03);


                            bank.AddAccount(account);

                            Console.WriteLine($"Savings Account created successfully. " +
                                $"Account Number: {account.AccountNumber}");
                        }    
                                
                            break;
     

                    case 2:
                        {
                            Console.Write("Enter owner name: ");
                            string ownerName = Console.ReadLine();

                            Console.Write("Enter overdraft limit: ");
                            double limit = Convert.ToDouble(Console.ReadLine());

                            CurrentAccount account = new CurrentAccount(ownerName, limit);


                            bank.AddAccount(account);

                            Console.WriteLine($"Crurrent Account create succcessfully:"+$"Account Number:{ account.AccountNumber}");
                          
                           
                           }

                            break;
                        

                    case 3:
                  
                        {
                            Console.Write("Enter owner name: ");
                            string ownerName = Console.ReadLine();

                            Console.Write("Enter deposit amount: ");
                            double amount = Convert.ToDouble(Console.ReadLine());

                            FixedDepositAccount account =
                                new FixedDepositAccount(ownerName, amount);

                            bank.AddAccount(account);

                            Console.WriteLine($"Account Number: {account.AccountNumber}" + $"Fixed Deposit Account created successfully. ")
                            }

                            break;

                 
                    case 4:
                        {
                            Console.Write("Enter account number: ");
                            int accNumber = Convert.ToInt32(Console.ReadLine());

                            Console.Write("Enter amount: ");
                            double amount = Convert.ToDouble(Console.ReadLine());

                            bank.Deposit(accNumber, amount);
                        }

                            break;
                        

                    case 5:
                  
                        {
                            Console.Write("Enter account number: ");
                            int accNumber = Convert.ToInt32(Console.ReadLine());

                            Console.Write("Enter amount: ");
                            double amount = Convert.ToDouble(Console.ReadLine());

                            bank.Withdraw(accNumber, amount);

                            break;
                        } 

                    case 6:
                        {
                            Console.Write("Enter account number: ");
                            int accNumber = Convert.ToInt32(Console.ReadLine());

                            bank.PrintStatement(accNumber);

                            break;
                        }

                    case 7:
                        { 

                            Console.Write("Enter account number: ");
                            int accNumber = Convert.ToInt32(Console.ReadLine());

                            bank.ApplyInterest(accNumber);

                            break;
                        }
                        
                    case 8:
                        { 
                            bank.PrintSummary();
                            break;
                        }
                       

                    case 9:
                        exit = true;
                        Console.WriteLine("Exiting system...");
                        break;



                    default:
                        Console.WriteLine("Invalid option");
                        break;
                }

                Console.WriteLine("\nPress key...");
                Console.ReadKey();
                Console.Clear();
            }
        }
    }


}


interface IDepositable
{
    void Deposit(double amount);
}

interface IWithdrawable
{
    void Withdraw(double amount);
}

interface IStatementPrintable
{
    void PrintStatement();
}


//// /////Abstract Base Class/////////////////
abstract class BankAccount :
    IDepositable,
    IWithdrawable,
    IStatementPrintable
{
    private static int nextAccountNumber = 1001;

    protected static int totalTransactionsProcessed;

    protected double balance;

    protected List<Transaction> transactions;


    public int AccountNumber { get; }

    public string OwnerName { get; }
    public string AccountType { get; protected set; }
    public BankAccount(string ownerName, double initialBalance) 
    {
        AccountNumber = nextAccountNumber++;

        OwnerName = ownerName;

        balance = initialBalance;
    }

    // IDepositable
    public void Deposit(double amount)
    {
        balance += amount;

        Console.WriteLine("Deposit successful");
    }

    // IWithdrawable
    public abstract void Withdraw(double amount);

    // IStatementPrintable
    public virtual void PrintStatement()
    {
        Console.WriteLine("====== Statement ======");
        Console.WriteLine($"Account Number: {AccountNumber}");
        Console.WriteLine($"Owner: {OwnerName}");
        Console.WriteLine($"Balance: {balance}");
    }
}


/// //////class SavingsAccount/////////////////

class SavingsAccount : BankAccount
{

    private double interestRate;
    private static double MinBalance = 100;
    public SavingsAccount(string ownerName, double interestRate = 0.03)

    : base(ownerName, 0)
    {
        AccountType = "Savings";

        this.interestRate = interestRate;
    }
    // Overridden Methods

    public override void Withdraw(double amount)
    {
        if (amount <= 0)
        {
            Console.WriteLine("Amount must be greater than zero");
            return;
        }

        if ((balance - amount) >= MinBalance)
        {
            balance -= amount;

            Transaction transaction = new Transaction("Withdraw", amount);


            transactions.Add(transaction);

            totalTransactionsProcessed++;

            Console.WriteLine("Withdraw successful");
        }
        else
        {
            Console.WriteLine("Withdrawal rejected: minimum balance must be 100");
                 
        }
    }

    // Additional Method

    public void ApplyInterest()

    {
        Deposit(double amount, string type)

        double interest = balance * interestRate;

        Deposit(interest, "Interest Credit");

    }
}

/// /////////class CurrentAccount////////

class CurrentAccount : BankAccount
{
    private double overdraftLimit;

    public double OverdraftLimit
    {
        get { return overdraftLimit; }
    }

    public CurrentAccount(string ownerName,Double overdraftLimit);

     : base(ownerName)
    
        AccountType = "Current";

        if (overdraftLimit >= 0)
        {
            this.overdraftLimit = overdraftLimit;
        }
        else
        {
            this.overdraftLimit = 0;
        }
    }

    public override void Withdraw(double amount)
    {
    if
    (
        amount > 0 &&
        (balance - amount) >= -overdraftLimit) ;
        
        {
            balance -= amount;

        Transaction transaction = new Transaction("Withdrawa", amount);

               

            transactions.Add(transaction);

            totalTransactionsProcessed++;
        
        else
        {
        Console.WriteLine("Withdrawal exceeds overdraft limit");
    public sealed override void PrintStatement()
{
    base.PrintStatement();

    Console.WriteLine($"Overdraft Limit: {overdraftLimit}");
}


/// ////class FixedDepositAccount////

class FixedDepositAccount : BankAccount
{
    private double lockedAmount;

    public FixedDepositAccount(string ownerName, double depositAmount);
    (
      
    base.(ownerName)

        AccountType = "Fixed Deposit";

        if (depositAmount > 0)
        {
            lockedAmount = depositAmount;

            Deposit(depositAmount);

    depositAmount,
                "Initial Fixed Deposit"
            
        
        else
        {
            Console.WriteLine("Deposit amount must be greater than zero.");
           

    public override void Withdraw(double amount)
    {
        Console.WriteLine($"Locked Amount: {lockedAmount}");

    }

    public override void PrintStatement()
    {
        base.PrintStatement();

        Console.WriteLine($"Locked Amount: {lockedAmount}");
    }


    /// ///// class Transaction////////
    class Transaction
    {
        private string type;

        private double amount;

        private DateTime date;

        private string note;

        public Transaction(string type, double amount, string note = "")
        {
            this.type = type;

            this.amount = amount;

            this.note = note;

            date = DateTime.Now;
        }

        public void DisplayInfo()
        {
            Console.WriteLine($"{date.ToShortDateString()}" + $"{type}" + $"{amount:C}");

            if (!string.IsNullOrEmpty(note))
            {
                Console.WriteLine($"Note: {note}");
            }
        }
    }
}

/// //////class Bank////////////////////////////
class Bank
{
    public string BankName { get; private set; }
    public string AccountType { get; protected set; }

    private List<BankAccount> accounts;

    public Bank(string name)
    {
        BankName = name;

        accounts = new List<BankAccount>();
    }

    public void OpenAccount(BankAccount account)
    {
        accounts.Add(account);

        Console.WriteLine($"Account Number: {account.AccountNumber}" + $"Account created successfully.");
        
    }

    public BankAccount FindAccount(int accountNumber)
    {
        return accounts.FirstOrDefault(a => a.AccountNumber == accountNumber);
    }
    

    public void ProcessDeposit
    (
        IDepositable account,
        double amount
    )
    {
        account.Deposit(amount);
    }

    public void ProcessWithdrawa
    (
        IWithdrawable account,
        double amount
    )
    {
        account.Withdraw(amount);
    }

    public void PrintAccountStatement(int accountNumber)
    {
        BankAccount account =
            FindAccount(accountNumber);

        if (account == null)
        {
            Console.WriteLine("Account not found.");
            return;
        }

        IStatementPrintable printable =
            account as IStatementPrintable;

        printable.PrintStatement();
    }

    public void DisplaySummary()
    {
        int savings = 0;
        int current = 0;
        int fixedAcc = 0;

        double totalBalance = 0;

        foreach (BankAccount acc in accounts)
        {
            totalBalance += acc.Balance;

            if (acc is SavingsAccount)
            {
                savings++;
            }
            else if (acc is CurrentAccount)
            {
                current++;
            }
            else if (acc is FixedDepositAccount)
            {
                fixedAcc++;
            }
        }

        Console.WriteLine("====== Bank Summary ======");

        Console.WriteLine($"Bank Name: {BankName}");

        Console.WriteLine($"Total Accounts: {accounts.Count}");

        Console.WriteLine($"Savings Accounts: {savings}");

        Console.WriteLine($"Current Accounts: {current}");

        Console.WriteLine($"Fixed Deposit Accounts: {fixedAcc}");

        Console.WriteLine($"Total Balance: {totalBalance}");

        Console.WriteLine($"Total Transactions:" + $"{BankAccount.GetTotalTransactions()}");
        
            
       
    }
}



