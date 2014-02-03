using System;
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
        private string _lastNumber;
        private bool _needToRecalculateNum;

        public MainWindow() {
            InitializeComponent();
            _listOfSelectors = new List<BinarySelector>();
            _needToRecalculateNum = true;
            for (var i = 31; i >= 0; --i) {
                var bs = new BinarySelector(i);
                bs.CheckBox.Checked += RecalculateNumber;
                bs.CheckBox.Unchecked += RecalculateNumber;
                _listOfSelectors.Add(bs);
                StackPanel.Children.Add(bs);
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
            _lastNumber = "0x" + number.ToString("X8");
            HexString.Content = _lastNumber;
            TexBlock.Text = HexString.Content.ToString();
        }

        private void RecalculateNumber(object sender, RoutedEventArgs e) {
            if (_needToRecalculateNum)
                RecalculateNumber();
        }

        private void buttonCopyToContext_Click(object sender, RoutedEventArgs e) {
            Clipboard.SetText(HexString.Content.ToString());
        }

        private void TexBlock_OnTextChanged(object sender, TextChangedEventArgs e) {
            int hexNumber;
            var text = TexBlock.Text.Trim(' ').ToLower();
            var isHexValue = false;
            var numStyles = NumberStyles.Integer;
            if (text.Length > 2 && text.Substring(0, 2) == "0x") {
                text = text.Substring(2);
                isHexValue = true;
                numStyles = NumberStyles.HexNumber;
            }
                
            try {
                int.TryParse(text, numStyles, CultureInfo.CurrentCulture, out hexNumber);
            } catch (ArgumentException) {
                TexBlock.Text = _lastNumber;
                return;
            }
            _lastNumber = isHexValue ? hexNumber.ToString("X") : hexNumber.ToString(CultureInfo.InvariantCulture);
            HexString.Content = "0x" + hexNumber.ToString("X8");
            var bit = 0x01;
            _needToRecalculateNum = false;
            if (_listOfSelectors != null) {
                for (var idx = _listOfSelectors.Count - 1; idx >= 0; --idx)
                {
                    _listOfSelectors[idx].CheckBox.IsChecked = (hexNumber & bit) != 0;
                    bit <<= 1;
                }    
            }
            _needToRecalculateNum = true;

        }
    }
}