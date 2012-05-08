using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using DotSpatial.Controls;
using HydroDesktop.Interfaces;
using HydroDesktop.Configuration;
using System.ComponentModel.Composition;
using DotSpatial.Controls.Header;
using DotSpatial.Controls.Docking;

namespace IpyModelingControl 
{
    public class IPyPLSPlugIn : Extension
    {
        private IPyModelingControl.IPyPLSControl _IPyPLS;

        [Import("Shell")]
        private ContainerControl Shell { get; set; }

        [Import("SeriesControl", typeof(ISeriesSelector))]
        private ISeriesSelector _seriesSelector { get; set; }

        private const string _panelName = "PLSModel";
        private const string kPLSModel = "kPLSModel";

        private RootItem _PLSModelTab;
        private SimpleActionItem btnRun;
        private SimpleActionItem btnGoToPred;


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
            _PLSModelTab.SortOrder = 120;
            App.HeaderControl.Add(_PLSModelTab);



            //add sub-ribbons
            //section for adding data
            const string tGroupCaption = "";

            btnRun = new SimpleActionItem(kPLSModel, "Run", btnImport_Click);
            btnRun.LargeImage = Properties.Resources.OpenFile;
            btnRun.GroupCaption = tGroupCaption;
            btnRun.Enabled = true;
            App.HeaderControl.Add(btnRun);



            //section for validating
            const string grpValidate = "Validate";

            btnValidate = new SimpleActionItem(kPLSModel, "Validate Data", btnValidate_Click);
            btnValidate.LargeImage = Properties.Resources.Validate;
            btnValidate.GroupCaption = grpValidate;
            btnValidate.Enabled = false;
            App.HeaderControl.Add(btnValidate);


            //section for working with data
            const string grpManipulate = "Work with Data";

            btnComputeAO = new SimpleActionItem(kDataSheet, "Compute A O", _frmDatasheet.btnComputeAO_Click);
            btnComputeAO.LargeImage = Properties.Resources.Compute;
            btnComputeAO.GroupCaption = grpManipulate;
            btnComputeAO.Enabled = false;
            App.HeaderControl.Add(btnComputeAO);


            btnManipulate = new SimpleActionItem(kDataSheet, "Manipulate", _frmDatasheet.btnManipulate_Click);
            btnManipulate.LargeImage = Properties.Resources.Manipulate;
            btnManipulate.GroupCaption = grpManipulate;
            btnManipulate.Enabled = false;
            App.HeaderControl.Add(btnManipulate);


            btnTransform = new SimpleActionItem(kDataSheet, "Transform", _frmDatasheet.btnTransform_Click);
            btnTransform.LargeImage = Properties.Resources.Transform;
            btnTransform.GroupCaption = grpManipulate;
            btnTransform.Enabled = false;
            App.HeaderControl.Add(btnTransform);


            btnGoToModeling = new SimpleActionItem(kDataSheet, "Go To Model", _frmDatasheet.btnGoToModeling_Click);
            btnGoToModeling.LargeImage = Properties.Resources.GoToModeling;
            btnGoToModeling.GroupCaption = grpManipulate;
            btnGoToModeling.Enabled = false;
            App.HeaderControl.Add(btnGoToModeling);

        }

        void AddIPyPLSPanel()
        {
            var dp = new DockablePanel(kPLSModel, _panelName, _frmDatasheet, DockStyle.Fill);
            dp.DefaultSortOrder = 200;
            App.DockManager.Add(dp);
        }



        void SerializationManager_Serializing(object sender, SerializingEventArgs e)
        {
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

        void btnImport_Click(object sender, EventArgs e)
        {
            _frmDatasheet.btnImportData_Click(sender, e);
            btnValidate.Enabled = true;

        }

        void btnValidate_Click(object sender, EventArgs e)
        {
            _frmDatasheet.btnValidateData_Click(sender, e);
            btnComputeAO.Enabled = true;
            btnManipulate.Enabled = true;
            btnTransform.Enabled = true;
            btnGoToModeling.Enabled = true;

        }
    }
}
