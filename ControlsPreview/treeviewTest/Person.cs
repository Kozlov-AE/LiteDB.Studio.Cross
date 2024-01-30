using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlsPreview.treeviewTest {
    public class Person {
        public string Name { get; set; } = "New Player";
    }

    public class Player : Person {
        public ObservableCollection<string> Positions { get; set; } = new ObservableCollection<string>();
    }

    public class Coach : Person {
        public string Title { get; set; }

        public string ProperName => $"{Title}: {Name}";
    }
}
