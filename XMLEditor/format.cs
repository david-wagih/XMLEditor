using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace XML_Editor
{
    class FormatXml
    {
        /* class members */
        public string XmlFileName;
        public bool firstTime; 

        /* constructor */
        public FormatXml(string file_name,bool first_time)  
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
                    sw.Write(str);    /* print data without new line */
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

            /* to avoid errors of empty node */
            if (r_node == null)
            {
                return;
            }

            for (int i = 0; i < r_node.getDepth(); i++)  /* loop until the depth of each node to indent */
            {
                writeOut("    ", false); /* indent without a new line */
            }
            
            /* to print open tags */
            if (r_node.getTagAttributes() == null) /* if there's no attributes */
            {
                    writeOut("<" + r_node.getTagName() + ">", true);
            }
            
            else  /* if the tag has an attribute */
            {
                    writeOut("<" + r_node.getTagName() + " " + r_node.getTagAttributes() + ">", true);
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
                foreach (Node child in children)
                {
                    format(child); /* format each child node if exists */
                }
            }
            
            for (int i = 0; i < r_node.getDepth(); i++)  /* loop until the depth of each node to indent */
            {
                writeOut("    ", false); /* indent without a new line */
            }

            /* to print the closing tags */
            writeOut("<" + "/" + r_node.getTagName() + ">", true); 
        }
    }
}
