using Extractor.WPF.Models;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ExtractorWindow : Window
    {
        private readonly ExtractorWindowViewModel vm;

        public ExtractorWindow()
        {
            InitializeComponent();

            vm = new ExtractorWindowViewModel(this);
            this.DataContext = vm;
        }
    }
}
