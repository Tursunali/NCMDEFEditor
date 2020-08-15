using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NCMDEFEditor
{
    class LoadCommands
    {
        private bool isComment;
        public bool Result { get; set; }
        public CommandsOfSections Commands { get; set; }

        public LoadCommands(string[] commandTexts)
        {
            Commands = new CommandsOfSections();
            Result = true;

            ReadCommands(commandTexts, "// Section General");
            if (Result == false)
                return;

            ReadCommands(commandTexts, "// Section Word Replacement");
            if (Result == false)
                return;

            ReadCommands(commandTexts, "// Section Word Definition");
            if (Result == false)
                return;

            ReadCommands(commandTexts, "// Section Function Definition");
            if (Result == false)
                return;

            ReadCommands(commandTexts, "// Section Misc Function Definition");
            if (Result == false)
                return;

            ReadCommands(commandTexts, "// Section Others");

            return;
        }

        private void ReadCommands(string[] commandTexts, string section)
        {
            int sectionID = Array.IndexOf(commandTexts, section);

            if (sectionID == -1)
            {
                MessageBox.Show(Resources.Res.cmdFileSectionXNotFound + section + "\"!", Resources.Res.infoHeader, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (sectionID == commandTexts.Length - 1)
            {
                MessageBox.Show(Resources.Res.cmdIncorrectFile + "\r" + Resources.Res.cmdFileErrorBeginSectionXInEnd1 + section + Resources.Res.cmdFileErrorBeginSectionXInEnd2 + "\r" + Resources.Res.readingIsImpossible, Resources.Res.errorHeader, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Result = false;
                return;
            }
            else
            {
                for (int i = sectionID + 1; i < commandTexts.Length; i++)
                {
                    if (commandTexts[i] == "// end section")
                        break;
                    else if (i == commandTexts.Length - 1)
                    {
                        MessageBox.Show(Resources.Res.cmdIncorrectFile + "\r" + Resources.Res.errorEndSectionNotFound + "\r" + Resources.Res.readingIsImpossible, Resources.Res.errorHeader, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Result = false;
                        return;
                    }
                    else if (commandTexts[i] == "")
                        continue;
                    else if (!CommentString(commandTexts[i], section))
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
                        var match = Regex.Match(commandTexts[i], @"^(\S+)\t+(.*)\t+(.*)\s*$");
                        if (match.Success == true)
                        {
                            var item = new { name = match.Groups[1].Value, symbol = match.Groups[2].Value, description = match.Groups[3].Value };

                            Command Example = new Command
                            {
                                Name = item.name,
                                Symbol = item.symbol,
                                Description = item.description,
                                IsAdded = false
                            };

                            if (section == "// Section General")
                                Commands.SectionGeneral.Add(Example);
                            else if (section == "// Section Word Replacement")
                                Commands.SectionWordReplacement.Add(Example);
                            else if (section == "// Section Word Definition")
                                Commands.SectionWordDefinition.Add(Example);
                            else if (section == "// Section Function Definition")
                                Commands.SectionFunctionDefinition.Add(Example);
                            else if (section == "// Section Misc Function Definition")
                                Commands.SectionMiscFunctionDefinition.Add(Example);
                            else
                                Commands.SectionOthers.Add(Example);
                        }
                    }
                }
            }
        }
        private bool CommentString(string commandTextsI, string section)
        {
            if (commandTextsI.Length >= 2 && commandTextsI.Substring(0, 2) == "//")
            {
                if (commandTextsI == "// Section General")
                {
                    MessageBox.Show(Resources.Res.cmdIncorrectFile + "\r" + Resources.Res.errorFileString + section + Resources.Res.errorFileStringSectionGeneral + "\r" + Resources.Res.readingIsImpossible, Resources.Res.errorHeader, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                else if (commandTextsI == "// Section Word Replacement")
                {
                    MessageBox.Show(Resources.Res.cmdIncorrectFile + "\r" + Resources.Res.errorFileString + section + Resources.Res.errorFileStringSectionWordReplacement + "\r" + Resources.Res.readingIsImpossible, Resources.Res.errorHeader, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                else if (commandTextsI == "// Section Word Definition")
                {
                    MessageBox.Show(Resources.Res.cmdIncorrectFile + "\r" + Resources.Res.errorFileString + section + Resources.Res.errorFileStringSectionWordDefinition + "\r" + Resources.Res.readingIsImpossible, Resources.Res.errorHeader, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                else if (commandTextsI == "// Section Function Definition")
                {
                    MessageBox.Show(Resources.Res.cmdIncorrectFile + "\r" + Resources.Res.errorFileString + section + Resources.Res.errorFileStringSectionFunctionDefinition + "\r" + Resources.Res.readingIsImpossible, Resources.Res.errorHeader, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                else if (commandTextsI == "// Section Misc Function Definition")
                {
                    MessageBox.Show(Resources.Res.cmdIncorrectFile + "\r" + Resources.Res.errorFileString + section + Resources.Res.errorFileStringSectionMiscFunctionDefinition + "\r" + Resources.Res.readingIsImpossible, Resources.Res.errorHeader, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                else if (commandTextsI == "// Section Others")
                {
                    MessageBox.Show(Resources.Res.cmdIncorrectFile + "\r" + Resources.Res.errorFileString + section + Resources.Res.errorFileStringSectionOthers + "\r" + Resources.Res.readingIsImpossible, Resources.Res.errorHeader, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                else
                {
                    MessageBox.Show(Resources.Res.cmdFileCommentString1 + section + Resources.Res.commentString2 + commandTextsI + "\".\r" + Resources.Res.commentString3, Resources.Res.infoHeader, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    isComment = true;
                    return false;
                }
            }
            else
                return true;
        }
    }
}
