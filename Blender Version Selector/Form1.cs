using IWshRuntimeLibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Blender_Version_Selector
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string path = @"C:\Program Files\Blender Foundation";

        private void Form1_Load(object sender, EventArgs e)
        {
            ComboBox1.Items.Clear();
            string userName = Environment.UserName;
            string[] filesindirectory = Directory.GetDirectories(path);
            int amount = 0;

            CreateShortcut("Blender Version Selector", @"C:\Users\" + userName + @"\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\", Path.Combine(Directory.GetCurrentDirectory(), System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName), "0");

            foreach (string subdir in filesindirectory)
            {

                amount++;
                ComboBox1.Items.Add(subdir.Remove(0, path.Length + 1));

            }
            if (amount == 1) label1.Text = "1 Version found";
            else
                label1.Text = amount.ToString() + " Versions found";
            ComboBox1.SelectedIndex = amount - 1;


            this.Height = 136;
            buttonall.Hide();
            buttoncurrent.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string[] filesindirectory = Directory.GetDirectories(path);

            if (ComboBox1.Text != "")
            {
                foreach (string subdir in filesindirectory)
                {

                    if (subdir.Contains(ComboBox1.Text))
                    {
                        string ExecPath = subdir + @"\blender.exe";
                        Debug.WriteLine(ExecPath);
                        Process.Start(ExecPath);
                        break;

                    }
                    Close();
                }

            }
            else Debug.WriteLine("No Path was selected");

        }

        bool extented = false;
        private void button2_Click(object sender, EventArgs e)
        {
            if (!extented)
            {
                extented = true;
                this.Height = 162;
                buttonall.Show();
                buttoncurrent.Show();
            }
            else
            {
                extented = false;
                this.Height = 136;
                buttonall.Hide();
                buttoncurrent.Hide();
            }
        }

        public static void CreateShortcut(string shortcutName, string shortcutPath, string targetFileLocation, string desc)
        {
            string shortcutLocation = System.IO.Path.Combine(shortcutPath, shortcutName + ".lnk");
            WshShell shell = new WshShell();
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutLocation);
            if (desc != "0")
            {
                shortcut.Description = "A shortcut to " + desc + " generated using Actiol's Blender Version Selector";
            }
            else
            {
                shortcut.Description = "A shortcut to Actiol's Blender Version Selector";
            }
            // The description of the shortcut
            // shortcut.IconLocation = @"c:\myicon.ico";           // The icon of the shortcut
            shortcut.TargetPath = targetFileLocation;                 // The path of the file that will launch when the shortcut is run
            shortcut.Save();                                    // Save the shortcut
        }

        private void buttoncurrent_Click(object sender, EventArgs e)
        {
            if ((MessageBox.Show("Do you want to overwrite the default Blender shortcut to version " + ComboBox1.Text.Replace("Blender ", "") + "?", "Set Version " + ComboBox1.Text.Replace("Blender ", "") + " as default?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)) == DialogResult.Yes)
            {

                string userName = Environment.UserName;
                string[] filesindirectory = Directory.GetDirectories(path);

                if (ComboBox1.Text != "")
                {
                    ClearDirectory();

                    foreach (string subdir in filesindirectory)
                    {

                        if (subdir.Contains(ComboBox1.Text))
                        {
                            string ExecPath = subdir + @"\blender.exe";

                            CreateShortcut("Blender", @"C:\Users\" + userName + @"\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\Blender\", ExecPath, ComboBox1.Text);

                            break;

                        }
                    }
                }
                else Debug.WriteLine("No Path was selected");
            }
        }
        public static void ClearDirectory()
        {
            string userName = Environment.UserName;
            System.IO.DirectoryInfo di = new DirectoryInfo(@"C:\Users\" + userName + @"\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\Blender\");
            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
        }


        private void buttonall_Click(object sender, EventArgs e)
        {
            if ((MessageBox.Show("Do you want to create a shortcut for each Blender version?", "Create a shortcut for each version?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)) == DialogResult.Yes)
            {
                string userName = Environment.UserName;
                string[] filesindirectory = Directory.GetDirectories(path);

                if (ComboBox1.Text != "")
                {
                    ClearDirectory();

                    foreach (string subdir in filesindirectory)
                    {
                        string ExecPath = subdir + @"\blender.exe";

                        CreateShortcut(subdir.Remove(0, path.Length + 1), @"C:\Users\" + userName + @"\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\Blender\", ExecPath, subdir.Remove(0, path.Length + 1));
                    }
                }
                else Debug.WriteLine("No Path was selected");
            }
        }
    }
}
