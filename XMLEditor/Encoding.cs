using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;
using System.Diagnostics;
namespace XMLEditor
{

     public class Node1
    {
        public char Symbol { get; set; }
        public int Frequency { get; set; }
        public Node1 Right { get; set; }
        public Node1 Left { get; set; }

        public List<bool> Traverse(char symbol, List<bool> data)
        {
            // Leaf
            if (Right == null && Left == null)
            {
                if (symbol.Equals(this.Symbol))
                {
                    return data;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                List<bool> left = null;
                List<bool> right = null;

                if (Left != null)
                {
                    List<bool> leftPath = new List<bool>();
                    leftPath.AddRange(data);
                    leftPath.Add(false);

                    left = Left.Traverse(symbol, leftPath);
                }

                if (Right != null)
                {
                    List<bool> rightPath = new List<bool>();
                    rightPath.AddRange(data);
                    rightPath.Add(true);
                    right = Right.Traverse(symbol, rightPath);
                }

                if (left != null)
                {
                    return left;
                }
                else
                {
                    return right;
                }
            }
        }
    }


    public class HuffmanTree
        {
            private List<Node1> Node1s = new List<Node1>();
            public Node1 Root { get; set; }
            public Dictionary<char, int> Frequencies = new Dictionary<char, int>();

            public void Build(string source)
            {
                for (int i = 0; i < source.Length; i++)
                {
                    if (!Frequencies.ContainsKey(source[i]))
                    {
                        Frequencies.Add(source[i], 0);
                    }

                    Frequencies[source[i]]++;
                }

                foreach (KeyValuePair<char, int> symbol in Frequencies)
                {
                    Node1s.Add(new Node1() { Symbol = symbol.Key, Frequency = symbol.Value });
                }

                while (Node1s.Count > 1)
                {
                    List<Node1> orderedNode1s = Node1s.OrderBy(Node1 => Node1.Frequency).ToList<Node1>();

                    if (orderedNode1s.Count >= 2)
                    {
                        // Take first two items
                        List<Node1> taken = orderedNode1s.Take(2).ToList<Node1>();

                        // Create a parent Node1 by combining the frequencies
                        Node1 parent = new Node1()
                        {
                            Symbol = '*',
                            Frequency = taken[0].Frequency + taken[1].Frequency,
                            Left = taken[0],
                            Right = taken[1]
                        };

                        Node1s.Remove(taken[0]);
                        Node1s.Remove(taken[1]);
                        Node1s.Add(parent);
                    }

                    this.Root = Node1s.FirstOrDefault();

                }

            }

            public BitArray Encode(string source)
            {
                List<bool> encodedSource = new List<bool>();

                for (int i = 0; i < source.Length; i++)
                {
                    List<bool> encodedSymbol = this.Root.Traverse(source[i], new List<bool>());
                    encodedSource.AddRange(encodedSymbol);
                }

                BitArray bits = new BitArray(encodedSource.ToArray());

                return bits;
            }

            public string Decode(BitArray bits)
            {
                Node1 current = this.Root;
                string decoded = "";

                foreach (bool bit in bits)
                {
                    if (bit)
                    {
                        if (current.Right != null)
                        {
                            current = current.Right;
                        }
                    }
                    else
                    {
                        if (current.Left != null)
                        {
                            current = current.Left;
                        }
                    }

                    if (IsLeaf(current))
                    {
                        decoded += current.Symbol;
                        current = this.Root;
                    }
                }

                return decoded;
            }

            public bool IsLeaf(Node1 Node1)
            {
                return (Node1.Left == null && Node1.Right == null);
            }

        }
    
    }

