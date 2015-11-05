﻿using ComponentModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentParser
{
    public class LogParser
    {
        public string logPath { get; set; }

        public List<LogModel> logModelList { get; set; }

        public int columnAmount { get; set; }

        public LogParser()
        {
            logPath = string.Empty;
            logModelList = new List<LogModel>();
            columnAmount = ParseColumnAmount();
        }

        public bool LoadLogFile(string path)
        {
            return string.IsNullOrEmpty(logPath = path);
        } 

        public List<LogModel> ParseLogToList()
        {
            string line = string.Empty;
            int count = 0;
            try
            {
                FileStream fStream = new FileStream(this.logPath, FileMode.Open, FileAccess.Read);
                using (StreamReader streamReader = new StreamReader(fStream))
                {
                    Stopwatch stopWatch = new Stopwatch();
                    stopWatch.Start();
                    while ((line = streamReader.ReadLine())!=null)
                    {
                        if(!string.IsNullOrEmpty(line))
                        {
                            Dictionary<string, string> paramList = this.ParseParamList(line);
                            if (paramList != null)
                            {
                                LogModel log = this.ParseParamListToLogModel(paramList);
                                this.logModelList.Add(log);
                            }
                        }
                        count++;
                    }

                    stopWatch.Stop();
                    TimeSpan ts = stopWatch.Elapsed;
                    string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}:{3:000}",
                                                                            ts.Hours, ts.Minutes, ts.Seconds,
                                                                            ts.Milliseconds);
                    Console.WriteLine("RunTime " + elapsedTime);
                    Console.WriteLine("Runlines: " + count);
                }
            }
           catch(Exception ex)
           {
               Console.WriteLine("Runlines: " + count);
               Console.WriteLine(line);
               Console.WriteLine(ex);
           }
            return this.logModelList;
        }

        public bool BulkInsert( List<LogModel> logList)
        {
            try
            { 
                string conn = @"Data Source=.\localdb;Initial Catalog=workDB;UID=sa;PWD=123456;";
                SqlConnection sqlcon = new SqlConnection(conn);
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                DataTable table = this.CreateDataTable();
                foreach (var log in logList)
                {
                    this.AddModelToDataTable(table, log);
                }
                sqlcon.Open();

                using (SqlBulkCopy bulk = new SqlBulkCopy(conn))
                {
                    bulk.BatchSize = this.logModelList.Count;
                    bulk.DestinationTableName = "CallInterfaceLog";
                    bulk.ColumnMappings.Add("ID", "ID");
                    bulk.ColumnMappings.Add("SoufunId", "SoufunId");
                    bulk.ColumnMappings.Add("OrderId", "OrderId");
                    bulk.ColumnMappings.Add("InterfaceName", "InterfaceName");
                    bulk.ColumnMappings.Add("ResponseStartTime", "ResponseStartTime");
                    bulk.ColumnMappings.Add("ResponseEndTime", "ResponseEndTime");
                    bulk.ColumnMappings.Add("Imei", "Imei");
                    bulk.ColumnMappings.Add("Appv", "Appv");
                    bulk.ColumnMappings.Add("AppType", "AppType");
                    bulk.ColumnMappings.Add("CreateTime", "CreateTime");
                    bulk.ColumnMappings.Add("InterfaceUrl", "InterfaceUrl");
                    bulk.ColumnMappings.Add("ParamsStr", "ParamsStr");
                    bulk.ColumnMappings.Add("IsDel", "IsDel");
                    bulk.ColumnMappings.Add("Status", "Status");
                    bulk.WriteToServer(table);
                }

                stopWatch.Stop();
                TimeSpan ts = stopWatch.Elapsed;
                string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}:{3:000}",
                                                                        ts.Hours, 
                                                                        ts.Minutes, 
                                                                        ts.Seconds,
                                                                        ts.Milliseconds);
                Console.WriteLine("RunTime " + elapsedTime);
                //
                table.Dispose();
                sqlcon.Close();
                sqlcon.Dispose();
           }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return true;
        }
        public bool WriteToInterfaceLog(LogModel log)
        {
            try
            {
                string conn = @"Data Source=.\localdb;Initial Catalog=workDB;UID=sa;PWD=123456;";
                SqlConnection sqlcon = new SqlConnection(conn);
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();

                DataTable table = this.CreateDataTable();
                this.AddModelToDataTable(table, log);
                sqlcon.Open();

                using (SqlBulkCopy bulk = new SqlBulkCopy(conn))
                {
                    bulk.BatchSize = this.logModelList.Count;
                    bulk.DestinationTableName = "CallInterfaceLog";
                    bulk.ColumnMappings.Add("ID", "ID");
                    bulk.ColumnMappings.Add("SoufunId", "SoufunId");
                    bulk.ColumnMappings.Add("OrderId", "OrderId");
                    bulk.ColumnMappings.Add("InterfaceName", "InterfaceName");
                    bulk.ColumnMappings.Add("ResponseStartTime", "ResponseStartTime");
                    bulk.ColumnMappings.Add("ResponseEndTime", "ResponseEndTime");
                    bulk.ColumnMappings.Add("Imei", "Imei");
                    bulk.ColumnMappings.Add("Appv", "Appv");
                    bulk.ColumnMappings.Add("AppType", "AppType");
                    bulk.ColumnMappings.Add("CreateTime", "CreateTime");
                    bulk.ColumnMappings.Add("InterfaceUrl", "InterfaceUrl");
                    bulk.ColumnMappings.Add("ParamsStr", "ParamsStr");
                    bulk.ColumnMappings.Add("IsDel", "IsDel");
                    bulk.ColumnMappings.Add("Status", "Status");
                    bulk.WriteToServer(table);
                }


                stopWatch.Stop();
                TimeSpan ts = stopWatch.Elapsed;
                string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}:{3:000}",
                                                                        ts.Hours, ts.Minutes, ts.Seconds,
                                                                        ts.Milliseconds);
                Console.WriteLine("RunTime " + elapsedTime);
                //
                table.Dispose();
                sqlcon.Close();
                sqlcon.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return true;
        }

        private DataTable CreateDataTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add("ID", typeof(int));
            table.Columns.Add("SoufunId", typeof(long));
            table.Columns.Add("OrderId", typeof(string));
            table.Columns.Add("InterfaceName", typeof(string));
            table.Columns.Add("ResponseStartTime", typeof(DateTime));
            table.Columns.Add("ResponseEndTime", typeof(DateTime));
            table.Columns.Add("Imei", typeof(string));
            table.Columns.Add("Appv", typeof(string));
            table.Columns.Add("AppType", typeof(int));
            table.Columns.Add("CreateTime", typeof(DateTime));
            table.Columns.Add("InterfaceUrl", typeof(string));
            table.Columns.Add("ParamsStr", typeof(string));
            table.Columns.Add("IsDel", typeof(int));
            table.Columns.Add("Status", typeof(int));
            return table;
        }

        private void AddModelToDataTable(DataTable table, LogModel model)
        {
            DataRow row = table.NewRow();
            //自增列
            //row["ID"] = logModel;
            row["SoufunId"] = string.IsNullOrEmpty(model.soufunId) ? 0L : long.Parse(model.soufunId);
            //未知字段
            //row["OrderId"] = logModel;
            row["InterfaceName"] = model.interfaceName;
            row["ResponseStartTime"] = DateTime.Now;
            row["ResponseEndTime"] = DateTime.Now;
            row["Imei"] = model.imei;
            row["Appv"] = model.appv;
            row["AppType"] = 1;
            row["CreateTime"] = string.IsNullOrEmpty(model.createTime) ? DateTime.Parse("1900/01/01") : DateTime.Parse(model.createTime.Replace("%2f", "/").Replace("%3a", ":").Replace("+", " "));
            row["InterfaceUrl"] = model.interfaceUrl;
            row["ParamsStr"] = model.paramsStr;
            row["IsDel"] = 0;
            row["Status"] = 3;
            table.Rows.Add(row);
        }

        private bool WriteLogInstance()
        {
            //使用Windows系统账户方式登录sqlserver
            //SqlConnection sqlcon = new SqlConnection(@"Data Source=.\localdb;Integrated Security=SSPI;Initial Catalog=workDB;");
            string conn = @"Data Source=.\localdb;Initial Catalog=workDB;UID=sa;PWD=123456;";
            SqlConnection sqlcon = new SqlConnection(conn);
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            //
            DataTable dt = new DataTable();
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("UserId", typeof(int));
            dt.Columns.Add("Note", typeof(string));
            
            for (int i = 1; i < 1000; i++)
            {
                DataRow row = dt.NewRow();
                row["UserId"] = i;
                row["Note"] = "测试用例"+i;
                dt.Rows.Add(row);
            }
            sqlcon.Open();

            using (SqlBulkCopy bulk = new SqlBulkCopy(conn))
            {
                bulk.BatchSize = 1000;
                bulk.DestinationTableName = "TestTable";
                bulk.ColumnMappings.Add("Id", "Id");
                bulk.ColumnMappings.Add("UserId", "UserId");
                bulk.ColumnMappings.Add("Note", "Note");
                bulk.WriteToServer(dt);
            }


            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}:{3:000}",
                                                                    ts.Hours, ts.Minutes, ts.Seconds,
                                                                    ts.Milliseconds);
            Console.WriteLine("RunTime " + elapsedTime);
            //
            dt.Dispose();
            sqlcon.Close();
            sqlcon.Dispose();
            return true;
        }

        private LogModel ParseParamListToLogModel(Dictionary<string, string> paramDict)
        {
            LogModel logModel = new LogModel();

            if (paramDict.ContainsKey("method"))
            {
                // 参数中Method
                logModel.interfaceUrl = paramDict["method"];
                // 参数中Method（同interfaceUrl）
                logModel.interfaceName = paramDict["method"];
            }

            if (paramDict.ContainsKey("RequestParamStr"))
            {
                // 请求参数get参数或post参数
                logModel.paramsStr = paramDict["RequestParamStr"];
            }

            if (paramDict.ContainsKey("timestamp"))
            {
                //  请求参数中timestamp
                logModel.createTime = paramDict["timestamp"];
            }

            if (paramDict.ContainsKey("imei"))
            {
                // 参数中的imei
                logModel.imei = paramDict["imei"];
            }

            if (paramDict.ContainsKey("appv"))
            {
                // 参数中的appv
                logModel.appv = paramDict["appv"];
            }

            if (paramDict.ContainsKey("RequestSoufunId"))
            {
                // 参数中的appv
                logModel.appv = paramDict["RequestSoufunId"];
            }

            return logModel;
        }

        private Dictionary<string,string> ParseParamList(string rowItem)
        {
            //参数键值对
            Dictionary<string, string> paramDict = new Dictionary<string, string>();
            //获取请求参数字符串
            string requestStr = string.Empty;
            string[] array = rowItem.Split(';');

            if (array.Length > 2)
            {
                //TODO:soufunId解析
                requestStr = array[2];
                string[] requestArray = requestStr.Split('?');
                if (!string.IsNullOrEmpty(requestStr) && requestArray.Length > 1)
                {
                    //获取Get参数列表信息
                    string paramStr = requestStr.Split('?')[1];

                    //如果参数请求字符串长度超过950则判断其为post请求，其请求记录不插入数据库
                    if (paramStr.Length > 950)
                    {
                        return paramDict = null;
                    }
                    else
                    {
                        //解析Get参数到数组
                        string[] paramList = paramStr.Split('&');
                        //新增请求参数
                        paramDict.Add("RequestParamStr", paramStr);

                        if (requestArray.Length > 36)
                        {
                            //新增搜房Id参数
                            paramDict.Add("RequestSoufunId", requestArray[36]);
                        }

                        for (int rowIndex = 0; rowIndex < paramList.Length; ++rowIndex)
                        {
                            var rowArray = paramList[rowIndex].Split('=');
                            if (!paramDict.ContainsKey(rowArray[0]))
                            {
                                paramDict.Add(rowArray[0], rowArray[1]);
                            }
                        }
                        return paramDict;
                    }
                }
                else
                {
                    return paramDict = null;
                }
            }
            else
            {
                return paramDict = null;
            }
        }

        private int ParseColumnAmount()
        {
            string sampleLine = "GetHandler_GetAndroidNoticeMsgSend;;http://interface.ebs.home.fang.com/Gethandler.ashx?apptype=1&messagename=GetHandler_GetAndroidNoticeMsgSend&messagetype=fzxapp&method=GetAndroidNoticeMsgSend&parameter=2fe6df5a2a14dc304dffc0c0244dcae2ad1a35b5cee1d60bcafaec7682025776c6977b85de085ccf2d65add61b2c6014ab4994081ea6212b095b2a040f9243bfc85f63db7993f148&returntype=0&version=v2.1.0&wirelesscode=93FA44F7B70ABB20326B0134E3EFD877&imei=863151021883662&cipher=5bdcd467c41493a90abb76711bfbb84f&appv=2.8.0&timestamp=2015%2f11%2f3+0%3a00%3a00;;2;;fzxapp;;0;;2015-11-03 00:00:00.193;;M355;;4.2.1;;183.206.180.50;;sfzx_android%7EM355%7E4.2.1;;????;;Wifi;;2.8.0;;gps%2Cwifi;;60018;;863151021883662;;0;;1;;;;;;;;;;";
            return sampleLine.Split(';').Length-1;
        }
    }
}
