using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for HelperCS
/// </summary>
public static class HelperCS
{
     static HelperCS()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static DateTime GetCurrentTime()
    {
        DateTime serverTime = DateTime.Now;
        DateTime _localTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(serverTime, TimeZoneInfo.Local.Id, "India Standard Time");
        //TimeZone localZone = TimeZone.CurrentTimeZone;

        //TimeZone zone = TimeZone.CurrentTimeZone;
        //// Demonstrate ToLocalTime and ToUniversalTime.
        //DateTime local = zone.ToLocalTime(DateTime.Now);
        //DateTime universal = zone.ToUniversalTime(DateTime.Now);
        return _localTime;
    }
}