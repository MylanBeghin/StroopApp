using StroopApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using StroopApp.ViewModels.Experiment.Stroop;
namespace StroopApp.Views.Experiment.Participant.Stroop
{
    public partial class AmorceControl : UserControl
    {
        public AmorceControl(AmorceType amorce)
        {
            InitializeComponent();
            DataContext = new AmorceControlViewModel(amorce);
        }
    }
}
