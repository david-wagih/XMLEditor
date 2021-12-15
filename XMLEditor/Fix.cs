using System;
using System.Collections;
using System.Text.RegularExpressions;
using System.Text;

namespace XMLEditor
{
    /************************************************************************************************************************************************/
    /*Known bugs: 
     * 1-open brackets missing which could stack exception ------------SOLVED
     * 2-If the first bracket is missing, stack exception as well.
     * 3-if something remains in stack, What do you do Mario?
     * 
     *
     *
     * 
     * 
     * 
     * 
     * 
     * 
     * 
     * 
     * 
     * 
     * 
     * 
     * 
     * 
     * 
     * 
     * 
     * 
     */
    class Fix
    {
        public string[] validator(string path)
        {
            bool missingOpeningTags = false;
            bool missingEndTags = false;
            //bool missingOpenBrackets = false;
            Stack tags = new Stack();
            Stack tagsLocation = new Stack();
            Stack brackets = new Stack() ;
            Stack bracketsLocation = new Stack();

            string[] lines = System.IO.File.ReadAllLines(path); //file directory, DAVIIIIID
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                for (int j = 0; j < line.Length; j++)
                {
                    if(line[j] == '<')
                    {
                        brackets.Push('<');
                        int[] location = new int[2];
                        location[0] = i;
                        location[1] = j;
                        bracketsLocation.Push(location);
                        if(line[j+1] == '/') //checks if it's an end-tag
                        {
                            int cnt = 2;
                            while (Regex.IsMatch(line[j + cnt].ToString(), "[a-z]", RegexOptions.IgnoreCase) && (j + cnt < line.Length-1)) //Want to account for missing brackets but not yet && line[j+cnt] == '<'
                            {           
                                cnt++;
                            }
                            string tag;
                            if (line[j+cnt] == '>') //found the closing bracket
                            {
                                tag = line.Substring(j + 2, cnt - 2);
                                j += cnt - 1;

                            }
                            else    //closing bracket is missing
                            {
                                lines[i] += ">";
                                line = lines[i];
                                Console.WriteLine("Adding a closing tag at the end of line " + i);
                                tag = line.Substring(j + 2, cnt - 1);
                                j += cnt - 1;

                            }
                            if(tags.Count != 0)//checking the stack is not empty
                            {
                                if (tags.Peek().ToString() == tag) //matching end-tag with open-tag in stack
                                {
                                    tags.Pop();
                                    tagsLocation.Pop();
                                }
                                else //missing open-tag or next opening tag
                                {
                                    string tempTag = (String)tags.Peek();
                                    int[] tempLocation = (int[])tagsLocation.Peek();
                                    tags.Pop();
                                    tagsLocation.Pop();
                                    if (tags.Count != 0)    //stack is not empty
                                    {
                                        if (tags.Peek().ToString() == tag)
                                        {
                                            Console.WriteLine("The tag " + tempTag + " in the line " + (int)(tempLocation[0]) + " and starting from " + (int)(tempLocation[1]) + " with length of " + (int)(tempLocation[2]) + " has its end-tag missing");
                                            missingEndTags = true;
                                            tags.Pop();
                                            tagsLocation.Pop();
                                        }
                                        else
                                        {

                                            Console.WriteLine("The tag " + tag + " in the line " + (int)(i + 1) + " and starting from " + (int)(j - cnt + 1) + " with length of " + (int)(cnt - 2) + " has its open-tag missing");
                                            missingOpeningTags = true;
                                            tags.Push(tempTag);
                                            tagsLocation.Push(tempLocation);
                                        }
                                    }
                                    else    //stack is empty
                                    {
                                        missingOpeningTags = true;
                                        Console.WriteLine("The tag " + tag + " in the line " + (int)(i + 1) + " and starting from " + (int)(j + 2) + " with length of " + (int)(cnt - 2) + " has its open-tag missing");
                                        tags.Push(tempTag);
                                        tagsLocation.Push(tempLocation);
                                    }
                                }
                            }
                            else //The stack is empty
                            {
                                missingOpeningTags = true;
                                Console.WriteLine("The tag " + tag + " in the line " + (int)(i+1) + " and starting from " + (int)(j+2) + " with length of " + (int)(cnt-2) + " has its open-tag missing");

                            }
                        }
                        else //it's an opening tag
                        {
                            int cnt = 1;
                            while (Regex.IsMatch(line[j + cnt].ToString(), @"[a-z]",RegexOptions.IgnoreCase) &&(j+cnt <line.Length-1)) //Want to account for missing brackets but not yet && line[j+cnt] == '<'
                            { 
                                cnt++;
                            }
                            if (line[j+cnt] == '>') //found the closing bracket
                            {
                                string tag = line.Substring(j + 1, cnt - 1);
                                tags.Push(tag);
                                int[] tagLocation = new int[3];
                                tagLocation[0] = i;
                                tagLocation[1] = j + 1;
                                tagLocation[2] = j + cnt - 1;
                                tagsLocation.Push(tagLocation);
                                j += cnt - 1;
                            }
                            else //closing bracket is missing :(
                            {
                                lines[i] += ">";
                                line = lines[i];
                                Console.WriteLine("Adding a closing tag at the end of line " + i);
                                string tag = line.Substring(j + 1, cnt);
                                tags.Push(tag);
                                int[] tagLocation = new int[3];
                                tagLocation[0] = i;
                                tagLocation[1] = j + 1;
                                tagLocation[2] = j + cnt - 1;
                                tagsLocation.Push(tagLocation);
                                j += cnt;
                                
                            }
                        }
                    }
                    else if(line[j] == '>')
                    {
                        if(brackets.Count != 0) //checking that the stack has brackets in order to be able to peek
                        {
                            if (brackets.Peek().ToString() == "<")
                            {
                                brackets.Pop();
                            }
                            else
                            {
                                int[] arr = (int[])bracketsLocation.Peek();


                                Console.WriteLine("The bracket in the line " + arr[0] + " and the subscript " + arr[2] + " is missing its opening bracket");
                            }
                        }
                        else //missing open bracket
                        {
                            j--;
                            //missingOpenBrackets = true;
                            Console.WriteLine("The bracket in the line " + i+1 + " is missing its opening bracket");
                            while (Regex.IsMatch(line[j].ToString(), "[a-z]", RegexOptions.IgnoreCase ) && j>0 && line[j] != ' '){
                                j--;
                            }
                            StringBuilder str = new StringBuilder(line);
                            if (j == 0)
                            {
                                line = "<" + line;
                            }
                            else
                            {
                                str[j] = '<';
                                line = str.ToString();
                            }
                            
                            lines[i] = line;
                            j--;
                            //Console.WriteLine("HEEEEEEEEEEELP " + line[j]);
                        }
                    }
                }
            }
            if (tags.Count != 0)
            {
                while (tags.Count != 0) { 
                    int[] tempLocation = (int[])tagsLocation.Peek();
                    string tempTag = (String)tags.Peek();
                    tags.Pop();
                    tagsLocation.Pop();
                    Console.WriteLine("The tag " + tempTag + " in the line " + (int)(tempLocation[0]) + " and starting from " + (int)(tempLocation[1]) + " with length of " + (int)(tempLocation[2]) + " has its end-tag missing");

                }
                missingOpeningTags = true;
            }

            Console.WriteLine("Is the file valid now? " +(tags.Count == 0 && brackets.Count == 0 && !missingOpeningTags && !missingEndTags));
            for(int i = 0; i < lines.Length; i++)
            {
                Console.WriteLine(lines[i]);
            }
            return lines;
        }
    }
}
