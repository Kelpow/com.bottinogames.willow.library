using System;

namespace Willow.Library
{
    public static class ForbiddenKnowledge
    {
        static DateTime January(this int year, int day)
        {
            return new DateTime(year, 1, day);
        }
        static DateTime February(this int year, int day)
        {
            return new DateTime(year, 2, day);
        }
        static DateTime March(this int year, int day)
        {
            return new DateTime(year, 3, day);
        }
        static DateTime April(this int year, int day)
        {
            return new DateTime(year, 4, day);
        }
        static DateTime May(this int year, int day)
        {
            return new DateTime(year, 5, day);
        }
        static DateTime June(this int year, int day)
        {
            return new DateTime(year, 6, day);
        }
        static DateTime July(this int year, int day)
        {
            return new DateTime(year, 7, day);
        }
        static DateTime August(this int year, int day)
        {
            return new DateTime(year, 8, day);
        }
        static DateTime September(this int year, int day)
        {
            return new DateTime(year, 9, day);
        }
        static DateTime October(this int year, int day)
        {
            return new DateTime(year, 10, day);
        }
        static DateTime November(this int year, int day)
        {
            return new DateTime(year, 11, day);
        }
        static DateTime December(this int year, int day)
        {
            return new DateTime(year, 12, day);
        }
    }
}