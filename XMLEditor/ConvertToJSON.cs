using System;
using System.Collections.Generic;
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
        public void Write(string s, bool add)
        {
            if (first == 0)
            {
                StreamWriter sw = new StreamWriter(filename, false);
                if (add == true)
                {
                    sw.WriteLine(s);
                    sw.Close();
                }
                else
                {
                    sw.Write(s);
                    sw.Close();
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
                }
                else
                {
                    sw.Write(s);
                    sw.Close();
                }
            }
        }
        public void Convert(Node node, Node parent)
        {
            List<Node> children = new List<Node>();
            children = node.getChildren();
            Stack<string> syntax = new Stack<string>();
            string space = " ";
            string indentation = "    ";
            string openCurlyBraces = "{";
            string closedCurlyBraces = "}";
            string openSquareBrackets = "[";
            string closedSquareBrackets = "]";
            //base case
            if (node == null)
            {
                return;
            }
            //adding identation
            for (int i = 0; i < parent.getDepth(); i++)
            {
                Write(indentation, false);
                syntax.Push(indentation);
            }
            //if node is the root
            if (node.getDepth() == 0)
            {
                Write(openCurlyBraces, true);
                syntax.Push(openCurlyBraces);
                Write('"' + node.getTagName() + '"' + ':' + space, true);

            }
            else
            { //if node has "siblings" of the same tag name (array) 
                if (parent.getChildren().Count() > 1 && node.getTagName() == parent.getChildren()[1].getTagName())
                {
                    if (node == parent.getChildren()[0])
                    {
                        Write(openCurlyBraces, true);
                        syntax.Push(openCurlyBraces);
                        if (children.Count == 0)
                        {
                            Write('"' + node.getTagName() + '"' + ":" + space + openSquareBrackets + '"' + node.getTagValue() + '"', true);
                        }
                        else
                        {
                            Write('"' + node.getTagName() + '"' + ":" + space + openSquareBrackets, true);
                        }
                        syntax.Push(openSquareBrackets);

                    }
                    else if (node != parent.getChildren()[0] && children.Count > 0)
                    {
                        Write(',' + openCurlyBraces, true);
                        syntax.Push(openCurlyBraces);
                    }
                    else if (node != parent.getChildren()[0] && children.Count == 0)
                    {
                        Write(',' + '"' + node.getTagValue() + '"', true);
                    }
                }
                else if (children.Count() == 0)
                {
                    // if node has siblings and no children
                    if (parent.getChildren().Count() > 1)
                    {
                        if (node == parent.getChildren()[0])
                        {
                            Write(openCurlyBraces + '"' + node.getTagName() + '"' + ':' + space + '"' + node.getTagValue() + '"', true);
                        }
                        else if (node != parent.getChildren()[0])
                        {
                            Write(',' + openCurlyBraces + '"' + node.getTagName() + '"' + ':' + space + '"' + node.getTagValue() + '"', true);
                        }

                    }
                    else
                    {
                        //if node has no children and no siblings
                        Write(openCurlyBraces + '"' + node.getTagName() + '"' + ':' + space + '"' + node.getTagValue() + '"', true);
                    }
                    syntax.Push(openCurlyBraces);
                }
                else if (children.Count() > 0)
                {
                    //if it has children and siblings
                    if (parent.getChildren().Count() > 1)
                    {
                        if (node == parent.getChildren()[0])
                        {
                            Write(openCurlyBraces + '"' + node.getTagName() + '"' + ':' + space, true);
                        }
                        else if (node != parent.getChildren()[0])
                        {
                            Write(',' + openCurlyBraces + '"' + node.getTagName() + '"' + ':' + space, true);
                        }

                    }
                    else
                    {
                        //if node has children and no siblings
                        Write(openCurlyBraces + '"' + node.getTagName() + '"' + ':' + space, true);
                    }
                    syntax.Push(openCurlyBraces);
                }

            }


            foreach (Node child in children)
            {
                Convert(child, node);
            }

            while (syntax.Count > 0)
            {
                if (syntax.Peek() == openCurlyBraces)
                {
                    Write(closedCurlyBraces, true);
                    syntax.Pop();
                }
                else if (syntax.Peek() == openSquareBrackets)
                {
                    Write(closedSquareBrackets, true);
                }
                else
                {
                    Write(syntax.Pop(), false);
                }
            }

        }
    }
}
