﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace StoringImages.Model
{
    class dBFunctions
    {
        public static string ConnectionStringSQLite
        {

            get
            {
                string database =
                AppDomain.CurrentDomain.BaseDirectory +
               "\\Database\\ImageLib.s3db";
                string connectionString =
                @"Data Source=" + Path.GetFullPath(database);
                return connectionString;
            }
        }
    }
}
