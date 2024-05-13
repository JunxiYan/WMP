using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WMP
{

    /// <summary>
    /// binding 目标：
    /// 播放进度绑定sliderPosition.Maximum = MusicMedia.NaturalDuration.TimeSpan.TotalSeconds;
    /// 
    /// </summary>


    class Model : INotifyPropertyChanged
    {


        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }


 


    }
}
