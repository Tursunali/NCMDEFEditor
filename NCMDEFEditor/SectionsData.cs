using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCMDEFEditor
{
    class SectionsData
    {
        public SectionsData()
        {
            SectionGeneral = new List<SectionGeneral>();
            SectionWordReplacement = new List<SectionWordReplacement>();
            SectionWordDefinition = new List<SectionWordDefinition>();
            SectionFunctionDefinition = new List<SectionFunctionDefinition>();
            SectionMiscFunctionDefinition = new List<SectionMiscFunctionDefinition>();
            SectionOthers = new List<SectionOthers>();
        }
        public List<SectionGeneral> SectionGeneral { get; set; }
        public List<SectionWordReplacement> SectionWordReplacement { get; set; }
        public List<SectionWordDefinition> SectionWordDefinition { get; set; }
        public List<SectionFunctionDefinition> SectionFunctionDefinition { get; set; }
        public List<SectionMiscFunctionDefinition> SectionMiscFunctionDefinition { get; set; }
        public List<SectionOthers> SectionOthers { get; set; }
    }
}
