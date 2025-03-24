using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StroopApp.Services.Navigation
{
    public interface INavigationService
    {
        void NavigateTo<T>(object parameter = null) where T : System.Windows.Controls.Page;
    }

}
