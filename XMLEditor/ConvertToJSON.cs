using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLEditor
{
    class ConvertToJSON
    {
        public int first;
        public string filename;

        public ConvertToJSON(string fname, int first)
        {
            filename = fname;
            this.first = first;
        }

        //Write: writes the passed string in the file, if add is true it adds new line after writing
        public void write(string s, bool add)
        {
            if (first == 0)
            {
                StreamWriter sw = new StreamWriter(filename, false);
                if (add == true)
                {
                    sw.WriteLine(s);
                    sw.Close();
                    //Console.WriteLine(s); //for debugging purpose
                }
                else
                {
                    sw.Write(s);
                    sw.Close();
                    //Console.Write(s); //for debugging purpose
                }
                first = 1;
            }
            else
            {
                StreamWriter sw = new StreamWriter(filename, true);
                if (add == true)
                {
                    sw.WriteLine(s);
                    sw.Close();
                    Console.WriteLine(s);
                }
                else
                {
                    sw.Write(s);
                    sw.Close();
                    Console.Write(s);
                }
            }
        }
        public string addIndentation(int depth)
        {
            string indent = null;
            for (int i = 0; i <= depth; i++)
            {
                indent += "   ";
            }
            return indent;
        }
        public void Convert(Node node)
        {
            List<Node> children = new List<Node>();
            Node parent = node.getParent();
            children = node.getChildren();
            Stack<string> syntax = new Stack<string>();
            string space = " ";
            string indentation = addIndentation(node.getDepth());
            string openCurlyBraces = "{";
            string closedCurlyBraces = "}";
            string openSquareBrackets = "[";
            string closedSquareBrackets = "]";

            //base case
            if (node == null)
            {
                return;
            }

            if (node.getDepth() == 0)
            {
                write(openCurlyBraces, true);
                write(indentation + '"' + node.getTagName() + '"' + ':' + space + openCurlyBraces, true);
            }
            else
            { //if node has "siblings" of the same tag name (array) 
                if (parent.getChildren().Count() > 1 && parent.getChildren()[0].getTagName() == parent.getChildren()[1].getTagName())
                {
                    if (node == parent.getChildren()[0])
                    {

                        write(indentation + '"' + node.getTagName() + '"' + ':' + space + openSquareBrackets, true);
                        if (children.Count == 0)
                        {
                            write(indentation + space + '"' + node.getTagValue() + '"' + ',', true);
                        }
                        else
                        {
                            write(indentation + openCurlyBraces, true);
                        }


                    }
                    else if (node != parent.getChildren()[0] && children.Count == 0)
                    {
                        if (node == parent.getChildren()[parent.getChildren().Count - 1])
                        {
                            write(indentation + space + '"' + node.getTagValue() + '"', true);
                        }
                        else
                        {
                            write(indentation + space + '"' + node.getTagValue() + '"' + ',', true);
                        }

                    }
                    else if (node != parent.getChildren()[0] && children.Count > 0)
                    {
                        write(indentation + openCurlyBraces, true);

                    }

                }
                else if (children.Count() == 0)
                {
                    // if node has siblings and no children
                    if (parent.getChildren().Count() > 1)
                    {

                        if (node == parent.getChildren()[parent.getChildren().Count - 1])
                        {
                            write(indentation + '"' + node.getTagName() + '"' + ':' + '"' + node.getTagValue() + '"', true);
                        }
                        else
                        {
                            write(indentation + '"' + node.getTagName() + '"' + ':' + '"' + node.getTagValue() + '"' + ',', true);
                        }
                    }
                    else
                    {
                        //if node has no children and no siblings
                        write(indentation + '"' + node.getTagName() + '"' + ':' + '"' + node.getTagValue() + '"', true);
                    }

                }
                else if (children.Count() > 0)
                {

                    //if it has children and siblings
                    if (parent.getChildren().Count() > 1)
                    {
                        if (node != parent.getChildren()[parent.getChildren().Count - 1])//eb2y shofy hna brdo
                        {
                            write(indentation + '"' + node.getTagName() + '"' + ':' + space + openCurlyBraces, true);
                        }
                        else
                        {
                            write(indentation + '"' + node.getTagName() + '"' + ':' + space + openCurlyBraces, true);
                        }

                    }
                    else
                    {
                        //if node has children and no siblings
                        write(indentation + '"' + node.getTagName() + '"' + ':' + space + openCurlyBraces, true);

                    }

                }

            }


            foreach (Node child in children)
            {
                Convert(child);
            }

            if (node.getDepth() == 0)
            {

                write(space + closedCurlyBraces, true);
                write(closedCurlyBraces, true);
            }
            else
            { //if node has "siblings" of the same tag name (array) 
                if (parent.getChildren().Count() > 1 && parent.getChildren()[0].getTagName() == parent.getChildren()[1].getTagName())
                {
                    if (children.Count() == 0 && node == parent.getChildren()[parent.getChildren().Count - 1])
                    {
                        write(indentation + closedSquareBrackets, true);
                    }
                    else if (children.Count() > 0)
                    {
                        if (node != parent.getChildren()[parent.getChildren().Count - 1])
                        {
                            write(indentation + closedCurlyBraces + ',', true);
                        }
                        else
                        {
                            write(indentation + closedCurlyBraces, true);
                            write(indentation + closedSquareBrackets, true);
                        }

                    }

                }


                else if (children.Count() > 0)
                {

                    //if it has children and siblings
                    if (parent.getChildren().Count() > 1)
                    {
                        if (node != parent.getChildren()[parent.getChildren().Count - 1])//eb2y shofy hna brdo
                        {
                            write(indentation + closedCurlyBraces + ',', true);
                        }
                        else
                        {
                            write(indentation + closedCurlyBraces, true);
                        }

                    }
                    else
                    {
                        //if node has children and no siblings
                        write(indentation + closedCurlyBraces, true);

                    }

                }

            }




        }

    }
}