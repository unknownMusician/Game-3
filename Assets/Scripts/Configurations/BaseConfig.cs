﻿using System;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class BaseConfig<TData> where TData : class
{
    protected string _path;
    protected TData Data;

    public BaseConfig(string name)
    {
        _path = Path.Combine(Application.streamingAssetsPath, name);
        Init();
    }

    protected void Init()
    {
        try
        {
            XmlSerializer formatter = new XmlSerializer(typeof(TData));

            using (FileStream fs = new FileStream(_path, FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, Data);
            }

        }
        catch (Exception ex)
        {
            Logger.LogW(ex, $"Error with getting data in {typeof(TData)} config");
        }

    }
}
