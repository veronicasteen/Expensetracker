using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

namespace ExpenseTracker
{
    public class Expense
    {
        public string Name;
        public string Category;
        public decimal IncVAT;
       
    }

    public class Program
    {
        static List<Expense> expenseList = new List<Expense>();

        public static void Main()

        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            Console.WriteLine("Välkommen till utgiftskollen!");
            Console.WriteLine();

            bool running = true;
            while (running)
            {
               
                int option = ShowMenu("Vad vill du göra?", new[]
                {
                    "Lägg till utgift",
                    "Visa alla utgifter",
                    "Visa summa per kategori",
                    "Ändra utgift",
                    "Ta bort utgift",
                    "Ta bort alla utgifter",
                    "Avsluta"
                });
                Console.Clear();

                if (option == 0)
                {
                    AddExpense();
                }
                if (option == 1)
                {
                    SeeAllExpenses();
                }
                if (option == 2)
                {
                    ExpensesByCategory();
                }
                if (option == 3)
                {
                    EditExpenses();
                }
                if (option == 4)
                {
                    RemoveOneExpense();
                }
                if (option == 5)
                {
                    RemoveAllExpenses();
                }
                else if (option == 6)
                {
                    Console.WriteLine("Hejdå!");
                    running = false;
                }
            }
        }
        
        public static void AddExpense()
        {
            Console.WriteLine("Lägg till utgift.");
            Console.Write("Namn: ");
            string name = Console.ReadLine();
            Console.Write("Prís inkl. moms: ");
            decimal price = decimal.Parse(Console.ReadLine());
            Console.WriteLine();

            Expense expense = new Expense
            {
                Name = name,
                IncVAT = price
            };

            //här anropas metoden ChooseCategory för att kunna välja kategori
            ChooseCategory(expense);
            expenseList.Add(expense);
            Console.WriteLine("Utgiften är tillagd!");
            Console.WriteLine();
        }

        public static void SeeAllExpenses()
        {
            if (expenseList.Count == 0)
            {
                Console.WriteLine("Du har inga sparade utgifter.");
            }
            else
            {
                //skapar två variabler med startvärdet 0 för att kunna använda senare som totala summor
                decimal totalIncVAT = 0;
                decimal totalExVAT = 0;
                foreach (Expense expense in expenseList)
                {
                    {
                        Console.WriteLine(expense.Name + ": " + expense.IncVAT.ToString("0.00") + "kr (" + expense.Category + ")");
                        
                    }
                   
                }
                //här anropas sumExpenses som returnerar totala summan med och utan moms
                totalIncVAT = SumExpenses(expenseList, true);
                totalExVAT = SumExpenses(expenseList, false);
                Console.WriteLine();

                Console.WriteLine("Antal utgifter: " + expenseList.Count);
                Console.WriteLine("Summa: " + totalIncVAT.ToString("0.00") + " kr " + "("
                    + totalExVAT.ToString("0.00") + " kr exkl. moms" + ")");
                Console.WriteLine();
            }
        }

        public static void ExpensesByCategory()
        {
            if (expenseList.Count == 0)
            {
                Console.WriteLine("Du har inga sparade utgifter.");
            }
            else
            {
                //Skapar en tom lista för varje kategori
                List<Expense> edcuationExpenses = new List<Expense>();
                List<Expense> bookExpenses = new List<Expense>();
                List<Expense> livsmedelExpenses = new List<Expense>();
                List<Expense> ovrigtExpenses = new List<Expense>();

                foreach (Expense expense in expenseList)
                {
                    string category = expense.Category;

                    // Lägger till utgifter i tillhörande kategori-lista
                    switch (category)
                    {
                        case "Utbildning":
                            edcuationExpenses.Add(expense);
                            break;
                        
                        case "Böcker":
                            bookExpenses.Add(expense);
                            break;
                       
                        case "Livsmedel":
                            livsmedelExpenses.Add(expense);
                            break;
                       
                        case "Övrigt":
                            ovrigtExpenses.Add(expense);
                            break;
                    }
                }

                // Beräknar totalsumman med eller utan moms, anropar sumExpenses där uträkningen sker
                decimal educationIncVAT = SumExpenses(edcuationExpenses, true);
                decimal educationExVAT = SumExpenses(edcuationExpenses, false);

                decimal booksIncVAT = SumExpenses(bookExpenses, true);
                decimal booksExVAT = SumExpenses(bookExpenses, false);

                decimal foodsIncVat = SumExpenses(livsmedelExpenses, true);
                decimal foodsExVAT = SumExpenses(livsmedelExpenses, false);

                decimal otherIncVAT = SumExpenses(ovrigtExpenses, true);
                decimal otherExVAT = SumExpenses(ovrigtExpenses, false);

                //skriver ut samtliga kategori-listor med summor
                Console.WriteLine($"Utbildning: inkl. moms: {educationIncVAT.ToString("0.00")} kr "
                    + $"| exkl. moms: {educationExVAT.ToString("0.00")}kr");
                    
                Console.WriteLine($"Böcker: inkl. moms: {booksIncVAT.ToString("0.00")} kr " 
                    + $"| exkl. moms: {booksExVAT.ToString("0.00")} kr");
                
                Console.WriteLine($"Livsmedel: inkl. moms: {foodsIncVat.ToString("0.00")} kr " 
                    + $"| exkl. moms: {foodsExVAT.ToString("0.00")} kr");
                
                Console.WriteLine($"Övrigt: inkl. moms: {otherIncVAT.ToString("0.00")} kr " 
                    + $"| exkl. moms: {otherExVAT.ToString("0.00")} kr");
               
                Console.WriteLine();
            }
        }

