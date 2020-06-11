﻿using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Dmarc.Common.Data;
using Dmarc.Common.Interface.Logging;
using Dmarc.Common.TestSupport;
using Dmarc.DnsRecord.Importer.Lambda.Config;
using Dmarc.DnsRecord.Importer.Lambda.Dao.Dmarc;
using Dmarc.DnsRecord.Importer.Lambda.Dao.Entities;
using Dmarc.DnsRecord.Importer.Lambda.Dns.Client.RecordInfos;
using FakeItEasy;
using Heijden.DNS;
using NUnit.Framework;

namespace Dmarc.DnsRecord.Importer.Lambda.Test.Dao.Dmarc
{
    [TestFixture]
    [Category("Integration")]
    public class DmarcRecordDaoTests : DatabaseTestBase
    {
        private const string Host1 = "hostname1";
        private const string Domain = "gov.uk";
        private const string DmarcRecord1 = "v=dmarc1";
        private const string DmarcRecord2 = "v=dmarc1";

        private DmarcRecordDao _dmarcRecordDao;
        private IConnectionInfoAsync _connectionInfoAsync;
        private IRecordImporterConfig _recordImporterConfig;

        [SetUp]
        protected override void SetUp()
        {
            base.SetUp();

            _connectionInfoAsync = A.Fake<IConnectionInfoAsync>();
            _recordImporterConfig = A.Fake<IRecordImporterConfig>();
            _dmarcRecordDao = new DmarcRecordDao(_connectionInfoAsync, _recordImporterConfig, A.Fake<ILogger>());

            A.CallTo(() => _connectionInfoAsync.GetConnectionStringAsync()).Returns(Task.FromResult(ConnectionString));
        }

        [Test]
        public async Task RetrievesNoRecordsWhenNoNsRecordsExist()
        {
            CreateDomain(Domain);

            SetConfig(_recordImporterConfig, 10, 1, 10);

            Dictionary<DomainEntity, List<RecordEntity>> domainEntities = await _dmarcRecordDao.GetRecordsForUpdate();

            Assert.That(domainEntities.Count, Is.EqualTo(0));
        }

        [Test]
        public async Task RetrievesEmptyRecordsToUpdateWhenNoRecordsExist()
        {
            var domainId = CreateDomain(Domain);
            CreateNsRecord(Host1, domainId, DateTime.UtcNow);

            SetConfig(_recordImporterConfig, 10, 1, 10);

            Dictionary<DomainEntity, List<RecordEntity>> domainEntities = await _dmarcRecordDao.GetRecordsForUpdate();

            Assert.That(domainEntities.Keys.Count, Is.EqualTo(1));
            Assert.That(domainEntities.Keys.First().Name, Is.EqualTo(Domain));
            Assert.That(domainEntities.Values.First().Count, Is.EqualTo(0));
        }

        [Test]
        public async Task RetrievesRecordsToUpdateWhenSuccessfulRecordsNeedUpdating()
        {
            int successInterval = 10;

            DateTime earlier = DateTime.UtcNow.AddSeconds(-(successInterval + 1));

            ulong domainId = CreateDomain(Domain);
            CreateNsRecord(Host1, domainId, earlier);
            CreateDmarcRecord(DmarcRecord1, domainId, earlier);

            SetConfig(_recordImporterConfig, successInterval, 1, 10);

            Dictionary<DomainEntity, List<RecordEntity>> domainEntities = await _dmarcRecordDao.GetRecordsForUpdate();

            Assert.That(domainEntities.Values.First().Count, Is.EqualTo(1));
            Assert.That(domainEntities.Values.First().First().Domain.Name, Is.EqualTo(Domain));
            Assert.That(((DmarcRecordInfo)domainEntities.Values.First().First().RecordInfo).Record, Is.EqualTo(DmarcRecord1));
        }

