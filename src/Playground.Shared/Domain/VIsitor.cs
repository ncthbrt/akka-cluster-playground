using System;
using System.Collections.Generic;
using System.Text;

namespace Playground.Shared.Domain
{
    public class Visitor
    {
        public string Name { get; }

        public Visitor(string name)

        {
            Name = name;
        }
    }
}