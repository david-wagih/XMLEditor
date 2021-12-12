using System;
using System.IO;
using System.Text;

namespace XMLEditor
{
    public class Minifying
    {
        public static String CompactWhitespaces(String s)
        {
            //To avoid the immutability of the string
            StringBuilder sb = new StringBuilder(s);

            CompactWhitespaces(sb);
            string Compressed = sb.ToString();

            return Compressed;
        }

        public static void CompactWhitespaces(StringBuilder sb)
        {
            if (sb.Length == 0)
                return;

            // set [start] to first not-whitespace char or to sb.Length

            int start = 0;

            while (start < sb.Length)
            {
                //if (Char.IsWhiteSpace(sb[start]))
                if (sb[start] == ' ' || sb[start] == '\v' || sb[start] == '\r' || sb[start] == '\t' || sb[start] == '\n' || sb[start] == '\'' ||
                    sb[start] == '\\' || sb[start] == '\0' || sb[start] == '\"' || sb[start] == '\b' || sb[start] == '\a' || sb[start] == '\f')
                    start++;
                else
                    break;
            }

            // if [sb] has only whitespaces, then return empty string

            if (start == sb.Length)
            {
                sb.Length = 0;
                return;
            }

            // set [end] to last not-whitespace char

            int end = sb.Length - 1;

            while (end >= 0)
            {
                //if (Char.IsWhiteSpace(sb[end]))
                if (sb[end] == ' ' || sb[end] == '\v' || sb[end] == '\r' || sb[end] == '\t' || sb[end] == '\n' || sb[end]=='\''||
                    sb[end] == '\\' || sb[end] == '\0' || sb[end] == '\"' || sb[end] == '\b' || sb[end] == '\a' || sb[end] == '\f')
                    end--;
                else
                    break;
            }

            // compact string

            int dest = 0;
            bool previousIsWhitespace = false;

            for (int i = start; i <= end; i++)
            {
                //if (Char.IsWhiteSpace(sb[i]))
                if (sb[i] == ' '|| sb[i] =='\v' || sb[i] == '\r' || sb[i] == '\t' || sb[i] == '\n' || sb[i] == '\'' ||
                    sb[i] == '\\' || sb[i] == '\0' || sb[i] == '\"' || sb[i] == '\b' || sb[i] == '\a' || sb[i] == '\f')
                {
                
                    if (!previousIsWhitespace)
                    {
                        previousIsWhitespace = true;
                        if (sb[i + 1] == '<' || sb[i - 1] == '>')
                        {
                            continue;
                        }
                        else
                        {
                            sb[dest] = ' ';
                            dest++;
                        }

                    }
                }
                else
                {
                    previousIsWhitespace = false;
                    sb[dest] = sb[i];
                    dest++;
                }
            }

            sb.Length = dest;
        }
    }
    //For testing
    class main {
        public static void Main()
        {

            string text = File.ReadAllText(@"C:\Users\Mayar El-Mallah\Downloads\sample.xml");
            Console.WriteLine(text);
            Console.WriteLine();
            string Compressed=Minifying.CompactWhitespaces(text);
            Console.WriteLine(Compressed);

       

        }
    }
}
