using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlternativeLanguageProject
{
    internal class FileReader
    {

        
        static public void Main(String[] args)
        {
            List<Cell> cellList = new List<Cell>();
            Dictionary<String, Cell> cellMap = new Dictionary<string, Cell>();
            using (var reader = new StreamReader(@"C:\Users\Aodhan\source\repos\AlternativeLanguageProject\cells.csv"))
            {
                reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    String nextLine = reader.ReadLine();
                    Cell tempCell = new Cell(nextLine);
                    String tempName = tempCell.Model;
                    String tempOem = tempCell.Oem;
                    string tempKey = tempName + tempOem;
                    if (!cellMap.ContainsKey(tempKey))
                    {
                        cellMap.Add(tempName + tempOem, tempCell);
                    }
                    cellList.Add(tempCell);
                    
                   
                }
                DisplayPhones(cellList);
                Console.WriteLine("Average Display Size: " + GetAverageDisplaySize(cellList) + " inches");
                Console.WriteLine("Year in 2000s with most phones launched: " + YearWithMostPhonesLaunched(cellList));

            }
        }

        public static void DisplayPhones(List<Cell> cellList)
        {
            foreach (Cell c in cellList)
            {
                Console.WriteLine(c.ToString());
            }
        }

        public static float GetAverageDisplaySize(List<Cell> cellList)
        {
            float totalSize = 0;
            int numberCounter = 0;
            foreach (Cell c in cellList)
            {
                if(c.DisplaySize > 0)
                {
                    totalSize += c.DisplaySize;
                    numberCounter++;
                }
            }
            return totalSize / numberCounter;
        }

        public static int YearWithMostPhonesLaunched(List<Cell> cellList)
        {
            int bigYear = 0;
            int numInYear = 0;
            Dictionary<int,int> yearMap = new Dictionary<int,int>();
            foreach(Cell c in cellList)
            {
                if(c.LaunchStatus != "Discontinued" && c.LaunchStatus != "Cancelled")
                {
                    int year = int.Parse(c.LaunchStatus);
                    if(year < 2000)
                    {
                        continue;
                    }
                    else if (yearMap.ContainsKey(year))
                    {
                        yearMap[year] = yearMap[year] + 1;
                    }
                    else
                    {
                        yearMap.Add(year, 1);
                    }
                    if(yearMap[year] > numInYear)
                    {
                        bigYear = year;
                        numInYear = yearMap[year];
                    }
                }
            }
            return bigYear;
        }

        
    }
}
