using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.IO;
using XML_Editor;
using System.Collections;
using System.Reflection;
using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.Layout.MDS;
using Microsoft.Msagl.WpfGraphControl;

namespace XMLEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // some variables to use through the program
        public string path = null;
        public static string directoryName = null;
        string Content = null;
        string outputEncoding = null;
        BitArray inputDecoding = null;
        HuffmanTree huffmanTree = new HuffmanTree();
        GraphViewer graphViewer = new GraphViewer();
        DockPanel graphViewerPanel = new DockPanel();


        // the main constructor
        public MainWindow()
        {
            InitializeComponent();
            
        }

        // this method is for choosing the XML file from your computer to perform operations on it
        public void Browse_Click(object sender, RoutedEventArgs e)
        {

            // open the dialogue to choose your XML File
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                Title = "Browse XML File",
                CheckFileExists = true,
                CheckPathExists = true,
                DefaultExt = "xml",
                Filter = "xml files (*.xml)|*.xml",

            };
            openFileDialog1.ShowDialog();
            string filePath = openFileDialog1.FileName; // getting the full file path of the selected XML file
            path = filePath;
            directoryName = System.IO.Path.GetDirectoryName(filePath);

            // load the XML file content from the file the user selected, we will need the file path of the selected one.

            string fileContent = File.ReadAllText(filePath); // fileContent contains the content of our selected file finally..
            Content = fileContent;
            // we then want to change the content of the input GUI element into the fileContent...

            inputField.Text = fileContent;
        }


        // this method is for saving the output of any operation in a new file with XML or JSON extensions
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Title = "Save XML & JSON Files";
            saveFileDialog1.DefaultExt = "xml";
            saveFileDialog1.Filter = "XML files (*.xml)|*.xml | Json files (*.json)|*.json";
            saveFileDialog1.ShowDialog(this);
            using (Stream s = File.Open(saveFileDialog1.FileName, FileMode.CreateNew))
            if(inputDecoding != null)
                {
                    using(BinaryWriter bw = new BinaryWriter(s))
                    {
                        
                        byte[] bytes = new byte[inputDecoding.Length / 8 + (inputDecoding.Length % 8 == 0 ? 0 : 1)];
                        inputDecoding.CopyTo(bytes, 0);
                        bw.Write(bytes, 0, bytes.Length);
                    }
                }
                else
                {
                    using (StreamWriter sw = new StreamWriter(s))
                    {
                        sw.Write(outputField.Text);
                    }
                }

        }




        // this button is to detect and fix the errors in the XML file
        private void Fix_Click(object sender, RoutedEventArgs e)
        {
            int num = 0;
            Queue errors = new Queue();
            Fix fix = new Fix();
            outputField.Text = string.Join("\n", fix.validator(ref num, ref errors,path));
            errosField.Text = "this file contains " + num + " errors" + "\n";
            while (errors.Count != 0)
            {
                errosField.Text += errors.Peek().ToString() + "\n";
                errors.Dequeue();
            }
        }

        private void graph_Click(object sender, RoutedEventArgs e)
        {
            /*Graph graph = new Graph();
                           graph.LayoutAlgorithmSettings=new MdsLayoutSettings();
             graph.AddNode("1");
             graph.AddNode("2");
             graph.AddNode("3");
                          graph.AddEdge("1", "2");
                           graph.AddEdge("1", "3");
                         graphViewer.Graph = graph;*/
            System.Windows.Forms.Form form = new System.Windows.Forms.Form();
            //create a viewer object 
            Microsoft.Msagl.GraphViewerGdi.GViewer viewer = new Microsoft.Msagl.GraphViewerGdi.GViewer();
            //create a graph object 
            Microsoft.Msagl.Drawing.Graph graph = new Microsoft.Msagl.Drawing.Graph("graph");
            //create the graph content 
            graph.AddEdge("1", "2");
            graph.AddEdge("1", "3");
            graph.AddEdge("2", "3");
            //bind the graph to the viewer 
            viewer.Graph = graph;
            //associate the viewer with the form 
            form.SuspendLayout();
            viewer.Dock = System.Windows.Forms.DockStyle.Fill;
            form.Controls.Add(viewer);
            form.ResumeLayout();
            //show the form 
            form.ShowDialog();
        }




        // here needs some improvement

        // this button is to format and add indentation for the XML file
        private void Format_Click(object sender, RoutedEventArgs e)
        {
            // creating the tree
            StreamReader sr = new StreamReader(path);
            Tree xml_tree = new Tree();
            xml_tree.insertFile(sr);
            //format using tree 
            StringBuilder sb = new StringBuilder();
            FormatXml xmlfile = new FormatXml(sb, false);
            xmlfile.format(xml_tree.getTreeRoot());
            outputField.Text = sb.ToString();
        }





        // this button is for converting the XML into JSON
        private void JSON_Click(object sender, RoutedEventArgs e)
        {
            // creating the tree
            StreamReader sr = new StreamReader(path);
            Tree tree = new Tree();
            tree.insertFile(sr);
            //convert to json using tree
            StringBuilder sb = new StringBuilder();
            ConvertToJSON j = new ConvertToJSON(sb, 0); // remaining is where to get the output :D
            j.Convert(tree.getTreeRoot());
            outputField.Text = sb.ToString();
        }




        // this buttin is to Compress the XML file size
        private void Compress_Click(object sender, RoutedEventArgs e)
        {
            // Build the Huffman tree
            huffmanTree.Build(Content);

            // Encode
            BitArray encoded = huffmanTree.Encode(Content);
            
            foreach (bool bit in encoded)
            {
                if (bit)
                {
                    outputEncoding += "1" + "";
                }
                else
                {
                    outputEncoding += "0" + "";
                }
            }
            outputField.Text = outputEncoding;
            inputDecoding = encoded;
        }



        // this method is for returning the file as it was before compressing.

        private void Decompress_Click(object sender, RoutedEventArgs e)
        {
            string decoded = huffmanTree.Decode(inputDecoding);
            outputField.Text = decoded;
            inputDecoding = null;
        }




        // this method is responsible for removing the spaces and indentation to minify the file.
        private void Minify_Click(object sender, RoutedEventArgs e)
        {
            outputField.Text = Minifying.CompactWhitespaces(Content);
        }

    }
}