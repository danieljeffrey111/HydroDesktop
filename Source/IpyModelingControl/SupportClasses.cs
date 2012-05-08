using System;
using System.Collections.Generic;
using System.Data;

namespace IpyModelingControl
{
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
        }

        public double Exponent
        {
            get { return _dblExponent; }
        }
    }
}