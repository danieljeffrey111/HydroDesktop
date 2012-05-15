using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using DotSpatial.Controls;
using HydroDesktop.Interfaces;
using HydroDesktop.Configuration;
using HydroDesktop.Database;
using System.ComponentModel.Composition;
using DotSpatial.Controls.Header;
using DotSpatial.Controls.Docking;

namespace IpyModelingControl 
{
    public class IPyPLSPlugIn : Extension
    {
        

        [Import("Shell")]
        private ContainerControl Shell { get; set; }

        [Import("SeriesControl", typeof(ISeriesSelector))]
        private ISeriesSelector _seriesSelector { get; set; }

        private const string _panelName = "PLSModel";
        private const string kPLSModel = "kPLSModel";

        private RootItem _PLSModelTab;

        private SimpleActionItem btnRun;
        private SimpleActionItem btnGoToPred;

        private IPyModelingControl.IPyPLSControl _IPyPLS;

        public override void Deactivate()
        {
            //remove ribbon tab
            App.HeaderControl.RemoveAll();

            //remove plugin panel
            App.DockManager.Remove(kPLSModel);

            
            _IPyPLS = null;

            //this line to ensure "enabled is set to false
            base.Deactivate();
        }

        public override void Activate()
        {
            //if (_seriesSelector != null)
            //{
            _IPyPLS = new IPyModelingControl.IPyPLSControl();
            AddIPyPLSPanel();
            AddIPyPLSRibbon();
            //}

            //when panel is selected activate seriesview and ribbon tab
            App.DockManager.ActivePanelChanged += new EventHandler<DotSpatial.Controls.Docking.DockablePanelEventArgs>(DockManager_ActivePanelChanged);

            App.DockManager.SelectPanel("PLSModelSeriesView");
            App.HeaderControl.SelectRoot(kPLSModel);

            //when root item is selected
            App.HeaderControl.RootItemSelected += new EventHandler<RootItemEventArgs>(HeaderControl_RootItemSelected);

            //assign project saving/loading events
            App.SerializationManager.Serializing += new EventHandler<SerializingEventArgs>(SerializationManager_Serializing);
            //allow plugin to deal with any custom settings that were deserialized
            App.SerializationManager.Deserializing += new EventHandler<SerializingEventArgs>(SerializationManager_Deserializing);

            base.Activate(); //ensures "enabled" is set to true
        }

        void HeaderControl_RootItemSelected(object sender, RootItemEventArgs e)
        {
            if (e.SelectedRootKey == kPLSModel)
            {
                App.DockManager.SelectPanel(kPLSModel);
            }
        }

        //add a datasheet plugin root item
        void AddIPyPLSRibbon()
        {
            _PLSModelTab = new RootItem(kPLSModel, _panelName);
            _PLSModelTab.SortOrder = 100;
            App.HeaderControl.Add(_PLSModelTab);



            //add sub-ribbons
            //section for adding data
            const string rGroupCaption = _panelName;

            btnRun = new SimpleActionItem(kPLSModel, "Run", btnRunn_Click);
            btnRun.LargeImage = Properties.Resources.Run;
            btnRun.GroupCaption = rGroupCaption;
            btnRun.Enabled = true;
            App.HeaderControl.Add(btnRun);



            btnGoToPred = new SimpleActionItem(kPLSModel, "Go To Prediction", _IPyPLS.btnSelectModel_Click);
            btnGoToPred.LargeImage = Properties.Resources.GoToPrediction;
            btnGoToPred.GroupCaption = rGroupCaption;
            btnGoToPred.Enabled = false;
            App.HeaderControl.Add(btnGoToPred);

        }

        void AddIPyPLSPanel()
        {
            var dp = new DockablePanel(kPLSModel, _panelName, _IPyPLS, DockStyle.Fill);
            dp.DefaultSortOrder = 60;
            App.DockManager.Add(dp);
        }



        void SerializationManager_Serializing(object sender, SerializingEventArgs e)
        {
            //go to packProject
            //_IPyPLS.PackProjectState();
            //throw new NotImplementedException();
        }
        void SerializationManager_Deserializing(object sender, SerializingEventArgs e)
        {
            //btnValidate.Enabled = true;
            //string customSettingValue = App.SerializationManager.GetCustomSetting<string>(_pluginName + "_Setting1", "enter custom setting:");
            //throw new NotImplementedException();
        }

        void DockManager_ActivePanelChanged(object sender, DotSpatial.Controls.Docking.DockablePanelEventArgs e)
        {
            if (e.ActivePanelKey == kPLSModel)
            {
                App.DockManager.SelectPanel("PLSModelSeriesView");
                App.HeaderControl.SelectRoot(kPLSModel);
            }
        }

        void rb_Click(object sender, EventArgs e)
        {
            //restore layout
            foreach (Extension ext in App.Extensions)
            {
                ext.Deactivate();
            }

            foreach (Extension ext in App.Extensions)
            {
                ext.Activate();
            }
        }

        void btnRunn_Click(object sender, EventArgs e)
        {
            _IPyPLS.btnRun_Click(sender, e);
            btnGoToPred.Enabled = true;

        }

        
    }
}
