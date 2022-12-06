using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace VideoPlayerWPF.Controls
{
    /// <summary>
    /// Interaction logic for ItemsMenu.xaml
    /// </summary>
    public partial class ItemsMenu : UserControl
    {
        public ItemsMenu()
        {
            InitializeComponent();
            SpeedRatioBox.MouseWheel += (object sender, MouseWheelEventArgs e) =>
            {
                if (e.Delta > 0)
                {
                    try
                    {
                        int value = Convert.ToInt16(SpeedRatioBox.Text);
                        value++;
                        SpeedRatioBox.Text = value.ToString();
                    }
                    catch (Exception) { }
                }
                else
                {
                    try
                    {
                        int value = Convert.ToInt16(SpeedRatioBox.Text);
                        if(value > 0)
                            value--;
                        SpeedRatioBox.Text = value.ToString();
                    }
                    catch (Exception) { }
                }
            };
            BalanceBox.MouseWheel += (object sender, MouseWheelEventArgs e) =>
            {
                if (e.Delta > 0)
                {
                    try
                    {
                        double value = Convert.ToDouble(BalanceBox.Text);
                        if(value < 1)
                        value += .1;
                        BalanceBox.Text = value.ToString();
                    }
                    catch (Exception) { }
                }
                else
                {
                    try
                    {
                        double value = Convert.ToDouble(BalanceBox.Text);
                        if(value > -1)
                            value -= .1;
                        BalanceBox.Text = value.ToString();
                    }
                    catch (Exception) { }
                }
            };
        }
    }
}
