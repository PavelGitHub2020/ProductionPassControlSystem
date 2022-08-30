using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionPassControlSystem.BLL
{
    public interface IChoosingDayThirdShift
    {
        void ChoosingTheDayThird(List<int> number);
        void ChoosingTheNightThird(List<int> number);
        void ChoosingTheDayOffThird(List<int> number);
        void ChoosingTheEndDayOffThird(List<int> number);

        void ClearTheDaysOfThird();
    }
}
