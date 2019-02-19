using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace XillioAPIService
{
    public static class WindowsNameConventions
    {
        private static List<char> INVALID_WINDOWS_PATH_CHARACTERS =
            new List<char>() {':', '?', '*', '"', '<', '>', '|', '/'};
        
        public static string MakeNameCompliant(string name)
        {
            List<char> foundChars = INVALID_WINDOWS_PATH_CHARACTERS.Where(c => name.Contains(c)).ToList();
            foreach (var character in foundChars)
            {
                name = name.Replace(character, ' ');
            }

            if (name.EndsWith("."))
            {
                name.Remove(name.Length - 1);
            }

            return name;
        }
    }
}