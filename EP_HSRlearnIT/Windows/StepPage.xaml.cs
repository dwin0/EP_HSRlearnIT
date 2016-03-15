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
        #region Private Members

        private int _step = 1;
        private const int _SMALLEST_STEP = 1;
        private const int _BIGGEST_STEP = 3;
        #endregion


        #region Constructors
        public StepPage()
        {
            InitializeComponent();
            ReplaceText(_step);
        }
        #endregion


        #region Private Methods

        private void OnPreviousStepButton_Click(object sender, RoutedEventArgs e)
        {
            ReplaceText(--_step);
        }

        private void OnNextStepButton_Click(object sender, RoutedEventArgs e)
        {
            ReplaceText(++_step);
        }

        private void ReplaceText(int stepNumber)
        {
            switch(stepNumber)
            {

                case _SMALLEST_STEP:
                    PreviousStepButton.IsEnabled = false;
                    NextStepButton.IsEnabled = true;
                    break;
                case _BIGGEST_STEP:
                    PreviousStepButton.IsEnabled = true;
                    NextStepButton.IsEnabled = false;
                    break;
                default:
                    PreviousStepButton.IsEnabled = true;
                    NextStepButton.IsEnabled = true;
                    break;
            }

            StepDescriptionBox.Text = Application.Current.FindResource("Step" + stepNumber) as string;
            StepTitle.Text = "Schritt " + stepNumber;
        }
        #endregion
    }
}
