using Playground.Shared.Domain;

namespace Playground.Protocol
{
    public static class VisitorProtocol
    {
        public abstract class VisitorMessage
        {
            public string Name { get; private set; }

            public bool HasTicket { get; private set; }

            public VisitorMessage(Visitor visitor)
            {
                if (visitor is null)
                {
                    Name = "";
                    HasTicket = false;
                }
                else
                {
                    Name = visitor.Name;
                    HasTicket = visitor.HasTicket;
                }
            }

            public VisitorMessage(string name, bool hasTicket = true)
            {
                Name = name;
                HasTicket = hasTicket;
            }
        }

        public class AddVisitor : VisitorMessage
        {
            public AddVisitor(string name)
                : base(name)
            {
            }
        }

        public abstract class VisitorCreationResponseMessage : VisitorMessage
        {
            public VisitorCreationResponseMessage(Visitor visitor)
                : base(visitor)
            {
            }
        }

        public class FindVisitor : VisitorMessage
        {
            public FindVisitor(string name)
                : base(name)
            {
            }
        }

        public abstract class FindVisitorResponseMessage : VisitorMessage
        {
            public FindVisitorResponseMessage(string name)
                : base(name)
            {
            }

            public FindVisitorResponseMessage(Visitor visitor)
                : base(visitor)
            {
            }
        }

        public class FoundVisitorResponse : FindVisitorResponseMessage
        {
            public FoundVisitorResponse(Visitor visitor)
                : base(visitor)
            {
            }
        }

        public class CouldNotFindVisitorResponse : FindVisitorResponseMessage
        {
            public CouldNotFindVisitorResponse(string name)
                : base(name)
            {
            }
        }

        public class VisitorAdded : VisitorCreationResponseMessage
        {
            public VisitorAdded(Visitor visitor)
                : base(visitor)
            {
            }
        }

        public class VisitorAlreadyAdded : VisitorCreationResponseMessage
        {
            public VisitorAlreadyAdded(Visitor visitor)
                : base(visitor)
            {
            }
        }
    }
}