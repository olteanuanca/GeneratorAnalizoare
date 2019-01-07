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
    /// Interaction logic for window2.xaml
    /// </summary>
    public partial class window2 : Window
    {
        
        public Grammar G = Grammar.Instance();
        

        public window2()
        {
            InitializeComponent();


         
        }

        public void start()
        {
            

         
            Duration time1 = new Duration(TimeSpan.FromSeconds(0.2));
            Duration time2 = new Duration(TimeSpan.FromSeconds(0.2));
            Duration timeall = new Duration(TimeSpan.FromSeconds(0.4));
            DoubleAnimation progressbar_animation1 = new DoubleAnimation(200.0, time1);
            DoubleAnimation progressbar_animation2 = new DoubleAnimation(200.0, time2);
            DoubleAnimation progressbar_animationall = new DoubleAnimation(200.0, timeall);

            G.Load_grammar();
            progressbar_check1.BeginAnimation(ProgressBar.ValueProperty, progressbar_animation1);
            progressbar_check2.BeginAnimation(ProgressBar.ValueProperty, progressbar_animation2);
            G.ModifyGrammar();

            textbox_content.Text = File.ReadAllText(@"D:\Work 3\Compilatoare\out.txt");

            progressbar_checkall.BeginAnimation(ProgressBar.ValueProperty, progressbar_animationall);

        }
        private void textbox_content_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            start();
        }

        private void button_next_Click(object sender, RoutedEventArgs e)
        {
            window3 w3 = new window3();
            App.Current.Windows[1].Close();
            w3.ShowDialog();

        }
    }
}