        public static void EditExpenses()
        {
            //Skapar en ny lista som heter options
            List<string> options = new List<string>();
            //loopar igenom listan expenseList där alla utgifter är sparade
            foreach (Expense currentExpenses in expenseList)
            {
                //lägger till namn, pris inkl. moms och kategori i options
                options.Add(currentExpenses.Name + ": " + currentExpenses.IncVAT.ToString("0.00") + 
                    "kr (" + currentExpenses.Category + ")"); ;
            }
            //använder får välja utgift
            int expenseIndex = ShowMenu("Välj utgift:", options);
            Console.Clear();

            //Valt index i expenseList läggs till i expense
            Expense expense = expenseList[expenseIndex];

            Console.WriteLine("Namn: " + expense.Name);
            Console.WriteLine("Pris: " + expense.IncVAT.ToString("0.00"));
            Console.Clear();


            int editExpenseMenu = ShowMenu("Vad vill du ändra?", new[]
            {
                "Namn",
                "Pris",
                "Kategori",
            });
            Console.Clear();

            if (editExpenseMenu == 0)
            {
                Console.WriteLine("Nytt namn: ");
                string newPurchase = Console.ReadLine();
                expense.Name = newPurchase;
                Console.WriteLine("Namnet har ändrats.");
                Console.WriteLine();
            }
            if (editExpenseMenu == 1)
            {
                Console.WriteLine("Nytt pris: ");
                decimal newPrice = decimal.Parse(Console.ReadLine());
                expense.IncVAT = newPrice;
                Console.WriteLine();
                //anropar ChooseCategory för att se till att priset läggs till i rätt kategori
                //så att momsen inte blir fel
                ChooseCategory(expense);
                Console.WriteLine("Priset har ändrats.");
                Console.WriteLine();
            }
            else if (editExpenseMenu == 2)
            {
                ChooseCategory(expense);
                Console.WriteLine("Kategorin har ändrats!");
                Console.WriteLine();
            }

        }

        public static void RemoveOneExpense()
        {
            if (expenseList.Count == 0)
            {
                Console.WriteLine("Du har inga sparade utgifter.");
            }
            else
            {
                //gör på samma sätt som i editExpense
                //skapar en ny tom lista, loopar igenom expenseList och lägger till sparade utgifter i options.
                List<string> options = new List<string>();
                foreach (Expense userPick in expenseList)
                {
                    options.Add(userPick.Name + ": " + userPick.IncVAT.ToString("0.00") + "kr (" + userPick.Category + ")");
                }
                int expenseIndex = ShowMenu("Välj utgift att ta bort:", options);
                Console.Clear();

                Expense selectedExpense = expenseList[expenseIndex];
                //på vald indexplats tas utgiften bort
                expenseList.RemoveAt(expenseIndex);

                Console.WriteLine(selectedExpense.Name + " har tagits bort.");
                Console.WriteLine();
            }
        }


        public static void RemoveAllExpenses()
        {
            if (expenseList.Count == 0)
            {
                Console.WriteLine("Du har inga sparade utgifter.");
            }
            else
            {
                int DeleteAll = ShowMenu("Är du säker på att du vill ta bort alla utgifter?", new[]

                {
                    "Ja",
                    "Nej"
                });

                if (DeleteAll == 0)
                {
                    expenseList.Clear();
                    Console.WriteLine("Alla utgifter har tagits bort.");
                    Console.WriteLine();
                   
                }
                else if (DeleteAll == 1)
                {
                    Console.WriteLine("Inga utgifter har tagits bort.");
                    Console.WriteLine();
                   
                }
            }
        }

        public static void ChooseCategory(Expense expense)
        {
            int categoryChoice = ShowMenu("Välj kategori", new[]
            {
                "Utbildning",
                "Böcker",
                "Livsmedel",
                "Övrigt"
            });
            
            //en tom sträng som ska hålla strängen med kategorin
            string selectedCategory = "";
            
            //alla kategorival med olika momssatser. 
            if (categoryChoice == 0)
            {
                selectedCategory = "Utbildning";
            }
            else if (categoryChoice == 1)
            {
                selectedCategory = "Böcker";
            }
            else if (categoryChoice == 2)
            {
                selectedCategory = "Livsmedel";
            }
            else if (categoryChoice == 3)
            {
                selectedCategory = "Övrigt";
            }

            Console.Clear();
            //Här läggs värdet i selectedCategory in i expense.Category
            expense.Category = selectedCategory;
            
        }
       
