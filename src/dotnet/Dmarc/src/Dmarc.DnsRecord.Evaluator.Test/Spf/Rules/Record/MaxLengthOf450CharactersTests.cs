﻿using System.Collections.Generic;
using Dmarc.DnsRecord.Evaluator.Spf.Domain;
using Dmarc.DnsRecord.Evaluator.Spf.Rules.Record;
using NUnit.Framework;

namespace Dmarc.DnsRecord.Evaluator.Test.Spf.Rules.Record
{
    [TestFixture]
    public class MaxLengthOf450CharactersTests
    {
        private const string RecordOk = "record record record record record record record record record record record record record record record record record record record record record record record record record record record record record record record record record record record record record record record record record record record record record record record record record record record record record record record record record record record record record record record record re";
        private const string RecordTooLong = "record record record record record record record record record record record record record record record record record record record record record record record record record record record record record record record record record record record record record record record record record record record record record record record record record record record record record record record record record record record record record record record record rec";

        private MaxLengthOf450Characters _rule;

        [SetUp]
        public void SetUp()
        {
            _rule = new MaxLengthOf450Characters();
        }

        [TestCase(RecordOk, false, TestName = "No error for string of 450 characters.")]
        [TestCase(RecordTooLong, true, TestName = "Error for string of 451 characters.")]
        public void NoErrorWhenFailureReportingOptionIsOne(string record, bool isErroredExpected)
        {
            SpfRecord spfRecord = new SpfRecord(record, new Version(string.Empty), new List<Term>(), string.Empty);

            bool isErrored = _rule.IsErrored(spfRecord, out Evaluator.Rules.Error error);

            Assert.That(isErrored, Is.EqualTo(isErroredExpected));

            Assert.That(error, isErroredExpected ? Is.Not.Null : Is.Null);
        }
    }
}
