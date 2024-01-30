using LiteDB.Studio.Cross.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ControlsPreview.treeviewTest {
    public class TVTestVM {
        public ObservableCollection<Conference> League { get; }
        public ObservableCollection<DatabaseViewModel> DataBases { get;  }
        public ObservableCollection<Team> Teams { get;  }

        public TVTestVM() {
            League = new ObservableCollection<Conference>(FillLeague());
            Teams = new ObservableCollection<Team>(FillTeam());
        }

        private List<Team> FillTeam() {
            return new() {
                new Team(){
                    TeamName = "Eastern Teams B",  
                    Players =  {
                        new Player () { Name = "Player 1", Positions = new (){"Лево"} },
                        new Player () { Name = "Player 2", Positions = new (){"Право"} },
                        new Player () { Name = "Player 3", Positions = new (){"Центр"} },
                    }, 
                    Coaches = {
                        new Coach () {Name = "Coach 1", Title = "Title 1" },
                        new Coach () {Name = "Coach 2", Title = "Title 2" },
                    }
                }
            };
        }


        private List<Conference> FillLeague() {
            return new List<Conference>()
            {
                new Conference()
                {
                    ConferenceName = "Eastern",
                    Teams =
                    {
                        new Team()
                        {
                            TeamName = "Eastern Teams A"
                        },
                        new Team()
                        {
                            TeamName = "Eastern Teams B",  
                            Players =  {
                                new Player () { Name = "Player 1", Positions = new (){"Лево"} },
                                new Player () { Name = "Player 2", Positions = new (){"Право"} },
                                new Player () { Name = "Player 3", Positions = new (){"Центр"} },
                            }, 
                            Coaches = {
                                new Coach () {Name = "Coach 1", Title = "Title 1" },
                                new Coach () {Name = "Coach 2", Title = "Title 2" },
                            }
                        },
                        new Team()
                        {
                            TeamName = "Eastern Teams C"
                        }
                    }
                },
                new Conference()
                {
                    ConferenceName = "Western",
                    Teams =
                    {
                        new Team()
                        {
                            TeamName = "Western Teams A"
                        },
                        new Team()
                        {
                            TeamName = "Western Teams B"
                        },
                        new Team()
                        {
                            TeamName = "Western Teams C"
                        }
                    }
                }
            };
        }
    }
}