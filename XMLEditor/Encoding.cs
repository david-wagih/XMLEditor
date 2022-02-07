using System;
using System.IO;
using System.Text;




namespace postSearch
{
    class Program
    {
        static char search_operation()
        {
            char op;
            Console.Write("Please,enter 1: if you wanna search based on post word. \n 2: if you wanna search based on the topic.");
            op = Convert.ToChar(Console.ReadLine());
            return op;
        }
        static void post_search(string word, char x)
        {
            bool FoundWord = false;
            bool FoundTopic = false;
            bool first = false;
            string[] Line = new string[500];
            int size = 0;

            //save all the text in string array
            foreach (string line in File.ReadLines(@"C:\Users\Mayar El-Mallah\Downloads\sample.xml"))
            {
                Line[size++] = line;
            }
            //Traverse the lines
            if (x == '1')
            {
                //for word search in posts
                for (int j = 0; j < size; j++)
                {
                    //Traverse till we find body of the post
                    if (Line[j].Contains("<body>"))
                    {
                        for (int i = j + 1; !Line[i].Contains("</body>"); i++)
                        {
                            if (Line[i].Contains(word))
                            {
                                FoundWord = true;
                                first = true;
                                break;

                            }

                        }

                    }
                    if (FoundWord == true)
                    {
                        for (int i = j + 1; !(Line[i].Contains("</body>")); i++)
                        {
                            Console.WriteLine(Line[i]);
                            Console.WriteLine();

                        }
                        FoundWord = false;
                    }
                }   
            }
            //topic search
            else if (x == '2')
            {
                for (int j = 0; j < size; j++)
                {

                    if (Line[j].Contains("<body>"))
                    {

                        for (int i = j + 1; !Line[i].Contains("</topic>"); i++)
                        {

                            if (Line[i].Contains("<topic>"))
                            {
                                if (Line[i + 1].Contains(word))
                                {
                                    FoundTopic = true;
                                    first = true;
                                    break;
                                }
                            }
                        }
                    }
                    if (FoundTopic == true)
                    {

                        for (int i = j + 1; !(Line[i].Contains("</body>")); i++)
                        {

                            Console.WriteLine(Line[i]);
                            Console.WriteLine();

                        }

                        FoundTopic = false;

                    }
                }

            }
         
            if (first == false)
            {
                Console.Write("Not Found");
            }

        }
        /*static void Main(string[] args)
        {
            //string word = "solar_energy";
            String word = "Lorem";
            char func1 = search_operation();
            post_search(word, func1);


        }*/
    }
}