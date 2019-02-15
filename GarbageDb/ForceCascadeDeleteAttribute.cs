namespace GarbageDb {
    using System;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ForceCascadeDeleteAttribute : Attribute {
        public string Name { get; }
        public ForceCascadeDeleteAttribute(string name) => Name = name;
    }
}
