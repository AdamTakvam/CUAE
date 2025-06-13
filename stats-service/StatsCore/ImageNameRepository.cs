using System;
using System.Collections.Specialized;

using Metreos.Utilities.Collections;

namespace Metreos.Stats
{
    /// <summary>
    /// This class distributes names for image files in a round-robin fashion 
    ///   ensuring that only a limited number of files are created.
    /// </summary>
    public class ImageNameRepository
    {
        private abstract class Consts
        {
            public const int NumImages      = 20;
            public const string DefaultName = "default.png";
        }

        private readonly TallyCollection names;

        public ImageNameRepository() 
            : this(Consts.NumImages) {}

        public ImageNameRepository(int numImages)
        {
            this.names = new TallyCollection();

            for(int i=0; i<numImages; i++)
            {
                this.names.AddItem(String.Format("image{0}.png", i.ToString()));
            }
        }

        public string GetName()
        {
            lock(names)
            {
                if(names.AllChecked)
                    names.UncheckAll();

                StringCollection ucNames = names.GetUncheckedNames();
                if(ucNames != null && ucNames.Count > 0)
                {
                    string name = ucNames[0];
                    names.Check(name);
                    return name;
                }
            }
            return Consts.DefaultName;   // should never happen
        }
    }
}
