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
using System.ComponentModel;
using Dynamo.Views;

namespace DynaFire
{
    public class DynaFireExtension : IViewExtension, INotifyPropertyChanged
    {
        private string _searchString;
        public string SearchString
        {
            get
            {
                return _searchString;
            }
            set
            {
                _searchString = value;
                RaisePropertyChangedHere("NodeShortcuts");
            }
        }

        private string ShortcutsFileName = "shortcuts.txt";

        private ObservableCollection<Shortcut> _nodeShortcuts = new ObservableCollection<Shortcut>();
        public ObservableCollection<Shortcut> NodeShortcuts
        {
            get
            {
                if (string.IsNullOrWhiteSpace(SearchString))
                {
                    return _nodeShortcuts;
                }
                return new ObservableCollection<Shortcut>(_nodeShortcuts.Where(n => n.NodeName.ToUpper().Contains(SearchString.ToUpper())));
            }
            set
            {
                _nodeShortcuts = value;
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

        public event PropertyChangedEventHandler PropertyChanged;

        public void SaveAllShortcuts()
        {
            List<Shortcut> assignedShortcuts = NodeShortcuts.Where(s => String.IsNullOrEmpty(s.Keys)).ToList();

        }

        public void Loaded(ViewLoadedParams p)
        {
            view = p.DynamoWindow as DynamoView;
            view.KeyDown += View_KeyDown;
            view.KeyUp += View_KeyUp;

            SetNodeShortcuts();
            ReadFile();
        }

        private void RaisePropertyChangedHere(string propertyName)
        {
            Console.WriteLine("property change event called");
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                Console.WriteLine("property change event triggered");
            }
        }

        private void SetNodeShortcuts()
        {
            DynamoViewModel vm = view.DataContext as DynamoViewModel;
            _nodeShortcuts = new ObservableCollection<Shortcut>();
            foreach (string name in vm.SearchViewModel.Model.SearchEntries.Select(s => s.CreationName))
            {
                _nodeShortcuts.Add(new Shortcut("", name));
            }
        }

        internal void ClearKeys()
        {
            foreach (Shortcut sc in NodeShortcuts)
            {
                sc.Keys = "";
            }
        }

        internal void ReadFile()
        {
            try
            {
                using (System.IO.StreamReader stream = new System.IO.StreamReader(ShortcutsFileName))
                {
                    while (!stream.EndOfStream)
                    {
                        string[] shortcutParts = stream.ReadLine().Split(new char[1] { '|' });
                        string nodeName = shortcutParts[1];
                        Shortcut target = NodeShortcuts.FirstOrDefault(s => s.NodeName.Equals(nodeName));
                        if (target != null)
                        {
                            target.Keys = shortcutParts[0];
                        }
                    }
                }

            }
            catch (Exception e)
            {
                // haha
            }
        }

        private void View_KeyUp(object sender, KeyEventArgs e)
        {
            lastChar = null;

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

                    if (key == "KS")
                    {
                        NodeNamesView v = new NodeNamesView(this);
                        v.ShowDialog();
                    }

                    TryAndPlaceNode(key);
                }
            }
        }

        private void TryAndPlaceNode(string key)
        {
            string nodeName = GetNodeNameFromKeysEntered(NodeShortcuts, key);
            if (nodeName != null)
            {
                DynamoViewModel vm = view.DataContext as DynamoViewModel;
                System.Windows.Point pnt;

                try
                {
                    WorkspaceView wsV = view.ChildOfType<WorkspaceView>();
                    pnt = Mouse.GetPosition(wsV);
                }
                catch (Exception)
                {
                    pnt = new System.Windows.Point(0,0);
                }
                vm.Model.ExecuteCommand(new CreateNodeCommand(Guid.NewGuid().ToString(), nodeName, pnt.X, pnt.Y, false, true));
            }
        }

        private string GetNodeNameFromKeysEntered(ObservableCollection<Shortcut> collection, string key)
        {
            Shortcut target = collection.FirstOrDefault(s => s.Keys.Equals(key));
            return target != null ? target.NodeName : null;
        }

        internal void WriteShortcutsToFile()
        {
            // First clear all the existing contents so we don't write duplicates
            System.IO.File.WriteAllText(ShortcutsFileName, string.Empty);

            // Then write all shortcuts with non-empty keys to the File
            using (System.IO.StreamWriter shortcutsFile = new System.IO.StreamWriter(ShortcutsFileName))
            {
                foreach (Shortcut shortcut in NodeShortcuts)
                {
                    if (!shortcut.Keys.Equals(""))
                    {
                        string shcut = shortcut.Keys.ToUpper() + "|" + shortcut.NodeName + "|";
                        shortcutsFile.WriteLine(shcut);
                    }
                }
            }
        }

        public void Shutdown()
        {

        }

        public void Startup(ViewStartupParams p)
        {

        }

    }
}
