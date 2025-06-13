#pragma once

#include <vcclr.h>

namespace Metreos
{
namespace SCCP
{
	public __gc class InitDll
	{
	public:
		static void Initialize();
		static void Terminate();
	};
}
}