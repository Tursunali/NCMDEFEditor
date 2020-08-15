using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace NCMDEFEditor
{
    public partial class MainForm : RibbonForm
    {
        SectionsData sectionsData = new SectionsData();
        readonly CommandsOfSections commands = new CommandsOfSections();
        bool isNewFile = true;
        bool openedFile = false;
        bool changed = false;
        bool isSearch = false;
        string[] searchTexts = new string[6];
        string fileName = "";
        string safeFileName = "";

        public MainForm()
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo(Properties.Settings.Default.Language);
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo(Properties.Settings.Default.Language);

            InitializeComponent();

            dataGeneral.DataSource = null;
            dataWordReplacement.DataSource = null;
            dataWordDefinition.DataSource = null;
            dataFunctionDefinition.DataSource = null;
            dataMiscFunctionDefinition.DataSource = null;
            dataOthers.DataSource = null;

            isNewFile = false;
            openedFile = false;
            searchTexts = new string[6];
            search.TextBoxText = "";
            search.Enabled = false;
            closeFile.Enabled = false;
            saveFile.Enabled = false;
            saveFileSmall.Enabled = false;
            saveAsFile.Enabled = false;

            try
            {
                LoadCommands loadCommands = new LoadCommands(File.ReadAllLines(@"commands.txt", System.Text.Encoding.Default));
                if (loadCommands.Result == true)
                {
                    commands = loadCommands.Commands;

                    commands.SectionGeneral.Sort((n1, n2) => n1.Name.CompareTo(n2.Name));
                    commands.SectionWordReplacement.Sort((n1, n2) => n1.Name.CompareTo(n2.Name));
                    commands.SectionWordDefinition.Sort((n1, n2) => n1.Name.CompareTo(n2.Name));
                    commands.SectionFunctionDefinition.Sort((n1, n2) => n1.Name.CompareTo(n2.Name));
                    commands.SectionMiscFunctionDefinition.Sort((n1, n2) => n1.Name.CompareTo(n2.Name));
                    commands.SectionOthers.Sort((n1, n2) => n1.Name.CompareTo(n2.Name));
                }
                else
                {
                    MessageBox.Show(Resources.Res.cmdIncorrectFileInfo, Resources.Res.infoHeader, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Environment.Exit(-1);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, Resources.Res.errorHeader, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(-1);
            }
        }

        private void NewFile_Click(object sender, EventArgs e)
        {
            if (openedFile && changed)
            {
                switch (MessageBox.Show(Resources.Res.saveFileText + safeFileName + '?', Resources.Res.saveFileTitle, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
                {
                    case DialogResult.Yes:
                        SaveFile_Click(sender, e);
                        break;
                    case DialogResult.No:
                        break;
                    case DialogResult.Cancel:
                        return;
                    default:
                        break;
                }
            }

            sectionsData = new SectionsData();
            dataGeneral.DataSource = null;
            dataWordReplacement.DataSource = null;
            dataWordDefinition.DataSource = null;
            dataFunctionDefinition.DataSource = null;
            dataMiscFunctionDefinition.DataSource = null;
            dataOthers.DataSource = null;

            foreach (var item in commands.SectionGeneral)
                item.IsAdded = false;
            foreach (var item in commands.SectionWordReplacement)
                item.IsAdded = false;
            foreach (var item in commands.SectionWordDefinition)
                item.IsAdded = false;
            foreach (var item in commands.SectionFunctionDefinition)
                item.IsAdded = false;
            foreach (var item in commands.SectionMiscFunctionDefinition)
                item.IsAdded = false;
            foreach (var item in commands.SectionOthers)
                item.IsAdded = false;

            MainFormStatus("New", "", "Untitled1.sm3");
            TabControl_SelectedIndexChanged(sender, e);
        }
        private void OpenFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                DefaultExt = ".sm3",
                Filter = "DEF 1|*.sm1|DEF 2|*.sm2|DEF 3|*.sm3|DEF 4|*.sm4|DEF 5|*.sm5",
                FilterIndex = 3
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (openedFile && changed)
                {
                    switch (MessageBox.Show(Resources.Res.saveFileText + safeFileName + '?', Resources.Res.saveFileTitle, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
                    {
                        case DialogResult.Yes:
                            SaveFile_Click(sender, e);
                            break;
                        case DialogResult.No:
                            break;
                        case DialogResult.Cancel:
                            return;
                        default:
                            break;
                    }
                }

                ReadFile readFile = new ReadFile(File.ReadAllLines(openFileDialog.FileName, System.Text.Encoding.Default));
                if (readFile.Result)
                {
                    sectionsData = new SectionsData();
                    sectionsData = readFile.SectionsData;

                    dataGeneral.DataSource = null;
                    dataWordReplacement.DataSource = null;
                    dataWordDefinition.DataSource = null;
                    dataFunctionDefinition.DataSource = null;
                    dataMiscFunctionDefinition.DataSource = null;
                    dataOthers.DataSource = null;

                    dataGeneral.DataSource = sectionsData.SectionGeneral;
                    dataWordReplacement.DataSource = sectionsData.SectionWordReplacement;
                    dataWordDefinition.DataSource = sectionsData.SectionWordDefinition;
                    dataFunctionDefinition.DataSource = sectionsData.SectionFunctionDefinition;
                    dataMiscFunctionDefinition.DataSource = sectionsData.SectionMiscFunctionDefinition;
                    dataOthers.DataSource = sectionsData.SectionOthers;

                    foreach (var item in commands.SectionGeneral)
                        item.IsAdded = false;
                    foreach (var item in commands.SectionWordReplacement)
                        item.IsAdded = false;
                    foreach (var item in commands.SectionWordDefinition)
                        item.IsAdded = false;
                    foreach (var item in commands.SectionFunctionDefinition)
                        item.IsAdded = false;
                    foreach (var item in commands.SectionMiscFunctionDefinition)
                        item.IsAdded = false;
                    foreach (var item in commands.SectionOthers)
                        item.IsAdded = false;

                    foreach (var item in sectionsData.SectionGeneral)
                        try
                        {
                            commands.SectionGeneral.Find(itemC => itemC.Name == item.Name).IsAdded = true;
                        }
                        catch (Exception)
                        {
                            Command Example = new Command
                            {
                                Name = item.Name,
                                Symbol = "*",
                                Description = "*",
                                IsAdded = true
                            };

                            commands.SectionGeneral.Add(Example);
                        }
                    foreach (var item in sectionsData.SectionWordDefinition)
                        try
                        {
                            commands.SectionWordDefinition.Find(itemC => itemC.Name == item.Keyword).IsAdded = true;
                        }
                        catch (Exception)
                        {
                            Command Example = new Command
                            {
                                Name = item.Keyword,
                                Symbol = "*",
                                Description = "*",
                                IsAdded = true
                            };

                            commands.SectionWordDefinition.Add(Example);
                        }
                    foreach (var item in sectionsData.SectionFunctionDefinition)
                        try
                        {
                            commands.SectionFunctionDefinition.Find(itemC => itemC.Name == item.Keyword).IsAdded = true;
                        }
                        catch (Exception)
                        {
                            Command Example = new Command
                            {
                                Name = item.Keyword,
                                Symbol = "*",
                                Description = "*",
                                IsAdded = true
                            };

                            commands.SectionFunctionDefinition.Add(Example);
                        }
                    foreach (var item in sectionsData.SectionMiscFunctionDefinition)
                        try
                        {
                            commands.SectionMiscFunctionDefinition.Find(itemC => itemC.Name == item.Keyword).IsAdded = true;
                        }
                        catch (Exception)
                        {
                            Command Example = new Command
                            {
                                Name = item.Keyword,
                                Symbol = "*",
                                Description = "*",
                                IsAdded = true
                            };

                            commands.SectionMiscFunctionDefinition.Add(Example);
                        }
                    foreach (var item in sectionsData.SectionOthers)
                        try
                        {
                            commands.SectionOthers.Find(itemC => itemC.Name == item.Keyword).IsAdded = true;
                        }
                        catch (Exception)
                        {
                            Command Example = new Command
                            {
                                Name = item.Keyword,
                                Symbol = "*",
                                Description = "*",
                                IsAdded = true
                            };

                            commands.SectionOthers.Add(Example);
                        }

                    MainFormStatus("Open", openFileDialog.FileName, openFileDialog.SafeFileName);
                    TabControl_SelectedIndexChanged(sender, e);
                }
                else
                    MainFormStatus("Close", "", "");
            }            
        }
        private void SaveFile_Click(object sender, EventArgs e)
        {
            SaveFile saveFile = new SaveFile(sectionsData, fileName, isNewFile);

            if (saveFile.Result)
                MainFormStatus("Save", saveFile.FileName, saveFile.SafeFileName);
        }
        private void SaveAsFile_Click(object sender, EventArgs e)
        {
            SaveFile saveAsFile = new SaveFile(sectionsData, fileName, true);
            if (saveAsFile.Result)
                MainFormStatus("Save", saveAsFile.FileName, saveAsFile.SafeFileName);
        }
        private void CloseFile_Click(object sender, EventArgs e)
        {
            if (openedFile && changed)
            {
                switch (MessageBox.Show(Resources.Res.saveFileText + safeFileName + '?', Resources.Res.saveFileTitle, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
                {
                    case DialogResult.Yes:
                        SaveFile_Click(sender, e);
                        break;
                    case DialogResult.No:
                        break;
                    case DialogResult.Cancel:
                        return;
                    default:
                        break;

                }
            }
            MainFormStatus("Close", "", "");
        }
        private void Search_TextBoxKeyUp(object sender, KeyEventArgs e)
        {            
            isSearch = true;
            TabControl_SelectedIndexChanged(sender, e);
            isSearch = false;

            switch (tabControl.SelectedIndex)
            {
                case 0:
                    if (search.TextBoxText == "" || search.TextBoxText == null)
                        dataGeneral.DataSource = sectionsData.SectionGeneral;
                    else
                        dataGeneral.DataSource = sectionsData.SectionGeneral.Where(item => (item.Name.ToLower() + " " + item.Value.ToLower()).Contains(search.TextBoxText.ToLower())).ToList();
                    searchTexts[0] = search.TextBoxText;
                    break;
                case 1:
                    if (search.TextBoxText == "" || search.TextBoxText == null)
                        dataWordReplacement.DataSource = sectionsData.SectionWordReplacement;
                    else
                        dataWordReplacement.DataSource = sectionsData.SectionWordReplacement.Where(item => (item.Operation.ToLower() + " " + item.Expression1.ToLower() + " " + item.Expression2.ToLower()).Contains(search.TextBoxText.ToLower())).ToList();
                    searchTexts[1] = search.TextBoxText;
                    break;
                case 2:
                    if (search.TextBoxText == "" || search.TextBoxText == null)
                        dataWordDefinition.DataSource = sectionsData.SectionWordDefinition;
                    else
                        dataWordDefinition.DataSource = sectionsData.SectionWordDefinition.Where(item => (item.Keyword.ToLower() + " " + item.Symbol.ToLower()).Contains(search.TextBoxText.ToLower())).ToList();
                    searchTexts[2] = search.TextBoxText;
                    break;
                case 3:
                    if (search.TextBoxText == "" || search.TextBoxText == null)
                        dataFunctionDefinition.DataSource = sectionsData.SectionFunctionDefinition;
                    else
                        dataFunctionDefinition.DataSource = sectionsData.SectionFunctionDefinition.Where(item => (item.Keyword.ToLower() + " " + item.PreparatoryGroupNumber.ToLower()).Contains(search.TextBoxText.ToLower())).ToList();
                    searchTexts[3] = search.TextBoxText;
                    break;
                case 4:
                    if (search.TextBoxText == "" || search.TextBoxText == null)
                        dataMiscFunctionDefinition.DataSource = sectionsData.SectionMiscFunctionDefinition;
                    else
                        dataMiscFunctionDefinition.DataSource = sectionsData.SectionMiscFunctionDefinition.Where(item => (item.Keyword.ToLower() + " " + item.PreparatoryGroupNumber.ToLower()).Contains(search.TextBoxText.ToLower())).ToList();
                    searchTexts[4] = search.TextBoxText;
                    break;
                case 5:
                    if (search.TextBoxText == "" || search.TextBoxText == null)
                        dataOthers.DataSource = sectionsData.SectionOthers;
                    else
                        dataOthers.DataSource = sectionsData.SectionOthers.Where(item => (item.Keyword.ToLower() + " " + item.Value.ToLower()).Contains(search.TextBoxText.ToLower())).ToList();
                    searchTexts[5] = search.TextBoxText;
                    break;
                default:
                    break;
            }
        }
        private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            commandsListBox.Items.Clear();
            commandDescription.Text = "";
            switch (tabControl.SelectedIndex)
            {
                case 0:
                    if (isSearch)
                        searchTexts[0] = search.TextBoxText;
                    else
                        search.TextBoxText = searchTexts[0];
                    foreach (var item in commands.SectionGeneral)
                        if (!item.IsAdded)
                            if (search.TextBoxText == "" || search.TextBoxText == null)
                                commandsListBox.Items.Add(item.Name);
                            else if (item.Name.ToLower().Contains(search.TextBoxText.ToLower()))
                                commandsListBox.Items.Add(item.Name);
                    break;
                case 1:
                    if (isSearch)
                        searchTexts[1] = search.TextBoxText;
                    else
                        search.TextBoxText = searchTexts[1];
                    foreach (var item in commands.SectionWordReplacement)
                        if (!item.IsAdded)
                            if (search.TextBoxText == "" || search.TextBoxText == null)
                                commandsListBox.Items.Add(item.Name);
                            else if (item.Name.ToLower().Contains(search.TextBoxText.ToLower()))
                                commandsListBox.Items.Add(item.Name);
                    break;
                case 2:
                    if (isSearch)
                        searchTexts[2] = search.TextBoxText;
                    else
                        search.TextBoxText = searchTexts[2];
                    foreach (var item in commands.SectionWordDefinition)
                        if (!item.IsAdded)
                            if (search.TextBoxText == "" || search.TextBoxText == null)
                                commandsListBox.Items.Add(item.Name);
                            else if (item.Name.ToLower().Contains(search.TextBoxText.ToLower()))
                                commandsListBox.Items.Add(item.Name);
                    break;
                case 3:
                    if (isSearch)
                        searchTexts[3] = search.TextBoxText;
                    else
                        search.TextBoxText = searchTexts[3];
                    foreach (var item in commands.SectionFunctionDefinition)
                        if (!item.IsAdded)
                            if (search.TextBoxText == "" || search.TextBoxText == null)
                                commandsListBox.Items.Add(item.Name);
                            else if (item.Name.ToLower().Contains(search.TextBoxText.ToLower()))
                                commandsListBox.Items.Add(item.Name);
                    break;
                case 4:
                    if (isSearch)
                        searchTexts[4] = search.TextBoxText;
                    else
                        search.TextBoxText = searchTexts[4];
                    foreach (var item in commands.SectionMiscFunctionDefinition)
                        if (!item.IsAdded)
                            if (search.TextBoxText == "" || search.TextBoxText == null)
                                commandsListBox.Items.Add(item.Name);
                            else if (item.Name.ToLower().Contains(search.TextBoxText.ToLower()))
                                commandsListBox.Items.Add(item.Name);
                    break;
                case 5:
                    if (isSearch)
                        searchTexts[5] = search.TextBoxText;
                    else
                        search.TextBoxText = searchTexts[5];
                    foreach (var item in commands.SectionOthers)
                        if (!item.IsAdded)
                            if (search.TextBoxText == "" || search.TextBoxText == null)
                                commandsListBox.Items.Add(item.Name);
                            else if (item.Name.ToLower().Contains(search.TextBoxText.ToLower()))
                                commandsListBox.Items.Add(item.Name);
                    break;
                default:
                    break;
            }
        }
        private void DataChanged(object sender, DataGridViewCellEventArgs e)
        {
            changed = true;
            this.Text = safeFileName + "* - NCMDEFEditor";
            saveFile.Enabled = true;
            saveFileSmall.Enabled = true;
        }
        private void MainFormStatus(string Event, string FileName, string SafeFileName)
        {
            fileName = FileName;
            safeFileName = SafeFileName;
            changed = false;
            if (Event == "Close")
                this.Text = "NCMDEFEditor";
            else
                this.Text = safeFileName + " - NCMDEFEditor";

            switch (Event)
            {
                case "New":
                    isNewFile = true;
                    openedFile = true;
                    searchTexts = new string[6];
                    search.TextBoxText = "";
                    search.Enabled = true;
                    closeFile.Enabled = true;
                    saveFile.Enabled = false;
                    saveFileSmall.Enabled = false;
                    saveAsFile.Enabled = true;
                    tabControl.Enabled = true;
                    break;
                case "Open":
                    isNewFile = false;
                    openedFile = true;
                    searchTexts = new string[6];
                    search.TextBoxText = "";
                    search.Enabled = true;
                    closeFile.Enabled = true;
                    saveFile.Enabled = false;
                    saveFileSmall.Enabled = false;
                    saveAsFile.Enabled = true;
                    tabControl.Enabled = true;
                    break;
                case "Save":
                    isNewFile = false;
                    saveFile.Enabled = false;
                    saveFileSmall.Enabled = false;
                    break;
                case "Close":
                    sectionsData = new SectionsData();
                    dataGeneral.DataSource = null;
                    dataWordReplacement.DataSource = null;
                    dataWordDefinition.DataSource = null;
                    dataFunctionDefinition.DataSource = null;
                    dataMiscFunctionDefinition.DataSource = null;
                    dataOthers.DataSource = null;

                    isNewFile = false;
                    openedFile = false;
                    searchTexts = new string[6];
                    search.TextBoxText = "";
                    search.Enabled = false;
                    //closeFile.Enabled = false;
                    saveFile.Enabled = false;
                    saveFileSmall.Enabled = false;
                    saveAsFile.Enabled = false;
                    tabControl.Enabled = false;

                    tabControl.SelectedIndex = 0;
                    commandsListBox.Items.Clear();
                    commandDescription.Text = "";
                    foreach (var item in commands.SectionGeneral)
                        item.IsAdded = false;
                    foreach (var item in commands.SectionWordReplacement)
                        item.IsAdded = false;
                    foreach (var item in commands.SectionWordDefinition)
                        item.IsAdded = false;
                    foreach (var item in commands.SectionFunctionDefinition)
                        item.IsAdded = false;
                    foreach (var item in commands.SectionMiscFunctionDefinition)
                        item.IsAdded = false;
                    foreach (var item in commands.SectionOthers)
                        item.IsAdded = false;
                    break;
                default:
                    break;
            }
        }
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (openedFile && changed)
            {
                switch (MessageBox.Show(Resources.Res.saveFileText + safeFileName + '?', Resources.Res.saveFileTitle, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
                {
                    case DialogResult.Yes:
                        SaveFile_Click(sender, e);
                        break;
                    case DialogResult.No:
                        break;
                    case DialogResult.Cancel:
                        e.Cancel = true;
                        break;
                    default:
                        break;
                }
            }
        }
        private void CommandsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (commandsListBox.SelectedIndex != -1)
                switch (tabControl.SelectedIndex)
                {
                    case 0:
                        commandDescription.Text = commands.SectionGeneral.Find(item => item.Name == commandsListBox.SelectedItem.ToString()).Description;
                        break;
                    case 1:
                        commandDescription.Text = commands.SectionWordReplacement.Find(item => item.Name == commandsListBox.SelectedItem.ToString()).Description;
                        break;
                    case 2:
                        commandDescription.Text = commands.SectionWordDefinition.Find(item => item.Name == commandsListBox.SelectedItem.ToString()).Description;
                        break;
                    case 3:
                        commandDescription.Text = commands.SectionFunctionDefinition.Find(item => item.Name == commandsListBox.SelectedItem.ToString()).Description;
                        break;
                    case 4:
                        commandDescription.Text = commands.SectionMiscFunctionDefinition.Find(item => item.Name == commandsListBox.SelectedItem.ToString()).Description;
                        break;
                    case 5:
                        commandDescription.Text = commands.SectionOthers.Find(item => item.Name == commandsListBox.SelectedItem.ToString()).Description;
                        break;
                    default:
                        break;
                }
        }
        private void CommandsListBox_DoubleClick(object sender, EventArgs e)
        {
            if (commandsListBox.SelectedItem != null)
            {
                switch (tabControl.SelectedIndex)
                {
                    case 0:
                        sectionsData.SectionGeneral.Add(new SectionGeneral()
                        {
                            Name = commandsListBox.SelectedItem.ToString(),
                            Value = commands.SectionGeneral.Find(item => item.Name == commandsListBox.SelectedItem.ToString()).Symbol
                        });

                        commands.SectionGeneral.Find(item => item.Name == commandsListBox.SelectedItem.ToString()).IsAdded = true;
                        commandsListBox.Items.RemoveAt(commandsListBox.SelectedIndex);

                        dataGeneral.DataSource = null;
                        if (search.TextBoxText == "" || search.TextBoxText == null)
                            dataGeneral.DataSource = sectionsData.SectionGeneral;
                        else
                            dataGeneral.DataSource = sectionsData.SectionGeneral.Where(item => (item.Name.ToLower() + " " + item.Value.ToLower()).Contains(search.TextBoxText.ToLower())).ToList();


                        //dataGeneral.UpdateCellValue(1, dataGeneral.Rows.Count - 1);// 

                        dataGeneral.CurrentCell = dataGeneral.Rows[dataGeneral.Rows.Count - 1].Cells[1];
                        dataGeneral.Focus();


                        break;
                    case 1:
                        sectionsData.SectionWordReplacement.Add(new SectionWordReplacement()
                        {
                            Operation = commandsListBox.SelectedItem.ToString(),
                            Expression1 = commands.SectionWordReplacement.Find(item => item.Name == commandsListBox.SelectedItem.ToString()).Symbol,
                            Expression2 = commands.SectionWordReplacement.Find(item => item.Name == commandsListBox.SelectedItem.ToString()).Symbol
                        });

                        dataWordReplacement.DataSource = null;
                        if (search.TextBoxText == "" || search.TextBoxText == null)
                            dataWordReplacement.DataSource = sectionsData.SectionWordReplacement;
                        else
                            dataWordReplacement.DataSource = sectionsData.SectionWordReplacement.Where(item => (item.Operation.ToLower() + " " + item.Expression1.ToLower() + " " + item.Expression2.ToLower()).Contains(search.TextBoxText.ToLower())).ToList();

                        dataWordReplacement.CurrentCell = dataWordReplacement.Rows[dataWordReplacement.Rows.Count - 1].Cells[1];
                        dataWordReplacement.Focus();
                        break;
                    case 2:
                        sectionsData.SectionWordDefinition.Add(new SectionWordDefinition()
                        {
                            Keyword = commandsListBox.SelectedItem.ToString(),
                            Symbol = commands.SectionWordDefinition.Find(item => item.Name == commandsListBox.SelectedItem.ToString()).Symbol
                        });

                        commands.SectionWordDefinition.Find(item => item.Name == commandsListBox.SelectedItem.ToString()).IsAdded = true;
                        commandsListBox.Items.RemoveAt(commandsListBox.SelectedIndex);

                        dataWordDefinition.DataSource = null;
                        if (search.TextBoxText == "" || search.TextBoxText == null)
                            dataWordDefinition.DataSource = sectionsData.SectionWordDefinition;
                        else
                            dataWordDefinition.DataSource = sectionsData.SectionWordDefinition.Where(item => (item.Keyword.ToLower() + " " + item.Symbol.ToLower()).Contains(search.TextBoxText.ToLower())).ToList();

                        dataWordDefinition.CurrentCell = dataWordDefinition.Rows[dataWordDefinition.Rows.Count - 1].Cells[1];
                        dataWordDefinition.Focus();
                        break;
                    case 3:
                        sectionsData.SectionFunctionDefinition.Add(new SectionFunctionDefinition()
                        {
                            Keyword = commandsListBox.SelectedItem.ToString(),
                            PreparatoryGroupNumber = commands.SectionFunctionDefinition.Find(item => item.Name == commandsListBox.SelectedItem.ToString()).Symbol
                        });

                        commands.SectionFunctionDefinition.Find(item => item.Name == commandsListBox.SelectedItem.ToString()).IsAdded = true;
                        commandsListBox.Items.RemoveAt(commandsListBox.SelectedIndex);

                        dataFunctionDefinition.DataSource = null;
                        if (search.TextBoxText == "" || search.TextBoxText == null)
                            dataFunctionDefinition.DataSource = sectionsData.SectionFunctionDefinition;
                        else
                            dataFunctionDefinition.DataSource = sectionsData.SectionFunctionDefinition.Where(item => (item.Keyword.ToLower() + " " + item.PreparatoryGroupNumber.ToLower()).Contains(search.TextBoxText.ToLower())).ToList();

                        dataFunctionDefinition.CurrentCell = dataFunctionDefinition.Rows[dataFunctionDefinition.Rows.Count - 1].Cells[1];
                        dataFunctionDefinition.Focus();
                        break;
                    case 4:
                        sectionsData.SectionMiscFunctionDefinition.Add(new SectionMiscFunctionDefinition()
                        {
                            Keyword = commandsListBox.SelectedItem.ToString(),
                            PreparatoryGroupNumber = commands.SectionMiscFunctionDefinition.Find(item => item.Name == commandsListBox.SelectedItem.ToString()).Symbol
                        });

                        commands.SectionMiscFunctionDefinition.Find(item => item.Name == commandsListBox.SelectedItem.ToString()).IsAdded = true;
                        commandsListBox.Items.RemoveAt(commandsListBox.SelectedIndex);

                        dataMiscFunctionDefinition.DataSource = null;
                        if (search.TextBoxText == "" || search.TextBoxText == null)
                            dataMiscFunctionDefinition.DataSource = sectionsData.SectionMiscFunctionDefinition;
                        else
                            dataMiscFunctionDefinition.DataSource = sectionsData.SectionMiscFunctionDefinition.Where(item => (item.Keyword.ToLower() + " " + item.PreparatoryGroupNumber.ToLower()).Contains(search.TextBoxText.ToLower())).ToList();

                        dataMiscFunctionDefinition.CurrentCell = dataMiscFunctionDefinition.Rows[dataMiscFunctionDefinition.Rows.Count - 1].Cells[1];
                        dataMiscFunctionDefinition.Focus();
                        break;
                    case 5:
                        sectionsData.SectionOthers.Add(new SectionOthers()
                        {
                            Keyword = commandsListBox.SelectedItem.ToString(),
                            Value = commands.SectionOthers.Find(item => item.Name == commandsListBox.SelectedItem.ToString()).Symbol
                        });

                        commands.SectionOthers.Find(item => item.Name == commandsListBox.SelectedItem.ToString()).IsAdded = true;
                        commandsListBox.Items.RemoveAt(commandsListBox.SelectedIndex);

                        dataOthers.DataSource = null;
                        if (search.TextBoxText == "" || search.TextBoxText == null)
                            dataOthers.DataSource = sectionsData.SectionOthers;
                        else
                            dataOthers.DataSource = sectionsData.SectionOthers.Where(item => (item.Keyword.ToLower() + " " + item.Value.ToLower()).Contains(search.TextBoxText.ToLower())).ToList();

                        dataOthers.CurrentCell = dataOthers.Rows[dataOthers.Rows.Count - 1].Cells[1];
                        dataOthers.Focus();
                        break;
                    default:
                        break;
                }

                Data_SelectionDescription(sender, e);
                changed = true;
                this.Text = safeFileName + "* - NCMDEFEditor";
                saveFile.Enabled = true;
                saveFileSmall.Enabled = true;
            }
        }
        private void CommandsListBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                CommandsListBox_DoubleClick(sender, e);
        }
        private void DataGeneral_KeyDown(object sender, KeyEventArgs e)
        {
            if (dataGeneral.Rows.Count != 0)
                if (e.KeyCode == Keys.Delete)
                {
                    string nameOfSelectedItem = dataGeneral.Rows[dataGeneral.CurrentRow.Index].Cells[0].Value.ToString();
                    commands.SectionGeneral[commands.SectionGeneral.FindIndex(item => item.Name == nameOfSelectedItem)].IsAdded = false;
                    TabControl_SelectedIndexChanged(sender, e);

                    sectionsData.SectionGeneral.Remove(sectionsData.SectionGeneral.Find(item => item.Name == nameOfSelectedItem));
                    dataGeneral.DataSource = null;
                    Search_TextBoxKeyUp(sender, e);
                    //dataGeneral.DataSource = sectionsData.SectionGeneral;

                    changed = true;
                    this.Text = safeFileName + "* - NCMDEFEditor";
                    saveFile.Enabled = true;
                    saveFileSmall.Enabled = true;
                }
        }
        private void DataWordReplacement_KeyDown(object sender, KeyEventArgs e)
        {
            if (dataWordReplacement.Rows.Count != 0)
                if (e.KeyCode == Keys.Delete)
                {
                    string nameOfSelectedItem = dataWordReplacement.Rows[dataWordReplacement.CurrentRow.Index].Cells[0].Value.ToString();
                    commands.SectionWordReplacement[commands.SectionWordReplacement.FindIndex(item => item.Name == nameOfSelectedItem)].IsAdded = false;
                    TabControl_SelectedIndexChanged(sender, e);

                    sectionsData.SectionWordReplacement.Remove(sectionsData.SectionWordReplacement.Find(item => item.Operation == nameOfSelectedItem));
                    dataWordReplacement.DataSource = null;
                    Search_TextBoxKeyUp(sender, e);

                    changed = true;
                    this.Text = safeFileName + "* - NCMDEFEditor";
                    saveFile.Enabled = true;
                    saveFileSmall.Enabled = true;
                }
        }
        private void DataWordDefinition_KeyDown(object sender, KeyEventArgs e)
        {
            if (dataWordDefinition.Rows.Count != 0)
                if (e.KeyCode == Keys.Delete)
                {
                    string nameOfSelectedItem = dataWordDefinition.Rows[dataWordDefinition.CurrentRow.Index].Cells[0].Value.ToString();
                    commands.SectionWordDefinition[commands.SectionWordDefinition.FindIndex(item => item.Name == nameOfSelectedItem)].IsAdded = false;
                    TabControl_SelectedIndexChanged(sender, e);

                    sectionsData.SectionWordDefinition.Remove(sectionsData.SectionWordDefinition.Find(item => item.Keyword == nameOfSelectedItem));
                    dataWordDefinition.DataSource = null;
                    Search_TextBoxKeyUp(sender, e);

                    changed = true;
                    this.Text = safeFileName + "* - NCMDEFEditor";
                    saveFile.Enabled = true;
                    saveFileSmall.Enabled = true;
                }
        }
        private void DataFunctionDefinition_KeyDown(object sender, KeyEventArgs e)
        {
            if (dataFunctionDefinition.Rows.Count != 0)
                if (e.KeyCode == Keys.Delete)
                {
                    string nameOfSelectedItem = dataFunctionDefinition.Rows[dataFunctionDefinition.CurrentRow.Index].Cells[0].Value.ToString();
                    commands.SectionFunctionDefinition[commands.SectionFunctionDefinition.FindIndex(item => item.Name == nameOfSelectedItem)].IsAdded = false;
                    TabControl_SelectedIndexChanged(sender, e);

                    sectionsData.SectionFunctionDefinition.Remove(sectionsData.SectionFunctionDefinition.Find(item => item.Keyword == nameOfSelectedItem));
                    dataFunctionDefinition.DataSource = null;
                    Search_TextBoxKeyUp(sender, e);

                    changed = true;
                    this.Text = safeFileName + "* - NCMDEFEditor";
                    saveFile.Enabled = true;
                    saveFileSmall.Enabled = true;
                }
        }
        private void DataMiscFunctionDefinition_KeyDown(object sender, KeyEventArgs e)
        {
            if (dataMiscFunctionDefinition.Rows.Count != 0)
                if (e.KeyCode == Keys.Delete)
                {
                    string nameOfSelectedItem = dataMiscFunctionDefinition.Rows[dataMiscFunctionDefinition.CurrentRow.Index].Cells[0].Value.ToString();
                    commands.SectionMiscFunctionDefinition[commands.SectionMiscFunctionDefinition.FindIndex(item => item.Name == nameOfSelectedItem)].IsAdded = false;
                    TabControl_SelectedIndexChanged(sender, e);

                    sectionsData.SectionMiscFunctionDefinition.Remove(sectionsData.SectionMiscFunctionDefinition.Find(item => item.Keyword == nameOfSelectedItem));
                    dataMiscFunctionDefinition.DataSource = null;
                    Search_TextBoxKeyUp(sender, e);

                    changed = true;
                    this.Text = safeFileName + "* - NCMDEFEditor";
                    saveFile.Enabled = true;
                    saveFileSmall.Enabled = true;
                }
        }
        private void DataOthers_KeyDown(object sender, KeyEventArgs e)
        {
            if (dataOthers.Rows.Count != 0)
                if (e.KeyCode == Keys.Delete)
                {
                    string nameOfSelectedItem = dataOthers.Rows[dataOthers.CurrentRow.Index].Cells[0].Value.ToString();
                    commands.SectionOthers[commands.SectionOthers.FindIndex(item => item.Name == nameOfSelectedItem)].IsAdded = false;
                    TabControl_SelectedIndexChanged(sender, e);

                    sectionsData.SectionOthers.Remove(sectionsData.SectionOthers.Find(item => item.Keyword == nameOfSelectedItem));
                    dataOthers.DataSource = null;
                    Search_TextBoxKeyUp(sender, e);

                    changed = true;
                    this.Text = safeFileName + "* - NCMDEFEditor";
                    saveFile.Enabled = true;
                    saveFileSmall.Enabled = true;
                }
        }
        private void Data_SelectionDescription(object sender, EventArgs e)
        {
            string nameOfSelectedItem;
            switch (tabControl.SelectedIndex)
            {
                case 0:
                    if (dataGeneral.CurrentRow != null && dataGeneral.Rows[dataGeneral.CurrentRow.Index].Cells[0].Value != null)
                    {
                        nameOfSelectedItem = dataGeneral.Rows[dataGeneral.CurrentRow.Index].Cells[0].Value.ToString();
                        commandDescription.Text = commands.SectionGeneral.Find(item => item.Name == nameOfSelectedItem).Description;
                    }
                    break;
                case 1:
                    if (dataWordReplacement.CurrentRow != null && dataWordReplacement.Rows[dataWordReplacement.CurrentRow.Index].Cells[0].Value != null)
                    {
                        nameOfSelectedItem = dataWordReplacement.Rows[dataWordReplacement.CurrentRow.Index].Cells[0].Value.ToString();
                        commandDescription.Text = commands.SectionWordReplacement.Find(item => item.Name == nameOfSelectedItem).Description;
                    }
                    break;
                case 2:
                    if (dataWordDefinition.CurrentRow != null && dataWordDefinition.Rows[dataWordDefinition.CurrentRow.Index].Cells[0].Value != null)
                    {
                        nameOfSelectedItem = dataWordDefinition.Rows[dataWordDefinition.CurrentRow.Index].Cells[0].Value.ToString();
                        commandDescription.Text = commands.SectionWordDefinition.Find(item => item.Name == nameOfSelectedItem).Description;
                    }
                    break;
                case 3:
                    if (dataFunctionDefinition.CurrentRow != null && dataFunctionDefinition.Rows[dataFunctionDefinition.CurrentRow.Index].Cells[0].Value != null)
                    {
                        nameOfSelectedItem = dataFunctionDefinition.Rows[dataFunctionDefinition.CurrentRow.Index].Cells[0].Value.ToString();
                        commandDescription.Text = commands.SectionFunctionDefinition.Find(item => item.Name == nameOfSelectedItem).Description;
                    }
                    break;
                case 4:
                    if (dataMiscFunctionDefinition.CurrentRow != null && dataMiscFunctionDefinition.Rows[dataMiscFunctionDefinition.CurrentRow.Index].Cells[0].Value != null)
                    {
                        nameOfSelectedItem = dataMiscFunctionDefinition.Rows[dataMiscFunctionDefinition.CurrentRow.Index].Cells[0].Value.ToString();
                        commandDescription.Text = commands.SectionMiscFunctionDefinition.Find(item => item.Name == nameOfSelectedItem).Description;
                    }
                    break;
                case 5:
                    if (dataOthers.CurrentRow != null && dataOthers.Rows[dataOthers.CurrentRow.Index].Cells[0].Value != null)
                    {
                        nameOfSelectedItem = dataOthers.Rows[dataOthers.CurrentRow.Index].Cells[0].Value.ToString();
                        commandDescription.Text = commands.SectionOthers.Find(item => item.Name == nameOfSelectedItem).Description;
                    }
                    break;
                default:
                    break;
            }
        }
        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                if (e.KeyCode == Keys.N && newFile.Enabled)
                    NewFile_Click(sender, e);
                else if (e.KeyCode == Keys.O && openFile.Enabled)
                    OpenFile_Click(sender, e);
                else if (e.Shift && e.KeyCode == Keys.S && saveAsFile.Enabled)
                    SaveAsFile_Click(sender, e);
                else if (e.KeyCode == Keys.S && saveFile.Enabled)
                    SaveFile_Click(sender, e);
                else if (e.KeyCode == Keys.W && closeFile.Enabled)
                    CloseFile_Click(sender, e);
                else if (e.KeyCode == Keys.F && search.Enabled)
                    search.StartEdit();
                else if (tabControl.Enabled)
                {
                    if (e.KeyCode == Keys.D1 || e.KeyCode == Keys.NumPad1)
                        tabControl.SelectTab(0);
                    else if (e.KeyCode == Keys.D2 || e.KeyCode == Keys.NumPad2)
                        tabControl.SelectTab(1);
                    else if (e.KeyCode == Keys.D3 || e.KeyCode == Keys.NumPad3)
                        tabControl.SelectTab(2);
                    else if (e.KeyCode == Keys.D4 || e.KeyCode == Keys.NumPad4)
                        tabControl.SelectTab(3);
                    else if (e.KeyCode == Keys.D5 || e.KeyCode == Keys.NumPad5)
                        tabControl.SelectTab(4);
                    else if (e.KeyCode == Keys.D6 || e.KeyCode == Keys.NumPad6)
                        tabControl.SelectTab(5);
                }
            }
        }
        private void Settings_Click(object sender, EventArgs e)
        {
            SettingsForm form1 = new SettingsForm();
            form1.ShowDialog();

            form1.Dispose();
        }    
    }
}