using System;
using System.Collections;

namespace DerivationExplode
{
	/// <summary>
	///     Takes an XSD file, and removes derivations
	/// </summary>
	class Entry
	{
		/// <summary> </summary>
		[STAThread]
		static void Main(string[] args)
		{
			DerivationExploder exploder = new DerivationExploder();
        
			ArrayList specificNamesList = new ArrayList();

			if(args.Length > 2)
			{
				for(int i = 2; i < args.Length; i++)
				{
					specificNamesList.Add(args[i]);
				}
			}

			string[] specificNames = null;

			if(specificNamesList.Count > 0)
			{
				specificNames = new string[specificNamesList.Count];
				specificNamesList.CopyTo(specificNames);
			}
            exploder.ExplodeAndSave(args[0], args[1], specificNames);
        }
	}
}
