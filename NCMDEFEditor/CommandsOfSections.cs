using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCMDEFEditor
{
    class CommandsOfSections
    {
        public CommandsOfSections()
        {
            SectionGeneral = new List<Command>();
            SectionWordReplacement = new List<Command>();
            SectionWordDefinition = new List<Command>();
            SectionFunctionDefinition = new List<Command>();
            SectionMiscFunctionDefinition = new List<Command>();
            SectionOthers = new List<Command>();
        }
        public List<Command> SectionGeneral { get; set; }
        public List<Command> SectionWordReplacement { get; set; }
        public List<Command> SectionWordDefinition { get; set; }
        public List<Command> SectionFunctionDefinition { get; set; }
        public List<Command> SectionMiscFunctionDefinition { get; set; }
        public List<Command> SectionOthers { get; set; }
    }
}
