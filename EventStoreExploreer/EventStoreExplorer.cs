using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.Reflection;

namespace EventStoreExploreer
{
    public partial class EventStoreExplorer : Form
    {
        private string fileStore = null;

        Eventing.FileSystemEventStore eventStore;

        public EventStoreExplorer()
        {
            InitializeComponent();

            fileStore = ConfigurationManager.AppSettings["EventStorePath"];
            eventStore = new Eventing.FileSystemEventStore(fileStore);
        }

        private void selectEventStoreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var result = OpenEventStoreDialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                fileStore = OpenEventStoreDialog.SelectedPath;
                eventStore = new Eventing.FileSystemEventStore(fileStore);
                ConfigurationManager.AppSettings["EventStorePath"] = fileStore;
            }
        }

        private void PopulateGrid()
        {
            treeView.Nodes.Clear();
            if (fileStore == null || fileStore.Length == 0)
            {
                MessageBox.Show("Choose an event store first.");
                return;
            }

            var eventSources = eventStore.GetEventSourceIndex();

            foreach (var source in eventSources)
            {
                AddSourceNodeToRoot(source, treeView.Nodes);
            }
        }

        private void AddSourceNodeToRoot(Guid source, TreeNodeCollection coll)
        {
            var node = new TreeNode()
            {
                Name = source.ToString(),
                Text = source.ToString()
            };

            node.ContextMenu = new System.Windows.Forms.ContextMenu(
                new MenuItem[]
                {
                    new MenuItem("Delete", new EventHandler( (sender, args) =>
                        {
                            var senderNode = ((sender as MenuItem).Parent.Tag as TreeNode);
                            if (senderNode.Nodes.Count > 0)
                            {
                                MessageBox.Show("Nodes containing events cannot be deleted.");
                                return;
                            }
                            eventStore.RemoveEmptyEventSource(Guid.Parse(node.Name));
                            senderNode.Remove();
                        })
                    )
                }
            );

            node.ContextMenu.Tag = node;

            coll.Add(node);

            var events = eventStore.GetAllEvents(source);
            foreach (var evt in events)
            {
                AddEvtNodeToSourceNode(evt, node);
            }
        }

        private void AddEvtNodeToSourceNode(Ncqrs.Eventing.Sourcing.ISourcedEvent evt, TreeNode sourceNode)
        {
            var evtNode = new TreeNode()
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
            var propNode = new TreeNode()
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
            eventStore.StoreEmptyEventSource(source);

            AddSourceNodeToRoot(source, treeView.Nodes);

        }

        private void EventStoreExplorer_Load(object sender, EventArgs e)
        {
            fileStore = ConfigurationManager.AppSettings["EventStorePath"];
            eventStore = new Eventing.FileSystemEventStore(fileStore);
            PopulateGrid();
        }


    }
}
