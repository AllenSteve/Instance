﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BaseFunction.ServiceInterface;
using BaseFunction.Service;
using BaseFunction.Service.EbsService;
using BaseFunction.Service.iTecService;

namespace StandardUnitTest.ServiceTest
{
    [TestClass]
    public class CommonServiceTest
    {
        private IServiceFactory factory { get; set; }

        public CommonServiceTest()
        {
            factory = new ServiceFactory();
        }

        [TestMethod]
        public void ParserTest()
        {
            var service = factory.CreateInstance<ParseCookie>();
            string sfut = service.ParseSfutCookie();
            string url = service.DecryptSfut2Url(sfut);
            object userInfo = service.ParseUserInfo(url);
        }

        [TestMethod]
        public void EncryptServiceTest()
        {
            var service = factory.CreateInstance<EncryptService>();
            string orgin = "soufunid";
            string pwd = "123456789012345678901234";
            string vi = "abcdefgh";
            //string ret = EncryptService.AESEncrypt(orgin,pwd,vi);
            string enstr = string.Empty;
            // 256位加密
            enstr = EncryptService.Encrypt(orgin);
            //128位加密
            enstr = EncryptService.AESEncrypt(orgin);
            enstr = EncryptService.AESDecrypt(enstr);
            object param = new { soufunId = "10000",timestamp = 0x00};
           // enstr = service.AdvancedEncrypt(param);
            DateTime now = DateTime.Now;
            string timestamp = EncryptService.ConvertDateTime2Timestamp(now.AddYears(1000)).ToString();
            bool flag = (timestamp.Equals(FunLayer.Transform.TimeStamp(now.AddYears(1000)).ToString()));
            DateTime time = service.GetTimestampDate("1450320329");



            //enstr = service.AdvancedEncrypt(timestamp, "1000000");
            //enstr = service.AdvancedDecrypt(enstr);
            //enstr = service.GetTimeStamp(enstr);

            //bool flag = service.VerifyParam("1000000",enstr,new TimeSpan(TimeSpan.TicksPerMinute*5));

        }

        [TestMethod]
        public void CodeTableServiceTest()
        {
            var service = factory.CreateInstance<CodeTableService>();
            var list = service.GetList("MoneySource");
            var code = service.Get("TaxType","1");
        }

    }
}
