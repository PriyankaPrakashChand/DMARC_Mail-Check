﻿using System;
using System.IO;
using System.Text;
using Dmarc.AggregateReport.Parser.Lambda.Compression;
using NUnit.Framework;

namespace Dmarc.Lambda.AggregateReport.Parser.Test.Compression
{
    [TestFixture]
    public class ZipDecompressorTests
    {
        private static readonly byte[] ZippedData = { 0x50, 0x4b, 0x03, 0x04, 0x0a, 0x00, 0x00, 0x00, 0x00, 0x00, 0x66, 0x78, 0x87, 0x49, 0x6e, 0xae, 0xaf, 0x05, 0x14, 0x00, 0x00, 0x00, 0x14, 0x00, 0x00, 0x00, 0x08, 0x00, 0x1c, 0x00, 0x74, 0x65, 0x73, 0x74, 0x64, 0x61, 0x74, 0x61, 0x55, 0x54, 0x09, 0x00, 0x03, 0xaf, 0x24, 0x48, 0x58, 0xaf, 0x24, 0x48, 0x58, 0x75, 0x78, 0x0b, 0x00, 0x01, 0x04, 0xe8, 0x03, 0x00, 0x00, 0x04, 0xe8, 0x03, 0x00, 0x00, 0x5a, 0x69, 0x70, 0x44, 0x65, 0x63, 0x6f, 0x6d, 0x70, 0x72, 0x65, 0x73, 0x73, 0x6f, 0x72, 0x54, 0x65, 0x73, 0x74, 0x0a, 0x50, 0x4b, 0x01, 0x02, 0x1e, 0x03, 0x0a, 0x00, 0x00, 0x00, 0x00, 0x00, 0x66, 0x78, 0x87, 0x49, 0x6e, 0xae, 0xaf, 0x05, 0x14, 0x00, 0x00, 0x00, 0x14, 0x00, 0x00, 0x00, 0x08, 0x00, 0x18, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0xb6, 0x81, 0x00, 0x00, 0x00, 0x00, 0x74, 0x65, 0x73, 0x74, 0x64, 0x61, 0x74, 0x61, 0x55, 0x54, 0x05, 0x00, 0x03, 0xaf, 0x24, 0x48, 0x58, 0x75, 0x78, 0x0b, 0x00, 0x01, 0x04, 0xe8, 0x03, 0x00, 0x00, 0x04, 0xe8, 0x03, 0x00, 0x00, 0x50, 0x4b, 0x05, 0x06, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x01, 0x00, 0x4e, 0x00, 0x00, 0x00, 0x56, 0x00, 0x00, 0x00, 0x00, 0x00 };
        private static readonly byte[] ZippedDataWithMulitpleFiles = { 0x50, 0x4b, 0x03, 0x04, 0x0a, 0x00, 0x00, 0x00, 0x00, 0x00, 0x66, 0x78, 0x87, 0x49, 0x6e, 0xae, 0xaf, 0x05, 0x14, 0x00, 0x00, 0x00, 0x14, 0x00, 0x00, 0x00, 0x09, 0x00, 0x1c, 0x00, 0x74, 0x65, 0x73, 0x74, 0x64, 0x61, 0x74, 0x61, 0x31, 0x55, 0x54, 0x09, 0x00, 0x03, 0xaf, 0x24, 0x48, 0x58, 0xaf, 0x24, 0x48, 0x58, 0x75, 0x78, 0x0b, 0x00, 0x01, 0x04, 0xe8, 0x03, 0x00, 0x00, 0x04, 0xe8, 0x03, 0x00, 0x00, 0x5a, 0x69, 0x70, 0x44, 0x65, 0x63, 0x6f, 0x6d, 0x70, 0x72, 0x65, 0x73, 0x73, 0x6f, 0x72, 0x54, 0x65, 0x73, 0x74, 0x0a, 0x50, 0x4b, 0x03, 0x04, 0x0a, 0x00, 0x00, 0x00, 0x00, 0x00, 0x13, 0x79, 0x87, 0x49, 0x6e, 0xae, 0xaf, 0x05, 0x14, 0x00, 0x00, 0x00, 0x14, 0x00, 0x00, 0x00, 0x09, 0x00, 0x1c, 0x00, 0x74, 0x65, 0x73, 0x74, 0x64, 0x61, 0x74, 0x61, 0x32, 0x55, 0x54, 0x09, 0x00, 0x03, 0xf6, 0x25, 0x48, 0x58, 0xf6, 0x25, 0x48, 0x58, 0x75, 0x78, 0x0b, 0x00, 0x01, 0x04, 0xe8, 0x03, 0x00, 0x00, 0x04, 0xe8, 0x03, 0x00, 0x00, 0x5a, 0x69, 0x70, 0x44, 0x65, 0x63, 0x6f, 0x6d, 0x70, 0x72, 0x65, 0x73, 0x73, 0x6f, 0x72, 0x54, 0x65, 0x73, 0x74, 0x0a, 0x50, 0x4b, 0x01, 0x02, 0x1e, 0x03, 0x0a, 0x00, 0x00, 0x00, 0x00, 0x00, 0x66, 0x78, 0x87, 0x49, 0x6e, 0xae, 0xaf, 0x05, 0x14, 0x00, 0x00, 0x00, 0x14, 0x00, 0x00, 0x00, 0x09, 0x00, 0x18, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0xb6, 0x81, 0x00, 0x00, 0x00, 0x00, 0x74, 0x65, 0x73, 0x74, 0x64, 0x61, 0x74, 0x61, 0x31, 0x55, 0x54, 0x05, 0x00, 0x03, 0xaf, 0x24, 0x48, 0x58, 0x75, 0x78, 0x0b, 0x00, 0x01, 0x04, 0xe8, 0x03, 0x00, 0x00, 0x04, 0xe8, 0x03, 0x00, 0x00, 0x50, 0x4b, 0x01, 0x02, 0x1e, 0x03, 0x0a, 0x00, 0x00, 0x00, 0x00, 0x00, 0x13, 0x79, 0x87, 0x49, 0x6e, 0xae, 0xaf, 0x05, 0x14, 0x00, 0x00, 0x00, 0x14, 0x00, 0x00, 0x00, 0x09, 0x00, 0x18, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0xb6, 0x81, 0x57, 0x00, 0x00, 0x00, 0x74, 0x65, 0x73, 0x74, 0x64, 0x61, 0x74, 0x61, 0x32, 0x55, 0x54, 0x05, 0x00, 0x03, 0xf6, 0x25, 0x48, 0x58, 0x75, 0x78, 0x0b, 0x00, 0x01, 0x04, 0xe8, 0x03, 0x00, 0x00, 0x04, 0xe8, 0x03, 0x00, 0x00, 0x50, 0x4b, 0x05, 0x06, 0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0x02, 0x00, 0x9e, 0x00, 0x00, 0x00, 0xae, 0x00, 0x00, 0x00, 0x00, 0x00 };
        private static readonly byte[] ZippedDataWithDirectory = { 0x50, 0x4b, 0x03, 0x04, 0x0a, 0x00, 0x00, 0x00, 0x00, 0x00, 0xd2, 0x7c, 0x87, 0x49, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x09, 0x00, 0x1c, 0x00, 0x74, 0x65, 0x73, 0x74, 0x64, 0x61, 0x74, 0x61, 0x2f, 0x55, 0x54, 0x09, 0x00, 0x03, 0xfc, 0x2c, 0x48, 0x58, 0xf4, 0x2c, 0x48, 0x58, 0x75, 0x78, 0x0b, 0x00, 0x01, 0x04, 0xe8, 0x03, 0x00, 0x00, 0x04, 0xe8, 0x03, 0x00, 0x00, 0x50, 0x4b, 0x01, 0x02, 0x1e, 0x03, 0x0a, 0x00, 0x00, 0x00, 0x00, 0x00, 0xd2, 0x7c, 0x87, 0x49, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x09, 0x00, 0x18, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x10, 0x00, 0xff, 0x41, 0x00, 0x00, 0x00, 0x00, 0x74, 0x65, 0x73, 0x74, 0x64, 0x61, 0x74, 0x61, 0x2f, 0x55, 0x54, 0x05, 0x00, 0x03, 0xfc, 0x2c, 0x48, 0x58, 0x75, 0x78, 0x0b, 0x00, 0x01, 0x04, 0xe8, 0x03, 0x00, 0x00, 0x04, 0xe8, 0x03, 0x00, 0x00, 0x50, 0x4b, 0x05, 0x06, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x01, 0x00, 0x4f, 0x00, 0x00, 0x00, 0x43, 0x00, 0x00, 0x00, 0x00, 0x00 };
        private static readonly byte[] NonZippedData = { 0x5a, 0x69, 0x70, 0x44, 0x65, 0x63, 0x6f, 0x6d, 0x70, 0x72, 0x65, 0x73, 0x73, 0x6f, 0x72, 0x54, 0x65, 0x73, 0x74, 0x0a };

