using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLEditor
{
    class Node
    {
        private string tagName;
        private string tagValue;
        private string tagAttributes;
        private int depth;
        private bool isClosingTag;
        private List<Node> children = new List<Node>();

        public Node(string tagName, string tagValue, string tagAttributes, bool isClosingTag, int depth)
        {
            this.tagName = tagName;
            this.tagValue = tagValue;
            this.depth = depth;
            this.tagAttributes = tagAttributes;
            this.isClosingTag = isClosingTag;
        }

        public Node()
        {
            tagName = null;
            tagValue = null;
            tagAttributes = null;
            isClosingTag = false;
            depth = 0;
        }

        public List<Node> getChildren()
        {
            return children;
        }

        public string getTagName()
        {
            return tagName;
        }

        public string getTagValue()
        {
            return tagValue;
        }

        public int getDepth()
        {
            return depth;
        }

        public string getTagAttributes()
        {
            return tagAttributes;
        }
        public bool getIsClosingTag()
        {
            return isClosingTag;
        }

        public void setTagName(string tn)
        {
            tagName = tn;
        }
        public void setTagValue(string tv)
        {
            tagValue = tv;
        }
        public void setIsClosingTag(bool check)
        {
            isClosingTag = check;
        }
        public void setDepth(int d)
        {
            depth = d;
        }

        public void setTagAttributes(string ta)
        {
            tagAttributes = ta;
        }
    }

    class Tree
    {
        private Node root;
        public Tree(Node root)
        {
            this.root = root;
        }

        public Tree()
        {
            root = null;
        }

        private char skipSpaces(StreamReader reader)
        {
            char letter = (char)reader.Read();
            while (letter == '\n' || letter == '\t') letter = (char)reader.Read();
            return letter;
        }

        public Node getTreeRoot()
        {
            return this.root.getChildren()[0];
        }
        private void insertFileAUX(StreamReader reader, Node parent)
        {
            char letter;



            while (reader.Peek() >= 0)
            {
                string name = null;
                string data = null;
                //read one char skipping spaces & new lines
                letter = skipSpaces(reader);
                if (letter == '<' && reader.Peek() != '/')
                {
                    letter = skipSpaces(reader);
                    while (Char.IsLetter(letter))
                    {
                        name += letter;
                        letter = (char)reader.Read();
                    }
                    Node child = new Node(name, null, null, true, parent.getDepth() + 1);
                    parent.getChildren().Add(child);
                    if (letter == '>' && Char.IsLetterOrDigit((char)reader.Peek()))
                    {


                        letter = (char)reader.Read();
                        while (letter != '<')
                        {
                            data += letter;
                            letter = (char)reader.Read();
                        }
                        child.setTagValue(data);




                    }
                    else
                    {
                        insertFileAUX(reader, child);
                    }
                    Console.WriteLine("depth: " + child.getDepth() + " tag name: " + child.getTagName() + " value: " + child.getTagValue());

                }


            }
        }
        public void insertFile(StreamReader reader)
        {
            Node parent = new Node();
            root = parent;
            parent.setDepth(-1);
            insertFileAUX(reader, parent);
        }
    }
}