        public static decimal SumExpenses(List<Expense> expenses, bool includeVAT)
        {

            decimal sum = 0;

            foreach (Expense expense in expenses)
            {
                // Om includeVAT är true, returneras summan inklusive moms
                if (includeVAT)
                {
                    sum += expense.IncVAT;
                }
                //här är includeVAT false och returnerar summan exklusive moms
                else
                {
                    decimal categoryVAT = CategoryVAT(expense.Category);
                    sum += expense.IncVAT / (1 + categoryVAT);
                }
            }
            //returnerar summan med och utan moms 
            return sum;
        }
        public static decimal CategoryVAT(string category)
        {
            switch (category)
            {
                //alla olika moms-satser CategoryVat ska returnera beroende på kategori
                case "Utbildning":
                    return 0.00m;
                
                case "Böcker":
                    return 0.06m;
                
                case "Livsmedel":
                    return 0.12m;
                
                case "Övrigt":
                    return 0.25m;
                
                default:
                    // Om kategorin inte matchar någon av ovanstående, använd noll momssats.
                    return 0.00m; 
            }
        }


        public static int ShowMenu(string prompt, IEnumerable<string> options)
        {
            if (options == null || options.Count() == 0)
            {
                throw new ArgumentException("Cannot show a menu for an empty list of options.");
            }

            Console.WriteLine(prompt);

            // Hide the cursor that will blink after calling ReadKey.
            Console.CursorVisible = false;

            // Calculate the width of the widest option so we can make them all the same width later.
            int width = options.Max(option => option.Length);

            int selected = 0;
            int top = Console.CursorTop;
            for (int i = 0; i < options.Count(); i++)
            {
                // Start by highlighting the first option.
                if (i == 0)
                {
                    Console.BackgroundColor = ConsoleColor.Blue;
                    Console.ForegroundColor = ConsoleColor.White;
                }

                var option = options.ElementAt(i);
                // Pad every option to make them the same width, so the highlight is equally wide everywhere.
                Console.WriteLine("- " + option.PadRight(width));

                Console.ResetColor();
            }
            Console.CursorLeft = 0;
            Console.CursorTop = top - 1;

            ConsoleKey? key = null;
            while (key != ConsoleKey.Enter)
            {
                key = Console.ReadKey(intercept: true).Key;

                // First restore the previously selected option so it's not highlighted anymore.
                Console.CursorTop = top + selected;
                string oldOption = options.ElementAt(selected);
                Console.Write("- " + oldOption.PadRight(width));
                Console.CursorLeft = 0;
                Console.ResetColor();

                // Then find the new selected option.
                if (key == ConsoleKey.DownArrow)
                {
                    selected = Math.Min(selected + 1, options.Count() - 1);
                }
                else if (key == ConsoleKey.UpArrow)
                {
                    selected = Math.Max(selected - 1, 0);
                }

                // Finally highlight the new selected option.
                Console.CursorTop = top + selected;
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.ForegroundColor = ConsoleColor.White;
                string newOption = options.ElementAt(selected);
                Console.Write("- " + newOption.PadRight(width));
                Console.CursorLeft = 0;
                // Place the cursor one step above the new selected option so that we can scroll and also see the option above.
                Console.CursorTop = top + selected - 1;
                Console.ResetColor();
            }

            // Afterwards, place the cursor below the menu so we can see whatever comes next.
            Console.CursorTop = top + options.Count();

            // Show the cursor again and return the selected option.
            Console.CursorVisible = true;
            return selected;
        }
    }

    [TestClass]
    public class ProgramTests
    {
        [TestMethod]
        public void SumExpenses_OneExpenseExVAT()
        
        {
            List<Expense> expenses = new List<Expense>
            {
                new Expense { Name = "iPhone", IncVAT = 2000.00m, Category = "Övrigt" },
                
            };

            decimal totalSum = Program.SumExpenses(expenses, false);

            Assert.AreEqual(1600.0m, totalSum);
        }

        [TestMethod]
        public void SumExpenses_MultipleExpensesExVAT()
        {
            List<Expense> expenses = new List<Expense>
            {
                new Expense { Name = "Toalettpapper", IncVAT = 34.50m, Category = "Livsmedel" },
                new Expense { Name = "Tuggummi", IncVAT = 21.0m, Category = "Livsmedel"},
                new Expense { Name = "Kjol", IncVAT = 249.0m, Category = "Övrigt" }
            };

            decimal totalSum = Program.SumExpenses(expenses, false);
           

            totalSum = Math.Round(totalSum, 2);

            Assert.AreEqual(248.75m, totalSum);
        }

        [TestMethod]
        public void SumExpenses_MultipleExpensesIncVAT()
        {
            List<Expense> expenses = new List<Expense>
            {
                new Expense { Name = "Utekväll", IncVAT = 1565.50m, Category = "Övrigt" },
                new Expense { Name = "Skolböcker", IncVAT = 699.0m, Category = "Utbildning"},
                new Expense { Name = "Apelsiner", IncVAT = 45.0m, Category = "Livsmedel" }
            };

            decimal totalSum = Program.SumExpenses(expenses, true);


            totalSum = Math.Round(totalSum, 2);

            Assert.AreEqual(2309.50m, totalSum);
        }
    }
}
