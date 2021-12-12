/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;
using System.Diagnostics;
namespace XMLEditor
{

    class HuffmanNode : IComparable<HuffmanNode>
    {
        public string symbol;   // For the character of char value. Public because Process class use it.
        public int frequency;          // Number of the count on file, string, text.
        public string code;            // Getting from a tree for making a huffman tree.
        public HuffmanNode parentNode; // Parent Node of current Node.
        public HuffmanNode leftTree;   // Left Node of current Node.
        public HuffmanNode rightTree;  // Right Node of current Node.
        public bool isLeaf;            // Shows it is a leaf.


        public HuffmanNode(string value)    // Creating a Node with given value(character).
        {
            symbol = value;     // Setting the symbol.
            frequency = 1;      // This is creation of Node, so now its count is 1.

            rightTree = leftTree = parentNode = null;       // Does not have a left or right tree and a parent.

            code = "";          // It will be Assigned on the making Tree. Now it is empty.
            isLeaf = true;      // Because all Node we create first does not have a parent Node.
        }


        public HuffmanNode(HuffmanNode node1, HuffmanNode node2) // Join the 2 Node to make Node.
        {
            // Firsly we are adding this 2 Nodes' variables. Except the new Node's left and right tree.
            code = "";
            isLeaf = false;
            parentNode = null;

            // Now the new Node need leaf. They are node1 and node2. if node1's frequency is bigger than or equal to node2's frequency. It is right tree. Otherwise left tree. The controllers are below:
            if (node1.frequency >= node2.frequency)
            {
                rightTree = node1;
                leftTree = node2;
                rightTree.parentNode = leftTree.parentNode = this;     // "this" means the new Node!
                symbol = node1.symbol + node2.symbol;
                frequency = node1.frequency + node2.frequency;
            }
            else if (node1.frequency < node2.frequency)
            {
                rightTree = node2;
                leftTree = node1;
                leftTree.parentNode = rightTree.parentNode = this;     // "this" means the new Node!
                symbol = node2.symbol + node1.symbol;
                frequency = node2.frequency + node1.frequency;
            }
        }


        public int CompareTo(HuffmanNode otherNode) // We just override the CompareTo method. Because when we compare two Node, it must be according to frequencies of the Nodes.
        {
            return this.frequency.CompareTo(otherNode.frequency);
        }


        public void frequencyIncrease()             // When facing a same value on the Node list, it is increasing the frequency of the Node.
        {
            frequency++;
        }
    }

    class ProcessMethods
    {
        //  Creates a Node List that reading the characters on the file.
        public List<HuffmanNode> getListFromFile()
        {
            List<HuffmanNode> nodeList = new List<HuffmanNode>();  // Node List.

            Test.setColor();
            Console.WriteLine("Example file: \"a.txt\"\n");
            Test.setColorDefault();
            Console.Write("Enter the path of the file: ");
            String filename = Console.ReadLine();




            try
            {
                // Creating a new unique node that reading from the file.
                // If it is the same character, increase the frequency of the value. It is possiple with "frequencyIncreas()" method.
                FileStream stream = new FileStream(filename, FileMode.Open, FileAccess.Read);
                for (int i = 0; i < stream.Length; i++)
                {
                    string read = Convert.ToChar(stream.ReadByte()).ToString();
                    if (nodeList.Exists(x => x.symbol == read)) // Checking the value that have you created before?
                        nodeList[nodeList.FindIndex(y => y.symbol == read)].frequencyIncrease(); // If is yes, find the index of the Node and increase the frequency of the Node.
                    else
                        nodeList.Add(new HuffmanNode(read));   // If is no, create a new node and add to the List of Nodes
                }
                nodeList.Sort();   // Sort the Nodes on the List according to their frequency value.
                return nodeList;
            }
            catch (Exception)
            {
                return null;
            }

        }


