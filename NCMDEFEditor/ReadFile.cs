using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace NCMDEFEditor
{
    class ReadFile
    {
        bool isComment = false;
        public SectionsData SectionsData { get; set; }
        public bool Result { get; set; }

        public ReadFile(string[] fileText)
        {
            SectionsData = new SectionsData();
            Result = true;

            ReadSectionGeneral(fileText);
            if (Result == false)
                return;

            ReadSectionWordReplacement(fileText);
            if (Result == false)
                return;

            ReadSectionWordDefinition(fileText);
            if (Result == false)
                return;

            ReadSectionFunctionDefinition(fileText);
            if (Result == false)
                return;

            ReadSectionMiscFunctionDefinition(fileText);
            if (Result == false)
                return;

            ReadSectionOthers(fileText);

            return;
        }
        private void ReadSectionGeneral(string[] fileText)
        {
            int sectionID = Array.IndexOf(fileText, "// Section General");

            if (sectionID == -1)
            {
                MessageBox.Show(Resources.Res.sectionGeneralNotFound, Resources.Res.infoHeader, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (sectionID == fileText.Length - 1)
            {
                MessageBox.Show(Resources.Res.IncorrentFile + "\r" + Resources.Res.errorBeginSectionInEndFileSectionGeneral + "\r" + Resources.Res.readingIsImpossible, Resources.Res.errorHeader, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Result = false;
                return;
            }
            else
            {
                for (int i = sectionID + 1; i < fileText.Length; i++)
                {
                    if (fileText[i] == "// end section")
                        break;
                    else if (i == fileText.Length - 1)
                    {
                        MessageBox.Show(Resources.Res.IncorrentFile + "\r" + Resources.Res.errorEndSectionNotFound + "\r" + Resources.Res.readingIsImpossible, Resources.Res.errorHeader, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Result = false;
                        return;
                    }
                    else if (fileText[i] == "")
                        continue;
                    else if (!CommentString(fileText[i], "Section General"))
                    {
                        if (!isComment)
                        {
                            Result = false;
                            return;
                        }
                        isComment = false;
                    }
                    else
                    {
                        var match = Regex.Match(fileText[i], @"^\s*(\S+)\s+(.*)\s*$");
                        if (match.Success == true)
                        {
                            var item = new { name = match.Groups[1].Value, value = match.Groups[2].Value };

                            SectionGeneral Example = new SectionGeneral
                            {
                                Name = item.name,
                                Value = (item.value == "") ? "" : item.value
                            };

                            SectionsData.SectionGeneral.Add(Example);
                        }
                    }
                }
            }
        }
        private void ReadSectionWordReplacement(string[] fileText)
        {
            int sectionID = Array.IndexOf(fileText, "// Section Word Replacement");

            if (sectionID == -1)
            {
                MessageBox.Show(Resources.Res.sectionWordReplacementNotFound, Resources.Res.infoHeader, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (sectionID == fileText.Length - 1)
            {
                MessageBox.Show(Resources.Res.IncorrentFile + "\r" + Resources.Res.errorBeginSectionInEndFileSectionWordReplacement + "\r" + Resources.Res.readingIsImpossible, Resources.Res.errorHeader, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Result = false;
                return;
            }
            else
            {
                for (int i = sectionID + 1; i < fileText.Length; i++)
                {
                    if (fileText[i] == "// end section")
                        break;
                    else if (i == fileText.Length - 1)
                    {
                        MessageBox.Show(Resources.Res.IncorrentFile + "\r" + Resources.Res.errorEndSectionNotFound + "\r" + Resources.Res.readingIsImpossible, Resources.Res.errorHeader, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Result = false;
                        return;
                    }
                    else if (fileText[i] == "")
                        continue;
                    else if (!CommentString(fileText[i], "Section Word Replacement"))
                    {
                        if (!isComment)
                        {
                            Result = false;
                            return;
                        }
                        isComment = false;
                    }
                    else
                    {
                        var match = Regex.Match(fileText[i], @"^\s*(\S+)\s+""([^""]*)""\s+""([^""]*)""\s*$");
                        if (match.Success == true)
                        {
                            var item = new { operation = match.Groups[1].Value, expression1 = match.Groups[2].Value, expression2 = match.Groups[3].Value };

                            SectionWordReplacement Example = new SectionWordReplacement
                            {
                                Operation = item.operation,
                                Expression1 = (item.expression1 == "") ? "" : item.expression1,
                                Expression2 = (item.expression2 == "") ? "" : item.expression2
                            };

                            SectionsData.SectionWordReplacement.Add(Example);
                        }
                    }
                }
            }
        }
        private void ReadSectionWordDefinition(string[] fileText)
        {
            int sectionID = Array.IndexOf(fileText, "// Section Word Definition");

            if (sectionID == -1)
            {
                MessageBox.Show(Resources.Res.sectionWordDefinitionNotFound, Resources.Res.infoHeader, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (sectionID == fileText.Length - 1)
            {
                MessageBox.Show(Resources.Res.IncorrentFile + "\r" + Resources.Res.errorBeginSectionInEndFileSectionWordDefinition + "\r" + Resources.Res.readingIsImpossible, Resources.Res.errorHeader, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Result = false;
                return;
            }
            else
            {
                for (int i = sectionID + 1; i < fileText.Length; i++)
                {
                    if (fileText[i] == "// end section")
                        break;
                    else if (i == fileText.Length - 1)
                    {
                        MessageBox.Show(Resources.Res.IncorrentFile + "\r" + Resources.Res.errorEndSectionNotFound + "\r" + Resources.Res.readingIsImpossible, Resources.Res.errorHeader, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Result = false;
                        return;
                    }
                    else if (fileText[i] == "")
                        continue;
                    else if (!CommentString(fileText[i], "Section Word Definition"))
                    {
                        if (!isComment)
                        {
                            Result = false;
                            return;
                        }
                        isComment = false;
                    }
                    else
                    {
                        var match = Regex.Match(fileText[i], @"^\s*(\S+)\s+(.*)\s*$");
                        if (match.Success == true)
                        {
                            var item = new { keyword = match.Groups[1].Value, symbol = match.Groups[2].Value };
                            SectionWordDefinition Example = new SectionWordDefinition
                            {
                                Keyword = item.keyword,
                                Symbol = (item.symbol == "") ? "" : item.symbol
                            };

                            SectionsData.SectionWordDefinition.Add(Example);
                        }
                    }
                }
            }
        }
        private void ReadSectionFunctionDefinition(string[] fileText)
        {
            int sectionID = Array.IndexOf(fileText, "// Section Function Definition");

            if (sectionID == -1)
            {
                MessageBox.Show(Resources.Res.sectionFunctionDefinitionNotFound, Resources.Res.infoHeader, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (sectionID == fileText.Length - 1)
            {
                MessageBox.Show(Resources.Res.IncorrentFile + "\r" + Resources.Res.errorBeginSectionInEndFileSectionFunctionDefinition + "\r" + Resources.Res.readingIsImpossible, Resources.Res.errorHeader, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Result = false;
                return;
            }
            else
            {
                for (int i = sectionID + 1; i < fileText.Length; i++)
                {
                    if (fileText[i] == "// end section")
                        break;
                    else if (i == fileText.Length - 1)
                    {
                        MessageBox.Show(Resources.Res.IncorrentFile + "\r" + Resources.Res.errorEndSectionNotFound + "\r" + Resources.Res.readingIsImpossible, Resources.Res.errorHeader, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Result = false;
                        return;
                    }
                    else if (fileText[i] == "")
                        continue;
                    else if (!CommentString(fileText[i], "Section Function Definition"))
                    {
                        if (!isComment)
                        {
                            Result = false;
                            return;
                        }
                        isComment = false;
                    }
                    else
                    {
                        var match = Regex.Match(fileText[i], @"^\s*(\S+)\s+([0-9]*[.,]?[0-9]+)\s*.*");
                        if (match.Success == true)
                        {
                            var item = new { keyword = match.Groups[1].Value, preparatoryGroupNumber = match.Groups[2].Value };

                            SectionFunctionDefinition Example = new SectionFunctionDefinition
                            {
                                Keyword = item.keyword,
                                PreparatoryGroupNumber = (item.preparatoryGroupNumber == "") ? "" : item.preparatoryGroupNumber
                            };

                            SectionsData.SectionFunctionDefinition.Add(Example);
                        }
                    }
                }
            }
        }
        private void ReadSectionMiscFunctionDefinition(string[] fileText)
        {
            int sectionID = Array.IndexOf(fileText, "// Section Misc Function Definition");

            if (sectionID == -1)
            {
                MessageBox.Show(Resources.Res.sectionMiscFunctionDefinitionNotFound, Resources.Res.infoHeader, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (sectionID == fileText.Length - 1)
            {
                MessageBox.Show(Resources.Res.IncorrentFile + "\r" + Resources.Res.errorBeginSectionInEndFileSectionMiscFunctionDefinition + "\r" + Resources.Res.readingIsImpossible, Resources.Res.errorHeader, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Result = false;
                return;
            }
            else
            {
                for (int i = sectionID + 1; i < fileText.Length; i++)
                {
                    if (fileText[i] == "// end section")
                        break;
                    else if (i == fileText.Length - 1)
                    {
                        MessageBox.Show(Resources.Res.IncorrentFile + "\r" + Resources.Res.errorEndSectionNotFound + "\r" + Resources.Res.readingIsImpossible, Resources.Res.errorHeader, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Result = false;
                        return;
                    }
                    else if (fileText[i] == "")
                        continue;
                    else if (!CommentString(fileText[i], "Section Misc Function Definition"))
                    {
                        if (!isComment)
                        {
                            Result = false;
                            return;
                        }
                        isComment = false;
                    }
                    else
                    {
                        var match = Regex.Match(fileText[i], @"^\s*(\S+)\s+([0-9]*[.,]?[0-9]+)");
                        if (match.Success == true)
                        {
                            var item = new { keyword = match.Groups[1].Value, preparatoryGroupNumber = match.Groups[2].Value };

                            SectionMiscFunctionDefinition Example = new SectionMiscFunctionDefinition
                            {
                                Keyword = item.keyword,
                                PreparatoryGroupNumber = (item.preparatoryGroupNumber == "") ? "" : item.preparatoryGroupNumber
                            };

                            SectionsData.SectionMiscFunctionDefinition.Add(Example);
                        }
                    }
                }
            }
        }
        private void ReadSectionOthers(string[] fileText)
        {
            int sectionID = Array.IndexOf(fileText, "// Section Others");

            if (sectionID == -1)
            {
                MessageBox.Show(Resources.Res.sectionOthersNotFound, Resources.Res.infoHeader, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (sectionID == fileText.Length - 1)
            {
                MessageBox.Show(Resources.Res.IncorrentFile + "\r" + Resources.Res.errorBeginSectionInEndFileSectionOthers + "\r" + Resources.Res.readingIsImpossible, Resources.Res.errorHeader, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Result = false;
                return;
            }
            else
            {
                for (int i = sectionID + 1; i < fileText.Length; i++)
                {
                    if (fileText[i] == "// end section")
                        break;
                    else if (i == fileText.Length - 1)
                    {
                        MessageBox.Show(Resources.Res.IncorrentFile + "\r" + Resources.Res.errorEndSectionNotFound + "\r" + Resources.Res.readingIsImpossible, Resources.Res.errorHeader, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Result = false;
                        return;
                    }
                    else if (fileText[i] == "")
                        continue;
                    else if (!CommentString(fileText[i], "Section Others"))
                    {
                        if (!isComment)
                        {
                            Result = false;
                            return;
                        }
                        isComment = false;
                    }
                    else
                    {
                        var match = Regex.Match(fileText[i], @"^\s*(\S+)\s+(.*)\s*$");
                        if (match.Success == true)
                        {
                            var item = new { keyword = match.Groups[1].Value, value = match.Groups[2].Value };

                            SectionOthers Example = new SectionOthers
                            {
                                Keyword = item.keyword,
                                Value = (item.value == "") ? "" : item.value
                            };

                            SectionsData.SectionOthers.Add(Example);
                        }
                    }
                }
            }
        }
        private bool CommentString(string fileTextI, string section)
        {
            if (fileTextI.Length >= 2 && fileTextI.Substring(0, 2) == "//")
            {
                if (fileTextI == "// Section General")
                {
                    MessageBox.Show(Resources.Res.IncorrentFile + "\r" + Resources.Res.errorFileString + section + Resources.Res.errorFileStringSectionGeneral + "\r" + Resources.Res.readingIsImpossible, Resources.Res.errorHeader, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                else if (fileTextI == "// Section Word Replacement")
                {
                    MessageBox.Show(Resources.Res.IncorrentFile + "\r" + Resources.Res.errorFileString + section + Resources.Res.errorFileStringSectionWordReplacement + "\r" + Resources.Res.readingIsImpossible, Resources.Res.errorHeader, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                else if (fileTextI == "// Section Word Definition")
                {
                    MessageBox.Show(Resources.Res.IncorrentFile + "\r" + Resources.Res.errorFileString + section + Resources.Res.errorFileStringSectionWordDefinition + "\r" + Resources.Res.readingIsImpossible, Resources.Res.errorHeader, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                else if (fileTextI == "// Section Function Definition")
                {
                    MessageBox.Show(Resources.Res.IncorrentFile + "\r" + Resources.Res.errorFileString + section + Resources.Res.errorFileStringSectionFunctionDefinition + "\r" + Resources.Res.readingIsImpossible, Resources.Res.errorHeader, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                else if (fileTextI == "// Section Misc Function Definition")
                {
                    MessageBox.Show(Resources.Res.IncorrentFile + "\r" + Resources.Res.errorFileString + section + Resources.Res.errorFileStringSectionMiscFunctionDefinition + "\r" + Resources.Res.readingIsImpossible, Resources.Res.errorHeader, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                else if (fileTextI == "// Section Others")
                {
                    MessageBox.Show(Resources.Res.IncorrentFile + "\r" + Resources.Res.errorFileString + section + Resources.Res.errorFileStringSectionOthers + "\r" + Resources.Res.readingIsImpossible, Resources.Res.errorHeader, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                else
                {
                    MessageBox.Show(Resources.Res.commentString1 + section + Resources.Res.commentString2 + fileTextI + "\".\r" + Resources.Res.commentString3, Resources.Res.infoHeader, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    isComment = true;
                    return false;
                }
            }
            else
                return true;
        }
    }
}
