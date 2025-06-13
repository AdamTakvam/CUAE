using System;


namespace Metreos.Max.Framework.Satellite
{
    public enum SatelliteTypes 
    {
        None, Explorer, Properties, Toolbox, Overview, Output,
        Breakpoints, Watch, CallStack, RemoteConsole 
    }


    /// <summary>Interface implemented by all docking satellite windows</summary>
    public interface MaxSatelliteWindow
    {
        SatelliteTypes SatelliteType { get; }
        Crownwood.Magic.Menus.MenuCommand ViewMenuItem { get; }
    }

}   // namespace