        //  Creates a Tree according to Nodes(frequency, symbol)
        public void getTreeFromList(List<HuffmanNode> nodeList)
        {
            while (nodeList.Count > 1)  // 1 because a tree need 2 leaf to make a new parent.
            {
                HuffmanNode node1 = nodeList[0];    // Get the node of the first index of List,
                nodeList.RemoveAt(0);               // and delete it.
                HuffmanNode node2 = nodeList[0];    // Get the node of the first index of List,
                nodeList.RemoveAt(0);               // and delete it.
                nodeList.Add(new HuffmanNode(node1, node2));    // Sending the constructor to make a new Node from this nodes.
                nodeList.Sort();        // and sort it again according to frequency.
            }
        }


        // Setting the codes of the nodes of tree. Recursive method.
        public void setCodeToTheTree(string code, HuffmanNode Nodes)
        {
            if (Nodes == null)
                return;
            if (Nodes.leftTree == null && Nodes.rightTree == null)
            {
                Nodes.code = code;
                return;
            }
            setCodeToTheTree(code + "0", Nodes.leftTree);
            setCodeToTheTree(code + "1", Nodes.rightTree);
        }




        // Printing all Tree! Recursive method.
        public void PrintTree(int level, HuffmanNode node)
        {
            if (node == null)
                return;
            for (int i = 0; i < level; i++)
            {
                Console.Write("\t");
            }
            Console.Write("[" + node.symbol + "]");
            Test.setColor();
            Console.WriteLine("(" + node.code + ")");
            Test.setColorDefault();
            PrintTree(level + 1, node.rightTree);
            PrintTree(level + 1, node.leftTree);
        }


        //  Printing the Node's information on the nodeList
        public void PrintInformation(List<HuffmanNode> nodeList)
        {
            foreach (var item in nodeList)
                Console.WriteLine("Symbol : {0} - Frequency : {1}", item.symbol, item.frequency);
        }


        // Printing the symbols and codes of the Nodes on the tree.
        public void PrintfLeafAndCodes(HuffmanNode nodeList)
        {
            if (nodeList == null)
                return;
            if (nodeList.leftTree == null && nodeList.rightTree == null)
            {
                Console.WriteLine("Symbol : {0} -  Code : {1}", nodeList.symbol, nodeList.code);
                return;
            }
            PrintfLeafAndCodes(nodeList.leftTree);
            PrintfLeafAndCodes(nodeList.rightTree);
        }

    }
    class Test
    {
        // Test class provide us to test our program.
        static void Main(string[] args)
        {
            Console.Title = "Huffman Code with File, by bilalarslan"; // Setting the Console name.
            List<HuffmanNode> nodeList; // store nodes on List.

            ProcessMethods pMethods = new ProcessMethods();


            while (true)
            {
                Console.Clear();
                nodeList = pMethods.getListFromFile();
                Console.Clear();
                if (nodeList == null)
                {
                    setColor();
                    Console.WriteLine("File cannot be read!");
                    Console.WriteLine("Pressthe any key to continue or Enter \"E\" to exit the program.");
                    setColorDefault();
                    string choise = Console.ReadLine();
                    if (choise.ToLower() == "e")
                        break;
                    else
                        continue;
                }
                else
                {
                    Console.Clear();
                    setColor();
                    Console.WriteLine("#Symbols   -   #Frequency");
                    setColorDefault();
                    pMethods.PrintInformation(nodeList);
                    pMethods.getTreeFromList(nodeList);
                    pMethods.setCodeToTheTree("", nodeList[0]);
                    Console.WriteLine("\n\n");
                    setColor();
                    Console.WriteLine(" #   Huffman Code Tree   # \n");
                    setColorDefault();
                    pMethods.PrintTree(0, nodeList[0]);
                    setColor();
                    Console.WriteLine("\n\n#Symbols    -    #Codes\n");
                    setColorDefault();
                    pMethods.PrintfLeafAndCodes(nodeList[0]);
                    Console.WriteLine("\n\n");
                    setColor();
                    Console.WriteLine("Press the any key to contunie");
                    Console.WriteLine("Enter the\"E\" to exit the program.");
                    setColorDefault();
                    string choise = Console.ReadLine();
                    if (choise.ToLower() == "e")
                        break;
                    else
                        continue;

                }
            }
        }


        // These are methods that to change the color of the console secren. These are public because they must be accessible from ProcessMethods class. Instant method.
        public static void setColor()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
        }

        public static void setColorDefault()
        {
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
*/