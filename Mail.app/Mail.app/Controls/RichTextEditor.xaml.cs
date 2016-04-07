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
using System.Windows.Documents;
namespace Mail.app.Controls
{

    /// <summary>
    /// Interaction logic for BindableRichTextbox.xaml
    /// </summary>
    /// 
    public partial class RichTextEditor : UserControl
    {
        

        public static readonly DependencyProperty TextProperty =
          DependencyProperty.Register("Text", typeof(string), typeof(RichTextEditor),
          new PropertyMetadata(string.Empty));

        public RichTextEditor()
        {
            InitializeComponent();
            //Paragraph p = rtb.Document.Blocks.FirstBlock as Paragraph;
            //p.LineHeight = 10;
        }

        public string Text
        {
            get { return GetValue(TextProperty) as string; }
            set
            {
                SetValue(TextProperty, value);
            }
        }
    }
}
