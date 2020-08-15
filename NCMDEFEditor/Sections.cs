using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCMDEFEditor
{
    class SectionGeneral
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
    class SectionWordReplacement
    {
        public string Operation { get; set; }
        public string Expression1 { get; set; }
        public string Expression2 { get; set; }
    }
    class SectionWordDefinition
    {
        public string Keyword { get; set; }
        public string Symbol { get; set; }
    }
    class SectionFunctionDefinition
    {
        public string Keyword { get; set; }
        public string PreparatoryGroupNumber { get; set; }
    }
    class SectionMiscFunctionDefinition
    {
        public string Keyword { get; set; }
        public string PreparatoryGroupNumber { get; set; }
    }
    class SectionOthers
    {
        public string Keyword { get; set; }
        public string Value { get; set; }
    }
}
