using Playground.Shared.Domain;

namespace Playground.Protocol
{
    public static class VisitorProtocol
    {
        public abstract class VisitorMessage
        {
            public string Name { get; private set; }

            public VisitorMessage(string name)
            {
                Name = name;
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
            public VisitorCreationResponseMessage(string name)
                : base(name)
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
        }

        public class FoundVisitorResponse : FindVisitorResponseMessage
        {
            public FoundVisitorResponse(Visitor visitor)
                : base(visitor.Name)
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
            public VisitorAdded(string name)
                : base(name)
            {
            }
        }

        public class VisitorAlreadyAdded : VisitorCreationResponseMessage
        {
            public VisitorAlreadyAdded(string name)
                : base(name)
            {
            }
        }
    }
}