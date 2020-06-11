﻿using System.Collections.Generic;
using Dmarc.Common.Interface.Tls.Domain;
using Dmarc.MxSecurityTester.Util;

namespace Dmarc.MxSecurityTester.Tls.Tests
{
    public class TlsWeakCipherSuitesRejected : ITlsTest
    {
        public int Id => (int)TlsTestType.TlsWeakCipherSuitesRejected;

        public string Name => nameof(TlsWeakCipherSuitesRejected);

        public TlsVersion Version => TlsVersion.TlsV12;

        public List<CipherSuite> CipherSuites => new List<CipherSuite>
        {
            CipherSuite.TLS_RSA_WITH_RC4_128_MD5,
            CipherSuite.TLS_NULL_WITH_NULL_NULL,
            CipherSuite.TLS_RSA_WITH_NULL_MD5,
            CipherSuite.TLS_RSA_WITH_NULL_SHA,
            CipherSuite.TLS_RSA_EXPORT_WITH_RC4_40_MD5,
            CipherSuite.TLS_RSA_EXPORT_WITH_RC2_CBC_40_MD5,
            CipherSuite.TLS_RSA_EXPORT_WITH_DES40_CBC_SHA,
            CipherSuite.TLS_RSA_WITH_DES_CBC_SHA,
            CipherSuite.TLS_DH_DSS_EXPORT_WITH_DES40_CBC_SHA,
            CipherSuite.TLS_DH_DSS_WITH_DES_CBC_SHA,
            CipherSuite.TLS_DH_RSA_EXPORT_WITH_DES40_CBC_SHA,
            CipherSuite.TLS_DH_RSA_WITH_DES_CBC_SHA,
            CipherSuite.TLS_DHE_DSS_EXPORT_WITH_DES40_CBC_SHA,
            CipherSuite.TLS_DHE_DSS_WITH_DES_CBC_SHA,
            CipherSuite.TLS_DHE_RSA_EXPORT_WITH_DES40_CBC_SHA,
        };
    }
}