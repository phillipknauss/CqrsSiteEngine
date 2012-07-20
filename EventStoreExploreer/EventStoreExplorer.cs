using System;
using System.Windows.Forms;
using System.Configuration;
using System.Reflection;

namespace EventStoreExploreer
{
    public partial class EventStoreExplorer : Form
    {
        private string fileStore = null;

        private Eventing.IExplorableEventStore store;

        public EventStoreExplorer()
        {
            InitializeComponent();

            Config.Config.LoadFromXml(Config.Config.DefaultPath);

            fileStore = Config.Config.Get("EventStoreExploreer.FileEventStorePath");
            store = new Eventing.FileSystemEventStore(fileStore);
        }

        private void selectEventStoreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var result = OpenEventStoreDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                fileStore = OpenEventStoreDialog.SelectedPath;
                store = new Eventing.FileSystemEventStore(fileStore);
                //ConfigurationManager.AppSettings["EventStorePath"] = fileStore;
            }
        }

        private void PopulateGrid()
        {
            treeView.Nodes.Clear();
            if (string.IsNullOrEmpty(fileStore))
            {
                MessageBox.Show("Choose an event store first.");
                return;
            }

            var eventSources = store.GetEventSourceIndex();

            foreach (var source in eventSources)
            {
                AddSourceNodeToRoot(source, treeView.Nodes);
            }
        }

        private void AddSourceNodeToRoot(Guid source, TreeNodeCollection coll)
        {
            var node = new TreeNode
                           {
                               Name = source.ToString(),
                               Text = source.ToString()
                           };

            node.ContextMenu = new ContextMenu(
                new[]
                    {
                        new MenuItem("Delete",
                                     (sender, args) =>
                                         {
                                             var senderNode = ((sender as MenuItem).Parent.Tag as TreeNode);
                                             if (senderNode.Nodes.Count > 0)
                                             {
                                                 MessageBox.Show("Nodes containing events cannot be deleted.");
                                                 return;
                                             }
                                             store.RemoveEmptyEventSource(Guid.Parse(node.Name));
                                             senderNode.Remove();
                                         }
                            )
                    }
                ) { Tag = node };


            coll.Add(node);

            var events = store.GetAllEvents(source);
            foreach (var evt in events)
            {
                AddEvtNodeToSourceNode(evt, node);
            }
        }

        private void AddEvtNodeToSourceNode(Ncqrs.Eventing.Sourcing.ISourcedEvent evt, TreeNode sourceNode)
        {
            var evtNode = new TreeNode
                              {
                                  Name = evt.EventIdentifier.ToString(),
                                  Text = evt.GetType().ToString(),
                                  Tag = evt
                              };

            sourceNode.Nodes.Add(evtNode);

            foreach (var prop in evt.GetType().GetProperties())
            {
                AddPropNodeToEvtNode(prop, evt, evtNode);
            }
        }

        private void AddPropNodeToEvtNode(PropertyInfo prop, Ncqrs.Eventing.Sourcing.ISourcedEvent evt, TreeNode evtNode)
        {
            var propNode = new TreeNode
                               {
                                   Name = prop.Name,
                                   Text = prop.Name + ": " + prop.GetValue(evt, null)
                               };

            evtNode.Nodes.Add(propNode);
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PopulateGrid();
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var source = Guid.NewGuid();
            store.StoreEmptyEventSource(source);

            AddSourceNodeToRoot(source, treeView.Nodes);

        }

        private void EventStoreExplorer_Load(object sender, EventArgs e)
        {
            fileStore = Config.Config.Get("EventStoreExploreer.FileEventStorePath");
            store = new Eventing.FileSystemEventStore(fileStore);
            PopulateGrid();
        }

        private void selectAzureDevToolStripMenuItem_Click(object sender, EventArgs e)
        {
            store = new Eventing.AzureSystemEventStore();
            PopulateGrid();
        }
    }
}
