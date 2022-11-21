using System.Globalization;

namespace System;

public static class DateTimeExtensions
{
    public static DateTime GetBRT(this DateTime dateUTC) => dateUTC.AddHours(-3);

    /// <summary>
    /// Converte data para BRT e exibe no formato pt-bt
    /// </summary>
    public static string ToStringBRT(this DateTime dateUTC) => dateUTC.GetBRT().ToString("g", new CultureInfo("pt-br"));
}