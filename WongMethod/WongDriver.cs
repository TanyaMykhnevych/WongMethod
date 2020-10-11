using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WongMethod
{
    public static class WongDriver
    {
        public static bool ExecuteWong(List<Clause> left, List<Clause> right)
        {
            right.Sort((x, y) => y.Variables.Count - x.Variables.Count);
            left.Sort((x, y) => y.Variables.Count - x.Variables.Count);

            if (MeetWongClause(left, right)) return true;

            MoveNegativeToRightIfPossible(left, right);

            if (left.All(c => c.HasOneValiable) && !MeetWongClause(left, right))
            {
                if (right.All(c => c.HasOneValiable)) return false;

                // Process right part
                foreach (Clause c in right)
                {
                    foreach (Variable v in c.Variables)
                    {
                        List<Clause> newRight = new List<Clause>
                            {
                                new Clause(Guid.NewGuid(), new List<Variable> { new Variable(v) })
                            };
                        newRight.AddRange(right.Where(l => l.Id != c.Id).Select(c => new Clause(c)));

                        List<Clause> newLeft = left.Select(c => new Clause(c)).ToList();
                        return ExecuteWong(newLeft, newRight);
                    }
                }
            }

            if (MeetWongClause(left, right)) return true;

            // Process left part
            foreach (Clause c in left)
            {
                foreach (Variable v in c.Variables)
                {
                    List<Clause> newLeft = new List<Clause>
                    {
                        new Clause(Guid.NewGuid(), new List<Variable> { new Variable(v) })
                    };

                    newLeft.AddRange(left.Where(l => l.Id != c.Id).Select(c => new Clause(c)));

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
    }
}
