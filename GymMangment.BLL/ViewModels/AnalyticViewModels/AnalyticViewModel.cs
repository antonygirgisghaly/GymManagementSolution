using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangment.BLL.ViewModels.AnalyticViewModels
{
    public class AnalyticViewModel
    {
        public int TotlaMembers { get; set; }
        public int ActiveMembers { get; set; }
        public int Trainers { get; set; }
        public int UpcomingSessions { get; set; }
        public int OngoingSessions { get; set; }
        public int CompletedSessions { get; set; }
    }
}