        [Test]
        public async Task RetrievesNoRecordsToUpdateWhenSuccessfulRecordsDontNeedUpdating()
        {
            int successInterval = 10;

            DateTime earlier = DateTime.UtcNow.AddSeconds(-(successInterval - 1));

            ulong domainId = CreateDomain(Domain);
            CreateNsRecord(Host1, domainId, earlier);
            CreateDmarcRecord(DmarcRecord1, domainId, earlier);

            SetConfig(_recordImporterConfig, successInterval, 1, 10);

            Dictionary<DomainEntity, List<RecordEntity>> domainEntities = await _dmarcRecordDao.GetRecordsForUpdate();

            Assert.That(domainEntities.Count, Is.EqualTo(0));
        }

        [Test]
        public async Task RetrievesRecordsToUpdateWhenUnsuccessfulRecordsNeedUpdating()
        {
            int failureInterval = 10;

            DateTime earlier = DateTime.UtcNow.AddSeconds(-(failureInterval + 1));

            ulong domainId = CreateDomain(Domain);
            CreateNsRecord(Host1, domainId, earlier);
            CreateDmarcRecord(DmarcRecord1, domainId, earlier, 1);

            SetConfig(_recordImporterConfig, 100, failureInterval, 10);

            Dictionary<DomainEntity, List<RecordEntity>> domainEntities = await _dmarcRecordDao.GetRecordsForUpdate();

            Assert.That(domainEntities.Values.First().Count, Is.EqualTo(1));
            Assert.That(domainEntities.Values.First().First().Domain.Name, Is.EqualTo(Domain));
            Assert.That(((DmarcRecordInfo)domainEntities.Values.First().First().RecordInfo).Record, Is.EqualTo(DmarcRecord1));
        }

        [Test]
        public async Task RetrievesNoRecordsToUpdateWhenUnsuccessfulRecordsDontNeedUpdating()
        {
            int failureInterval = 10;

            DateTime earlier = DateTime.UtcNow.AddSeconds(-(failureInterval - 1));

            ulong domainId = CreateDomain(Domain);
            CreateNsRecord(Host1, domainId, earlier);
            CreateDmarcRecord(DmarcRecord1, domainId, earlier, 1);

            SetConfig(_recordImporterConfig, 100, failureInterval, 10);

            Dictionary<DomainEntity, List<RecordEntity>> domainEntities = await _dmarcRecordDao.GetRecordsForUpdate();

            Assert.That(domainEntities.Count, Is.EqualTo(0));
        }

        [Test]
        public async Task CorrectlyInsertsNewRecords()
        {
            ulong domainId = CreateDomain(Domain);

            List<RecordEntity> recordEntities = new List<RecordEntity>
            {
                new RecordEntity(null, new DomainEntity((int)domainId, Domain), new DmarcRecordInfo(DmarcRecord1, string.Empty, false, false), RCode.NoError, 0),
                new RecordEntity(null, new DomainEntity((int)domainId, Domain), new DmarcRecordInfo(DmarcRecord2, string.Empty, false, false), RCode.NoError, 0)
            };

            await _dmarcRecordDao.InsertOrUpdateRecords(recordEntities);

            List<DmarcRecord> recordEntitiesFromDb = GetAllRecordEntities();

            Assert.That(recordEntitiesFromDb.Count, Is.EqualTo(2));

            Assert.That(recordEntitiesFromDb[0].DomainId, Is.EqualTo(domainId));
            Assert.That(recordEntitiesFromDb[0].Record, Is.EqualTo(DmarcRecord1));
            Assert.That(recordEntitiesFromDb[0].StartDate, Is.Not.Null);
            Assert.That(recordEntitiesFromDb[0].EndDate, Is.Null);
            Assert.That(recordEntitiesFromDb[0].LastChecked, Is.Not.Null);
            Assert.That(recordEntitiesFromDb[0].FailureCount, Is.EqualTo(0));
            Assert.That(recordEntitiesFromDb[0].ResultCode, Is.EqualTo(0));

            Assert.That(recordEntitiesFromDb[1].DomainId, Is.EqualTo(domainId));
            Assert.That(recordEntitiesFromDb[1].Record, Is.EqualTo(DmarcRecord2));
            Assert.That(recordEntitiesFromDb[1].StartDate, Is.Not.Null);
            Assert.That(recordEntitiesFromDb[1].EndDate, Is.Null);
            Assert.That(recordEntitiesFromDb[1].LastChecked, Is.Not.Null);
            Assert.That(recordEntitiesFromDb[1].FailureCount, Is.EqualTo(0));
            Assert.That(recordEntitiesFromDb[1].ResultCode, Is.EqualTo(0));
        }

