﻿using System.Windows.Controls;
using StroopApp.Models;
using StroopApp.ViewModels.Experiment;
namespace StroopApp.Views.Experiment.Experimenter
{
    public partial class ExperimentProgressView : UserControl
    {
        public ExperimentProgressView(ExperimentSettings Settings)
        {
            InitializeComponent();
            DataContext = new ExperimentProgressViewModel(Settings);
        }
    }
}