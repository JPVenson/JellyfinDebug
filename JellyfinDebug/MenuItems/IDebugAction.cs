using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JellyfinDebug.MenuItems;

public interface IDebugAction
{
	string Name { get; }
	ValueTask Execute();
}