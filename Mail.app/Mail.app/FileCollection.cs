using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows.Controls;

namespace Mail.app
{
    public class Img
    {
        public Img(string value, Image img) { Str = value; Image = img; }
        public string Str { get; set; }
        public Image Image { get; set; }
    }
    class FileCollection
    {

        public ObservableCollection<string> Files
        {
            get
            {
                return _files;
            }
        }

        public static ObservableCollection<string> _files = new ObservableCollection<string>();
    }
}
