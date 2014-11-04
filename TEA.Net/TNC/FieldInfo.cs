namespace TEAC
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class FieldInfo
    {
        public string Name { get; set; }
        public TypeDefinition Type { get; set; }
        public bool IsStatic { get; set; }
        public bool IsPublic { get; set; }
        public bool IsProtected { get; set; }
        public int Offset { get; set; }
    }
}
