using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Parserproject;

namespace ParserTest
{
    [TestFixture]
    class NodeTest
    {
        [Test]
        public void AddChild_isEmpty_HasOneChild()
        {
            var parent = new Node("");
            var child = new Node("");

            parent.AddChild(child);

            Assert.AreEqual(1, parent.Children.Count);
        }

        [Test]
        public void AddChild_isEmpty_SetsChildsParent()
        {
            var parent = new Node("parent");
            var child = new Node("child");

            parent.AddChild(child);

            Assert.AreEqual("parent", child.Parent.NodeLabel);
        }

        [Test]
        public void AddChild_isNotEmpty_appendsAtEnd()
        {
            var parent = new Node("parent");
            var child1 = new Node("first child");
            var child2 = new Node("second child");


            parent.AddChild(child1);
            parent.AddChild(child2);


            //Assert.AreEqual("second child", parent.Children[1].Type);
        }

    }
}
