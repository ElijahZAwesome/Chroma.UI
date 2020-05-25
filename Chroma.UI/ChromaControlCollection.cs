using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;

namespace Chroma.UI
{
    public class ChromaControlCollection : ObservableCollection<ChromaControl>
    {
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnCollectionChanged(e);

            if (e.OldStartingIndex >= 0)
            {
                this[e.OldStartingIndex].Parent = null;
            }
        }
    }
}
