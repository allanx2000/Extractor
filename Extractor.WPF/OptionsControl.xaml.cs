using Extractor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Extractor.WPF
{
    /// <summary>
    /// Interaction logic for OptionsControl.xaml
    /// </summary>
    public partial class OptionsControl : UserControl
    {
        public OptionsControl()
        {
            InitializeComponent();
        }

        private AbstractExtractor extractor;
        public void SetExtractor(AbstractExtractor extractor)
        {
            this.extractor = extractor;

            var keys = extractor.GetItemKeys();
            
                        
        }

        public string PrependText
        {
            get
            {
                return PrependTextBox.Text;
            }
        }

        public string AppendText
        {
            get
            {
                return AppendTextBox.Text;
            }
        }
    }
}
