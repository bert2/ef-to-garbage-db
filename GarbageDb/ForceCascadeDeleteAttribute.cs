namespace GarbageDb {
    using System;

    [AttributeUsage(AttributeTargets.Property)]
    public class ForceCascadeDeleteAttribute : Attribute { }
}
