﻿namespace Byte.Core.SqlSugar.ConfigOptions;

public class SqlLog
{
    public bool Enabled { get; set; }
    public ToDb ToDb { get; set; }
    public ToFile ToFile { get; set; }
    public ToConsole ToConsole { get; set; }
    public ToElasticsearch ToElasticsearch { get; set; }
}

public class ToDb
{
    public bool Enabled { get; set; }
}

public class ToFile
{
    public bool Enabled { get; set; }
}

public class ToConsole
{
    public bool Enabled { get; set; }
}

public class ToElasticsearch
{
    public bool Enabled { get; set; }
}
