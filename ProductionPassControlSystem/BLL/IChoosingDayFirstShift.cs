using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionPassControlSystem.BLL
{
    public interface IChoosingDayFirstShift
    {
        void ChoosingTheDayFirst(List<int> number);
        void ChoosingTheNightFirst(List<int> number);
        void ChoosingTheDayOffFirst(List<int> number);
        void ChoosingTheEndDayOffFirst(List<int> number);

        void ClearTheDaysOfFirst();
    }
}
