using System;
using System.Collections.ObjectModel;
using Dynamo.Wpf.Extensions;
using System.Linq;
using Dynamo.ViewModels;
using Dynamo.Controls;
using static Dynamo.Models.DynamoModel;

using System.Collections.Generic;


using System.Windows.Input;
using Dynamo.UI.Commands;

namespace DynaFire
{
    public class DynaFireExtension : IViewExtension
    {

        Dictionary<string, string> Map = new Dictionary<string, string>();
        private ObservableCollection<Shortcut> _nodeNames = new ObservableCollection<Shortcut>();
        public ObservableCollection<Shortcut> NodeShortcuts
        {
            get
            {
                return _nodeNames;
            }
            set
            {
                _nodeNames = value;
                
            }
        }
        
        public string Name
        {
            get
            {
                return "DynaFire"; 
            }
        }

        public string UniqueId
        {
            get
            {
                return "83482364-4536-41DD-9367-358329D2E31C";
            }
        }

        public void Dispose()
        {

        }

        DynamoView view;
        char? lastChar;

        public void SaveAllShortcuts()
        {
            List<Shortcut> assignedShortcuts = NodeShortcuts.Where(s => s.Keys != string.IsNullOrEmpty);

        }

        public void Loaded(ViewLoadedParams p)
        {
            view = p.DynamoWindow as DynamoView;
            view.KeyDown += View_KeyDown;
            view.KeyUp += View_KeyUp;

            SetNodeShortcuts();

            NodeNamesView v = new NodeNamesView(this);
            v.ShowDialog();
        }

        private void SetNodeShortcuts()
        {
            DynamoViewModel vm = view.DataContext as DynamoViewModel;
            NodeShortcuts = new ObservableCollection<Shortcut>();
            foreach( string name in  vm.SearchViewModel.Model.SearchEntries.Select(s => s.CreationName) )
            {
                NodeShortcuts.Add(new Shortcut("", name));
            }
        }

        private void View_KeyUp(object sender, KeyEventArgs e)
        {
            lastChar = null;

            System.Windows.Forms.MessageBox.Show(System.Windows.Forms.Cursor.Position.ToString());
        }

        private void View_KeyDown(object sender, KeyEventArgs e)
        {
            if (lastChar == null)
            {
                char[] chars = e.Key.ToString().ToCharArray();
                if (chars.Length == 1)
                {
                    lastChar = chars[0];
                }
            }
            else
            {
                char[] chars = e.Key.ToString().ToCharArray();
                if (chars.Length == 1)
                {
                    string key = lastChar.ToString() + chars[0].ToString();
                    string val;
                    if (Map.TryGetValue(key, out val))
                    {
                        DynamoViewModel vm = view.DataContext as DynamoViewModel;
                        vm.Model.ExecuteCommand(new CreateNodeCommand(Guid.NewGuid().ToString(), val, 0, 0, false, true));
                    }
                }
            }
        }

        public void Shutdown()
        {

        }

        public void Startup(ViewStartupParams p)
        {
            Map.Add("CR", "Color Range");
            Map.Add("IN", "Input");
            Map.Add("OT", "SortIndexByValue@double[],bool");
        }

        public DelegateCommand TryOKCommand = new DelegateCommand(TryOK);

        private static void TryOK(object obj)
        {
            System.Windows.Forms.MessageBox.Show("ok clicked");
        }
    }
}
