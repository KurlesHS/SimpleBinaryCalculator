using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace BinaryCalculator
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public class BinarySelector : StackPanel
    {
        public BinarySelector(int number) {
            BitNumber = number;
            Orientation = Orientation.Vertical;
            HorizontalAlignment = HorizontalAlignment.Stretch;
            VerticalAlignment = VerticalAlignment.Center;
            var gb = new GroupBox {Header = BitNumber.ToString(CultureInfo.InvariantCulture)};
            CheckBox = new CheckBox {IsChecked = false};
            gb.Content = CheckBox;
            Children.Add(gb);
        }

        public int BitNumber { get; set; }
        public CheckBox CheckBox { get; set; }
    }

    public partial class MainWindow
    {
        private readonly List<BinarySelector> _listOfSelectors;

        public MainWindow() {
            InitializeComponent();
            _listOfSelectors = new List<BinarySelector>();
            for (var i = 31; i >= 0; --i) {
                var bs = new BinarySelector(i);
                bs.CheckBox.Checked += RecalculateNumber;
                bs.CheckBox.Unchecked += RecalculateNumber;
                _listOfSelectors.Add(bs);
                stackPanel.Children.Add(bs);
            }
            RecalculateNumber();
        }

        private void RecalculateNumber() {
            var number = 0;
            foreach (var bi in _listOfSelectors) {
                number <<= 1;
                if (bi.CheckBox.IsChecked == true) {
                    number |= 0x01;
                }
            }
            hexString.Content = "0x" + number.ToString("X8");
        }

        private void RecalculateNumber(object sender, RoutedEventArgs e) {
            RecalculateNumber();
        }

        private void buttonCopyToContext_Click(object sender, RoutedEventArgs e) {
            Clipboard.SetText(hexString.Content.ToString());
        }
    }
}