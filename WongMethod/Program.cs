using System;
using System.Collections.Generic;

namespace WongMethod
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Clause> leftClauses = new List<Clause>();
            List<Clause> rightClauses = new List<Clause>();
            WongFileReader.ReadFile(@"../../../Inputs/task4.in", leftClauses, rightClauses);

            bool result = WongDriver.ExecuteWong(leftClauses, rightClauses);
            Console.WriteLine(result ? "No contradiction was found" : "Contradiction was found");
            Console.ReadLine();
        }
    }
}
