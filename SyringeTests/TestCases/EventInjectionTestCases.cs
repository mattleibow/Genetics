using Syringe;
using System;
using Syringe.Attributes;

namespace SyringeTests.TestCases
{
    public class ExactEventInjectionTargetObject
    {
        [InjectClick(Resource.Id.simpleButton)]
        public void ExactParametersMethod(object sender, EventArgs e)
        {
            throw new NotImplementedException("ExactParametersMethod");
        }
    }

    public class SimilarEventInjectionTargetObject
    {
        [InjectClick(Resource.Id.simpleButton)]
        public void SimilarParametersMethod(object sender, object e)
        {
            throw new NotImplementedException("SimilarParametersMethod");
        }
    }

    public class DifferentEventInjectionTargetObject
    {
        [InjectClick(Resource.Id.simpleButton)]
        public void SenderPerameterMethod(object sender)
        {
            throw new NotImplementedException("SenderPerameterMethod");
        }
    }

    public class ParameterlessEventInjectionTargetObject
    {
        [InjectClick(Resource.Id.simpleButton)]
        public void ParameterlessMethod()
        {
            throw new NotImplementedException("ParameterlessMethod");
        }
    }

    public class InvalidEventInjectionTargetObject
    {
        [InjectItemLongClick(Resource.Id.simpleButton)]
        public void InvalidEvent(object sender, EventArgs e)
        {
            throw new NotImplementedException("InvalidEvent");
        }
    }
}
