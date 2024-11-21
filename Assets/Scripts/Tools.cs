using System;
using System.Collections.Generic;

namespace Assets.Scripts
{
    public static class Tools
    {
        private static List<char> bannedSymbols = new List<char>() { '#', '%', '@', '!', '&' };

        public static bool CheckFileName(string fileName)
        {
            foreach (char c in bannedSymbols)
                if (fileName.Contains(c))
                    return false;

            return true;
        }
    }
}