        [Test]
        public async Task CorrectlyUpdatesUnchangedRecords()
        {
            ulong domainId = CreateDomain(Domain);
            ulong recordId1 = CreateDmarcRecord(DmarcRecord1, domainId, DateTime.UtcNow);
            ulong recordId2 = CreateDmarcRecord(DmarcRecord2, domainId, DateTime.UtcNow);

            List<RecordEntity> recordEntities = new List<RecordEntity>
            {
                new RecordEntity((int)recordId1, new DomainEntity((int)domainId, Domain), new DmarcRecordInfo(DmarcRecord1, string.Empty, false, false), RCode.NoError, 0),
                new RecordEntity((int)recordId2, new DomainEntity((int)domainId, Domain), new DmarcRecordInfo(DmarcRecord2, string.Empty, false, false), RCode.NoError, 0),
            };

            await _dmarcRecordDao.InsertOrUpdateRecords(recordEntities);

            List<DmarcRecord> recordEntitiesFromDb = GetAllRecordEntities();

            Assert.That(recordEntitiesFromDb.Count, Is.EqualTo(2));

            Assert.That(recordEntitiesFromDb[0].DomainId, Is.EqualTo(domainId));
            Assert.That(recordEntitiesFromDb[0].Record, Is.EqualTo(DmarcRecord1));
            Assert.That(recordEntitiesFromDb[0].StartDate, Is.Not.Null);
            Assert.That(recordEntitiesFromDb[0].EndDate, Is.Null);
            Assert.That(recordEntitiesFromDb[0].LastChecked, Is.Not.Null);
            Assert.That(recordEntitiesFromDb[0].FailureCount, Is.EqualTo(0));
            Assert.That(recordEntitiesFromDb[0].ResultCode, Is.EqualTo(0));

            Assert.That(recordEntitiesFromDb[1].DomainId, Is.EqualTo(domainId));
            Assert.That(recordEntitiesFromDb[1].Record, Is.EqualTo(DmarcRecord2));
            Assert.That(recordEntitiesFromDb[1].StartDate, Is.Not.Null);
            Assert.That(recordEntitiesFromDb[1].EndDate, Is.Null);
            Assert.That(recordEntitiesFromDb[1].LastChecked, Is.Not.Null);
            Assert.That(recordEntitiesFromDb[1].FailureCount, Is.EqualTo(0));
            Assert.That(recordEntitiesFromDb[1].ResultCode, Is.EqualTo(0));
        }

        [Test]
        public async Task CorrectlyExpiresOldRecords()
        {
            ulong domainId = CreateDomain(Domain);
            ulong recordId1 = CreateDmarcRecord(DmarcRecord1, domainId, DateTime.UtcNow);
            ulong recordId2 = CreateDmarcRecord(DmarcRecord2, domainId, DateTime.UtcNow);

            DateTime now = DateTime.Now.Truncate(TimeSpan.FromSeconds(1));
            DateTime nowPlusOne = now.AddSeconds(1);

            List<RecordEntity> recordEntities = new List<RecordEntity>
            {
                new RecordEntity((int)recordId1, new DomainEntity((int)domainId, Domain), new DmarcRecordInfo(DmarcRecord1, string.Empty, false, false), RCode.NoError, 0, now),
                new RecordEntity((int)recordId2, new DomainEntity((int)domainId, Domain), new DmarcRecordInfo(DmarcRecord2, string.Empty, false, false), RCode.NoError, 0, nowPlusOne),
            };

            await _dmarcRecordDao.InsertOrUpdateRecords(recordEntities);

            List<DmarcRecord> recordEntitiesFromDb = GetAllRecordEntities();

            Assert.That(recordEntitiesFromDb.Count, Is.EqualTo(2));

            Assert.That(recordEntitiesFromDb[0].DomainId, Is.EqualTo(domainId));
            Assert.That(recordEntitiesFromDb[0].Record, Is.EqualTo(DmarcRecord1));
            Assert.That(recordEntitiesFromDb[0].StartDate, Is.Not.Null);
            Assert.That(recordEntitiesFromDb[0].EndDate, Is.EqualTo(now));
            Assert.That(recordEntitiesFromDb[0].LastChecked, Is.Not.Null);
            Assert.That(recordEntitiesFromDb[0].FailureCount, Is.EqualTo(0));
            Assert.That(recordEntitiesFromDb[0].ResultCode, Is.EqualTo(0));

            Assert.That(recordEntitiesFromDb[1].DomainId, Is.EqualTo(domainId));
            Assert.That(recordEntitiesFromDb[1].Record, Is.EqualTo(DmarcRecord2));
            Assert.That(recordEntitiesFromDb[1].StartDate, Is.Not.Null);
            Assert.That(recordEntitiesFromDb[1].EndDate, Is.EqualTo(nowPlusOne));
            Assert.That(recordEntitiesFromDb[1].LastChecked, Is.Not.Null);
            Assert.That(recordEntitiesFromDb[1].FailureCount, Is.EqualTo(0));
            Assert.That(recordEntitiesFromDb[1].ResultCode, Is.EqualTo(0));
        }

