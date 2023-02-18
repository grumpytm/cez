using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace cez
{
    public enum FormType
    {
        MainMenu,
        Intrare,
        Retur,
    }

    public class FormFactory
    {
        public Form MakeForm(FormType formType, FormManager formManager)
        {
            Form result = null;

            switch (formType)
            {
                case FormType.MainMenu:
                    result = new Form1();
                    break;
                case FormType.Intrare:
                    result = new Intrare(formManager);
                    break;
                case FormType.Retur:
                    result = new Retur(formManager);
                    break;
            }

            return result;
        }
    }
}
