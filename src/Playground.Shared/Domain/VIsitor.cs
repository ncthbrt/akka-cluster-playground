using System;
using System.Collections.Generic;
using System.Text;

namespace Playground.Shared.Domain
{
    public class Visitor
    {
        public string Name { get; }

        public bool HasTicket { get; }

        public Visitor(string name, bool hasTicket = true)

        {
            Name = name;
            HasTicket = hasTicket;
        }
    }
}