using DocumentFormat.OpenXml.Presentation;
using StroopApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace StroopApp.Models.Simon
{
    public class SimonKeyMapping
    {
        public SimonAnswer Answer { get; set; }
        public Key Key { get; set; }
        public SimonKeyMapping (SimonAnswer answer, Key key){
            Answer = answer;
            Key = key;
        } 
        
    }
}


