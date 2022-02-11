using System;
using NOxAcquisition;
using CsvHelper;
using CsvHelper.Configuration;
using System.IO;

public class CsvProvider : StorageProviderBase
{
    #region Private

    private CsvWriter _writer;

    #endregion

    public static string Delimeter { get; set; }

    public CsvProvider(string folder, RegisterSet regs) : base(regs)
    {
        string p = Path.Combine(folder, DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".csv");
        var cc = new CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture);
        if (Delimeter != null) cc.Delimiter = Delimeter;
        _writer = new CsvWriter(new StreamWriter(p), cc);
        _writer.WriteField("Time");
        foreach (var item in regs.GetAll())
        {
            _writer.WriteField(item.Name);
        }
        _writer.NextRecord();
    }

    public override void Store()
    {
        _writer.WriteField(DateTime.Now.ToLongTimeString());
        foreach (var item in _regs.GetAll())
        {
            _writer.WriteField(item.GetValue());
        }
        _writer.NextRecord();
        _writer.FlushAsync();
    }

    public override void Dispose()
    {
        _writer.Flush();
        _writer.Dispose();
    }
}