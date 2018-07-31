using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebtCalculator.Interfaces
{
    internal abstract class SaveLoad
    {
        protected static string _delim = "|*|";
        public abstract string SaveString();
        public abstract void LoadString(string s);
    }
}
