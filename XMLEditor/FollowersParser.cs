using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;

namespace XMLEditor
{
   
    class FollowersParser
    {
        public static Tuple<int,int>[] followers()
        {
            
            int edgeCounter = 0;

            IDictionary<string, string> idToName = new Dictionary<string, string>();

            Tuple<int, int>[] follower = new Tuple<int, int>[20]; //max number of edges


            string text = System.IO.File.ReadAllText(@"C:\Users\Marina Ehab\sample.xml");
            //string text = System.IO.File.ReadAllText(@"D:\Engineering\Senior 1\Data Structures and Algorithms\Trials\Text.txt"); //file directory, DAVIIIIID
            bool insideUser = false;
            bool userIdFound = false;
            bool hasEdges = false;
            string userId = "";
            for (int i = 0; i < text.Length; i++)
            {
                if (!insideUser)
                {
                    if (text[i] == '<')
                    {
                        if (text[i + 1] == 'u')
                        {
                            if (text[i + 5] == '>') // inside a user
                            {
                                insideUser = true;
                                //Console.WriteLine("Inside a new user");
                            }
                        }
                    }
                }
                else //insideUser
                {
                    
                    string id;

                    if(text[i] == '<')
                    {
                        if(text[i+1] == '/')
                        {
                            if(text[i+2] == 'u')
                            {
                                if(text[i+6] == '>')
                                {
                                    insideUser = false;
                                    userIdFound = false;

                                    if (!hasEdges)
                                    {
                                        follower[edgeCounter] = new Tuple<int, int>(-1, Int32.Parse(userId));
                                        edgeCounter++;
                                    }
                                    hasEdges = false;
                                }
                            }
                        }


                        if(text[i+1] == 'i' && text[i+2] == 'd')
                        {
                            
                            int start = i + 4; // first character after 
                            int end = start+1;
                            while(text[end] != '<')
                            {
                                end++;
                            }
                            end--;
                            id = text.Substring(start, end - start + 1).Trim();
                            if (!userIdFound)
                            {
                                userIdFound = true;
                                userId = id;
                            }
                            else
                            {
                                //Console.WriteLine(id + "," + userId);
                                follower[edgeCounter] = new Tuple<int, int>(Int32.Parse(id), Int32.Parse(userId));
                                edgeCounter++;
                                hasEdges = true;
                            }
                            //Console.WriteLine(id); //Debugging

                        }

                        
                    }

                }
            }

            return follower;

        }


       /* static void Main(string[] args)
        {

            Tuple<int, int>[]  hamoksha = followers();
            
            for(int i = 0; i < hamoksha.Length; i++)
            {
                if (hamoksha[i]!= null) { 
                Console.WriteLine(hamoksha[i].Item1.ToString() + " " + hamoksha[i].Item2.ToString()); //prints elements 
                }
            }
        }*/
    }
}
