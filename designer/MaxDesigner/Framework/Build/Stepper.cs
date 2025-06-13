using System;

namespace Metreos.Max.Framework
{
    /// <summary>
    /// The purpose of stepper is that there are very individual steps relating to checking the map, as well as
    /// quite a few steps to constructing the metadata.  
    /// 
    /// Without stepper, it was necessary to check between every single function if a method generated an error,
    /// which leads to coding errors.  
    /// 
    /// In addition, the order of the steps is critical, because many methods depend on certain elements of the map
    /// to be verified safe. With this methodology, the order in function are being called is much easier to see, 
    /// and keep straight.
    /// </summary>
    public abstract class Stepper 
    {
        protected ConstructionErrorInformation errorInformation;
        protected ConstructionWarningInformation warningInformation;
        protected bool error;
        protected bool warning;
        protected UpdateErrorStatus errorOccuredDelegate;
        protected UpdateErrorStatus warningOccuredDelegate;
    
        protected System.Delegate executeStep;
        protected Stepper parent;
    
        public Stepper(Stepper parent)
        {
            this.parent = parent;
            this.executeStep = null;
            this.error = false;
            this.warning = false;
            errorOccuredDelegate = new UpdateErrorStatus(ErrorOccured);
            warningOccuredDelegate = new UpdateErrorStatus(WarningOccured);
            errorInformation = new ConstructionErrorInformation(errorOccuredDelegate);
            warningInformation = new ConstructionWarningInformation(warningOccuredDelegate);

            Initialize();
        }
  
        protected virtual void Reset()
        {
            executeStep = null;
        }

        protected virtual void Initialize()
        { 
            ConstructSteps();
        }

        protected abstract void ConstructSteps();


        /// <summary>Call to construct (or reconstruct) metadata</summary>
        public void Execute(object[] args)
        {
            Delegate[] allSteps = executeStep.GetInvocationList();

            for(int i = 0; i < allSteps.Length; i++)
            {
                if(error) break;

                allSteps[i].DynamicInvoke(args);
            }

            PropogateErrors();
        }


        public void PropogateErrors()
        {
            if(parent != null)
            {
                if(errorInformation.Errors != null)
                    parent.AccumulateErrors(errorInformation);
                if(warningInformation.Warnings != null)
                    parent.AccumulateWarnings(warningInformation);
            }    
        }


        public void AccumulateErrors(ConstructionErrorInformation errorInformation)
        {
            this.errorInformation.AddErrors(errorInformation.Errors);
        }


        public void AccumulateWarnings(ConstructionWarningInformation warningInformation)
        {
            this.warningInformation.AddWarnings(warningInformation.Warnings);
        }


        protected void ErrorOccured(bool status)
        {
            error = status;
        }


        protected void WarningOccured(bool status)
        {
            warning = status;
        }
    }
}
