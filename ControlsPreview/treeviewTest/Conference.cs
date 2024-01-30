using System.Collections.ObjectModel;

namespace ControlsPreview.treeviewTest {
    public class Conference {
        public string ConferenceName { get; set; }

        public ObservableCollection<Team> Teams { get; }
            = new ObservableCollection<Team>();
    }
}