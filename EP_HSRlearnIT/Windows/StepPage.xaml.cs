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

namespace EP_HSRlearnIT.Windows
{
    /// <summary>
    /// Interaction logic for StepPage.xaml
    /// </summary>
    public partial class StepPage : Page
    {
        private int _step;

        public StepPage()
        {
            InitializeComponent();
            _step = 1;
            ReplaceText(_step);
        }

        private void OnPreviousStepButton_Click(object sender, RoutedEventArgs e)
        {
            ReplaceText(--_step);
        }

        private void OnNextStepButton_Click(object sender, RoutedEventArgs e)
        {
            ReplaceText(++_step);
        }

        private void ReplaceText(int numOfStep)
        {
            switch(numOfStep)
            {
                case 1:
                    PreviousStepButton.IsEnabled = false;
                    NextStepButton.IsEnabled = true;
                    break;
                case 3:
                    PreviousStepButton.IsEnabled = true;
                    NextStepButton.IsEnabled = false;
                    break;
                default:
                    PreviousStepButton.IsEnabled = true;
                    NextStepButton.IsEnabled = true;
                    break;
            }
            StepDescriptionBox.Text = (string)Application.Current.FindResource("Step" + numOfStep);
            StepTitle.Text = "Schritt " + numOfStep;
        }
    }
}
