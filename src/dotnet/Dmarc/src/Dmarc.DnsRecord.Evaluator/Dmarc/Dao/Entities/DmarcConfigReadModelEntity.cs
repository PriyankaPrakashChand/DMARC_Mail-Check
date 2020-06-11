﻿namespace Dmarc.DnsRecord.Evaluator.Dmarc.Dao.Entities
{
    public class DmarcConfigReadModelEntity
    {
        public DmarcConfigReadModelEntity(int domainId, int errorCount, ErrorType? maxErrorSeverity, string readModel)
        {
            DomainId = domainId;
            ErrorCount = errorCount;
            MaxErrorSeverity = maxErrorSeverity;
            ReadModel = readModel;
        }

        public int DomainId { get; }
        public int ErrorCount { get; }
        public ErrorType? MaxErrorSeverity { get; }
        public string ReadModel { get; }

        protected bool Equals(DmarcConfigReadModelEntity other)
        {
            return DomainId == other.DomainId && ErrorCount == other.ErrorCount && MaxErrorSeverity == other.MaxErrorSeverity && string.Equals(ReadModel, other.ReadModel);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((DmarcConfigReadModelEntity) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = DomainId;
                hashCode = (hashCode * 397) ^ ErrorCount;
                hashCode = (hashCode * 397) ^ MaxErrorSeverity.GetHashCode();
                hashCode = (hashCode * 397) ^ (ReadModel != null ? ReadModel.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
