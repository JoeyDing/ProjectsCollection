﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation.UI.Shell.Wpf.Infrastructure.Core.Contracts
{
    public interface IDispatcher
    {
        void Invoke(Action action);

        void BeginInvoke(Action action);
    }
}