        [Test]
        public async Task CorrectlyUpdatesFailureCount()
        {
            ulong domainId = CreateDomain(Domain);
            ulong recordId1 = CreateDmarcRecord(DmarcRecord1, domainId, DateTime.UtcNow);

            DateTime now = DateTime.Now.Truncate(TimeSpan.FromSeconds(1));

            int failureCount = 1;
            List<RecordEntity> recordEntities = new List<RecordEntity>
            {
                new RecordEntity((int)recordId1, new DomainEntity((int)domainId, Domain), new DmarcRecordInfo(DmarcRecord1, string.Empty, false, false), RCode.NoError, failureCount, now)
            };

            await _dmarcRecordDao.InsertOrUpdateRecords(recordEntities);

            List<DmarcRecord> recordEntitiesFromDb = GetAllRecordEntities();

            Assert.That(recordEntitiesFromDb.Count, Is.EqualTo(1));

            Assert.That(recordEntitiesFromDb[0].DomainId, Is.EqualTo(domainId));
            Assert.That(recordEntitiesFromDb[0].Record, Is.EqualTo(DmarcRecord1));
            Assert.That(recordEntitiesFromDb[0].StartDate, Is.Not.Null);
            Assert.That(recordEntitiesFromDb[0].EndDate, Is.EqualTo(now));
            Assert.That(recordEntitiesFromDb[0].LastChecked, Is.Not.Null);
            Assert.That(recordEntitiesFromDb[0].FailureCount, Is.EqualTo(failureCount));
            Assert.That(recordEntitiesFromDb[0].ResultCode, Is.EqualTo(0));
        }

        [Test]
        public async Task CorrectlyUpdatesErrorStatus()
        {
            ulong domainId = CreateDomain(Domain);
            ulong recordId1 = CreateDmarcRecord(DmarcRecord1, domainId, DateTime.UtcNow);

            DateTime now = DateTime.Now.Truncate(TimeSpan.FromSeconds(1));

            RCode responseCode = RCode.ServFail;
            List<RecordEntity> recordEntities = new List<RecordEntity>
            {
                new RecordEntity((int)recordId1, new DomainEntity((int)domainId, Domain), new DmarcRecordInfo(DmarcRecord1, string.Empty, false, false), responseCode, 0, now)
            };

            await _dmarcRecordDao.InsertOrUpdateRecords(recordEntities);

            List<DmarcRecord> recordEntitiesFromDb = GetAllRecordEntities();

            Assert.That(recordEntitiesFromDb.Count, Is.EqualTo(1));

            Assert.That(recordEntitiesFromDb[0].DomainId, Is.EqualTo(domainId));
            Assert.That(recordEntitiesFromDb[0].Record, Is.EqualTo(DmarcRecord1));
            Assert.That(recordEntitiesFromDb[0].StartDate, Is.Not.Null);
            Assert.That(recordEntitiesFromDb[0].EndDate, Is.EqualTo(now));
            Assert.That(recordEntitiesFromDb[0].LastChecked, Is.Not.Null);
            Assert.That(recordEntitiesFromDb[0].FailureCount, Is.EqualTo(0));
            Assert.That(recordEntitiesFromDb[0].ResultCode, Is.EqualTo((ushort)responseCode));
        }

