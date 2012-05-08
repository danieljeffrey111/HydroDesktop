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

namespace DatasheetTest
{
    public class DatasheetPlugIn : Extension
    { 
        private frmDatasheet _frmDatasheet;

        [Import("Shell")]
        private ContainerControl Shell { get; set; }

        [Import("SeriesControl", typeof(ISeriesSelector))]
        private ISeriesSelector _seriesSelector { get; set; }

        private const string _panelName = "Datasheet";
        private const string kDataSheet = "kDataSheet";

        private RootItem _DatasheetTab;
        private SimpleActionItem btnImport;
        private SimpleActionItem btnValidate;
        private SimpleActionItem btnComputeAO;
        private SimpleActionItem btnManipulate;
        private SimpleActionItem btnTransform;
        private SimpleActionItem btnGoToModeling;
        
        
        public override void Deactivate()
        {
            //remove ribbon tab
            App.HeaderControl.RemoveAll();

            //remove plugin panel
            App.DockManager.Remove(kDataSheet);

            _frmDatasheet = null;

            //this line to ensure "enabled is set to false
            base.Deactivate();
        }

        public override void Activate()
        {
            //if (_seriesSelector != null)
            //{
                _frmDatasheet = new frmDatasheet();
                AddDatasheetPanel();
                AddDatasheetRibbon();
            //}

            //when panel is selected activate seriesview and ribbon tab
            App.DockManager.ActivePanelChanged += new EventHandler<DotSpatial.Controls.Docking.DockablePanelEventArgs>(DockManager_ActivePanelChanged);

            App.DockManager.SelectPanel("datasheetSeriesView");
            App.HeaderControl.SelectRoot(kDataSheet);

            //when root item is selected
            App.HeaderControl.RootItemSelected +=new EventHandler<RootItemEventArgs>(HeaderControl_RootItemSelected);
            
            //assign project saving/loading events
            App.SerializationManager.Serializing += new EventHandler<SerializingEventArgs>(SerializationManager_Serializing);
            //allow plugin to deal with any custom settings that were deserialized
            App.SerializationManager.Deserializing += new EventHandler<SerializingEventArgs>(SerializationManager_Deserializing);

            base.Activate(); //ensures "enabled" is set to true
        }

        void HeaderControl_RootItemSelected(object sender, RootItemEventArgs e)
        {
            if (e.SelectedRootKey == kDataSheet)
            {
                App.DockManager.SelectPanel(kDataSheet);
            }
        }
        
        //add a datasheet plugin root item
        void AddDatasheetRibbon()
        {
            _DatasheetTab = new RootItem(kDataSheet, _panelName);
            _DatasheetTab.SortOrder = 120;
            App.HeaderControl.Add(_DatasheetTab);
            

            
            //add sub-ribbons
            //section for adding data
            const string tGroupCaption = "Add";

            btnImport = new SimpleActionItem(kDataSheet, "Import Data", btnImport_Click);
            btnImport.LargeImage = Properties.Resources.OpenFile;
            btnImport.GroupCaption = tGroupCaption;
            btnImport.Enabled = true;
            App.HeaderControl.Add(btnImport);
            
            

            //section for validating
            const string grpValidate = "Validate";

            btnValidate = new SimpleActionItem(kDataSheet, "Validate Data", btnValidate_Click);
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

        void AddDatasheetPanel()
        {
            var dp = new DockablePanel(kDataSheet, _panelName, _frmDatasheet, DockStyle.Fill);
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
            if (e.ActivePanelKey == kDataSheet)
            {
                App.DockManager.SelectPanel("TestSeriesView");
                App.HeaderControl.SelectRoot(kDataSheet);
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