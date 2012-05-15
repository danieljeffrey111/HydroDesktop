using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel.Composition;
using DotSpatial.Controls;
using HydroDesktop.Interfaces;
using HydroDesktop.Configuration;
using DotSpatial.Controls.Header;
using DotSpatial.Controls.Docking;

namespace MLRPrediction
{
    class IPypredictionPlugin : Extension

    {
        [Import("Shell")]
        private ContainerControl Shell { get; set; }

        [Import("SeriesControl", typeof(ISeriesSelector))]
        private ISeriesSelector _seriesSelector { get; set; }

        private const string _panelName = "IPyPrediction";
        private const string kIPyPrediction = "kIPyPrediction";

        private RootItem _MLRpredTab;

        private SimpleActionItem btnImportIVs;
        private SimpleActionItem btnImportOBs;
        private SimpleActionItem btnPlot;
        private SimpleActionItem btnClear;
        private SimpleActionItem btnExportAsCSV;
        private SimpleActionItem btnIVDataValidation;
        private SimpleActionItem btnMakePrediction;

        private MLRPrediction.frmIPyPrediction _frmIPyPred;

        public override void Deactivate()
        {
            //remove ribbon tab
            App.HeaderControl.RemoveAll();

            //remove plugin panel
            App.DockManager.Remove(kIPyPrediction);


            _frmIPyPred = null;

            //this line to ensure "enabled is set to false
            base.Deactivate();
        }

        public override void Activate()
        {
            //if (_seriesSelector != null)
            //{
            _frmIPyPred = new MLRPrediction.frmIPyPrediction();
            AddMLRPanel();
            AddMLRRibbon();
            //}

            //when panel is selected activate seriesview and ribbon tab
            App.DockManager.ActivePanelChanged += new EventHandler<DotSpatial.Controls.Docking.DockablePanelEventArgs>(DockManager_ActivePanelChanged);

            App.DockManager.SelectPanel("IpyPredSeriesView");
            App.HeaderControl.SelectRoot(kIPyPrediction);

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
            if (e.SelectedRootKey == kIPyPrediction)
            {
                App.DockManager.SelectPanel(kIPyPrediction);
            }
        }

        void AddMLRRibbon()
        {
            _MLRpredTab = new RootItem(kIPyPrediction, _panelName);
            _MLRpredTab.SortOrder = 110;
            App.HeaderControl.Add(_MLRpredTab);

            //subribbon
            const string rGroupCaption = "Import";

            btnImportIVs = new SimpleActionItem(kIPyPrediction, "Import IVs", _frmIPyPred.btnImportIVs_Click);
            btnImportIVs.LargeImage = Properties.Resources.ImportIVs;
            btnImportIVs.GroupCaption = rGroupCaption;
            btnImportIVs.Enabled = true;
            App.HeaderControl.Add(btnImportIVs);

            btnImportOBs = new SimpleActionItem(kIPyPrediction, "Import OBs", _frmIPyPred.btnImportObs_Click);
            btnImportOBs.LargeImage = Properties.Resources.ImportOBs;
            btnImportOBs.GroupCaption = rGroupCaption;
            btnImportOBs.Enabled = true;
            App.HeaderControl.Add(btnImportOBs);

            const string sGroupCaption = "Manipulate";

            btnPlot = new SimpleActionItem(kIPyPrediction, "Plot", _frmIPyPred.btnPlot_Click);
            btnPlot.LargeImage = Properties.Resources.Plot;
            btnPlot.GroupCaption = sGroupCaption;
            btnPlot.Enabled = true;
            App.HeaderControl.Add(btnPlot);

            btnClear = new SimpleActionItem(kIPyPrediction, "Clear", _frmIPyPred.btnClearTable_Click);
            btnClear.LargeImage = Properties.Resources.Clear;
            btnClear.GroupCaption = sGroupCaption;
            btnClear.Enabled = true;
            App.HeaderControl.Add(btnClear);

            btnExportAsCSV = new SimpleActionItem(kIPyPrediction, "Export as CSV", _frmIPyPred.btnExportTable_Click);
            btnExportAsCSV.LargeImage = Properties.Resources.ExportAsCSV;
            btnExportAsCSV.GroupCaption = sGroupCaption;
            btnExportAsCSV.Enabled = true;
            App.HeaderControl.Add(btnExportAsCSV);
            
            const string tGroupCaption = "Validate";

            btnIVDataValidation = new SimpleActionItem(kIPyPrediction, "IV Data Validation", _frmIPyPred.btnIVDataValidation_Click);
            btnIVDataValidation.LargeImage = Properties.Resources.IVDataVal;
            btnIVDataValidation.GroupCaption = tGroupCaption;
            btnIVDataValidation.Enabled = true;
            App.HeaderControl.Add(btnIVDataValidation);

            btnMakePrediction = new SimpleActionItem(kIPyPrediction, "Make Predictions", _frmIPyPred.btnMakePredictions_Click);
            btnMakePrediction.LargeImage = Properties.Resources.MakePred;
            btnMakePrediction.GroupCaption = tGroupCaption;
            btnMakePrediction.Enabled = true;
            App.HeaderControl.Add(btnMakePrediction);

        }

        void AddMLRPanel()
        {
            var dp = new DockablePanel(kIPyPrediction, _panelName, _frmIPyPred, DockStyle.Fill);
            dp.DefaultSortOrder = 80;
            App.DockManager.Add(dp);
        }



        void SerializationManager_Serializing(object sender, SerializingEventArgs e)
        {

            //throw new NotImplementedException();
        }
        void SerializationManager_Deserializing(object sender, SerializingEventArgs e)
        {
            //throw new NotImplementedException();
        }

        void DockManager_ActivePanelChanged(object sender, DotSpatial.Controls.Docking.DockablePanelEventArgs e)
        {
            if (e.ActivePanelKey == kIPyPrediction)
            {
                App.DockManager.SelectPanel("IpyPredSeriesView");
                App.HeaderControl.SelectRoot(kIPyPrediction);
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

    }
}
