﻿using Microsoft.Win32;
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

        }
    }
}
