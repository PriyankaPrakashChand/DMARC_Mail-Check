module "dnsrecord-mx-processor" {
  source	      	= "../../modules/lambdaProcessor-module"
  env-name            	= "${var.env-name}"
  aws-region          	= "${var.aws-region}"
  aws-account-id	= "${var.aws-account-id}"
  lambda-filename     	= "DnsRecordImporter.zip"
  source-code-hash	= "${base64sha256(file("DnsRecordImporter.zip"))}"
  lambda-function-name	= "TF-${var.env-name}-dnsrecord-mx-processor"
  handler		= "Dmarc.DnsRecord.Importer.Lambda::Dmarc.DnsRecord.Importer.Lambda.MxRecordImporter::HandleScheduledEvent"
  connection-string	= "Server = ${aws_rds_cluster.rds-cluster.endpoint}; Port = 3306; Database = dmarc; Uid = ${var.env-name}_${lookup(var.db-users,"domaininformation")};Connection Timeout=5;"
  subnet-ids		= "${join(",", aws_subnet.dmarc-env-subnet.*.id)}"
  security-group-ids	= "${aws_security_group.lambda-domaininformation-processor.id}"
  lambda-memory	        = "128"
  lambda-timeout        = "300"
  scheduler-interval	= "10"
  RemainingTimeThresholdSeconds = "30"
  RefreshIntervalSeconds = "86400"
  FailureRefreshIntervalSeconds = "1800"
  DnsRecordLimit = "50"
}