        [TearDown]
        protected override void TearDown()
        {
            base.TearDown();
        }

        #region Test Support

        private void SetConfig(IRecordImporterConfig config, int refreshIntervalSeconds, int failureRefreshIntervalSeconds, int dnsRecordLimit)
        {
            A.CallTo(() => config.RefreshIntervalSeconds).Returns(refreshIntervalSeconds);
            A.CallTo(() => config.FailureRefreshIntervalSeconds).Returns(failureRefreshIntervalSeconds);
            A.CallTo(() => config.DnsRecordLimit).Returns(dnsRecordLimit);
        }

        private ulong CreateDomain(string domainName)
        {
            MySqlHelper.ExecuteNonQuery(ConnectionString, $@"INSERT INTO `domain`(`name`, `created_by`) VALUES('{domainName}', 'test');");
            return (ulong)MySqlHelper.ExecuteScalar(ConnectionString, "SELECT LAST_INSERT_ID();");
        }

        private ulong CreateNsRecord(string hostName, ulong domainId, DateTime lastChecked, int failureCount = 0)
        {
            MySqlHelper.ExecuteNonQuery(ConnectionString, $@"INSERT INTO `dns_record_ns`(`domain_id`, `hostname`, `last_checked`, `failure_count`, `result_code`) VALUES({domainId}, '{hostName}', '{lastChecked:yyyy-MM-dd HH:mm:ss}', {failureCount}, 0);");
            return (ulong)MySqlHelper.ExecuteScalar(ConnectionString, "SELECT LAST_INSERT_ID();");
        }

        private ulong CreateDmarcRecord(string record, ulong domainId, DateTime lastChecked, int failureCount = 0)
        {
            MySqlHelper.ExecuteNonQuery(ConnectionString, $@"INSERT INTO `dns_record_dmarc`(`domain_id`, `record`, `last_checked`, `failure_count`, `result_code`) VALUES({domainId}, '{record}', '{lastChecked:yyyy-MM-dd HH:mm:ss}', {failureCount}, 0);");
            return (ulong)MySqlHelper.ExecuteScalar(ConnectionString, "SELECT LAST_INSERT_ID();");
        }

        private List<DmarcRecord> GetAllRecordEntities()
        {
            List<DmarcRecord> records = new List<DmarcRecord>();
            using (DbDataReader reader = MySqlHelper.ExecuteReader(ConnectionString, "SELECT * FROM dns_record_dmarc"))
            {
                while (reader.Read())
                {
                    DmarcRecord recordEntity = new DmarcRecord(
                        reader.GetInt32("id"),
                        reader.GetInt32("domain_id"),
                        reader.GetString("record"),
                        reader.GetDateTime("start_date"),
                        reader.GetDateTimeNullable("end_date"),
                        reader.GetDateTime("last_checked"),
                        reader.GetInt16("failure_count"),
                        reader.GetInt16("result_code"));

                    records.Add(recordEntity);
                }
            }
            return records;
        }

        private class DmarcRecord
        {
            public DmarcRecord(
                int id,
                int domainId,
                string record,
                DateTime startDate,
                DateTime? endDate,
                DateTime lastChecked,
                int failureCount,
                int resultCode)
            {
                Id = id;
                DomainId = domainId;
                Record = record;
                StartDate = startDate;
                EndDate = endDate;
                LastChecked = lastChecked;
                FailureCount = failureCount;
                ResultCode = resultCode;
            }

            public int Id { get; }
            public int DomainId { get; }
            public string Record { get; }
            public DateTime StartDate { get; }
            public DateTime? EndDate { get; }
            public DateTime LastChecked { get; }
            public int FailureCount { get; }
            public int ResultCode { get; }
        }

        #endregion Test Support
    }
}
