using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace XML_Editor
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


    class FormatXml
    {
        /* class members */
        public string XmlFileName;
        public bool firstTime; //int

        /* constructor */
        public FormatXml(string file_name,bool first_time)  //int
        {
            XmlFileName = file_name;
            firstTime = first_time;
        }

        /* class methods */
        public void writeOut(string str,bool append) /* to write in the output */
        {
            /* if it is the 1st time or Not to use format */
            if (firstTime == false)    
            {
                StreamWriter sw = new StreamWriter(XmlFileName, false); /* to link with XmlFile directory to write to & append data to the file or overwrite the file */
                if (append == true)    /* append data to the file if true */
                {
                    sw.WriteLine(str); /* printy data with new line */
                    sw.Close();
                }
                else                   /* overwrite the file if false */
                {
                    sw.Write(str);     /* print data without new line */
                    sw.Close();
                }
                firstTime = true;
            }
            else 
            {
                StreamWriter sw = new StreamWriter(XmlFileName, true); /* to link with XmlFile directory to write to & append data to the file or overwrite the file */
                if (append == true)    /* append data to the file if true */
                {
                    sw.WriteLine(str); /* printy data with new line */
                    sw.Close();
                }
                else                   /* overwrite the file if false */
                {
                    sw.Write(str);     /* print data without new line */
                    sw.Close();
                }
            }
        }

        public void format(Node root) /* to format & indent XML */
        {
            Node r_node = root;
            List<Node> children = new List<Node>();
            children = root.getChildren();
            Console.WriteLine(" not NULL "); //to check 

            /* to avoid errors of empty node */
            if (r_node == null)
            {
                Console.WriteLine(" NULL "); //to check x
                return;
            }

            for (int i = 0; i < r_node.getDepth(); i++)  /* loop until the end to indent */
            {
                Console.WriteLine(r_node.getDepth()); //to check
                writeOut("    ", false); /* indent without a new line */
            }
            
            if (r_node.getTagAttributes() == null) /* if there's no attributes */
            {
                if (r_node.getIsClosingTag()) /* if closing tag */
                {
                    writeOut("<" + "/" + r_node.getTagName() + ">", true); 
                }
                else  /* if open tag */
                {
                    writeOut("<" + r_node.getTagName() + ">", true);
                }
            }
            
            else  /* if the tag has an attribute */
            {
                if (r_node.getIsClosingTag())
                {
                    writeOut("<" + "/" + r_node.getTagName() + ">", true); //+ " " + r_node.getTagAttributes() 
                }
                else
                {
                    writeOut("<" + r_node.getTagName() + " " + r_node.getTagAttributes() + ">", true);
                }
            }
            
            if (r_node.getTagValue() != null)  /* if there exists a TagValue */
            {    
                for (int i = 0; i < r_node.getDepth() + 1; i++)
                {
                    writeOut("    ", false); 
                }

                writeOut(r_node.getTagValue(), true);
            }

            if (r_node != null) 
            {
                Console.WriteLine("!null"); //to check
                foreach (Node child in children)
                {
                    format(child); /* format each node if exists */
                }
            }
            Console.WriteLine("end"); //to check
            //if (r_node.getIsClosingTag() == false)
            //{
            //    for (int i = 0; i < r_node.getDepth(); i++)
            //    {
            //        writeOut("    ", false);
            //    }
            //    writeOut("</" + r_node.getTagName() + ">", true);
            //}
        }
    }
/*    public class DS_project
    {
        static int Main(String[] args)
        {
            string path = @"C:\Users\Carin\Desktop\sample.xml";
            string input = File.ReadAllText(@"C:\Users\Carin\Desktop\sample.xml");
            Console.WriteLine(input);
            Console.WriteLine(" INPUT FINISHED ");
            
            FormatXml xmlfile = new FormatXml(path,false);
            //Node r=new Node();
            //xmlfile.format(node);
            Console.WriteLine(" OUTPUT ");

            Tree xml_tree = new Tree();
            using (StreamReader reader2 = new StreamReader(path))
            {
                xml_tree.insertFile(reader2);
                reader2.Close();
            }
            xmlfile.format(xml_tree.getTreeRoot());
            return 0;
        }
    }*/
}
