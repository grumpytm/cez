using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

using System.Windows.Forms;

namespace cez
{
    public class FormManager
    {
        private Form _currentForm;
        private FormType _currentFormType;

        private Stack<FormType> _previousFormTypes;
        private Stack<Form> _previousForms;

        private FormFactory _formFactory;

        //flag to avoid removing twice from the stack.
        private bool _removedFromStack;

        public FormManager(Form defaultForm, FormType currentFormType)
        {
            if (defaultForm == null)
            {
                throw new Exception("wrong parameters to constructor");
            }
            _currentForm = defaultForm;
            _currentForm.Closing += new System.ComponentModel.CancelEventHandler(OnFormClosing);
            _currentFormType = currentFormType;

            _previousForms = new Stack<Form>();
            _formFactory = new FormFactory();
            _previousFormTypes = new Stack<FormType>();

            _removedFromStack = false;

        }

        public void ShowForm(FormType form)
        {
            //don't hide the previous form.
            //There were some problems on the target when I tried that.
            //The form will be under the new one, so the user will not see it.
            _previousFormTypes.Push(_currentFormType);
            _previousForms.Push(_currentForm);

            _currentFormType = form;
            _currentForm = _formFactory.MakeForm(form, this);
            _currentForm.Closing += new System.ComponentModel.CancelEventHandler(OnFormClosing);
            _currentForm.Show();
        }

        public void HideForm()
        {
            //avoid removing twice from the stack
            //Close triggeres an Closing event which is handled
            //by OnFormClosing
            _removedFromStack = true;
            _currentForm.Close();

            if (_previousForms.Count > 0)
            {
                //the previous form is already visible, so it is
                //no need to show it again. Because the form on top
                //was closed, it will be visible to the user.
                _currentFormType = _previousFormTypes.Pop();
                _currentForm = _previousForms.Pop();
            }
        }

        void OnFormClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //the current form is already closed by .NET framework.
            //only remove from the stack if HideForm was not called
            if (_previousForms.Count > 0 && !_removedFromStack)
            {
                _currentFormType = _previousFormTypes.Pop();
                _currentForm = _previousForms.Pop();
            }

            //prepare for the next form to be hidden.
            _removedFromStack = false;
        }

        public FormType GetPreviousForm()
        {
            if (_previousFormTypes.Count > 0)
            {
                return _previousFormTypes.Peek();
            }
            else
            {
                throw new Exception("no previous form");
            }
        }
    }
}
