using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace NCMDEFEditor
{
    class SaveFile
    {
        public string FileName { get; set; }
        public string SafeFileName { get; set; }
        public bool Result { get; set; }

        public SaveFile(SectionsData sectionsData, string fileName, bool isNewFile)
        {
            Result = false;
            if (isNewFile)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    DefaultExt = ".sm3",
                    Filter = "DEF 1|*.sm1|DEF 2|*.sm2|DEF 3|*.sm3|DEF 4|*.sm4|DEF 5|*.sm5",
                    FilterIndex = 3
                };
                if (fileName != "")
                    saveFileDialog.FileName = new FileInfo(fileName).Name;
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    fileName = saveFileDialog.FileName;
                else
                    return;
                
            }
            using (StreamWriter streamWriter = new StreamWriter(fileName, false, System.Text.Encoding.Default))
            {
                streamWriter.WriteLine("//");
                streamWriter.WriteLine("//");
                streamWriter.WriteLine();

                SaveSectionGeneral(sectionsData.SectionGeneral, streamWriter);
                SaveSectionWordReplacement(sectionsData.SectionWordReplacement, streamWriter);
                SaveSectionWordDefinition(sectionsData.SectionWordDefinition, streamWriter);
                SaveSectionFunctionDefinition(sectionsData.SectionFunctionDefinition, streamWriter);
                SaveSectionMiscFunctionDefinition(sectionsData.SectionMiscFunctionDefinition, streamWriter);
                SaveSectionOthers(sectionsData.SectionOthers, streamWriter);
                SaveSectionCycleDefinition(streamWriter);

                streamWriter.Close();
            }
            Result = true;
            FileName = fileName;
            SafeFileName = new FileInfo(fileName).Name;
        }
        private void SaveSectionGeneral(List<SectionGeneral> sectionGeneral, StreamWriter streamWriter)
        {
            streamWriter.WriteLine("// Section General");
            for (int i = 0; i < sectionGeneral.Count; i++)
            {
                streamWriter.WriteLine(sectionGeneral[i].Name + "\t" + sectionGeneral[i].Value);
            }
            streamWriter.WriteLine("// end section");
            streamWriter.WriteLine();
        }
        private void SaveSectionWordReplacement(List<SectionWordReplacement> sectionWordReplacement, StreamWriter streamWriter)
        {
            streamWriter.WriteLine("// Section Word Replacement");
            for (int i = 0; i < sectionWordReplacement.Count; i++)
            {
                streamWriter.WriteLine(sectionWordReplacement[i].Operation + "\t\"" + sectionWordReplacement[i].Expression1 + "\"\t\"" + sectionWordReplacement[i].Expression2 + "\"");
            }
            streamWriter.WriteLine("// end section");
            streamWriter.WriteLine();
        }
        private void SaveSectionWordDefinition(List<SectionWordDefinition> sectionWordDefinition, StreamWriter streamWriter)
        {
            streamWriter.WriteLine("// Section Word Definition");
            for (int i = 0; i < sectionWordDefinition.Count; i++)
            {
                streamWriter.WriteLine(sectionWordDefinition[i].Keyword + "\t" + sectionWordDefinition[i].Symbol);
            }
            streamWriter.WriteLine("// end section");
            streamWriter.WriteLine();
        }
        private void SaveSectionFunctionDefinition(List<SectionFunctionDefinition> sectionFunctionDefinition, StreamWriter streamWriter)
        {
            streamWriter.WriteLine("// Section Function Definition");
            for (int i = 0; i < sectionFunctionDefinition.Count; i++)
            {
                streamWriter.WriteLine(sectionFunctionDefinition[i].Keyword + "\t" + sectionFunctionDefinition[i].PreparatoryGroupNumber + "\t1\tN");
            }
            streamWriter.WriteLine("// end section");
            streamWriter.WriteLine();
        }
        private void SaveSectionMiscFunctionDefinition(List<SectionMiscFunctionDefinition> sectionMiscFunctionDefinition, StreamWriter streamWriter)
        {
            streamWriter.WriteLine("// Section Misc Function Definition");
            for (int i = 0; i < sectionMiscFunctionDefinition.Count; i++)
            {
                streamWriter.WriteLine(sectionMiscFunctionDefinition[i].Keyword + "\t" + sectionMiscFunctionDefinition[i].PreparatoryGroupNumber);
            }
            streamWriter.WriteLine("// end section");
            streamWriter.WriteLine();
        }
        private void SaveSectionOthers(List<SectionOthers> sectionOthers, StreamWriter streamWriter)
        {
            streamWriter.WriteLine("// Section Others");
            for (int i = 0; i < sectionOthers.Count; i++)
            {
                streamWriter.WriteLine(sectionOthers[i].Keyword + "\t" + sectionOthers[i].Value);
            }
            streamWriter.WriteLine("// end section");
            streamWriter.WriteLine();
        }
        private void SaveSectionCycleDefinition(StreamWriter streamWriter)
        {
            streamWriter.WriteLine("// Section Cycle Definition");
            streamWriter.WriteLine("// end section");
        }
    }
}
