using System;
using System.Collections.Generic;
using System.Linq;

namespace WongMethod
{
    public class Clause : IComparable, IComparable<Clause>
    {
        public Clause(Guid? id = null, List<Variable> variables = null)
        {
            Variables = variables ?? new List<Variable>();
            this.Id = id ?? Guid.NewGuid();
        }
        public Clause(Clause clause)
        {
            Variables = clause.Variables != null ?
                clause.Variables.Select(v => new Variable(v)).ToList() : new List<Variable>();
            this.Id = clause.Id;
        }

        public List<Variable> Variables { get; private set; }
        public Guid Id { get; private set; }

        public bool HasOneValiable { get => Variables.Count == 1; }

        public bool AddVariable(Variable toAdd)
        {
            Variables.Add(toAdd);
            return true;
        }

        public int CompareTo(Clause other)
        {
            if (Variables.Count == other.Variables.Count)
            {
                return Id.CompareTo(other.Id);
            }
            return Variables.Count - other.Variables.Count;
        }

        public int CompareTo(object other) => other is Clause ? CompareTo((Clause)other) : 0;

        public override bool Equals(Object obj)
        {
            if (obj is Clause other)
            {
                if (Variables.Count != other.Variables.Count)
                {
                    return false;
                }
                for (int i = 0; i < Variables.Count; i++)
                {
                    if (!Variables[i].Equals(other.Variables[i]))
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            int sum = 0;
            Variables.ForEach(v => sum += v.GetHashCode());

            return sum;
        }
    }
}
