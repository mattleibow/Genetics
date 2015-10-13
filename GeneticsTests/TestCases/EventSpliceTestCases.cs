using Genetics;
using System;
using Genetics.Attributes;

namespace GeneticsTests.TestCases
{
    public class ExactEventSpliceTargetObject
    {
        [SpliceClick(Resource.Id.simpleButton)]
        public void ExactParametersMethod(object sender, EventArgs e)
        {
            throw new NotImplementedException("ExactParametersMethod");
        }
    }

    public class SimilarEventSpliceTargetObject
    {
        [SpliceClick(Resource.Id.simpleButton)]
        public void SimilarParametersMethod(object sender, object e)
        {
            throw new NotImplementedException("SimilarParametersMethod");
        }
    }

    public class DifferentEventSpliceTargetObject
    {
        [SpliceClick(Resource.Id.simpleButton)]
        public void SenderPerameterMethod(object sender)
        {
            throw new NotImplementedException("SenderPerameterMethod");
        }
    }

    public class ParameterlessEventSpliceTargetObject
    {
        [SpliceClick(Resource.Id.simpleButton)]
        public void ParameterlessMethod()
        {
            throw new NotImplementedException("ParameterlessMethod");
        }
    }

    public class InvalidEventSpliceTargetObject
    {
        [SpliceItemLongClick(Resource.Id.simpleButton)]
        public void InvalidEvent(object sender, EventArgs e)
        {
            throw new NotImplementedException("InvalidEvent");
        }
    }

    public class MultipleEventSpliceTargetObject
    {
        [SpliceClick(Resource.Id.simpleButton)]
        [SpliceClick(Resource.Id.simpleCheckBox)]
        public void MultipleMethod(object sender, EventArgs e)
        {
            throw new NotImplementedException(sender.GetType().Name);
        }
    }

    public class EventNotFoundTargetObject
    {
        [SpliceClick(Resource.Id.javaCastNativeToolbar)]
        public void MissingMethod(object sender, EventArgs e)
        {
            throw new NotImplementedException("MissingMethod");
        }
    }

    public class OptionalEventNotFoundTargetObject
    {
        [SpliceClick(Resource.Id.javaCastNativeToolbar, Optional = true)]
        public void MissingMethod(object sender, EventArgs e)
        {
            throw new NotImplementedException("MissingMethod");
        }
    }
}
