﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionPassControlSystem.BLL
{
    public interface IChoosingTheNumberOfDay
    {
        void ChoosingTheNumberOfDay(List<int> number);
        void ClearNumberOfDay();
    }
}