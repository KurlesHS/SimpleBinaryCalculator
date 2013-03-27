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

namespace BinaryCalculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    public class BinarySelector : StackPanel
    {
        public BinarySelector(int number) {
            bitNumber = number;
            Orientation = Orientation.Vertical;
            HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            VerticalAlignment = System.Windows.VerticalAlignment.Center;   
            GroupBox gb = new GroupBox();
            gb.Header = bitNumber.ToString();
            checkBox = new CheckBox();
            checkBox.IsChecked = false;
            gb.Content = checkBox;
            Children.Add(gb);
        }
        public int bitNumber { get; set; }
        public CheckBox checkBox { get; set; }
    }

    public partial class MainWindow : Window
    {
        private List<BinarySelector> listOfSelectors;
        public MainWindow()
        {
            InitializeComponent();
            listOfSelectors = new List<BinarySelector>();
            for (int i = 31; i >= 0; --i)
            {
                BinarySelector bs = new BinarySelector(i);
                bs.checkBox.Checked += recalculateNumber;
                bs.checkBox.Unchecked += recalculateNumber;
                listOfSelectors.Add(bs);
                stackPanel.Children.Add(bs);
            }
            recalculateNumber();
        }

        private void recalculateNumber()
        {
            int number = 0;
            foreach (BinarySelector bi in listOfSelectors)
            {
                number <<= 1;
                if (bi.checkBox.IsChecked == true)
                {
                    number |= 0x01;
                }
            }
            hexString.Content = "0x" + number.ToString("X8");
        }

        void recalculateNumber(object sender, RoutedEventArgs e)
        {
            recalculateNumber();
       }

        private void buttonCopyToContext_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(hexString.Content.ToString());
        }
    }
}
