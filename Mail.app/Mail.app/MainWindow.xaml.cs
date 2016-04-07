using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Collections.ObjectModel;
using Mail.app.Commands;
using System.IO;
using System.Drawing;
using System.Net.Mail;
using System.Xml;
using System.Xml.Linq;


namespace Mail.app
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static readonly DependencyProperty MyListProperty = DependencyProperty.Register("MyList", typeof(ObservableCollection<Group>), typeof(MainWindow));

        public ObservableCollection<Group> MyList
        {
            get { return (ObservableCollection<Group>)GetValue(MyListProperty); }
            set { SetValue(MyListProperty, value); }
        }
        public ObservableCollection<Group> taskList = new ObservableCollection<Group>() { };
        public static string HoldGrpValue = "";
        public bool grpvalidate = true;
        public string[] ParsedMails = new string[] { };
        public int mailCount = 0;
        public int PassCount = 0;
        public int FailCount = 0;
        List<string> grpItems = new List<string>();
        public int delCnt = 0;
        private RotateTransform trRot;
        private TransformGroup trGrp;
        public static bool validate = true;
        public string[] files = null;
        static string[] mediaExtensions = {
                                             ".WAV", ".MID", ".MIDI", ".WMA", ".MP3", ".OGG", ".RMA", //etc
                                             ".AVI", ".MP4", ".DIVX", ".WMV", ".FLV"//etc
                                          };
        static string[] ImageExtensions = {
                                             ".PNG", ".JPG", ".JPEG", ".BMP", ".GIF", //etc
                                          };
        static string[] FileExtensions =  {
                                            ".PNG", ".JPG", ".JPEG", ".BMP", ".GIF", //etc
                                            ".WAV", ".MID", ".MIDI", ".WMA", ".MP3", ".OGG", ".RMA", //etc
                                            ".AVI", ".MP4", ".DIVX", ".WMV", ".FLV"//etc
                                          };
        public MainWindow()
        {
            InitializeComponent();
            trRot = new RotateTransform(0);
            trGrp = new TransformGroup();
            trGrp.Children.Add(trRot);
            this.SizeToContent = SizeToContent.Width;
            this.SizeToContent = SizeToContent.Height;
            this.SizeToContent = SizeToContent.WidthAndHeight;
            this.grid_col1.Width = GridLength.Auto;

            this.DataContext = new PageViewModel();
            //  DropBox.DataContext = new FileCollection();
            // MyDropDownList.Select()


        }

        private void ToolTipOpenedHandler(object sender, RoutedEventArgs e)
        {
            ToolTip toolTip = (ToolTip)sender;
            UIElement target = toolTip.PlacementTarget;
            Point adjust = target.TranslatePoint(new Point(8, 0), toolTip);
            toolTip.Tag = new Thickness(adjust.X, 0, 0, -1.5);
        }

        private void DropBox_Drop(object sender, DragEventArgs e)
        {

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // _files.Clear();

                files = (string[])e.Data.GetData(DataFormats.FileDrop);

                foreach (string filePath in files)
                {
                    FileCollection._files.Add(filePath);
                    Image image = new Image();
                    image.Height = 90;
                    image.Width = 80;
                    if (IsImageExtensions(filePath) == true)
                    {
                        BitmapImage bi = new BitmapImage(new Uri(filePath));
                        image.Source = bi;
                        string filenameNoExtension = System.IO.Path.GetFileNameWithoutExtension(filePath);
                        DropBox.Items.Add(new Img(filenameNoExtension, image));
                        delCnt++;
                    }
                    if (IsMediaFile(filePath) == true)
                    {
                        string path = System.IO.Directory.GetCurrentDirectory();
                        path = path.Replace("bin\\Debug", "Images\\media.png");
                        string filenameNoExtension = System.IO.Path.GetFileNameWithoutExtension(filePath);
                        BitmapImage bi = new BitmapImage(new Uri(path));
                        image.Source = bi;
                        DropBox.Items.Add(new Img(filenameNoExtension, image));
                        delCnt++;
                    }
                    //else
                    //{
                    //    string path = System.IO.Directory.GetCurrentDirectory();
                    //    path = path.Replace("bin\\Debug", "Images\\document.ico");
                    //    string filenameNoExtension = System.IO.Path.GetFileNameWithoutExtension(path);
                    //    BitmapImage bi = new BitmapImage(new Uri(path));
                    //    image.Source = bi;
                    //    DropBox.Items.Add(new Img(filenameNoExtension, image));
                    //}
                }
            }
            var listbox = sender as ListBox;
            listbox.Background = new SolidColorBrush(Color.FromRgb(226, 226, 226));

        }
        public bool IsValid(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public static bool IsMediaFile(string path)
        {
            return -1 != Array.IndexOf(mediaExtensions, System.IO.Path.GetExtension(path).ToUpperInvariant());
        }
        public static bool IsImageExtensions(string path)
        {
            return -1 != Array.IndexOf(ImageExtensions, System.IO.Path.GetExtension(path).ToUpperInvariant());
        }
        public static bool IsFileExtensions(string path)
        {
            return -1 != Array.IndexOf(FileExtensions, System.IO.Path.GetExtension(path).ToUpperInvariant());
        }
        private void DropBox_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Copy;
                var listbox = sender as ListBox;
                listbox.Background = new SolidColorBrush(Color.FromRgb(155, 155, 155));
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private void DropBox_DragLeave(object sender, DragEventArgs e)
        {
            var listbox = sender as ListBox;
            listbox.Background = new SolidColorBrush(Color.FromRgb(226, 226, 226));
        }
        private void _txtbx_password_GotFocus(object sender, RoutedEventArgs e)
        {
            lbl_password.Visibility = Visibility.Hidden;
            validate = false;
        }

        private void _txtbx_password_LostFocus(object sender, RoutedEventArgs e)
        {
            if (validate == false)
            {
                lbl_password.Visibility = Visibility.Hidden;
                validate = false;
            }
            else
            {
                lbl_password.Visibility = Visibility.Visible;
                validate = true;
            }
        }
        private void txtbx_username_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtbx_username.Text.Length >= 1)
            {
                lbl_username.Visibility = Visibility.Hidden;
                validate = false;
            }
            else
            {

                validate = true;
            }
        }
        private void txtbx_username_GotFocus(object sender, RoutedEventArgs e)
        {

            lbl_username.Visibility = Visibility.Hidden;
            validate = false;


        }

        private void txtbx_username_LostFocus(object sender, RoutedEventArgs e)
        {
            if (validate == false)
            {
                lbl_username.Visibility = Visibility.Hidden;
                validate = false;
            }
            else
            {
                lbl_username.Visibility = Visibility.Visible;
                validate = true;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //this.ActiveControl = yourtextboxname;
            //MyTextBox.Select();

            login_bdr.Visibility = Visibility.Visible;
            email_bdr.Visibility = Visibility.Collapsed;
        }

        public void checkEmailValidation(string username, string password)
        {

        }
        private void btn_login_Click(object sender, RoutedEventArgs e)
        {
            if (IsValid(txtbx_username.Text))
            {
                GMailer.GmailUsername = txtbx_username.Text;
                GMailer.GmailPassword = _txtbx_password.Password;
                login_bdr.Visibility = Visibility.Collapsed;
                email_bdr.Visibility = Visibility.Visible;
                group_bdr.Visibility = Visibility.Visible;
                lbl_login_status.Visibility = Visibility.Collapsed;
                if (this.grid_col2.Width == new GridLength(45, GridUnitType.Star))
                {

                    this.grid_col1.Width = new GridLength(35, GridUnitType.Star);
                    this.grid_col2.Width = new GridLength(45, GridUnitType.Star);

                }
                else
                {
                    this.grid_col2.Width = new GridLength(45, GridUnitType.Star);
                    this.grid_col1.Width = new GridLength(0, GridUnitType.Star);

                }
            }
            else
            {
                lbl_login_status.Visibility = Visibility.Visible;
                lbl_login_status.Text = "Incorrect Email or Password";
                lbl_login_status.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF0000"));
            }
        }

        private void _txtbx_password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (_txtbx_password.Password.Length >= 1)
            {
                lbl_password.Visibility = Visibility.Hidden;
                validate = false;
            }
            else
            {

                validate = true;
            }
        }

        private void lstbx_del_btn_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                DropBox.Items.RemoveAt(delCnt - 1);
                FileCollection._files.RemoveAt(delCnt - 1);
                delCnt--;
            }
            else
            {
                return;
            }
        }

        private void group_btn_Click(object sender, RoutedEventArgs e)
        {
            groups1.Visibility = Visibility.Visible;
            lstBxTask.Visibility = Visibility.Visible;
            group_bdr.Visibility = Visibility.Collapsed;
            GetTask();
            Thickness margin1 = lstBxTask.Margin;
            margin1.Left = 10;
            margin1.Right = 5;
           
            margin1.Top = -2;
            lstBxTask.Margin = margin1;
            if (this.grid_grp_1.Width == new GridLength(6, GridUnitType.Star))
            {

                Thickness margin = groups1_btn.Margin;
                margin.Left = 10;
                margin.Top = -1;
                margin.Right = 5;
                groups1_btn.Height = 30;
                groups1_btn.Margin = margin;
                // group_btn.Content = "GROUPS";
                this.grid_grp_1.Width = new GridLength(45, GridUnitType.Star);
                this.grid_grp_2.Width = new GridLength(95, GridUnitType.Star);
            }
            else
            {
                this.grid_grp_1.Width = new GridLength(6, GridUnitType.Star);
                this.grid_grp_2.Width = new GridLength(95, GridUnitType.Star);



            }
        }
        public void GetTask()
        {
            try
            {
                taskList.Clear();
                try
                {
                    lstBxTask.ItemsSource = taskList;
                }
                catch(Exception EX)
                { }
                if (File.Exists("groups.xml") == false)
                {
                    if (HoldGrpValue != string.Empty)
                    {
                        XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
                        xmlWriterSettings.Indent = true;
                        xmlWriterSettings.NewLineOnAttributes = true;
                        using (XmlWriter xmlWriter = XmlWriter.Create("groups.xml", xmlWriterSettings))
                        {
                            xmlWriter.WriteStartDocument();
                            xmlWriter.WriteStartElement("Group");
                            xmlWriter.WriteElementString("ID", HoldGrpValue);
                            xmlWriter.WriteEndElement();
                            xmlWriter.WriteEndDocument();
                            xmlWriter.Flush();
                            xmlWriter.Close();
                        }
                    }
                }
                else
                {
                    if (HoldGrpValue != string.Empty)
                    {
                        XDocument xDocument = XDocument.Load("groups.xml");
                        XElement root = xDocument.Element("Group");
                        IEnumerable<XElement> rows = root.Descendants("ID");
                        XElement firstRow = rows.First();

                        firstRow.AddBeforeSelf(
                      new XElement(new XElement("ID", HoldGrpValue)));

                        xDocument.Save("groups.xml");
                    }
                }
                if (File.Exists("groups.xml") != false)
                {

                    List<string> grp_names = getGrpNms();
                    foreach (var v in grp_names)
                    {
                        taskList.Add(new Group(v));
                    }
                }
                lstBxTask.ItemsSource = taskList;
                taskList = new ObservableCollection<Group>() { };
                HoldGrpValue = string.Empty;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something happened wrong, Please try again.");
            }

        }
        public List<string> getGrpNms()
        {
            List<string> grp_names = (from grp in XDocument.Load("groups.xml").Descendants("ID")
                                      select grp.Value).Distinct().ToList(); 
            return grp_names;

        }
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
          
            var button = sender as Button;
            if (button != null)
            {
                var task = button.DataContext as Group;

                ((ObservableCollection<Group>)lstBxTask.ItemsSource).Remove(task);
                XDocument xDocument = XDocument.Load("groups.xml");
                foreach (var profileElement in xDocument.Descendants("ID")
                                                        .ToList())
                {
                    if (profileElement.Value == task.Taskname)
                    {
                        profileElement.Remove();
                    }
                }
                xDocument.Save("groups.xml");
                List<string> grp_names = getGrpNms();
                if(grp_names.Count==0)
                {
                    if (System.IO.File.Exists("groups.xml") == true)
                    {
                        System.IO.File.Delete("groups.xml");
                    }   
                }
            }
            else
            {
                return;
            }
        }

        private void groups1_btn_Click(object sender, RoutedEventArgs e)
        {
            group_bdr.Visibility = Visibility.Visible;
            lstBxTask.Visibility = Visibility.Collapsed;
            groups1.Visibility = Visibility.Collapsed;
            if (this.grid_grp_1.Width == new GridLength(6, GridUnitType.Star))
            {
                this.grid_grp_1.Width = new GridLength(45, GridUnitType.Star);
                this.grid_grp_2.Width = new GridLength(95, GridUnitType.Star);

            }
            else
            {
                this.grid_grp_1.Width = new GridLength(6, GridUnitType.Star);
                this.grid_grp_2.Width = new GridLength(95, GridUnitType.Star);
            }
        }

        private void btn_add_Click(object sender, RoutedEventArgs e)
        {
            Thickness margin1 = lstBxTask.Margin;
            margin1.Left = -140;
            margin1.Right = 9;
            margin1.Top = 5;
            margin1.Bottom = 5;
            addGrps_bdr.Margin = margin1;
            btn_add.Visibility = Visibility.Collapsed;
            ok_btn.Visibility = Visibility.Visible;
            grp_enter_bdr.Visibility = Visibility.Visible;
        }
        public void hidingCollapsing()
        {
            grp_enter_bdr.BorderBrush = Brushes.Black;
            grp_enter_bdr.BorderThickness = new Thickness(0);
            Thickness margin1 = lstBxTask.Margin;
            margin1.Left = -40;
            margin1.Right = 9;
            margin1.Top = 5;
            margin1.Bottom = 5;
            addGrps_bdr.Margin = margin1;
            btn_add.Visibility = Visibility.Visible;
            ok_btn.Visibility = Visibility.Collapsed;
            grp_enter_bdr.Visibility = Visibility.Collapsed;
        }
        private void ok_btn_Click(object sender, RoutedEventArgs e)
        {
            string[] count = new string[] { };
           
            if (!string.IsNullOrEmpty(to_txtbx.Text))
            {
                count = to_txtbx.Text.Split(';');
                if (grp_enter_txtbx.Text != string.Empty)
                {
                    HoldGrpValue = grp_enter_txtbx.Text;
                    GetTask();
                    createEmailGroupXMLFiles(grp_enter_txtbx.Text);
                    hidingCollapsing();
                    if (grp_enter_txtbx.Text != null)
                    {
                        grpItems.Add(grp_enter_txtbx.Text);
                    }
                    grp_enter_txtbx.Text = "";
                }
               
                else
                {
                    grp_enter_bdr.BorderBrush = Brushes.Red;
                    grp_enter_bdr.BorderThickness = new Thickness(1);
                }
            }
            if (string.IsNullOrEmpty(to_txtbx.Text))
            {
                MessageBox.Show("Please enter emails and then add a group.");
                hidingCollapsing();
            }
        }
     

        private void to_txtbx_LostFocus(object sender, RoutedEventArgs e)
        {
          
            if (lstBxTask.SelectedIndex >= 0)
            {
                var grpnme = (lstBxTask.SelectedItem);
            }

        }
        public void createEmailGroupXMLFiles(string grpname)
        {

            if (grp_enter_txtbx.Text != string.Empty)
            {
                ParseEmails(to_txtbx.Text, out ParsedMails, out grpvalidate, out mailCount);
                if (grpvalidate == true && mailCount>0)
                {
                    if (File.Exists(grpname + ".xml") == false)
                    {
                        XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
                        xmlWriterSettings.Indent = true;
                        xmlWriterSettings.NewLineOnAttributes = true;
                        using (XmlWriter xmlWriter = XmlWriter.Create(grpname + ".xml", xmlWriterSettings))
                        {
                            xmlWriter.WriteStartDocument();
                            xmlWriter.WriteStartElement("emails");
                            foreach (var v in ParsedMails)
                            {
                                xmlWriter.WriteElementString("emailAddress", v);
                            }
                            xmlWriter.WriteEndElement();
                            xmlWriter.WriteEndDocument();
                            xmlWriter.Flush();
                            xmlWriter.Close();
                        }
                    }
                }
                else
                {
                    if (HoldGrpValue != string.Empty)
                    {
                        XDocument xDocument = XDocument.Load(grpname + ".xml");
                        XElement root = xDocument.Element("emails");
                        IEnumerable<XElement> rows = root.Descendants("emailAddress");
                        XElement firstRow = rows.First();
                        foreach (var v in ParsedMails)
                        {
                            firstRow.AddBeforeSelf(
                           new XElement(new XElement("emailAddress", v)));
                        }
                        xDocument.Save(grpname + ".xml");
                    }
                }
            }
        }
        public void ParseEmails(string emails, out string[] ParsedMails, out bool grpvalidate, out int mailCount)
        {
            ParsedMails = new string[] { };
            grpvalidate = true;
            mailCount = 0;
            if (emails.Contains(";"))
            {
                grpvalidate = true;
                ParsedMails = emails.Split(';').Distinct().ToArray();
                mailCount = ParsedMails.Count();
                validateEachEmail(ParsedMails, out PassCount, out FailCount);
                if (FailCount > 0)
                {
                    MessageBox.Show("Entered" + FailCount + "Incorrect Emails: ");
                    grpvalidate = false;
                } 
            }
            if(!emails.Contains(";"))
            {
                int count = 0;
                while (count < emails.Length && emails[count] == '@') count++;
                if (count > 0)
                {
                    MessageBox.Show("Please Provide emails in a correct Format (i.e) (example@gamil.com;example1@gmail.com and so on...");
                }
                else
                {
                    mailCount = 1;
                    bool validMail = IsValid(emails);
                    if (validMail == false)
                    {
                        MessageBox.Show("Entered Incorrect Email");
                        grpvalidate = false;
                    }
                    if (validMail == true)
                    {
                        MessageBox.Show("2 or more mails should be needed for ading in a group.");
                    }
                }
               
            }
            if (string.IsNullOrEmpty(emails))
            {
                grpvalidate = false;
            }
        }
        public void deleteDuplicateEntryXML(string path)
        {

            //XDocument xDocument = XDocument.Load("groups.xml");
            //foreach (var profileElement in xDocument.Descendants("ID")
            //                                        .ToList())
            //{
            //    if (profileElement.Value == task.Taskname)
            //    {
            //        profileElement.Remove();
            //    }
            //}
            //xDocument.Save("groups.xml");

            var xDoc = XDocument.Load(path);

            var duplicates = xDoc.Root
                                 .Elements("Group")
                                 .SelectMany(s => s.Elements("ID")
                                                   .GroupBy(b => (int)b)
                                                   .SelectMany(g => g.Skip(1)))
                                 .ToList();

            foreach (var item in duplicates)
                item.Remove();
            xDoc.Save(path);

            //XDocument xDoc = XDocument.Parse(path);
            //xDoc.Root.Elements("Group")
            //         .SelectMany(s => s.Elements("ID").GroupBy(g => g.Descendants("ID"))
            //                           .SelectMany(m => m.Skip(1))).Remove();

//            var doc = XDocument.Load(path);
//var elements = doc.Root.Elements().Distinct();

//var newDoc = new XDocument(
//    new XElement("ID", elements));
//newDoc.Save(path);

            //XDocument xDocument = XDocument.Load("groups.xml");
            //foreach (var profileElement in xDocument.Descendants("ID")
            //                                        .ToList())
            //{
            //    if (profileElement.Value == task.Taskname)
            //    {
            //        profileElement.Remove();
            //    }
            //}
            //xDocument.Save("groups.xml");
        }
        public void validateEachEmail(string[] _ParsedMails, out int PassCount, out int FailCount)
        {
            bool validMail=false;
            PassCount = 0;
            FailCount = 0;
            foreach(var email in _ParsedMails)
            {
                validMail = IsValid(email);
                if(validMail==false)
                {
                    FailCount++;
                }
                if(validMail==true)
                {
                    PassCount++;
                }
            }
          
        }


        private void lstBxTask_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           var grpnme = (lstBxTask.SelectedValue);
           var grp=(Group)grpnme;
           grp_enter_txtbx.Text = grp.Taskname;
           HoldGrpValue = grp.Taskname;
        }
    }

    #region PageViewModel
    public class PageViewModel : ObservableBase
    {
        public DelegateCommand GetXamlCommand { get; private set; }

        #region Constructor
        public PageViewModel()
        {
            GetXamlCommand = new DelegateCommand(() =>
            {
                Prop.BODY = this.text;
                GMailer.Send(this.Text,FileCollection._files);
                MessageBox.Show(this.Text);
            });
        }
        #endregion

        #region Name
        private string text = string.Empty;
        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                text = value;
                this.RaisePropertyChanged(p => p.Text);
            }
        }
        #endregion
    }
    #endregion

    #region Observable
    public abstract class ObservableBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    public static class ObservableBaseEx
    {
        public static void RaisePropertyChanged<T, TProperty>(this T observableBase, Expression<Func<T, TProperty>> expression) where T : ObservableBase
        {
            observableBase.RaisePropertyChanged(observableBase.GetPropertyName(expression));
        }

        public static string GetPropertyName<T, TProperty>(this T owner, Expression<Func<T, TProperty>> expression)
        {
            var memberExpression = expression.Body as MemberExpression;
            if (memberExpression == null)
            {
                var unaryExpression = expression.Body as UnaryExpression;
                if (unaryExpression != null)
                {
                    memberExpression = unaryExpression.Operand as MemberExpression;

                    if (memberExpression == null)
                        throw new NotImplementedException();
                }
                else
                    throw new NotImplementedException();
            }

            var propertyName = memberExpression.Member.Name;
            return propertyName;
        }
    }
    #endregion
}
