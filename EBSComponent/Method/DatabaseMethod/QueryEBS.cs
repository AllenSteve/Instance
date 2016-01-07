﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComponentORM.SQLHelper;

namespace EbsComponent.Method.DatabaseMethod
{
    public class QueryEBS
    {
        private string connection { get; set; }

        public QueryEBS()
        { 

        }

        public Repository<T> Query<T>() where T : class
        {
            var repository = new Repository<T>((int)ComponentORM.ORMappingTools.DBHelper.Sqldb.SandBox);
            return repository;
        }
    }
}