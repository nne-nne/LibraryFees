using System;
using System.Collections.Generic;

/// THE TASK:
/*Create the following .NET Console App:
We need a library app to keep track of books and penalty fees borrowers pay (per session, so we don’t need to store them in any db/file etc).
Library has the following policy
	• All books should be returned the next day
	• For IT Books borrower should pay a penalty of 5 PLN for next each day ( so if book return due date was 01-Jan and borrower returned it at 03-Jan, then s/he should pay 10 PLN)
	• For History Books borrower should pay a penalty of 3 PLN.
	• For classics, law, medical, philosophy books the penalty is 2 PLN.
Application should work in the following way:
	• Please pick a book category [1] IT, [2] History, [3] etc…….
	• Please write the day, month, year of the borrow
	• Please write the day, month, year of the return
	• …borrower penalty fee is ….. OR … borrower has no fee to pay …
*/

/// <summary>
/// Adam Leczkowski Home Assignment
/// 
/// well it did take me ~2h
/// </summary>
namespace LibraryFees
{
    class Program
    {
        const string exitCommand = "q";
        const uint categoriesCount = 6;
        const int allowedDelay = 1;

        #region I/O Helpers
        private static bool AssignCategory(ref uint category, ref string buffer)
        {
            try
            {
                category = UInt32.Parse(buffer);
                return (category <= categoriesCount);
            }
            catch
            {
                return false;
            }
        }
        private static void InitialQuery(ref string buffer)
        {
            Console.Write($"Please pick a book category:\n [1] IT\n [2] History\n [3] Classics\n [4] Law\n [5] Medical\n [6] Philosophy\n or enter '{exitCommand}' to quit");
            buffer = Console.ReadLine();
        }

        private static DateTime ReadTime(string relatedAction)
        {
            string buffer;
            Console.WriteLine($"Please write the day, month, year of the {relatedAction}");
            buffer = Console.ReadLine();
            while(true)
            {
                try
                {
                    DateTime date = DateTime.Parse(buffer);
                    return date;
                }
                catch (FormatException)
                {
                    Console.WriteLine($"{buffer} does not seem to be a valid date");
                }
                catch(ArgumentNullException)
                {
                    Console.WriteLine("Please provide a date");
                }
                buffer = Console.ReadLine();
            }
        }

        private static Tuple<DateTime, DateTime> ReadDates()
        { 
            DateTime borrowDate = ReadTime("borrow");
            DateTime returnDate = ReadTime("return");
            Tuple<DateTime, DateTime> dates = new Tuple<DateTime, DateTime>(borrowDate, returnDate);
            return dates;
        }
        #endregion

        private static float CalculateFee(ref Dictionary<uint, float> feeTariffs, uint category, Tuple<DateTime, DateTime> dates)
        {
            int daysOfDelay = CalculateDelay(dates.Item1, dates.Item2);
            return daysOfDelay > 0 ? daysOfDelay * feeTariffs[category] : 0f;  
        }

        private static int CalculateDelay(DateTime borrowDate, DateTime returnDate)
        {
            return (returnDate - borrowDate).Days - allowedDelay;
        }

        private static void InitializeFeeTariffs(ref Dictionary<uint, float> feeTariffs)
        {
            feeTariffs.Add(1, 5.0f);
            feeTariffs.Add(2, 3.0f);
            for (uint i = 3; i <= categoriesCount; i++)
                feeTariffs.Add(i, 2.0f);
        }

        static void Main(string[] args)
        {
            string buffer="";
            uint category = 0;
            int borrowerIndex = 1;
            Dictionary<uint, float> feeTariffs = new Dictionary<uint, float>();
            InitializeFeeTariffs(ref feeTariffs);

            InitialQuery(ref buffer);
            while (!buffer.Equals(exitCommand))
            {
                if (AssignCategory(ref category, ref buffer))
                {
                    Tuple<DateTime, DateTime> dates = ReadDates();
                    float fee = CalculateFee(ref feeTariffs, category, dates);
                    if(fee > 0)
                        Console.WriteLine($"#{borrowerIndex++} borrower penalty fee is {fee} PLN");
                    else
                        Console.WriteLine($"#{borrowerIndex++} borrower has no fee to pay");
                }
                else
                {
                    Console.WriteLine($"{buffer} is not a valid category");
                }
                InitialQuery(ref buffer);
            }
            Console.WriteLine("No more borrowers can possibly repair the library's budget"); 
        }
    }
}
