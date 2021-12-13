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
            string data;
            string name;

            while (reader.Peek() >= 0)
            {
                //read one char skipping spaces & new lines
                letter = skipSpaces(reader);

                //check if we are reading an opening tag
                if (letter == '<' && reader.Peek() != (int)('/'))
                {
                    letter = skipSpaces(reader);

                    if (Char.IsLetter(letter))
                    {
                        //add new child to current parent and update child depth
                        Node child = new Node(null, null, null, false, parent.getDepth() + 1);
                        parent.getChildren().Add(child);

                        //save the tagName in name
                        name = letter.ToString();
                        letter = (char)reader.Read();

                        while (letter != '>')
                        {
                            name += letter.ToString();
                            letter = (char)reader.Read();

                            //save attributes
                            if (letter == ' ')
                            {
                                child.setTagName(name);
                                data = ((char)reader.Read()).ToString();
                                letter = (char)reader.Read();
                                while (letter != '>')
                                {
                                    //check for self closing tag
                                    if (letter == '/' && reader.Peek() == (int)('>'))
                                    {
                                        child.setIsClosingTag(true);
                                        child.setTagAttributes(data);
                                        letter = (char)reader.Read();
                                        continue;
                                    }

                                    data += letter.ToString();

                                    letter = (char)reader.Read();
                                }

                                child.setTagAttributes(data);
                            }

                            //check for self closing tag
                            if (letter == '/' && reader.Peek() == (int)('>'))
                            {
                                child.setIsClosingTag(true);
                                child.setTagName(name);
                                letter = (char)reader.Read();
                                continue;
                            }
                        }

                        //****save the tagname in node****
                        child.setTagName(name);

                        //skip spaces & new lines without consuming the letter
                        while (reader.Peek() == (int)('\n') || reader.Peek() == (int)('\t') || reader.Peek() == (int)(' '))
                        {
                            letter = (char)reader.Read();
                        }

                        //save the data inside the tag if there is data
                        if (reader.Peek() != (int)('<'))
                        {
                            letter = (char)reader.Read();
                            data = letter.ToString();

                            //save the data until reaching the closing tag
                            while (reader.Peek() != (int)('<'))
                            {
                                letter = (char)reader.Read();
                                data += letter.ToString();
                            }

                            //remove any /n from the end of the data before saving it 
                            int index = data.Length;
                            while (data[index - 1] == '\n' || data[index - 1] == ' ' || data[index - 1] == '\t')
                            {
                                data = data.Substring(0, index - 1);
                                index = data.Length;
                            }
                            //****save the data in node****
                            child.setTagValue(data);

                        }

                        else if (child.getIsClosingTag() == false)
                        {
                            //this child also has children
                            insertFileAUX(reader, child);
                        }

                    }
                }

                //check if we are reading a closing tag for that parent
                else if (letter == '<' && reader.Peek() == (int)('/'))
                {
                    letter = (char)reader.Read();
                    letter = (char)reader.Read();
                    name = letter.ToString();
                    letter = (char)reader.Read();
                    while (letter != '>')
                    {
                        name += letter.ToString();
                        letter = (char)reader.Read();
                    }

                    if (name == parent.getTagName())
                        return;
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
