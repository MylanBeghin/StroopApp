using StroopApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace StroopApp.ViewModels.Experiment.Stroop
{
    public class AmorceControlViewModel
    {
        public AmorceType Amorce { get; set; }
        public AmorceControlViewModel(AmorceType amorce)
        {
            Amorce = amorce;
        }
    }
}
