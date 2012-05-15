using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotSpatial.Controls;
using HydroDesktop.Interfaces;

namespace IPyModeling
{
    public interface IPythonModeling
    {
        event EventHandler IronPythonInterfaceRequested;
        dynamic IronPythonInterface { get; set; }
    }


    public enum DependentVariableTransforms
    {
        none,
        Log10,
        Ln,
        Power
    }


    [Serializable]
    public class Transform
    {
        private DependentVariableTransforms _transformType;
        private double _dblExponent;

        public Transform() { }

        public Transform(DependentVariableTransforms type)
        {
            this._transformType = type;
            this._dblExponent = 1;
        }

        public Transform(DependentVariableTransforms type, double exponent)
        {
            this._transformType = type;
            this._dblExponent = exponent;
        }

        public DependentVariableTransforms Type
        {
            get { return _transformType; }
            set { _transformType = value; }
        }

        public double Exponent
        {
            get { return _dblExponent; }
            set { _dblExponent = value; }
        }
    }


    [Serializable]
    public class ModelState
    {
        private string strModel;
        private string strMethod;
        private double dblRegulatoryThreshold;
        private double dblDecisionThreshold;
        private Transform _depVarTransform;
        private dynamic ipyModel;

        public ModelState() { }

        public ModelState(dynamic model)
        {
            ipyModel = model;
        }

        public ModelState(string serializedModel, string method, Transform transform)
        {
            this.strModel = serializedModel;
            this.strMethod = method;
            this._depVarTransform = transform;
        }

        public dynamic Model
        {
            get { return ipyModel; }
        }

        public string ModelString
        {
            get { return strModel; }
            set { this.strModel = value; }
        }

        public string Method
        {
            get { return strMethod; }
            set { this.strMethod = value; }
        }

        public Transform DependentVariableTransform
        {
            get { return this._depVarTransform; }
            set { this._depVarTransform = value; }
        }

        public double RegulatoryThreshold
        {
            get { return dblRegulatoryThreshold; }
            set { this.dblRegulatoryThreshold = value; }
        }

        public double DecisionThreshold
        {
            get { return dblDecisionThreshold; }
            set { this.dblDecisionThreshold = value; }
        }
    }
}



