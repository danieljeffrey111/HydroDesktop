using System;
using System.Collections.Generic;
using System.Data;
using IPyModeling;


namespace IPyModelingControl
{
    [Serializable]
    public class ProjectState
    {
        private ModelState _modelState;
        private Dictionary<string, List<double>> _dictValidation;
        private Dictionary<string, List<double>> _dictThresholding;
        private string _strThreshold;
        private string _strExponent;
        private bool _boolVirgin;
        private int _intThresholdIndex;
        private bool _boolThresholdingButtonsVisible;
        
        public ProjectState() { }

        public ProjectState(IPyModelingControl project) //string serializedModel, string method, List<double> specificity, List<double> tpos, List<double> tneg, List<double> fpos, List<double> fneg)
        {
            //Save the model state
            this._modelState = new ModelState(project.Model);
            _modelState.ModelString = project.IronPythonInterface.Serialize(project.Model);
            _modelState.DependentVariableTransform = project.DependentVariableTransform;
            _modelState.Method = project.Method;

            try
            {
                _modelState.RegulatoryThreshold = Convert.ToDouble(project.ThresholdTextbox);
                _modelState.DecisionThreshold = Convert.ToDouble(project.DecisionThresholdLabel);
            }
            catch (InvalidCastException)
            { return; }

            //Save the project state
            //Save the lists that we use to make the validation chart
            this._dictValidation = new Dictionary<string, List<double>>();
            _dictValidation.Add("tpos", project.TruePositives);
            _dictValidation.Add("tneg", project.TrueNegatives);
            _dictValidation.Add("fpos", project.FalsePositives);
            _dictValidation.Add("fneg", project.FalseNegatives);
            _dictValidation.Add("specificity", project.ValidationSpecificities);

            //Save the lists that we use to set the model's decision threshold.
            this._dictThresholding = new Dictionary<string, List<double>>();
            _dictThresholding.Add("specificity", project.CandidateSpecificities);
            _dictThresholding.Add("threshold", project.Thresholds);
            _intThresholdIndex = project.ThresholdingIndex;

            //Save the state of UI elements.
            this._strThreshold = project.ThresholdTextbox;
            this._strExponent = project.ExponentTextbox;
            this._boolVirgin = project.VirginState;
            this._boolThresholdingButtonsVisible = project.ThresholdingButtonsVisible;
        }

        public ModelState ModelState
        {
            get { return _modelState; }
            set { this._modelState = value; }
        }

        public Dictionary<string, List<double>> ValidationDictionary
        {
            get { return _dictValidation; }
            set { this._dictValidation = value; }
        }

        public Dictionary<string, List<double>> ThresholdingDictionary
        {
            get { return _dictThresholding; }
            set { this._dictThresholding = value; }
        }

        public string ThresholdTextBox
        {
            get { return _strThreshold; }
            set { this._strThreshold = value; }
        }

        public string ExponentTextbox
        {
            get { return _strExponent; }
            set { this._strExponent = value; }
        }

        public bool VirginState
        {
            get { return _boolVirgin; }
            set { this._boolVirgin = value; }
        }

        public bool ThresholdingButtonsVisible
        {
            get { return _boolThresholdingButtonsVisible; }
            set { this._boolThresholdingButtonsVisible = value; }
        }

        public int ThresholdingIndex
        {
            get { return _intThresholdIndex; }
            set { this._intThresholdIndex = value; }
        }
    }
}