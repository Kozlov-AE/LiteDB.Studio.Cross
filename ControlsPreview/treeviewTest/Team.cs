using System.Collections.ObjectModel;

namespace ControlsPreview.treeviewTest {
    public class Team {
        public string TeamName { get; set; } = "New Teams";
        //public ObservableCollection<Person> Roster { get; } = new ObservableCollection<Person>();
        public ObservableCollection<Coach> Coaches { get; } = new ObservableCollection<Coach>();
        public ObservableCollection<Player> Players { get; } = new ObservableCollection<Player>();
    }
}