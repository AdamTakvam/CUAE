using System;  
using Metreos.Core.IPC.Flatmaps;


namespace Metreos.Core.IPC.FlatmapTest
{
	class FlatmapTest
	{
        /// <summary>
        /// Simple test to load a FlatmapList, convert the list to a binary flatmap,
        /// convert the binary back to a FlatmapList, and verify they are the same
        /// </summary>
		[STAThread]
		static void Main(string[] args)
		{
            // Create a flatmap to embed in outer flatmap
            FlatmapList embeddedFlatmapList = new FlatmapList();
            embeddedFlatmapList.Add(64,"it works!");
            embeddedFlatmapList.Add(16, 16);
            byte[] embeddedFlatmap = embeddedFlatmapList.ToFlatmap();            

            // Create outer flatmap

            FlatmapList list = new FlatmapList();
            byte[] databytes = new byte[] { 60,61,62,63 };
            list.Add(5, databytes);
            list.Add(4, embeddedFlatmap);
            list.Add(3,"three"); 
            list.Add(1,1); 
            list.Add(2,2); 
            list.Add(3,"three (duplicate key)");
            list.Add(8, (double)3.14159);
            list.Add(7, (long)1000000000);
            list.Sort();

            Console.WriteLine("-- original list sorted --");
             
            foreach(Flatmap.MapEntry entry in list)
            {
                bool isEmbeddedMap = entry.dataType == Flatmap.ValueType.Flatmap;
                string val = isEmbeddedMap? "embedded flatmap": entry.dataValue.ToString();

                Console.WriteLine(entry.key + ": " + val);
            }


            Flatmap.MapEntry x = (Flatmap.MapEntry)list[3]; 
            Console.WriteLine("\nindexing test: list[3] is " + x.dataValue.ToString());

            // Convert list to binary flatmap
            byte[] flatmap = list.ToFlatmap(); 

            Console.WriteLine("\n" + list.BinaryFlatmapLength(0) + ": calculated length of binary map");
            Console.WriteLine(flatmap.Length + ": actual length of binary map");

            // Convert binary flatmap back to list
            list = new FlatmapList(flatmap);

            Console.WriteLine("\n-- same list reconstituted from binary --");

            foreach(Flatmap.MapEntry entry in list)
            {                   
                bool isEmbeddedMap = entry.dataType == Flatmap.ValueType.Flatmap;
                string val = isEmbeddedMap? "embedded flatmap": entry.dataValue.ToString();

                Console.WriteLine(entry.key + ": " + val);

                if (isEmbeddedMap)
                {
                    embeddedFlatmapList = new FlatmapList((byte[])entry.dataValue);

                    Console.WriteLine("    -- embedded map --");

                    foreach(Flatmap.MapEntry ntry in embeddedFlatmapList)                     
                            Console.WriteLine("    " + ntry.key + ": " + ntry.dataValue.ToString());
                }
            }

            Console.WriteLine("\n");
		}
	} // class FlatmapTest
}   
