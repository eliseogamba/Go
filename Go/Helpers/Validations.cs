namespace Go.Helpers
{
    public class Validations
    {
        public static bool ValidEmail(string email)
        {
            if (email.Equals("")) return false;
            else if (!email.Contains("@")) return false;
            else if (!email.Contains(".")) return false;
            else if (email.Contains("@.")) return false;
            else if (email.LastIndexOf(".") == (email.Length - 1)) return false;
            else return true;
        }
    }
}