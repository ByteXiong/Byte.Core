namespace Byte.Core.Common
{

    public class SnowflakeIdGenerator
    {
        private readonly long workerId;
        private readonly long datacenterId;
        private readonly long epoch = new DateTimeOffset(2023, 1, 1, 0, 0, 0, TimeSpan.Zero).ToUnixTimeMilliseconds();

        private const int workerIdBits = 5;
        private const int datacenterIdBits = 5;
        private const int sequenceBits = 12;

        private const long maxWorkerId = -1L ^ (-1L << workerIdBits);
        private const long maxDatacenterId = -1L ^ (-1L << datacenterIdBits);
        private const long maxSequence = -1L ^ (-1L << sequenceBits);

        private long sequence;
        private long lastTimestamp = -1;

        public SnowflakeIdGenerator(long workerId, long datacenterId)
        {
            // 验证工作机器ID是否在有效范围内
            if (workerId < 0 || workerId > maxWorkerId)
                throw new ArgumentException($"Worker ID must be between 0 and {maxWorkerId}");

            // 验证数据中心ID是否在有效范围内
            if (datacenterId < 0 || datacenterId > maxDatacenterId)
                throw new ArgumentException($"Datacenter ID must be between 0 and {maxDatacenterId}");

            this.workerId = workerId;
            this.datacenterId = datacenterId;
        }

        public long NextId()
        {
            lock (this)
            {
                var timestamp = GetCurrentTimestamp();

                // 如果发生时钟回拨，抛出异常
                if (timestamp < lastTimestamp)
                    throw new InvalidOperationException("Clock moved backwards. Refusing to generate ID.");

                if (timestamp == lastTimestamp)
                {
                    // 同一毫秒内的序列递增
                    sequence = (sequence + 1) & maxSequence;

                    if (sequence == 0)
                        timestamp = WaitNextTimestamp(lastTimestamp);
                }
                else
                {
                    // 不同毫秒，序列从零开始
                    sequence = 0;
                }

                lastTimestamp = timestamp;

                // 组装雪花ID
                var id = ((timestamp - epoch) << (workerIdBits + datacenterIdBits + sequenceBits))
                        | (datacenterId << (workerIdBits + sequenceBits))
                        | (workerId << sequenceBits)
                        | sequence;

                return id;
            }
        }

        private long GetCurrentTimestamp()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }

        private long WaitNextTimestamp(long lastTimestamp)
        {
            var timestamp = GetCurrentTimestamp();
            while (timestamp <= lastTimestamp)
            {
                timestamp = GetCurrentTimestamp();
            }

            return timestamp;
        }
    }

    public class SnowflakeIdGeneratorConfig
    {
        public long WorkerId { get; set; }
        public long DatacenterId { get; set; }
    }

}