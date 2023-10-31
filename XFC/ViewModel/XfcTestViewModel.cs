using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using XFC.View;

namespace XFC.ViewModel
{

  
    public class XfcTestViewModel:ViewModelBase
    {
        public ICommand ConfirmCommand { get; }
        public ICommand CancelCommand { get; }

      
        public XfcTestViewModel() {

            ConfirmCommand = new  RelayCommand(ConfirmClick);
            CancelCommand = new RelayCommand(CancelClick);
         

        }   

        private void ConfirmClick()
        {

        }
        private void CancelClick()
        {
            Form_ShiYanCanShu.GetInstance().Close();
        }

     


    }
}
