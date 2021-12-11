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
namespace XMLEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            outputField.Text = "hi i am a test string";
        }


        private void Browse_Click(object sender, RoutedEventArgs e)
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

            // load the XML file content from the file the user selected, we will need the file path of the selected one.

            string fileContent = File.ReadAllText(filePath); // fileContent contains the content of our selected file finally..

            // we then want to change the content of the input GUI element into the fileContent...

            inputField.Text = fileContent;
        }


        // want to be able to save the file as xml file or JSON file
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Title = "Save XML & JSON Files";
            saveFileDialog1.DefaultExt = "xml";
            saveFileDialog1.Filter = "XML files (*.xml)|*.xml|JSON Files (*.json*)|*.json*";
            saveFileDialog1.ShowDialog(this);
            using (Stream s = File.Open(saveFileDialog1.FileName, FileMode.CreateNew))
            using (StreamWriter sw = new StreamWriter(s))
            {
                sw.Write(outputField.Text);
            }
        }

        // this button is to detect and fix the errors in the XML file
        private void Fix_Click(object sender, RoutedEventArgs e)
        {

        }

        // this button is to format and add indentation for the XML file
        private void Format_Click(object sender, RoutedEventArgs e)
        {

        }

        // this button is for converting the XML into JSON
        private void JSON_Click(object sender, RoutedEventArgs e)
        {

        }

        // this buttin is to Compress the XML file size
        private void Compress_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
