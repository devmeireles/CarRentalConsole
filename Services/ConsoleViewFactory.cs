using CarRentalConsole.Interfaces;
using CarRentalConsole.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarRentalConsole.Services
{
    internal class ConsoleViewFactory
    {
        private IView GetView(EMenuOption menuOption)
        {

            switch (menuOption)
            {
                case EMenuOption.Main:
                    return new MainMenu();
                default:
                    return new NotFoundMenu();
            }
        }

        public void DisplayView(EMenuOption menuOption)
        {
            IView view = GetView(menuOption);
            view.Display();
        }
    }
}
