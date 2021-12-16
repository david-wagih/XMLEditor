using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLEditor
{

   public  class Node
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
}