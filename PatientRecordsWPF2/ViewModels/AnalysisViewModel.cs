using MVVMSample.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PatientRecordsWPF2.ViewModels
{
    class AnalysisViewModel : INotifyPropertyChanged
    {
        private readonly Domain.Analysis analysis;
        private readonly ICommand analysisCmd;
        private readonly ObservableCollection<String> symptoms;

        public AnalysisViewModel()
        {
            analysis = new Domain.Analysis();
            analysisCmd = new RelayCommand(Analysis, CanAnalysis);
            symptoms = new ObservableCollection<string>();
            symptoms.Add("test");
        }

        public DateTime? From
        {
            get { return analysis.From; }
            set
            {
                analysis.From = value;
                RaisePropertyChanged(() => this.From);
            }
        }

        public DateTime? To
        {
            get { return analysis.To; }
            set
            {
                analysis.To = value;
                RaisePropertyChanged(() => this.To);
            }
        }

        public ObservableCollection<String> Symptoms { get { return symptoms; } }

        public ICommand AnalysisCmd { get { return analysisCmd; } }

        private bool CanAnalysis(object obj)
        {
            if(Symptoms.Count>0 && To!=null && From!=null)
                return true;
            return false;
        }

        private void Analysis(object obj)
        {
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string propertyName)
        {
            //Fire the PropertyChanged event in case somebody subscribed to it
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        protected void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            if (propertyExpression.Body.NodeType == ExpressionType.MemberAccess)
            {
                var memberExpr = propertyExpression.Body as MemberExpression;
                string propertyName = memberExpr.Member.Name;
                this.RaisePropertyChanged(propertyName);
            }
        }
    }
}
