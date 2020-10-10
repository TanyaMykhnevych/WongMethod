using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WongMethod
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Clause> leftClauses = new List<Clause>();
            List<Clause> rightClauses = new List<Clause>();
            ReadFile(@"../../../Inputs/task2.in", leftClauses, rightClauses);

            bool result = ExecuteWong(leftClauses, rightClauses);
            Console.WriteLine(result ? "No contradiction was found" : "Contradiction was found");
            Console.ReadLine();
        }

        private static bool ExecuteWong(List<Clause> left, List<Clause> right)
        {
            if (MeetWongClause(left, right)) return true;

            MoveNegativeToRightIfPossible(left, right);

            if (left.All(c => c.HasOneValiable) && !MeetWongClause(left, right)) return false;
            if (MeetWongClause(left, right)) return true;

            foreach (Clause c in left)
            {
                foreach (Variable v in c.Variables)
                {
                    List<Clause> newLeft = new List<Clause>
                    {
                        new Clause(Guid.NewGuid(), new List<Variable> { new Variable(v) })
                    };

                    newLeft.AddRange(left.Where(l => l.Id != c.Id).Select(c => new Clause(c)));
                    newLeft.Sort((x, y) => y.Variables.Count - x.Variables.Count);

                    List<Clause> newRight = right.Select(c => new Clause(c)).ToList();
                    return ExecuteWong(newLeft, newRight);
                }
            }

            return false;
        }

        private static bool MeetWongClause(List<Clause> left, List<Clause> right) =>
            right.Intersect(left).Any();

        private static void MoveNegativeToRightIfPossible(List<Clause> left, List<Clause> right)
        {
            var sigleClauses = left.Where(l => l.HasOneValiable && l.Variables[0].Negated).ToList();
            for (int i = 0; i < sigleClauses.Count; i++)
            {
                sigleClauses[i].Variables[0].Negated = false;
                left.Remove(sigleClauses[i]);
                right.Add(sigleClauses[i]);
            }
        }

        private static void ReadFile(string file, List<Clause> leftClauses, List<Clause> rightClauses)
        {
            using StreamReader reader = new StreamReader(file);
            string line;
            bool left = true;
            while ((line = reader.ReadLine()) != null)
            {
                Clause c = new Clause();
                if (string.IsNullOrEmpty(line))
                {
                    left = false;
                    continue;
                }

                line.Split(' ').ToList().ForEach(v =>
                    c.AddVariable(new Variable(v.StartsWith('~') ? v.Substring(1) : v, v.StartsWith('~'))));

                if (left) leftClauses.Add(c);
                else rightClauses.Add(c);
            }
        }
    }
}
