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
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using System.IO;

namespace ALL1
{
    /// <summary>
    /// Interaction logic for window3.xaml
    /// </summary>
    public partial class window3 : Window
    {
        public Grammar G = Grammar.Instance();
        public window3()
        {
            InitializeComponent();
        }

        private void button_next_Click(object sender, RoutedEventArgs e)
        {
            window4 w4 = new window4();
            App.Current.Windows[1].Close();
            w4.ShowDialog();

        }

        private void textbox_content_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            start();
        }

        public void start()
        {
            Duration time1 = new Duration(TimeSpan.FromSeconds(0.2));
            Duration time2 = new Duration(TimeSpan.FromSeconds(0.2));
            Duration timeall = new Duration(TimeSpan.FromSeconds(0.4));
            DoubleAnimation progressbar_animation1 = new DoubleAnimation(200.0, time1);
            DoubleAnimation progressbar_animation2 = new DoubleAnimation(200.0, time2);
            DoubleAnimation progressbar_animationall = new DoubleAnimation(200.0, timeall);


            

            progressbar_first.BeginAnimation(ProgressBar.ValueProperty, progressbar_animationall);
            progressbar_follow.BeginAnimation(ProgressBar.ValueProperty, progressbar_animationall);
            progressbar_dirsymb.BeginAnimation(ProgressBar.ValueProperty, progressbar_animationall);

            G.FIRSTandFOLLOW();
            textbox_content.Text = File.ReadAllText(@"D:\Work 3\Compilatoare\FF.txt");

            if (G.isLL1() == false)
            {
                textbox_content.AppendText(System.Environment.NewLine);
                textbox_content.AppendText("Gramatica nu este o gramatica LL1!");


            }
            else
            {
                textbox_content.AppendText(System.Environment.NewLine);
                textbox_content.AppendText("Gramatica este o gramatica LL1!");
            }
        }

    }
}
