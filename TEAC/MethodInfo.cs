namespace TEAC
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class MethodInfo
    {
        private List<ParameterInfo> parameters = new List<ParameterInfo>();

        public MethodInfo(TypeDefinition type)
        {
            this.Type = type;
        }

        public TypeDefinition Type { get; private set; }

        public List<ParameterInfo> Parameters
        {
            get { return this.parameters; }
        }

        public string Name { get; set; }
        public bool IsStatic { get; set; }
        public bool IsVirtual { get; set; }
        public bool IsPublic { get; set; }
        public bool IsProtected { get; set; }
        public TypeDefinition ReturnType { get; set; }

        public string MangledName
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(this.Type.MangledName);
                sb.AppendFormat("{0}{1}E", this.Name.Length, this.Name);
                if (this.Parameters.Count > 0)
                {
                    foreach (var parameter in this.Parameters)
                    {
                        sb.Append(parameter.Type.MangledName);
                    }
                }
                else
                {
                    sb.Append("v");
                }

                return sb.ToString();
            }
        }
    }
}
