namespace DebtCalculator.Models.Interfaces
{
    internal abstract class SaveLoad
    {
        protected static string _delim = "|*|";
        public abstract string SaveString();
        public abstract void LoadString(string s);
    }
}
