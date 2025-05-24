using NLog;
using System.Text;
using SzlqTech.Common.Nlogs;

namespace SzlqTech.Equipment.Scanner
{
    /// <summary>
    /// 通用TCP读码器TCP协议
    /// </summary>
    public class TCPCommonScanner : TCPLongScanner
    {
        private const byte dataStart = 2;

        private const byte dataEnd = 13;

        private readonly string pattern = Convert.ToChar(dataStart).ToString() + ".*?" + Convert.ToChar(dataEnd).ToString();

        private volatile Queue<string> queue = new Queue<string>();

        private readonly object sync = new object();

        private Task? task;

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public TCPCommonScanner(string portName, string portKey, int codeLevel) : base(portName, portKey, codeLevel)
        {

        }

        /// <summary>
        /// 后台任务读取读码器数据
        /// </summary>
        private void ExecuteTask()
        {
            int count = 0;
            while (base.IsOpen)
            {
                try
                {
                    lock (sync)
                    {
                        if (queue.Count > 0)
                        {
                            ScanData scanData = new ScanData()
                            {
                                Data = queue.Dequeue(),
                                PortKey = PortKey,
                                PortName = PortName,
                                CodeLevel = base.CodeLevel
                            };
                            RaiseDataReceived(scanData);
                            count++;
                            Console.WriteLine(count);
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.ErrorHandler(ex);
                }
                Thread.Sleep(10);
            }
        }

        /// <summary>
        /// 打开并启动线程
        /// </summary>
        public override void Open()
        {
            base.Open();
            // task = new Task(ExecuteTask);
            // task.Start();
            task = Task.Factory.StartNew(ExecuteTask, TaskCreationOptions.LongRunning);
        }

        /// <summary>
        /// 等待并关闭线程
        /// </summary>
        public override void Close()
        {
            base.IsOpen = false;
            task?.Wait();
            base.Close();
        }

        protected override void ProcessData(byte[] bytes)
        {
            lock (sync)
            {          
                string scandata = Encoding.Default.GetString(bytes);
                logger.Debug($"收到数据[{scandata}]");
                queue.Enqueue(scandata);
            }
        }
    }
}
