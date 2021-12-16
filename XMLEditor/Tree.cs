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
        private Node parent;
        private List<Node> children = new List<Node>();

        public Node(string tagName, string tagValue, string tagAttributes, int depth, Node parent)
        {
            this.tagName = tagName;
            this.tagValue = tagValue;
            this.depth = depth;
            this.tagAttributes = tagAttributes;

            this.parent = parent;
        }

        public Node()
        {
            tagName = null;
            tagValue = null;
            tagAttributes = null;
            depth = 0;
            parent = null;
        }


        public Node getParent()
        {
            return parent;
        }
        public void setParent(Node parent)
        {
            this.parent = parent;
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


        public void setTagName(string tn)
        {
            tagName = tn;
        }
        public void setTagValue(string tv)
        {
            tagValue = tv;
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




        public Node getTreeRoot()
        {
            return this.root.getChildren()[0];
        }
        private void insertFileAUX_New(StreamReader reader, Node node)
        {
            char character;
            while (reader.Peek() >= 0)
            {
                character = (char)reader.Read();
                if (character == '<' && reader.Peek() != '/')
                {
                    string name = null;
                    string value = null;
                    Node child = new Node(null, null, null, node.getDepth() + 1, node);
                    node.getChildren().Add(child);
                    character = (char)reader.Read();
                    while (character != '>')
                    {
                        if (character == '\n' || character == '\r' || character == '\t')
                        {
                            character = (char)reader.Read();
                            continue;
                        }
                        name += character;
                        character = (char)reader.Read();
                    }
                    child.setTagName(name);
                    // character = (char)reader.Read();
                    while (reader.Peek() == '\n' || reader.Peek() == '\t' || reader.Peek() == '\r' || reader.Peek() == ' ')
                    {
                        character = (char)reader.Read();
                    }
                    if (reader.Peek() == '<')
                    {
                        insertFileAUX_New(reader, child);
                    }
                    else
                    {
                        character = (char)reader.Read();
                        while (character != '<')
                        {
                            if (character == '\n' || character == '\r' || character == '\t')
                            {
                                character = (char)reader.Read();
                                continue;
                            }
                            if ((character == ' ' && reader.Peek() == ' ') || (character == ' ' && reader.Peek() == '<'))
                            {
                                character = (char)reader.Read();
                                continue;
                            }
                            value += character;
                            character = (char)reader.Read();

                        }
                        child.setTagValue(value);
                    }
                    //used for debugging
                    /* Console.WriteLine("depth: " + child.getDepth() + " tag name: " + child.getTagName() + " value: " + child.getTagValue() );
                     if(node.getParent() != null)
                     {
                         Console.WriteLine(" parent name:" + child.getParent().getTagName());
                     }*/
                }
                else if (character == '<' && reader.Peek() == '/')
                {
                    insertFileAUX_New(reader, node.getParent());
                }


            }
        }


        public void insertFile(StreamReader reader)
        {
            Node parent = new Node();
            root = parent;
            parent.setDepth(-1);
            insertFileAUX_New(reader, parent);
        }
    }
}
