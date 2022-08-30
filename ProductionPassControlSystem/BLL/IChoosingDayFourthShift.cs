using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionPassControlSystem.BLL
{
    public interface IChoosingDayFourthShift
    {
        void ChoosingTheDayFourth(List<int> number);
        void ChoosingTheNightFourth(List<int> number);
        void ChoosingTheDayOffFourth(List<int> number);
        void ChoosingTheEndDayOffFourth(List<int> number);

        void ClearTheDaysOfFourth();
    }
}
