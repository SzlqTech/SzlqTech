
using SzlqTech.Common.EnumType;

namespace SzlqTech.Common.Views
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = false)]
    public class FuncAttribute : Attribute
    {
        // public FuncAttribute() { }

        public FuncAttribute(string text)
        {
            this.Text = text;
        }

        public FuncAttribute(string text, EntryType entryType, int ordinal)
        {
            this.Text = text;
            this.EntryType = entryType;
            this.Ordinal = ordinal;
        }

        public FuncAttribute(string text, string description, string group)
        {
            this.Text = text;
            this.Description = description;
            this.Group = group;
        }

        public FuncAttribute(string text, string description)
        {
            this.Text = text;
            this.Description = description;
        }

        public EntryType EntryType { get; set; } = EntryType.Button;

        public string Text { get; set; }

        public string? TextEN { get; set; }

        public string? TextZH { get; set; }

        public string? Description { get; set; }

        public string? Group { get; set; }


        public string? ViewKey { get; set; }

        public int Ordinal { get; set; }

        // public bool Async { get; set; }

        internal FuncStrip ToFunction()
        {
            FuncStrip func = new FuncStrip
            {
                Ordinal = Ordinal,
                Text = Text
            };
            return func;
        }

    }
}
