using System;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model.Core;
using Xpand.ExpressApp.ModelDifference.DictionaryStores;
using Xpand.ExpressApp;

namespace Xpand.ExpressApp.ModelDifference{
    public abstract class ModelDifferenceBaseModule : XpandModuleBase 
    {
        protected internal abstract bool? PersistentApplicationModelUpdated { get; set; }
        public event EventHandler<CreateCustomModelDifferenceStoreEventArgs> CreateCustomModelDifferenceStore;

        public void OnCreateCustomModelDifferenceStore(CreateCustomModelDifferenceStoreEventArgs e)
        {
            EventHandler<CreateCustomModelDifferenceStoreEventArgs> handler = CreateCustomModelDifferenceStore;
            if (handler != null) handler(this, e);
        }
        public override void Setup(XafApplication application)
        {
            base.Setup(application);
            application.LoggingOn += OnLoggingOn;
        }

        void OnLoggingOn(object sender, LogonEventArgs e)
        {
            var customModelDifferenceStoreEventArgs = new CreateCustomModelDifferenceStoreEventArgs();
            OnCreateCustomModelDifferenceStore(customModelDifferenceStoreEventArgs);
            if (!customModelDifferenceStoreEventArgs.Handled)
                new XpoModelDictionaryDifferenceStore(Application,  GetPath(), customModelDifferenceStoreEventArgs.ExtraDiffStores).Load((ModelApplicationBase) Application.Model);            
        }

        public abstract string GetPath();
    }
}