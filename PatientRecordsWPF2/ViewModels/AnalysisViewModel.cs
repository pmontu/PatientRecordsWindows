using MVVMSample.Common;
using System;
using System.Collections;
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
        private readonly ICommand removeSymptomCmd;
        private readonly ICommand addSymptomCmd;
        private readonly ObservableCollection<String> symptoms;

        private String symptom;

        public AnalysisViewModel()
        {
            analysis = new Domain.Analysis();
            analysisCmd = new RelayCommand(Analysis, CanAnalysis);
            removeSymptomCmd = new RelayCommand(RemoveSymptom);
            addSymptomCmd = new RelayCommand(AddSymptom, CanAddSymptom);
            symptoms = new ObservableCollection<string>();
            SymptomsDb = new ObservableCollection<String>(BusinessLogicLayer.Symptom.getSymptomsDb());
        }

        public ObservableCollection<String> SymptomsDb { get; private set; }

        public String Symptom
        {
            get { return symptom; }
            set
            {
                symptom = value;
                RaisePropertyChanged(() => this.Symptom);
            }
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

        public ICommand RemoveSymptomCmd { get { return removeSymptomCmd; } }

        private void RemoveSymptom(object obj)
        {
            Symptoms.Remove((String)obj);
        }

        public ICommand AddSymptomCmd { get { return addSymptomCmd; } }

        private void AddSymptom(object obj)
        {
            Symptoms.Add(Symptom);
            Symptom = "";
        }

        private bool CanAddSymptom(object obj)
        {
            return !String.IsNullOrEmpty(Symptom);
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