        private ZipDecompressor _zipDecompressor;

        [SetUp]
        public void SetUp()
        {
            _zipDecompressor = new ZipDecompressor();
        }

        [Test]
        public void ZipCompressedStreamIsSucessfullyDecompressed()
        {
            Stream compressedStream = new MemoryStream(ZippedData);
            Stream decompressedStream = _zipDecompressor.Decompress(compressedStream);

            string decodedString = Encoding.UTF8.GetString(((MemoryStream) decompressedStream).ToArray());

            Assert.That(decodedString, Is.EqualTo("ZipDecompressorTest\n"));
        }

        [Test]
        public void ZipCompressedStreamWithMultipleFilesThrows()
        {
            Stream compressedStream = new MemoryStream(ZippedDataWithMulitpleFiles);
            Assert.Throws<ArgumentException>(() => _zipDecompressor.Decompress(compressedStream));
        }

        [Test]
        public void ZipCompressedStreamWithDirectoryStructureThrows()
        {
            Stream compressedStream = new MemoryStream(ZippedDataWithDirectory);
            Assert.Throws<ArgumentException>(() => _zipDecompressor.Decompress(compressedStream));
        }
    
        [Test]
        public void NonZippedStreamThrows()
        {
            Stream compressedStream = new MemoryStream(NonZippedData);
            Assert.Throws<InvalidDataException>(() => _zipDecompressor.Decompress(compressedStream));
        }
    }
}
